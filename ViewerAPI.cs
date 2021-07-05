using OVFSliceViewer.Classes;
using OpenTK;
using OpenVectorFormat;
using OpenVectorFormat.AbstractReaderWriter;
using System;
using System.IO;
using System.Linq;
using OpenVectorFormat.FileReaderWriterFactory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OVFSliceViewer
{
    public class ViewerAPI
    {
        Painter _painter;
        FileReader _currentFile;
        public int CurrentLayer { get; set; }
        public int NumberOfLayer { get; private set; }
        public int MaxNumberOfLines { get; private set; }
        public int NumberOfLines { get; private set; }
        public void SetNumberOfLines(int number) { NumberOfLines = Math.Min(MaxNumberOfLines, number); }

        public bool DrawThreeD { get; set; }
        public bool ShowGrid { get; set; } = true;

        public ViewerAPI(Painter painter)
        {
            _painter = painter;
        }

        public async Task DisplayWorkplane(WorkPlane workplane)
        {

        }

        public async Task LoadJob(FileReader fileReader, string filePath)
        {
            if (fileReader.CacheState == CacheState.NotCached)
            {
                var task = fileReader.CacheJobToMemoryAsync();

                task.Wait();
            }
            _currentFile = fileReader;
            NumberOfLayer = _currentFile.JobShell.NumWorkPlanes - 1;
            CurrentLayer = 0;
            LoadPartNames();
        }
        public async Task LoadJob(string filename)
        {
            if (_currentFile != null)
            {
                _currentFile.CloseFile();
                _currentFile.Dispose();
            }
            _currentFile = FileReaderFactory.CreateNewReader(Path.GetExtension(filename));
            var command = new FileHandlerProgress();

            await _currentFile.OpenJobAsync(filename, command);
            //SetDefaultLpbfMetaData();
            _painter.DrawableParts = new Dictionary<int, DrawablePart>();
            var shader = _painter.Shader;
            int i = 1;
            if (_currentFile.JobShell.PartsMap == null || _currentFile.JobShell.PartsMap.Count == 0)
            {
                //no parts in MetaData, add dummy part 0
                _painter.DrawableParts.Add(0, new DrawablePart(shader, _currentFile.JobShell.NumWorkPlanes, 0));
                _currentFile.JobShell.PartsMap.Add(0, new Part() { Name = "no part" });
            }
            else
            {
                foreach (var item in _currentFile.JobShell.PartsMap.Keys)
                {
                    _painter.DrawableParts.Add(item, new DrawablePart(shader, _currentFile.JobShell.NumWorkPlanes, item));
                    i += 2;
                }
            }

            NumberOfLayer = _currentFile.JobShell.NumWorkPlanes - 1;
            _painter.Camera.MoveToPosition2D(new Vector2(0, 0));
            LoadPartNames();
            LoadContours();
        }

        public List<string> LoadPartNames()
        {
            var list = new List<string>();
            if (_currentFile != null && _currentFile.CacheState != CacheState.NotCached)
            {
                foreach (var part in _currentFile.JobShell.PartsMap.Values)
                {
                    list.Add(part.Name);
                }
            }
            return list;
        }

        float _minPower = 0;
        float _maxPower = 1;
        private async void LoadContours()
        {
            if (_currentFile == null)
            {
                return;
            }

            for (int j = 0; j < _currentFile.JobShell.NumWorkPlanes; j++)
            {
                var workplane = await _currentFile.GetWorkPlaneAsync(j);

                LoadWorkplaneContour(j, workplane);
            }
            _painter.DrawableParts.ToList().ForEach(x => { x.Value.UpdateContour(); x.Value.VectorFactory.SetPowerLevels(_minPower, _maxPower); });
        }

        private async void LoadWorkplaneContour(int index, WorkPlane workplane)
        {
            var pointOrderManagement = new PointOrderManagement(index);
            if (workplane.MetaData != null && workplane.MetaData.MaxPower != 0 && workplane.MetaData.MinPower != 0)
            {
                if (_minPower > (workplane.MetaData.MinPower))
                {
                    _minPower = workplane.MetaData.MinPower;
                }
                if (_maxPower < workplane.MetaData.MaxPower)
                {
                    _maxPower = workplane.MetaData.MaxPower;
                }
            }

            var blocks = workplane.VectorBlocks;
            var numBlocks = blocks.Count();
            var numberOfPoints = 0;

            for (int i = 0; i < numBlocks; i++)
            {
                bool isContour = blocks[i].LpbfMetadata != null ? blocks[i].LpbfMetadata.PartArea == VectorBlock.Types.PartArea.Contour : true;
                if (blocks[i].VectorDataCase == VectorBlock.VectorDataOneofCase.LineSequence)
                {
                    numberOfPoints = (blocks[i].LineSequence.Points.Count / 2 - 1);
                }
                else if (blocks[i].VectorDataCase == VectorBlock.VectorDataOneofCase.Hatches)
                {
                    numberOfPoints = blocks[i].Hatches.Points.Count / 2;
                }
                else if (blocks[i].VectorDataCase == VectorBlock.VectorDataOneofCase.HatchParaAdapt)
                {
                    numberOfPoints = blocks[i].HatchParaAdapt.HatchAsLinesequence.Sum(x => x.PointsWithParas.Count / 3 * 2 - 1);
                }
                else if (blocks[i].VectorDataCase == VectorBlock.VectorDataOneofCase.LineSequenceParaAdapt)
                {
                    numberOfPoints = blocks[i].LineSequenceParaAdapt.PointsWithParas.Count / 3 * 2 - 1;
                }
                var newInfo = new PartVectorblockInfo() { Partnumber = blocks[i].MetaData != null ? blocks[i].MetaData.PartKey : 0, NumberOfPoints = numberOfPoints, IsContour = isContour };
                pointOrderManagement.AddVectorblockInfo(newInfo);

                if (blocks[i].LpbfMetadata == null || blocks[i].LpbfMetadata.PartArea == VectorBlock.Types.PartArea.Contour)
                {
                    _painter.DrawableParts[newInfo.Partnumber].AddContour(blocks[i], workplane.WorkPlaneNumber, workplane.ZPosInMm);
                }
            }
            _painter.LayerPointManager[index] = pointOrderManagement;
        }
        public async void Draw()
        {
            _painter.Is3d = DrawThreeD;
            _painter.ShowGrid = ShowGrid;
            _painter.Draw(CurrentLayer);
        }
        public async Task DrawWorkplane(int layernumber)
        {
            CurrentLayer = layernumber;
            int fromLayer;

            fromLayer = DrawThreeD ? 0 : CurrentLayer;
            _painter.DrawableParts.ToList().ForEach(x => { x.Value.RemoveVolume(); x.Value.SetContourRangeToDraw3d(CurrentLayer, fromLayer); });
            if (DrawThreeD)
            {
                _painter.Draw(CurrentLayer);
            }


            for (int j = CurrentLayer; j < CurrentLayer + 1; j++)
            {
                if (_currentFile != null)
                {
                    var workplane = await _currentFile.GetWorkPlaneAsync(j);

                    var blocks = workplane.VectorBlocks;
                    var numBlocks = blocks.Count();

                    for (int i = 0; i < numBlocks; i++)
                    {
                        if (blocks[i].LpbfMetadata == null)
                        {
                            continue;
                        }
                        if (blocks[i].LpbfMetadata.PartArea == VectorBlock.Types.PartArea.Contour || j == CurrentLayer)
                        {
                            _painter.DrawableParts[blocks[i].MetaData.PartKey].AddVectorBlock(blocks[i], workplane.WorkPlaneNumber, workplane.ZPosInMm);
                            //mapper.CalculateVectorBlock(blocks[i], workplane.ZPosInMm);
                        }
                    }
                }
            }
            //GC.Collect();
            MaxNumberOfLines = _painter.LayerPointManager[CurrentLayer].GetPointNumbersToDraw(null).Sum(x => x.Value.HatchNumberOfPoints + x.Value.ContourNumberOfPoints);
            NumberOfLines = MaxNumberOfLines;
            if (DrawThreeD)
            {
                _painter.DrawableParts.ToList().ForEach(x => { x.Value.SetContourRangeToDraw3d(CurrentLayer, fromLayer); x.Value.UpdateVolume(); });
            }
            else
            {
                _painter.DrawableParts.ToList().ForEach(x => { x.Value.UpdateVolume(); });
            }
            _painter.SetNumberOfLinesToDraw(NumberOfLines);
            Draw();
        }

        public void Zoom(int amount)
        {
            if (amount > 0)
            {
                _painter.Camera.Zoom(true);
            }
            else
            {
                _painter.Camera.Zoom(false);
            }
            Draw();
        }
        public void Move(Vector2 move)
        {
            _painter.Camera.Move(move);
            Draw();
        }
        public void Rotate(Vector2 rotation)
        {
            _painter.Camera.Rotate(rotation);
            Draw();
        }

        public void CenterView()
        {
            _painter.TargetCenter();
        }

        public void SetHighlightColors(int highlightIndex)
        {
            _painter.SetHighlightColors(highlightIndex);
        }

        public void MoveToPosition2D(Vector2 position)
        {
            _painter.Camera.MoveToPosition2D(position);

        }

        public void Dispose()
        {
            _painter.DisposeShader();
        }
    }

}
