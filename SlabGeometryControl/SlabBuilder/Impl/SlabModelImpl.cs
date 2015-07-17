using System;
using System.Collections.Generic;
using System.Linq;
using Alvasoft.SlabGeometryControl;
using Alvasoft.Utils.Mathematic3D;
using NHibernate.Mapping;

namespace Alvasoft.SlabBuilder.Impl
{
    public class SlabModelImpl : ISlabModel
    {
        public double TopLimit { get; set; }
        public double BottomLimit { get; set; }
        public double LeftLimit { get; set; }
        public double RightLimit { get; set; }
        public double LengthLimit { get; set; }

        /// <summary>
        /// Точки должны быть отсортированы по возрастанию координаты Z,
        /// по возрастанию координаты X.
        /// </summary>
        public Point3D[][] TopLines { get; set; }
        public Point3D[][] BottomLines { get; set; }
        public Point3D[][] LeftLines { get; set; }
        public Point3D[][] RightLines { get; set; }
        
        public double GetTopLimit()
        {
            return TopLimit;
        }

        public double GetBottomLimit()
        {
            return BottomLimit;
        }

        public double GetLeftLimit()
        {
            return LeftLimit;
        }

        public double GetRightLimit()
        {
            return RightLimit;
        }

        public double GetLengthLimit()
        {
            return LengthLimit;
        }

        public Point3D GetTopSidePoint(double aX, double aZ)
        {
            if (aX < GetLeftLimit() || aX > GetRightLimit() ||
                aZ < 0 || aZ > GetLengthLimit()) {
                throw new ArgumentException("Точка выходит за пределы слитка.");
            }            

            if (aX <= TopLines[0][0].X) {
                var leftPointIndex = GetLowerBoundPoint(TopLines[0], aZ);
                var rightPointIndex = TopLines[0].Length - 1;
                if (leftPointIndex != TopLines[0].Length - 1) {
                    rightPointIndex = leftPointIndex + 1;
                }
                var leftPoint = TopLines[0][leftPointIndex];
                var rightPoint = TopLines[0][rightPointIndex];
                var difference = 0.0;
                if (rightPoint != leftPoint) {
                    difference = (aZ - leftPoint.Z) / (rightPoint.Z - leftPoint.Z);
                }                
                return new Point3D {
                    X = aX, 
                    Y = leftPoint.Y + difference * (rightPoint.Y - leftPoint.Y),
                    Z = aZ
                };
            } 
            else if (aX >= TopLines[TopLines.Length - 1][0].X) {
                var leftPointIndex = GetLowerBoundPoint(TopLines[TopLines.Length - 1], aZ);
                var rightPointIndex = TopLines[TopLines.Length - 1].Length - 1;
                if (leftPointIndex != TopLines[TopLines.Length - 1].Length - 1) {
                    rightPointIndex = leftPointIndex + 1;
                }
                var leftPoint = TopLines[TopLines.Length - 1][leftPointIndex];
                var rightPoint = TopLines[TopLines.Length - 1][rightPointIndex];
                var difference = 0.0;
                if (rightPoint != leftPoint) {
                    difference = (aZ - leftPoint.Z) / (rightPoint.Z - leftPoint.Z);
                }   
                return new Point3D {
                    X = aX,
                    Y = leftPoint.Y + difference*(rightPoint.Y - leftPoint.Y),
                    Z = aZ
                };
            }
            else {
                var leftLineIndex = GetLowerBoundLineByX(TopLines, aX);
                var leftLine = TopLines[leftLineIndex];                
                var leftPointIndex = GetLowerBoundPoint(leftLine, aZ);
                var rightPointIndex = leftLine.Length - 1;
                if (leftPointIndex != rightPointIndex) {
                    rightPointIndex = leftPointIndex + 1;
                }
                var leftPointLeftLine = leftLine[leftPointIndex];
                var rightPointLeftLine = leftLine[rightPointIndex];
                var difference = 0.0;
                if (rightPointLeftLine != leftPointLeftLine) {
                    difference = (aZ - leftPointLeftLine.Z) / (rightPointLeftLine.Z - leftPointLeftLine.Z);
                }
                var leftY = leftPointLeftLine.Y + difference*(rightPointLeftLine.Y - leftPointLeftLine.Y);

                var rightLine = TopLines[leftLineIndex + 1];
                var leftPointRightLine = rightLine[leftPointIndex];
                var rightPointRightLine = rightLine[rightPointIndex];
                difference = 0.0;
                if (rightPointRightLine != leftPointRightLine) {
                    difference = (aZ - leftPointRightLine.Z) / (rightPointRightLine.Z - leftPointRightLine.Z);
                }
                var rightY = leftPointRightLine.Y + difference*(rightPointRightLine.Y - leftPointRightLine.Y);

                difference = (aX - rightPointLeftLine.X)/(rightPointRightLine.X - rightPointLeftLine.X);
                return new Point3D {
                    X = aX,
                    Y = leftY + difference*(rightY - leftY),
                    Z = aZ
                };
            }            
        }        

