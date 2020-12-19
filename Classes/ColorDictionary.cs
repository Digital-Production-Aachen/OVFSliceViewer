using OpenTK;
using OpenVectorFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OVFSliceViewer.Classes
{
    public class ColorDictionary
    {
        readonly Dictionary<VectorBlock.Types.PartArea, Vector4> _contourColors;
        readonly Dictionary<VectorBlock.Types.LPBFMetadata.Types.SkinType, Vector4> _colorsSkinType;
        readonly Dictionary<VectorBlock.Types.StructureType, Vector4> _structureColors;

        public ColorDictionary()
        {
            _rWTHColors = new List<Vector4>()
            {
                new Vector4(0f, 84f/255f, 159f/255f, 0f),
                new Vector4(227f / 255f, 0f, 102f / 255f, 0f),
                new Vector4(1f, 237f / 255f, 0f, 0f),
                new Vector4(0f, 97f / 255f, 101f / 255f, 0f),
                new Vector4(0f, 152f / 255f, 161f / 255f, 0f),
                new Vector4(87f / 255f, 171f / 255f, 39f / 255f, 0f),
                new Vector4(189f / 255f, 205f / 255f, 0f, 0f),
                new Vector4(246f / 255f, 168f / 255f, 0f, 0f),
                new Vector4(204f / 255f, 7f / 255f, 30f / 255f, 0f),
                new Vector4(216f / 255f, 92f / 255f, 65f / 255f, 0f),
                new Vector4(182f / 255f, 82f / 255f, 86f / 255f, 0f),
                new Vector4(131f / 255f, 78f / 255f, 117f / 255f, 0f)
            };

            _contourColors = new Dictionary<VectorBlock.Types.PartArea, Vector4>()
            {
                { VectorBlock.Types.PartArea.Contour, _rWTHColors[5] },
                { VectorBlock.Types.PartArea.TransitionContour, _rWTHColors[2] },
                { VectorBlock.Types.PartArea.Volume, new Vector4(1f,0f,0f,0f) }
            };

            _colorsSkinType = new Dictionary<VectorBlock.Types.LPBFMetadata.Types.SkinType, Vector4>()
            {
                { VectorBlock.Types.LPBFMetadata.Types.SkinType.DownSkin, _rWTHColors[3] },
                { VectorBlock.Types.LPBFMetadata.Types.SkinType.InSkin, _rWTHColors[4] },
                { VectorBlock.Types.LPBFMetadata.Types.SkinType.UpSkin, _rWTHColors[6] }
            };

            _structureColors = new Dictionary<VectorBlock.Types.StructureType, Vector4>()
            {
                { VectorBlock.Types.StructureType.Support, _rWTHColors[7] },
                { VectorBlock.Types.StructureType.Points, _rWTHColors[8] },
                { VectorBlock.Types.StructureType.Part, _rWTHColors[9] },
                { VectorBlock.Types.StructureType.Wirestructure, _rWTHColors[10] }
            };
        }

        public Vector4 TryGetColor(VectorBlock.Types.PartArea type)
        {
            Vector4 color;
            _contourColors.TryGetValue(type, out color);

            if (color == null)
            {
                color = new Vector4(1f, 0f, 0f, 0f);
            }

            return color;
        }

        public Vector4 TryGetColor(VectorBlock.Types.LPBFMetadata.Types.SkinType type)
        {
            Vector4 color;
            _colorsSkinType.TryGetValue(type, out color);

            if (color == null)
            {
                color = new Vector4(1f, 0f, 0f, 0f);
            }

            return color;
        }

        public Vector4 TryGetColor(VectorBlock.Types.StructureType type)
        {
            Vector4 color;
            _structureColors.TryGetValue(type, out color);

            if (color == null)
            {
                color = new Vector4(1f, 0f, 0f, 0f);
            }
            return color;
        }

        readonly List<Vector4> _rWTHColors;



        //new Vector4(168, 133, 158),
        //new Vector4(210, 192, 205),
        //new Vector4(237, 229, 234),
        //new Vector4(122, 111, 172),
        //new Vector4(155, 145, 193),
        //new Vector4(188, 181, 215),
        //new Vector4(222, 218, 235),
        //new Vector4(242, 240, 247)};

    }
}
