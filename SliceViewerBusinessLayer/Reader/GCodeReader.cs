using OpenVectorFormat;
using OpenVectorFormat.AbstractReaderWriter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenTK;
using System.Globalization;
using System.Diagnostics;
using OpenTK.Mathematics;

namespace OVFSliceViewerCore.Reader
{
    public class GCodeReader : FileReader
    {
        public override CacheState CacheState => CacheState.CompleteJobCached;

        private Job _job;
        public override Job JobShell => _job;

        public override Job CacheJobToMemory()
        {
            return _job;
        }

        public override void Dispose()
        {
            _job = null;
        }

        public override VectorBlock GetVectorBlock(int i_workPlane, int i_vectorblock)
        {
            return _job.WorkPlanes[i_workPlane].VectorBlocks[i_vectorblock];
        }

        public override WorkPlane GetWorkPlane(int i_workPlane)
        {
            return _job.WorkPlanes[i_workPlane];
        }

        public override WorkPlane GetWorkPlaneShell(int i_workPlane)
        {
            return _job.WorkPlanes[i_workPlane];
        }

        private bool _extrusionIsAbsolute = true;
        public override void OpenJob(string filename, IFileReaderWriterProgress progress)
        {
            var gcodeFile = File.ReadAllLines(filename);
            var job = new Job();
            var regex = new Regex(@"G1(\sX(?<X>\d+\.\d+))*(\sY(?<Y>\d+\.\d+))*(\sZ(?<Z>\d+\.\d+))*(\sE(?<E>-*\d+\.\d+))*");
            var extruderResetRegex = new Regex(@"G92 E0");
            var typeRegex = new Regex(@";TYPE:(?<type>.*)");
            var extruderIsRelative = new Regex("M83");
            var extruderIsAbsolute = new Regex("M82");

            var extruderOffset = 0f;
            List<string> types = new List<string>();
            string lastType = "External Perimeter";

            Vector4 position = new Vector4();
            List<Vector4> movements = new List<Vector4>();

            for (int i = 0; i < gcodeFile.Length; i++)
            {
                var gcodeLine = gcodeFile[i];
                var extrusionModeAbs = extruderIsAbsolute.Match(gcodeLine);
                var extrusionModeRel = extruderIsRelative.Match(gcodeLine);

                if (extrusionModeAbs.Success)
                {
                    _extrusionIsAbsolute = true;
                }
                else if (extrusionModeRel.Success)
                {
                    _extrusionIsAbsolute = false;
                }

                var matchType = typeRegex.Match(gcodeLine);
                if (matchType.Success)
                {
                    lastType = matchType.Groups["type"].Value;
                }

                var matchExtruderReset = extruderResetRegex.Match(gcodeLine);
                if (matchExtruderReset.Success)
                {
                    extruderOffset = position.W;
                }

                var matchMove = regex.Match(gcodeLine);

                if (matchMove.Success)
                {
                    var newExtruderPosition = matchMove.Groups["E"];
                    var newXString = matchMove.Groups["X"];
                    var newYString = matchMove.Groups["Y"];
                    var newZString = matchMove.Groups["Z"];

                    var newX = !string.IsNullOrWhiteSpace(newXString.Value) ? Convert.ToSingle(newXString.Value, CultureInfo.InvariantCulture) : position.X;
                    var newY = !string.IsNullOrWhiteSpace(newYString.Value) ? Convert.ToSingle(newYString.Value, CultureInfo.InvariantCulture) : position.Y;
                    var newZ = !string.IsNullOrWhiteSpace(newZString.Value) ? Convert.ToSingle(newZString.Value, CultureInfo.InvariantCulture) : position.Z;

                    if (string.IsNullOrWhiteSpace(newExtruderPosition.Value))
                    {
                        position.X = newX;
                        position.Y = newY;
                        position.Z = newZ;
                    }
                    else
                    {
                        float extruderPosValue = Convert.ToSingle(newExtruderPosition.Value, CultureInfo.InvariantCulture);

                        if (position.X != newX || position.Y != newY)
                        {
                            movements.Add(position);
                            position.X = newX;
                            position.Y = newY;
                            position.Z = newZ;
                            if (_extrusionIsAbsolute)
                            {
                                position.W = extruderPosValue + extruderOffset;
                            }
                            else
                            {
                                position.W = position.W + extruderPosValue;
                            }

                            movements.Add(position);
                            types.Add(lastType);
                            types.Add(lastType);
                        }
                        else
                        {
                            if (_extrusionIsAbsolute)
                            {
                                position.W = extruderPosValue + extruderOffset;
                            }
                            else
                            {
                                position.W = position.W + extruderPosValue;
                            }
                        }
                    }
                }
            }

            var workplanes = new Dictionary<float, WorkPlane>();

            float conversionFactor = 22.23f;
            //conversionFactor = 1.5f;
            float inverseConversionFactor = 1f / conversionFactor;

            for (int i = 1; i < movements.Count; i += 2)
            {
                var zHeight = movements[i - 1].Z;
                var start = movements[i - 1];
                var end = movements[i];

                if (types[i] != "External perimeter"/* && types[i] != "Perimeter"*/)
                {
                    continue;
                }

                if (!workplanes.ContainsKey(zHeight))
                {
                    workplanes[zHeight] = new WorkPlane();
                    workplanes[zHeight].ZPosInMm = zHeight;
                    workplanes[zHeight].VectorBlocks.Add
                        (
                            new VectorBlock()
                            {
                                MetaData = new VectorBlock.Types.VectorBlockMetaData()
                                {
                                    PartKey = 0
                                },
                                LpbfMetadata = new VectorBlock.Types.LPBFMetadata()
                                {
                                    PartArea = VectorBlock.Types.PartArea.Contour,
                                    SkinType = VectorBlock.Types.LPBFMetadata.Types.SkinType.UpSkin,
                                    StructureType = VectorBlock.Types.StructureType.Part
                                }
                            }
                        );
                    workplanes[zHeight].NumBlocks = 1;
                    workplanes[zHeight].VectorBlocks[0].HatchParaAdapt = new VectorBlock.Types.HatchesParaAdapt();
                }

                var block = workplanes[zHeight].VectorBlocks[0];
                var lineSequenceParaAdapt = new VectorBlock.Types.LineSequenceParaAdapt()
                {
                    Parameter = VectorBlock.Types.LineSequenceParaAdapt.Types.AdaptedParameter.LaserPowerInW
                };

                lineSequenceParaAdapt.PointsWithParas.Add(start.X);
                lineSequenceParaAdapt.PointsWithParas.Add(start.Y);

                //var extruderValue = (float)(((start.W * conversionFactor) % (Math.PI * 2)) / (Math.PI * 2));
                var extruderValue = (float)(start.W * inverseConversionFactor % 1);
                extruderValue = extruderValue >= 0.5f ? 1f - extruderValue : extruderValue;
                lineSequenceParaAdapt.PointsWithParas.Add(extruderValue);

                var distanceToFirstExtremePoint = conversionFactor * 0.5f - start.W % conversionFactor;
                var newPosition = start.W + distanceToFirstExtremePoint;
                while (newPosition < end.W)
                {
                    var lerpValue = (newPosition - start.W) / (end.W - start.W);
                    if (lerpValue > 0 && lerpValue < 1)
                    {
                        var intermediatePosition = Vector4.Lerp(start, end, lerpValue);
                        var periodicValue = intermediatePosition.W * inverseConversionFactor % 1;
                        periodicValue = periodicValue > 0.5f ? 1f - periodicValue : periodicValue;

                        lineSequenceParaAdapt.PointsWithParas.Add(intermediatePosition.X);
                        lineSequenceParaAdapt.PointsWithParas.Add(intermediatePosition.Y);
                        lineSequenceParaAdapt.PointsWithParas.Add(periodicValue);
                    }

                    newPosition += conversionFactor * 0.5f;
                }

                lineSequenceParaAdapt.PointsWithParas.Add(end.X);
                lineSequenceParaAdapt.PointsWithParas.Add(end.Y);

                //extruderValue = (float)(((end.W * conversionFactor) % (Math.PI * 2)) / (Math.PI * 2));
                extruderValue = (float)(end.W * inverseConversionFactor % 1f);
                extruderValue = extruderValue >= 0.5f ? 1f - extruderValue : extruderValue;
                lineSequenceParaAdapt.PointsWithParas.Add(extruderValue);

                block.HatchParaAdapt.HatchAsLinesequence.Add(lineSequenceParaAdapt);
            }

            //types = types.Distinct().ToList();

            var distinctZHeights = movements.Select(x => x.Z).Distinct().OrderBy(x => x).ToList();

            foreach (var height in distinctZHeights)
            {
                var extrusionValues = movements.Where(x => x.Z == height).Select(x => x.W).ToList();

                var minExtrusion = extrusionValues.Min();
                var maxExtrusion = extrusionValues.Max();

                var extrusionDelta = maxExtrusion - minExtrusion;

                //Debug.WriteLine($"{height} {extrusionDelta}");
            }
            Dictionary<float, List<Line>> linesInWorkplane = new Dictionary<float, List<Line>>();
            for (int i = 1; i < movements.Count; i += 2)
            {
                var start = movements[i - 1];
                var end = movements[i];
                var line = new Line()
                {
                    ExtrusionStart = start.W,
                    ExtrusionEnd = end.W,
                    ExtrusionLength = end.W - start.W,
                    Type = types[i]
                };
                if (!linesInWorkplane.ContainsKey(start.Z))
                {
                    linesInWorkplane[start.Z] = new List<Line>();
                }
                linesInWorkplane[start.Z].Add(line);
            }

            var test = types.Distinct();

            var workplanesList = workplanes.Select(x => x.Value).OrderBy(x => x.ZPosInMm).ToList();
            float lastDelta = 0f;
            var lastWpExtrusion = 0f;
            for (int i = 0; i < workplanesList.Count; i++)
            {
                var workplane = workplanesList[i];

                //prevWorkplane.VectorBlocks[0].HatchParaAdapt.HatchAsLinesequence[0].PointsWithParas[2];
                var thisWpPerimeterExtrusionStart = linesInWorkplane[workplane.ZPosInMm]/*.Where(x => x.Type == "External perimeter")*/.Last().ExtrusionEnd;

                var extrusionDelta = conversionFactor - (thisWpPerimeterExtrusionStart + lastDelta) % conversionFactor;
                lastDelta = extrusionDelta;

                lastWpExtrusion = lastWpExtrusion + thisWpPerimeterExtrusionStart;
                Debug.WriteLine($"{i} {lastWpExtrusion} ");

                //string test123 = $"<range min_z=\"{prevWorkplane.ZPosInMm.ToString(CultureInfo.InvariantCulture)}\" max_z=\"{workplane.ZPosInMm.ToString(CultureInfo.InvariantCulture)}\"><option opt_key=\"extruder\">0</option><option opt_key=\"fill_density\">{Math.Min(100f,(layerInfill*100f)).ToString(CultureInfo.InvariantCulture)}%</option><option opt_key=\"fill_pattern\">rectilinear</option><option opt_key=\"layer_height\">0.12</option></range>";
                //Debug.WriteLine(test123);
            }


            _job = new Job();
            _job.WorkPlanes.AddRange(workplanes.Select(x => x.Value).OrderBy(x => x.ZPosInMm).ToList());
            _job.NumWorkPlanes = _job.WorkPlanes.Count();
            _job.PartsMap.Add(0, new Part() { Name = "GCodeFile" });
        }

        struct Line
        {
            public float ExtrusionStart;
            public float ExtrusionEnd;
            public float ExtrusionLength;
            public string Type;
        }
        public override void UnloadJobFromMemory()
        {
            throw new NotImplementedException();
        }
    }
}
