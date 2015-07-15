using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Alvasoft.DataProvider.Impl.Emulator
{
    public enum SideType
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT
    }

    public class IngotSide
    {
        private const string NODE_TYPE = "type";
        private const string NODE_SAG = "sag";

        public SideType Position { get; private set; }
        /// <summary>
        /// Прогиб поверхности по центру.
        /// </summary>
        public double Sag { get; private set; }

        public void LoadFromXmlNode(XmlNode aNode)
        {
            var items = aNode.ChildNodes;
            for (var i = 0; i < items.Count; ++i) {
                var item = items[i];
                switch (item.Name) {
                    case NODE_TYPE:
                        Position = CreatePosition(item.InnerText);
                        break;
                    case NODE_SAG:
                        Sag = Convert.ToDouble(item.InnerText);
                        break;
                }
            }
        }

        private SideType CreatePosition(string aType)
        {
            var names = Enum.GetNames(typeof(SideType));
            var values = Enum.GetValues(typeof(SideType));
            for (var i = 0; i < names.Length; ++i) {
                if (names[i].ToLower().Equals(aType.ToLower())) {
                    return (SideType)values.GetValue(i);
                }
            }

            throw new ArgumentException("Нет такого типа стороны: " + aType);
        }
    }

    public class Ingot
    {
        private const string NODE_WIDTH = "width";
        private const string NODE_HEIGHT = "height";
        private const string NODE_LENGTH = "length";
        private const string NODE_SIDES = "sides";
        private const string NODE_SEEDINESS = "seedines";

        public IngotSide[] Sides { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public double Length { get; private set; }
        /// <summary>
        /// Шероховатость.
        /// </summary>
        public double Seediness { get; private set; }

        public void LoadFromXmlNode(XmlNode aNode)
        {
            var items = aNode.ChildNodes;
            for (var i = 0; i < items.Count; ++i) {
                var item = items[i];
                switch (item.Name) {
                    case NODE_HEIGHT:
                        Height = Convert.ToDouble(item.InnerText);
                        break;
                    case NODE_WIDTH:
                        Width = Convert.ToDouble(item.InnerText);
                        break;
                    case NODE_LENGTH:
                        Length = Convert.ToDouble(item.InnerText);
                        break;
                    case NODE_SEEDINESS:
                        Seediness = Convert.ToDouble(item.InnerText);
                        break;
                    case NODE_SIDES:
                        Sides = new IngotSide[item.ChildNodes.Count];
                        for (var j = 0; j < item.ChildNodes.Count; ++j) {
                            Sides[j] = new IngotSide();
                            Sides[j].LoadFromXmlNode(item.ChildNodes[j]);
                        }
                        break;
                }
            }
        }
    }
}
