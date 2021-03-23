using OpenTK;
using OpenVectorFormat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenVectorFormat.AbstractReaderWriter;
using OpenVectorFormat.FileReaderWriterFactory;

namespace OVFSliceViewer.Classes
{
    public class JobViewer
    {
        FileReader _currentFile;
        FileHandlerProgress _progress;

        List<Vector2> _edges = new List<Vector2>();
        public Vector2 Center { get; protected set; }

        public int NumberOfWorkplanes { get; protected set; } = 0;
        public JobViewer(FileReader fileReader)
        {
            _currentFile = fileReader;
            NumberOfWorkplanes = _currentFile.JobShell.NumWorkPlanes - 1;
            GetBoundingBox();
        }

        public JobViewer(string filename)
        {
            _progress = new FileHandlerProgress();
            _currentFile = FileReaderFactory.CreateNewReader(Path.GetExtension(filename));
            _currentFile.OpenJobAsync(filename, _progress);

            NumberOfWorkplanes = _currentFile.JobShell.NumWorkPlanes - 1;
        }

        private void GetBoundingBox()
        {
            GetWorkplaneBoundingBox(0);
            GetWorkplaneBoundingBox(NumberOfWorkplanes/2);
            GetWorkplaneBoundingBox(NumberOfWorkplanes);

            var min = new Vector2(_edges.Min(x => x.X), _edges.Min(x => x.Y));
            var max = new Vector2(_edges.Max(x => x.X), _edges.Max(x => x.Y));

            foreach (var item in _edges)
            {
                Console.WriteLine(item.ToString());
            }

            Center = min + (max - min) * 0.5f;
        }
        private async void GetWorkplaneBoundingBox(int workplaneIndex)
        {
            WorkPlane workPlane = await _currentFile.GetWorkPlaneAsync(workplaneIndex);
            var temp = new List<float>();

            foreach (var vectorblock in workPlane.VectorBlocks)
            {
                switch (vectorblock.VectorDataCase)
                {
                    case VectorBlock.VectorDataOneofCase.None:
                        break;
                    case VectorBlock.VectorDataOneofCase.LineSequence:
                        temp.AddRange(vectorblock.LineSequence.Points);
                        break;
                    case VectorBlock.VectorDataOneofCase.Hatches:
                        temp.AddRange(vectorblock.Hatches.Points);
                        break;
                    case VectorBlock.VectorDataOneofCase.PointSequence:
                        temp.AddRange(vectorblock.PointSequence.Points);
                        break;
                    case VectorBlock.VectorDataOneofCase.LineSequenceParaAdapt:
                        temp.AddRange(vectorblock.LineSequenceParaAdapt.PointsWithParas.Where((x, index) => (index+1) % 3 != 0).Select(x => x).ToList());
                        break;
                    case VectorBlock.VectorDataOneofCase.HatchParaAdapt:
                        foreach (var hatch in vectorblock.HatchParaAdapt.HatchAsLinesequence)
                        {
                            temp.AddRange(hatch.PointsWithParas.Where((x, index) => (index + 1) % 3 != 0).Select(x => x).ToList());
                        }
                        break;
                    case VectorBlock.VectorDataOneofCase.LineSequence3D:
                        var linesequence3d = vectorblock.LineSequence3D.Points;
                        temp.AddRange(linesequence3d.Where((x, index) => (index + 1) % 3 != 0).Select(x => x).ToList());
                        break;
                    case VectorBlock.VectorDataOneofCase.Hatches3D:
                        var hatch3d = vectorblock.Hatches3D.Points;
                        temp.AddRange(hatch3d.Where((x, index) => (index + 1) % 3 != 0).Select(x => x).ToList());
                        break;
                    default:
                        break;
                }
            }

            var xValues = temp.Where((x, index) => (index) % 2 == 0).ToList();
            var yValues = temp.Where((x, index) => (index) % 2 != 0).ToList();

            _edges.Add(new Vector2(xValues.Min(), yValues.Min()));
            _edges.Add(new Vector2(xValues.Max(), yValues.Max()));
        }
    }
}
