namespace Alvasoft.DataWriter
{
    public class StandartSizeImpl : IStandartSize
    {
        public int Id { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }

        public int GetId()
        {
            return Id;
        }

        public double GetHeight()
        {
            return Height;
        }

        public double GetWidth()
        {
            return Width;
        }

        public double GetLength()
        {
            return Length;
        }
    }
}
