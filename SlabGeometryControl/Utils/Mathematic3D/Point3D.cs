namespace Alvasoft.Utils.Mathematic3D
{
    public class Point3D
    {
        public double X;
        public double Y;
        public double Z;

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3D(float x, float y, float z)
        {
            X = (double)x;
            Y = (double)y;
            Z = (double)z;
        }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3D()
        {
        }

        public override string ToString()
        {
            return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
        }
    }
}
