using System;
using System.Drawing;

namespace Alvasoft.Utils.Mathematic3D
{
    public class Math3D
    {        
        public class Camera
        {
            public Point3D Position = new Point3D();
        }

        public static Point3D RotateX(Point3D point3D, double degrees)
        {
            //[ a  b  c ] [ x ]   [ x*a + y*b + z*c ]
            //[ d  e  f ] [ y ] = [ x*d + y*e + z*f ]
            //[ g  h  i ] [ z ]   [ x*g + y*h + z*i ]

            //[ 1    0        0   ]
            //[ 0   cos(x)  sin(x)]
            //[ 0   -sin(x) cos(x)]

            double cDegrees = (Math.PI * degrees) / 180.0f;
            double cosDegrees = Math.Cos(cDegrees);
            double sinDegrees = Math.Sin(cDegrees);

            double y = (point3D.Y * cosDegrees) + (point3D.Z * sinDegrees);
            double z = (point3D.Y * -sinDegrees) + (point3D.Z * cosDegrees);

            return new Point3D(point3D.X, y, z);
        }

        public static Point3D RotateY(Point3D point3D, double degrees)
        {
            //Y-axis

            //[ cos(x)   0    sin(x)]
            //[   0      1      0   ]
            //[-sin(x)   0    cos(x)]

            double cDegrees = (Math.PI * degrees) / 180.0;
            double cosDegrees = Math.Cos(cDegrees);
            double sinDegrees = Math.Sin(cDegrees);

            double x = (point3D.X * cosDegrees) + (point3D.Z * sinDegrees);
            double z = (point3D.X * -sinDegrees) + (point3D.Z * cosDegrees);

            return new Point3D(x, point3D.Y, z);
        }

        public static Point3D RotateZ(Point3D point3D, double degrees)
        {
            //Z-axis

            //[ cos(x)  sin(x) 0]
            //[ -sin(x) cos(x) 0]
            //[    0     0     1]

            double cDegrees = (Math.PI * degrees) / 180.0;
            double cosDegrees = Math.Cos(cDegrees);
            double sinDegrees = Math.Sin(cDegrees);

            double x = (point3D.X * cosDegrees) + (point3D.Y * sinDegrees);
            double y = (point3D.X * -sinDegrees) + (point3D.Y * cosDegrees);

            return new Point3D(x, y, point3D.Z);
        }

        public static Point3D[] RotateX(Point3D[] points3D, double degrees)
        {
            for (int i = 0; i < points3D.Length; i++) {
                points3D[i] = RotateX(points3D[i], degrees);
            }
            return points3D;
        }

        public static Point3D[] RotateY(Point3D[] points3D, double degrees)
        {
            for (int i = 0; i < points3D.Length; i++) {
                points3D[i] = RotateY(points3D[i], degrees);
            }
            return points3D;
        }

        public static Point3D[] RotateZ(Point3D[] points3D, double degrees)
        {
            for (int i = 0; i < points3D.Length; i++) {
                points3D[i] = RotateZ(points3D[i], degrees);
            }
            return points3D;
        }

        public static PointF Convert3DTo2D(Point3D aPoint, Camera aCamera, double aZoom, PointF aSenter)
        {
            var resPoint = new PointF();
            if (aPoint.Z >= aCamera.Position.Z) {
                resPoint.X = (float)((aPoint.X - aCamera.Position.X) / (-0.1f) * aZoom) + aSenter.X;
                resPoint.Y = (float)(-(aPoint.Y - aCamera.Position.Y) / (-0.1f) * aZoom) + aSenter.Y;
            }
            else {
                resPoint.X = (float)(-(aPoint.X - aCamera.Position.X) /
                    (aPoint.Z - aCamera.Position.Z) * aZoom + aSenter.X);
                resPoint.Y = (float)((aPoint.Y - aCamera.Position.Y) /
                    (aPoint.Z - aCamera.Position.Z) * aZoom + aSenter.Y);
            }

            return resPoint;
        }
    }
}