        public Point3D GetBottomSidePoint(double aX, double aZ)
        {
            if (aX < GetLeftLimit() || aX > GetRightLimit() ||
                aZ < 0 || aZ > GetLengthLimit()) {
                throw new ArgumentException("Точка выходит за пределы слитка.");
            }

            if (aX <= BottomLines[0][0].X) {
                var leftPointIndex = GetLowerBoundPoint(BottomLines[0], aZ);
                var rightPointIndex = BottomLines[0].Length - 1;
                if (leftPointIndex != BottomLines[0].Length - 1) {
                    rightPointIndex = leftPointIndex + 1;
                }
                var leftPoint = BottomLines[0][leftPointIndex];
                var rightPoint = BottomLines[0][rightPointIndex];
                var difference = 0.0;
                if (rightPoint != leftPoint) {
                    difference = (aZ - leftPoint.Z) / (rightPoint.Z - leftPoint.Z);
                }
                return new Point3D {
                    X = aX,
                    Y = leftPoint.Y + difference * (rightPoint.Y - leftPoint.Y),
                    Z = aZ
                };
            }
            else if (aX >= BottomLines[BottomLines.Length - 1][0].X) {
                var leftPointIndex = GetLowerBoundPoint(BottomLines[BottomLines.Length - 1], aZ);
                var rightPointIndex = BottomLines[BottomLines.Length - 1].Length - 1;
                if (leftPointIndex != BottomLines[BottomLines.Length - 1].Length - 1) {
                    rightPointIndex = leftPointIndex + 1;
                }
                var leftPoint = BottomLines[BottomLines.Length - 1][leftPointIndex];
                var rightPoint = BottomLines[BottomLines.Length - 1][rightPointIndex];
                var difference = 0.0;
                if (rightPoint != leftPoint) {
                    difference = (aZ - leftPoint.Z) / (rightPoint.Z - leftPoint.Z);
                }
                return new Point3D {
                    X = aX,
                    Y = leftPoint.Y + difference * (rightPoint.Y - leftPoint.Y),
                    Z = aZ
                };
            }
            else {
                var leftLineIndex = GetLowerBoundLineByX(BottomLines, aX);
                var leftLine = BottomLines[leftLineIndex];
                var leftPointIndex = GetLowerBoundPoint(leftLine, aZ);
                var rightPointIndex = leftLine.Length - 1;
                if (leftPointIndex != rightPointIndex) {
                    rightPointIndex = leftPointIndex + 1;
                }
                var leftPointLeftLine = leftLine[leftPointIndex];
                var rightPointLeftLine = leftLine[rightPointIndex];
                var difference = 0.0;
                if (rightPointLeftLine != leftPointLeftLine) {
                    difference = (aZ - leftPointLeftLine.Z) / (rightPointLeftLine.Z - leftPointLeftLine.Z);
                }
                var leftY = leftPointLeftLine.Y + difference * (rightPointLeftLine.Y - leftPointLeftLine.Y);

                var rightLine = BottomLines[leftLineIndex + 1];
                var leftPointRightLine = rightLine[leftPointIndex];
                var rightPointRightLine = rightLine[rightPointIndex];
                difference = 0.0;
                if (rightPointRightLine != leftPointRightLine) {
                    difference = (aZ - leftPointRightLine.Z) / (rightPointRightLine.Z - leftPointRightLine.Z);
                }
                var rightY = leftPointRightLine.Y + difference * (rightPointRightLine.Y - leftPointRightLine.Y);

                difference = (aX - rightPointLeftLine.X) / (rightPointRightLine.X - rightPointLeftLine.X);
                return new Point3D {
                    X = aX,
                    Y = leftY + difference * (rightY - leftY),
                    Z = aZ
                };
            }
        }

