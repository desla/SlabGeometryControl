using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Alvasoft.DataProvider.Impl.Emulator
{
    public interface ISensorCallback
    {
        void OnValueChanged(Sensor aSensor, SensorValue aValue);
    }

    public enum SensorType
    {
        /// <summary>
        /// Верхняя сторона слитка.
        /// </summary>
        TOP,
        /// <summary>
        /// Нижняя сторона слитка.
        /// </summary>
        BOTTOM,
        /// <summary>
        /// Левая сторона слитка.
        /// </summary>
        LEFT,
        /// <summary>
        /// Правая сторона слитка.
        /// </summary>
        RIGHT,
        /// <summary>
        /// Тип датчика положения.
        /// </summary>
        POSITION
    }

    public class SensorValue
    {
        /// <summary>
        /// Значение.
        /// </summary>
        public double Value { get; private set; }
        /// <summary>
        /// Время установки значения.
        /// </summary>
        public long Time { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="aValue">Значение.</param>
        /// <param name="aTime">Время.</param>
        public SensorValue(double aValue, long aTime)
        {
            Value = aValue;
            Time = aTime;
        }
    }

    public class SensorConfiguration
    {
        private const string NODE_TYPE = "side";
        private const string NODE_HORIZONT = "horizont";
        private const string NODE_REMOVE = "remove";
        private const string NODE_NAME = "name";
        private const string NODE_SHIFT = "shift";

        /// <summary>
        /// Определяет сторону слитка, подлежащую измерению.
        /// </summary>
        public SensorType SideType { get; private set; }

        /// <summary>
        /// Значение, по которому можно фильтровать измерение пустоты.
        /// </summary>
        public double Horizont { get; private set; }

        /// <summary>
        /// Определяет расстояние до нулевой точки.
        /// </summary>
        public double Remove { get; private set; }

        public string Name { get; private set; }

        public double Shift { get; private set; }

        public SensorConfiguration(XmlNode aNode)
        {
            if (aNode == null || aNode.ChildNodes == null) {
                throw new ArgumentNullException("aNode");
            }
            var items = aNode.ChildNodes;
            for (var i = 0; i < items.Count; ++i) {
                var item = items[i];
                switch (item.Name) {
                    case NODE_TYPE:
                        SideType = MakeSensorType(item.InnerText);
                        break;
                    case NODE_HORIZONT:
                        Horizont = Convert.ToDouble(item.InnerText);
                        break;
                    case NODE_REMOVE:
                        Remove = Convert.ToDouble(item.InnerText);
                        break;
                    case NODE_NAME:
                        Name = item.InnerText;
                        break;
                    case NODE_SHIFT:
                        Shift = Convert.ToDouble(item.InnerText);
                        break;
                }
            }
        }

        private SensorType MakeSensorType(string aType)
        {
            if (string.IsNullOrEmpty(aType)) {
                throw new ArgumentNullException("aType");
            }

            var lowerType = aType.ToLower();
            var typeNames = Enum.GetNames(typeof(SensorType));
            var typeValues = Enum.GetValues(typeof(SensorType));
            for (var i = 0; i < typeNames.Length; ++i) {
                if (string.Equals(typeNames[i].ToLower(), lowerType)) {
                    return (SensorType)typeValues.GetValue(i);
                }
            }

            throw new ArgumentException(aType + " не существует такого типа стороны для измерения.");
        }
    }

    public class Sensor
    {
        private SensorValue currentValue;
        private ISensorCallback callback;
        public SensorConfiguration Configuration { get; private set; }        

        public void SetCallback(ISensorCallback aCallback)
        {
            callback = aCallback;
        }

        public void SetConfiguration(SensorConfiguration aConfiguration)
        {
            Configuration = aConfiguration;
        }
    }
}
