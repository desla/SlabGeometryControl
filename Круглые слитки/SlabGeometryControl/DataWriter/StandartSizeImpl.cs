namespace Alvasoft.DataWriter
{
    public class StandartSizeImpl : IStandartSize
    {
        public int Id { get; set; }
        public double Diameter { get; set; }

        public int GetId()
        {
            return Id;
        }        

        public double GetDiameter()
        {
            return Diameter;
        }
    }
}