        public Point3D GetLeftSidePoint(double aY, double aZ)
        {
            if (aY < GetBottomLimit() || aY > GetTopLimit() ||
                aZ < 0 || aZ > GetLengthLimit()) {
                throw new ArgumentException("Точка выходит за пределы слитка.");
            }

            if (aY <= LeftLines[0][0].Y) {
                var leftPointIndex = GetLowerBoundPoint(LeftLines[0], aZ);
                var rightPointIndex = LeftLines[0].Length - 1;
                if (leftPointIndex != LeftLines[0].Length - 1) {
                    rightPointIndex = leftPointIndex + 1;
                }
                var leftPoint = LeftLines[0][leftPointIndex];
                var rightPoint = LeftLines[0][rightPointIndex];
                var difference = 0.0;
                if (rightPoint != leftPoint) {
                    difference = (aZ - leftPoint.Z) / (rightPoint.Z - leftPoint.Z);
                }
                return new Point3D {
                    X = leftPoint.X + difference * (rightPoint.X - leftPoint.X),
                    Y = aY,
                    Z = aZ
                };
            }
            else if (aY >= LeftLines[LeftLines.Length - 1][0].Y) {
                var leftPointIndex = GetLowerBoundPoint(LeftLines[LeftLines.Length - 1], aZ);
                var rightPointIndex = LeftLines[LeftLines.Length - 1].Length - 1;
                if (leftPointIndex != LeftLines[LeftLines.Length - 1].Length - 1) {
                    rightPointIndex = leftPointIndex + 1;
                }
                var leftPoint = LeftLines[LeftLines.Length - 1][leftPointIndex];
                var rightPoint = LeftLines[LeftLines.Length - 1][rightPointIndex];
                var difference = 0.0;
                if (rightPoint != leftPoint) {
                    difference = (aZ - leftPoint.Z) / (rightPoint.Z - leftPoint.Z);
                }
                return new Point3D {
                    X = leftPoint.X + difference * (rightPoint.X - leftPoint.X),
                    Y = aY,
                    Z = aZ
                };
            }
            else {
                var leftLineIndex = GetLowerBoundLineByY(LeftLines, aY);
                var leftLine = LeftLines[leftLineIndex];
                var leftPointIndex = GetLowerBoundPoint(leftLine, aZ);
                var rightPointIndex = leftLine.Length - 1;
                if (leftPointIndex != rightPointIndex) {
                    rightPointIndex = leftPointIndex + 1;
                }
                var leftPointLeftLine = leftLine[leftPointIndex];
                var rightPointLeftLine = leftLine[rightPointIndex];
                var difference = 0.0;
                if (rightPointLeftLine != leftPointLeftLine) {
                    difference = (aZ - leftPointLeftLine.Z) / (rightPointLeftLine.Z - leftPointLeftLine.Z);
                }
                var leftX = leftPointLeftLine.X + difference * (rightPointLeftLine.X - leftPointLeftLine.X);

                var rightLine = LeftLines[leftLineIndex + 1];
                var leftPointRightLine = rightLine[leftPointIndex];
                var rightPointRightLine = rightLine[rightPointIndex];
                difference = 0.0;
                if (rightPointRightLine != leftPointRightLine) {
                    difference = (aZ - leftPointRightLine.Z) / (rightPointRightLine.Z - leftPointRightLine.Z);
                }
                var rightX = leftPointRightLine.X + difference * (rightPointRightLine.X - leftPointRightLine.X);

                difference = (aY - rightPointLeftLine.Y) / (rightPointRightLine.Y - rightPointLeftLine.Y);
                return new Point3D {
                    X = leftX + difference * (rightX - leftX),
                    Y = aY,
                    Z = aZ
                };
            }
        }

