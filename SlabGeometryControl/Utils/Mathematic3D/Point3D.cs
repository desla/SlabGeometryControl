using System;

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

        /// <summary>
        /// Считает расстояние от прямой aP - aQ до текущей точки.
        /// </summary>
        /// <param name="aP"></param>
        /// <param name="aQ"></param>        
        /// <returns></returns>
        public double DistanceToLine(Point3D aP, Point3D aQ)
        {
            // A, B, c - коэфициенты прямой.
            var a = aQ.X - aP.X;
            var b = aQ.Y - aP.Y;
            var c = aQ.Z - aP.Z;

            var x0 = aP.X;
            var y0 = aP.Y;
            var z0 = aP.Z;
            
            var numerator = Math.Pow(b * (X - x0) - a * (Y - y0), 2) +
                            Math.Pow(c * (Y - y0) - b * (Z - z0), 2) +
                            Math.Pow(c * (X - x0) - a * (Z - z0), 2);
            var distance = Math.Sqrt(numerator) / Math.Sqrt(a * a + b * b + c * c);

            return distance;
        }

        public override string ToString()
        {
            return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
        }
    }
}
