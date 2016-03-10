using System;
using System.Collections.Generic;
using System.Drawing;
using REmulatorConfiguration;

namespace RoundSlabEmulator {
    /// <summary>
    /// Эмулятор круглых слитков.
    /// </summary>
    public class RSlabEmulator {
        private EmulatorConfiguration configuration;

        /// <summary>
        /// Показания датчиков.
        /// </summary>
        private Dictionary<int, double[]> sensorValues;

        /// <summary>
        /// Устанавливает кнфигурацию эмулятора.
        /// </summary>
        /// <param name="aConfiguration">Конфигурация эмулятора.</param>
        public void setConfiguration(EmulatorConfiguration aConfiguration) {
            configuration = aConfiguration;
        }

        /// <summary>
        /// Генерирует показания датчиков.
        /// </summary>
        public void GenerateSensorValues() {
            if (configuration == null) {
                throw new ArgumentException("Конфигурация не установлена.");
            }

            // Генерируем центры окружностей.
            var centers = GenerateCenters();
            sensorValues = new Dictionary<int, double[]>();

            var sensors = configuration.Frame.Sensors;
            for (var i = 0; i < sensors.Length; ++i) {
                if (sensors[i].Id != 4) { // если обычный датчик.
                    var values = new List<double>();
                    for (var j = 0; j < centers.Length; ++j) {
                        values.Add(IntersectionLength(sensors[i], centers[j]));
                    }
                    sensorValues[sensors[i].Id] = values.ToArray();
                } else { // если датчик положения.
                    sensorValues[sensors[i].Id] = GeneratePositionValues();
                }
            }
        }

        private double[] GeneratePositionValues() {
            var pointsCount = (int)(1.0 * configuration.Length * configuration.Frame.ScanSpeed / configuration.Speed);
            var results = new double[pointsCount];
            var currentPosition = 0.0;
            for (var i = 0; i < pointsCount; ++i) {
                results[i] = currentPosition;
                currentPosition += 1.0 * configuration.Length / pointsCount;
            }

            return results;
        }

        /// <summary>
        /// Возвращает показания датчика.
        /// </summary>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <returns>Показания датчика.</returns>
        public double[] GetValues(int aSensorId) {
            return new List<double>(sensorValues[aSensorId]).ToArray();
        }

        /// <summary>
        /// Генерирует центры окружностй срезов слитка.
        /// </summary>
        /// <returns>Центры срезов слитка.</returns>
        private PointF[] GenerateCenters() {
            // количество срезов
            var centersCount = (int)(1.0 * configuration.Length * configuration.Frame.ScanSpeed / configuration.Speed);
            var centers = new PointF[centersCount];

            // первый прогон - инициализируем.
            for (var i = 0; i < centersCount; ++i) {
                centers[i] = new PointF(0, 0);
            }

            // накладываем искривление с максимумом в указанной позиции.
            // используем интерполирование.
            MakeFlexs(centers);
            //for (var i = 0; i < configuration.Flexs.Length; ++i) {
            //    MakeFlexs(centers, configuration.Flexs[i]);
            //}            


            // накладываем подпрыгивания.            
            for (var i = 0; i < configuration.Bumps.Length; ++i) {                
                var bump = configuration.Bumps[i];
                var normal = Math.Sqrt(bump.Direction.X * bump.Direction.X + bump.Direction.Y * bump.Direction.Y);
                var dx = bump.Direction.X / normal;
                var dy = bump.Direction.Y / normal;
                var currentPosition = 0.0;
                for (var j = 0; j < centersCount; ++j) {
                    if (currentPosition >= bump.Position) {
                        centers[j].X += (float)(dx * bump.Value);
                        centers[j].Y += (float)(dy * bump.Value);
                    }
                    currentPosition += 1.0 * configuration.Length / centersCount;
                }                
            }

            // накладываем дребезжание.
            var rnd = new Random(141289);
            for (var i = 0; i < centers.Length; ++i) {
                centers[i].X += (float)((rnd.Next() % 2 == 0 ? 1 : -1) * configuration.RattleLimit * rnd.Next(100) / 100.0);
                centers[i].Y += (float)((rnd.Next() % 2 == 0 ? 1 : -1) * configuration.RattleLimit * rnd.Next(100) / 100.0);
            }

            return centers;
        }