        public Point3D GetRightSidePoint(double aY, double aZ)
        {
            if (aY < GetBottomLimit() || aY > GetTopLimit() ||
                aZ < 0 || aZ > GetLengthLimit()) {
                throw new ArgumentException("Точка выходит за пределы слитка.");
            }

            if (aY <= RightLines[0][0].Y) {
                var leftPointIndex = GetLowerBoundPoint(RightLines[0], aZ);
                var rightPointIndex = RightLines[0].Length - 1;
                if (leftPointIndex != RightLines[0].Length - 1) {
                    rightPointIndex = leftPointIndex + 1;
                }
                var leftPoint = RightLines[0][leftPointIndex];
                var rightPoint = RightLines[0][rightPointIndex];
                var difference = 0.0;
                if (rightPoint != leftPoint) {
                    difference = (aZ - leftPoint.Z) / (rightPoint.Z - leftPoint.Z);
                }
                return new Point3D {
                    X = leftPoint.X + difference * (rightPoint.X - leftPoint.X),
                    Y = aY,
                    Z = aZ
                };
            }
            else if (aY >= RightLines[RightLines.Length - 1][0].Y) {
                var leftPointIndex = GetLowerBoundPoint(RightLines[RightLines.Length - 1], aZ);
                var rightPointIndex = RightLines[RightLines.Length - 1].Length - 1;
                if (leftPointIndex != RightLines[RightLines.Length - 1].Length - 1) {
                    rightPointIndex = leftPointIndex + 1;
                }
                var leftPoint = RightLines[RightLines.Length - 1][leftPointIndex];
                var rightPoint = RightLines[RightLines.Length - 1][rightPointIndex];
                var difference = 0.0;
                if (rightPoint != leftPoint) {
                    difference = (aZ - leftPoint.Z) / (rightPoint.Z - leftPoint.Z);
                }
                return new Point3D {
                    X = leftPoint.X + difference * (rightPoint.X - leftPoint.X),
                    Y = aY,
                    Z = aZ
                };
            }
            else {
                var leftLineIndex = GetLowerBoundLineByY(RightLines, aY);
                var leftLine = RightLines[leftLineIndex];
                var leftPointIndex = GetLowerBoundPoint(leftLine, aZ);
                var rightPointIndex = leftLine.Length - 1;
                if (leftPointIndex != rightPointIndex) {
                    rightPointIndex = leftPointIndex + 1;
                }
                var leftPointLeftLine = leftLine[leftPointIndex];
                var rightPointLeftLine = leftLine[rightPointIndex];
                var difference = 0.0;
                if (rightPointLeftLine != leftPointLeftLine) {
                    difference = (aZ - leftPointLeftLine.Z) / (rightPointLeftLine.Z - leftPointLeftLine.Z);
                }
                var leftX = leftPointLeftLine.X + difference * (rightPointLeftLine.X - leftPointLeftLine.X);

                var rightLine = RightLines[leftLineIndex + 1];
                var leftPointRightLine = rightLine[leftPointIndex];
                var rightPointRightLine = rightLine[rightPointIndex];
                difference = 0.0;
                if (rightPointRightLine != leftPointRightLine) {
                    difference = (aZ - leftPointRightLine.Z) / (rightPointRightLine.Z - leftPointRightLine.Z);
                }
                var rightX = leftPointRightLine.X + difference * (rightPointRightLine.X - leftPointRightLine.X);

                difference = (aY - rightPointLeftLine.Y) / (rightPointRightLine.Y - rightPointLeftLine.Y);
                return new Point3D {
                    X = leftX + difference * (rightX - leftX),
                    Y = aY,
                    Z = aZ
                };
            }
        }

