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
            var currentPosition = 0.0;
            for (var i = 0; i < centersCount; ++i) {
                if (currentPosition <= configuration.Flexs[0].Position) {
                    var flex = configuration.Flexs[0].Maxumum * currentPosition / configuration.Flexs[0].Position;
                    centers[i].Y -= (float)flex;
                } else {
                    var flex = configuration.Flexs[0].Maxumum * (configuration.Length - currentPosition) / (configuration.Length - configuration.Flexs[0].Position);
                    centers[i].Y -= (float)flex;
                }
                currentPosition += 1.0 * configuration.Length / centersCount;
            }

            // накладываем подпрыгивания.
            // накладываем дребезжание.

            return centers;
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
    }
}
