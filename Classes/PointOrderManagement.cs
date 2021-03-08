using System;
using System.Collections.Generic;
using System.Linq;

namespace OVFSliceViewer.Classes
{
    public class PointOrderManagement
    {
        public PointOrderManagement(int layernumber)
        {
            Layernumber = layernumber;
            _partVectorblockInfo = new List<PartVectorblockInfo>();
        }
        public int Layernumber { get; protected set; }
        List<PartVectorblockInfo> _partVectorblockInfo;

        public void AddVectorblockInfo(PartVectorblockInfo partVectorblockInfo)
        {
            _partVectorblockInfo.Add(partVectorblockInfo);
        }

        public Dictionary<int, PartDrawInfo> GetPointNumbersToDraw(List<int> partsToDraw, int numberOfPoints = Int32.MaxValue)
        {
            List<PartVectorblockInfo> temp = new List<PartVectorblockInfo>();
            if (partsToDraw == null)
            {
                partsToDraw = _partVectorblockInfo.Select(x => x.Partnumber).Distinct().ToList();
            }
            temp = _partVectorblockInfo.Where(x => partsToDraw.Contains(x.Partnumber)).ToList();

            Dictionary<int, PartDrawInfo> partDrawInfo = new Dictionary<int, PartDrawInfo>();

            //Initialize dictionary values
            foreach (var item in partsToDraw)
            {
                partDrawInfo[item] = new PartDrawInfo(item);
            }

            var totalNumberToDraw = 0;
            foreach (var item in temp)
            {
                bool doBreak = false;
                totalNumberToDraw += item.NumberOfPoints;
                int pointsToAdd = 0;

                if (totalNumberToDraw > numberOfPoints)
                {
                    totalNumberToDraw -= item.NumberOfPoints;
                    pointsToAdd = (numberOfPoints - totalNumberToDraw);

                    doBreak = true;
                }
                else pointsToAdd = item.NumberOfPoints;

                if (item.IsContour) partDrawInfo[item.Partnumber].AddContour(pointsToAdd);
                else partDrawInfo[item.Partnumber].AddHatch(pointsToAdd);

                if (doBreak) break;
            }

            return partDrawInfo;
        }


        /// <summary>
        /// Returns the number of points that can be drawn with the current choice of parts to draw. Used to set the slider max value.
        /// </summary>
        /// <param name="partsToDraw"></param>
        /// <returns></returns>
        public int GetMaxPoints(List<int> partsToDraw)
        {
            var temp = _partVectorblockInfo.Where(x => partsToDraw.Contains(x.Partnumber));
            return temp.Sum(x => x.NumberOfPoints);
        }
    }
}
