using System.Collections.Generic;

namespace OVFSliceViewer.Classes
{
    public struct PartVectorblockInfo
    {
        public bool IsContour;
        public int Partnumber;
        public int NumberOfPoints;
    }

    public class PartDrawInfo
    {
        public PartDrawInfo(int partnumber)
        {
            Partnumber = partnumber;
        }

        public int Partnumber { get; set; }
        public int HatchNumberOfPoints { get; protected set; }
        public int ContourNumberOfPoints { get; protected set; }

        public void AddHatch(int numberOfPoints)
        {
            HatchNumberOfPoints += numberOfPoints;
        }
        public void AddContour(int numberOfPoints)
        {
            ContourNumberOfPoints += numberOfPoints;
        }

    }
}
