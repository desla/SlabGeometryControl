namespace REmulatorConfiguration {
    using System.IO;
    using System.Xml.Serialization;

    public class EmulatorConfiguration {
        /// <summary>
        /// Длина слитка, мм.
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Диаметр слитка, мм.
        /// </summary>
        public double Diameter { get; set; }

        /// <summary>
        /// Скорость движения, мм/с.
        /// </summary>
        public double Speed { get; set; }        

        /// <summary>
        /// Шаг подпрыгивания.
        /// </summary>
        public BumpConfiguration[] Bumps { get; set; }

        /// <summary>
        /// Предел дребезжания.
        /// </summary>
        public double RattleLimit { get; set; }

        /// <summary>
        /// Искривления.
        /// </summary>
        public FlexConfiguration[] Flexs { get; set; }

        /// <summary>
        /// Описание рамки.
        /// </summary>
        public FrameConfiguration Frame { get; set; }

        public void Serialize(string aXmlFile) {
            using (var stream = new StreamWriter(aXmlFile)) {
                var file = new XmlSerializer(typeof(EmulatorConfiguration));
                file.Serialize(stream, this);
            }
        }

        public static EmulatorConfiguration Deserialize(string aXmlFile) {
            using (var stream = new StreamReader(aXmlFile)) {
                var file = new XmlSerializer(typeof(EmulatorConfiguration));
                return (EmulatorConfiguration)file.Deserialize(stream);
            }
        }
    }
}