        private void MakeFlexs(PointF[] aCenters) {
            // составим таблицу интерполяционных узлов.
            var centersCount = aCenters.Length;
            var pointsCount = 2 + configuration.Flexs.Length;
            var xValues = new double[pointsCount];
            var yValues = new double[pointsCount];
            xValues[0] = 0;
            yValues[0] = aCenters[0].Y;
            xValues[pointsCount - 1] = configuration.Length;
            yValues[pointsCount - 1] = aCenters[centersCount - 1].Y;
            for (var i = 1; i <= configuration.Flexs.Length; ++i) {
                xValues[i] = configuration.Flexs[i-1].Position;
                yValues[i] = configuration.Flexs[i-1].Maximum;
            }

            var currentPosition = 0.0;            
            for (var i = 0; i < centersCount; ++i) {
                aCenters[i].Y = (float)InterpolateLagrangePolynomial(currentPosition, xValues, yValues, pointsCount);
                currentPosition += 1.0 * configuration.Length / centersCount;
            }
        }

        /// <summary>
        /// https://ru.wikipedia.org/wiki/Интерполяционный_многочлен_Лагранжа
        /// </summary>
        /// <param name="x"></param>
        /// <param name="xValues"></param>
        /// <param name="yValues"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private double InterpolateLagrangePolynomial(double x, double[] xValues, double[] yValues, int size) {
            double lagrangePol = 0;
            for (int i = 0; i < size; i++) {
                double basicsPol = 1;
                for (int j = 0; j < size; j++) {
                    if (j != i) {
                        basicsPol *= (x - xValues[j]) / (xValues[i] - xValues[j]);
                    }
                }
                lagrangePol += basicsPol * yValues[i];
            }

            return lagrangePol;
        }

        /// <summary>
        /// Накладывает искривление.
        /// </summary>
        /// <param name="centers"></param>
        /// <param name="flexConfiguration"></param>
        private void MakeFlexs(PointF[] aCenters, FlexConfiguration aFlex) {
            var centersCount = aCenters.Length;
            // укажем три точки на слитке.
            var a = aCenters[0];
            var b = aCenters[centersCount - 1];
            PointF c = new PointF();
            var currentPosition = 0.0;
            var find = false;
            for (var i = 0; i < aCenters.Length; ++i) {
                if (currentPosition >= aFlex.Position) {
                    c = aCenters[i];
                    find = true;
                    break;
                }
                currentPosition += 1.0 * configuration.Length / centersCount;
            }
            if (!find) {
                throw new ArgumentException("MakeFlexs: положение искривления не в пределах размеров слитка.");
            }

            // Сдвинем точку C на заданное отклонение.
            c.Y -= (float)aFlex.Maximum;

            // Все точик лежат в плоскости YOZ, поэтому учитываем это.

            // теперь найдем центр и радиус окружности. 
            var pa = new Point3D { X = 0, Y = a.Y, Z = 0 };
            var pb = new Point3D { X = configuration.Length, Y = b.Y, Z = 0 };
            var pc = new Point3D { X = aFlex.Position, Y = c.Y, Z = 0 };

            var center = CalcCircleCenter(pa, pb, pc);
            var radius = CalcCircleDiameter(pa, pb, pc) / 2.0;

            // переведем центр обратно в систему координат.
            center.Z = center.X;
            center.X = 0;

            // теперь сдвигаем все точки в направлении радиус-вектора.
            currentPosition = 0.0;
            for (var i = 0; i < centersCount; ++i) {
                // dr: найдем разницу между радиусом и расстоянием от центра до точки.
                var pt = new Point3D { X = aCenters[i].X, Y = aCenters[i].Y, Z = currentPosition };
                var dr = radius - center.DistanceToPoint(pt);
                // vr: найдем вектор радиуса и нормализуем его.
                var vr = new Point3D { X = pt.X - center.X, Y = pt.Y - center.Y, Z = pt.Z - center.Z };
                var vsize = Math.Sqrt(vr.X * vr.X + vr.Y * vr.Y + vr.Z * vr.Z);
                vr.X /= vsize;
                vr.Y /= vsize;                
                // теперь сдвинем исходную точку по направлению вектора на разницу.
                pt.X += dr * vr.X;
                pt.Y += dr * vr.Y;                
                aCenters[i].X = (float)pt.X;
                aCenters[i].Y = (float)pt.Y;

                currentPosition += 1.0 * configuration.Length / centersCount;
            }
        }