        public SlabPoint[] ToPoints()
        {
            var points = new List<SlabPoint>();
            for (var i = 0; i < TopLines.Length; ++i) {
                for (var j = 0; j < TopLines[i].Length; ++j) {
                    var linePoint = TopLines[i][j];
                    var point = new SlabPoint {
                        X = linePoint.X, 
                        Y = linePoint.Y, 
                        Z = linePoint.Z
                    };
                    points.Add(point);
                }                
            }

            for (var i = 0; i < BottomLines.Length; ++i) {
                for (var j = 0; j < BottomLines[i].Length; ++j) {
                    var linePoint = BottomLines[i][j];
                    var point = new SlabPoint {
                        X = linePoint.X,
                        Y = linePoint.Y,
                        Z = linePoint.Z
                    };
                    points.Add(point);
                }
            }

            for (var i = 0; i < LeftLines.Length; ++i) {
                for (var j = 0; j < LeftLines[i].Length; ++j) {
                    var linePoint = LeftLines[i][j];
                    var point = new SlabPoint {
                        X = linePoint.X,
                        Y = linePoint.Y,
                        Z = linePoint.Z
                    };
                    points.Add(point);
                }
            }

            for (var i = 0; i < RightLines.Length; ++i) {
                for (var j = 0; j < RightLines[i].Length; ++j) {
                    var linePoint = RightLines[i][j];
                    var point = new SlabPoint {
                        X = linePoint.X,
                        Y = linePoint.Y,
                        Z = linePoint.Z
                    };
                    points.Add(point);
                }
            }

            return points.ToArray();
        }

        private int GetLowerBoundPoint(Point3D[] aMassive, double aZ)
        {
            if (aMassive == null) {
                throw new ArgumentNullException("aMassive");
            }

            if (aZ < aMassive[0].Z) {
                return 0;
            }

            if (aZ > aMassive[aMassive.Length - 1].Z) {
                return aMassive.Length - 1;
            }

            // TODO: Сделать бинарный поиск.
            for (var i = 0; i < aMassive.Length -1; ++i) {
                if (aMassive[i].Z <= aZ && aMassive[i + 1].Z > aZ) {
                    return i;
                }
            }

            throw new ArgumentException("Непонятная ситуация: не найдена точка.");
        }

        private int GetLowerBoundLineByX(Point3D[][] aLines, double aX)
        {
            if (aLines == null) {
                throw new ArgumentNullException("aLines");
            }

            if (aLines[0][0].X >= aX) {
                return 0;
            }

            if (aLines[aLines.Length - 1][0].X <= aX) {
                return aLines.Length - 1;
            }

            for (var i = 0; i < aLines.Length - 1; ++i) {
                if (aLines[i][0].X <= aX && aLines[i + 1][0].X > aX) {
                    return i;
                }
            }

            throw new ArgumentException("Непонятная ситуация: нет подходящей линии.");
        }

        private int GetLowerBoundLineByY(Point3D[][] aLines, double aY)
        {
            if (aLines == null) {
                throw new ArgumentNullException("aLines");
            }

            if (aLines[0][0].Y >= aY) {
                return 0;
            }

            if (aLines[aLines.Length - 1][0].Y <= aY) {
                return aLines.Length - 1;
            }

            for (var i = 0; i < aLines.Length - 1; ++i) {
                if (aLines[i][0].Y <= aY && aLines[i + 1][0].Y > aY) {
                    return i;
                }
            }

            throw new ArgumentException("Непонятная ситуация: нет подходящей линии.");
        }
    }
}
