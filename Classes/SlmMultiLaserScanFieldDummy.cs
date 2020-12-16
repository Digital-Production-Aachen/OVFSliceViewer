using modularEmulator.FileReader.SLMFileReaderAdapter.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OVFSliceViewer
{
    public class SlmMultiLaserScanFieldDummy : ISlmMultiLaserScanFieldSettings
    {
        public SlmMultiLaserScanFieldDummy()
        {
            OverlapPosition = new List<double>() { 0 };
            OverlapContourSize = new List<double>() { 0 };
            OverlapHatchSize = new List<double>() { 0 };
        }
        public IList<double> OverlapPosition { get; set; }

        public IList<double> OverlapContourSize { get; set; }

        public IList<double> OverlapHatchSize { get; set; }

        public double MinimalHatchLength { get; set; } = 0;

        public bool OptimizationActive { get; set; } = false;

        public int NumOptics { get; set; } = 0;

        public double BuildEnvelopeDepthMM { get; set; } = 0;

        public double MinZDistMm { get; set; } = 0;

        public int MinLayerHeightUM { get; set; } = 0;
    }
}