        /// <summary>
        /// Возвращает расстояние от датчика до среза слитка.
        /// </summary>
        /// <param name="aSensorConfiguration">Конфигурация датчика.</param>
        /// <param name="aCenter">Положение центра.</param>
        /// <returns>Расстояние.</returns>
        public double IntersectionLength(SensorConfiguration aSensorConfiguration, PointF aCenter) {
            // найдем уравнение прямой вида Ax+By+C=0 по точке M(xm, ym) и направляющему вектору P(xp, yp).
            var xm = aSensorConfiguration.Position.X;
            var ym = aSensorConfiguration.Position.Y;
            var xp = aSensorConfiguration.ScanVertor.X;
            var yp = aSensorConfiguration.ScanVertor.Y;
            double A, B, C;
            BuildLine(xm, ym, xm + xp, ym + yp, out A, out B, out C);

            // запишем данные окружности с центром O(xo, yo) и радиусом R.
            var xo = aCenter.X;
            var yo = aCenter.Y;
            var R = configuration.Diameter / 2;
            
            PointF a, b;
            StraightContactCricle(xo, yo, R, A, B, C, out a, out b);

            // выберем ближайшую точку до датчика и вернем расстояние до нее.
            return Math.Min(
                Math.Sqrt((xm - a.X) * (xm - a.X) + (ym - a.Y) * (ym - a.Y)),
                Math.Sqrt((xm - b.X) * (xm - b.X) + (ym - b.Y) * (ym - b.Y))
                );
        }

        /// <summary>
        /// Откроем доступ для тестов.
        /// </summary>        
        private void StraightContactCricle(
            double xo, double yo, double R, 
            double A, double B, double C, 
            out PointF a, out PointF b) 
        {
            // найдем точки пересечения окружности и прямой http://e-maxx.ru/algo/circle_line_intersection
            PointF t = new PointF();
            double EPS = 0.00000001;
            C = A * xo + B * yo + C;
            t.X = (float)(-A * C / (A * A + B * B));
            t.Y = (float)(-B * C / (A * A + B * B));
            if (C * C > R * R * (A * A + B * B) + EPS)
                throw new ArgumentException("Нет пересечений луча сканиварония и слитка.");
            else if (Math.Abs(C * C - R * R * (A * A + B * B)) < EPS) {
                throw new ArgumentException("Единственное пересечение луча сканирования и окружности слитка.");
            } else {
                double d = R * R - C * C / (A * A + B * B);
                double mult = Math.Sqrt(d / (A * A + B * B));
                a = new PointF((float)(t.X + B * mult + xo), (float)(t.Y - A * mult + yo));
                b = new PointF((float)(t.X - B * mult + xo), (float)(t.Y + A * mult + yo));                
            }
        }

        /// <summary>
        ///  Строит уравнение прямой по двум точкам.
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        private void BuildLine(double x0, double y0, double x1, double y1, out double A, out double B, out double C) {
            A = y0 - y1;
            B = x1 - x0;
            C = -A * x0 - B * y0;
        }

        /// <summary>
        /// Вычисляет диаметр описанной окружности.
        /// </summary>
        /// <param name="aA"></param>
        /// <param name="aB"></param>
        /// <param name="aC"></param>
        /// <returns>Диаметр.</returns>
        private double CalcCircleDiameter(Point3D aA, Point3D aB, Point3D aC) {
            // Формула:
            // D = a*b*c / 2 * sqrt(p*(p-a)*(p-b)*(p-c)), 
            // где a,b,c - длины сторон треугольника, p - полупериметр треугольника.
            var a = aA.DistanceToPoint(aB);
            var b = aB.DistanceToPoint(aC);
            var c = aC.DistanceToPoint(aA);
            var p = (a + b + c) / 2.0;

            return (2 * a * b * c) / (4 * Math.Sqrt(p * (p - a) * (p - b) * (p - c)));
        }

        /// <summary>
        /// Вычисляет центр описанной окружности.
        /// </summary>        
        /// <returns></returns>
        private Point3D CalcCircleCenter(Point3D aA, Point3D aB, Point3D aC) {
            var epsilon = 0.0001;
            if (Math.Abs(aA.Z - aB.Z) > epsilon || Math.Abs(aA.Z - aC.Z) > epsilon) {
                throw new ArgumentException("CalcCircleCenter: точки находятся не в одной плоскости.");
            }

            // формула http://www.cyberforum.ru/geometry/thread1190053.html

            double x1 = aA.X, x2 = aB.X, x3 = aC.X,
                   y1 = aA.Y, y2 = aB.Y, y3 = aC.Y;
            double x12 = x1 - x2,
                x23 = x2 - x3,
                x31 = x3 - x1,
                y12 = y1 - y2,
                y23 = y2 - y3,
                y31 = y3 - y1;
            double z1 = x1 * x1 + y1 * y1,
                z2 = x2 * x2 + y2 * y2,
                z3 = x3 * x3 + y3 * y3;
            double zx = y12 * z3 + y23 * z1 + y31 * z2,
                zy = x12 * z3 + x23 * z1 + x31 * z2,
                z = x12 * y31 - y12 * x31;

            return new Point3D {
                X = -zx / (2.0 * z),
                Y = zy / (2.0 * z),
                Z = aA.Z
            };
        }
    }
}
