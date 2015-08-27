using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace SGCUserInterface.SlabVisualizationFormPrimitivs.Panels
{
    public class SlabModelPanel : IDisposable
    {
        private static bool isGlutInited = false;

        private SimpleOpenGlControl control;
        private DimentionControl dimentionControl;
        private int slabId;
        private int standartSizeId;

        private SlabModel3D slabModel;
        private int translateX = 0;
        private int translateY = 0;
        private int translateZ = -5000;
        private int angleX = 15;
        private int angleY = 70;
        private int angleZ = 0;
        private int deltaX = 0;
        private int deltaY = 0;
        private int lastPositionX = 0;
        private int lastPositionY = 0;
        private int lastAngleX = 0;
        private int lastAngleY = 0;

        private List<DimentionGraphicPrimitiveBase> dimentions = new List<DimentionGraphicPrimitiveBase>();
        private int greenDimentionsCount = 0;
        private int redDimentionsCount = 0;

        private Dictionary<string, int> objectsList = new Dictionary<string, int>();        
        private const string KEY_SURFACE = "surface";
        private const string KEY_SENSOR_VALUES = "sensorValues";
        private const string KEY_SLAB_DIMENTIONS = "slabDimentions";

        private Dimention[] systemDimentions;
        private DimentionResult[] slabDimentionsResults;
        private Regulation[] regulations;

        private bool isShowGridSurface = true;
        private bool isShowSensorValues = true;
        private bool isShowSlabDimentions = true;
        private bool isUseSmooth = true;

        public SlabModelPanel(int aSlabId, int aStandartSizeId, SimpleOpenGlControl aModelControl)
        {
            if (aModelControl == null) {
                throw new ArgumentNullException("aModelControl");
            }

            slabId = aSlabId;
            standartSizeId = aStandartSizeId;
            control = aModelControl;
            dimentionControl = new DimentionControl();
            dimentionControl.Hide();
            dimentionControl.Parent = control;

            Initialize();
        }

        private void Initialize()
        {
            control.MouseWheel += control_MouseWheel;
            control.InitializeContexts();
            if (!isGlutInited) {
                Glut.glutInit();
                Glut.glutInitDisplayMode(Glut.GLUT_RGBA | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
                isGlutInited = true;
            }

            Gl.glClearColor(255, 255, 255, 1);
            Gl.glViewport(0, 0, control.Width, control.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45, (float)control.Width / control.Height, 10, 100000);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            //Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_MULTISAMPLE_ARB);
            Gl.glEnable(Gl.GL_LINE_SMOOTH);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glHint(Gl.GL_MULTISAMPLE_FILTER_HINT_NV, Gl.GL_FASTEST);
            Gl.glHint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_FASTEST);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);        
        }

        private void control_MouseWheel(object sender, MouseEventArgs e)
        {
            translateZ += e.Delta;
            ShowModel();
        }

        public void AddDimention(DimentionGraphicPrimitiveBase aDimention, CheckBox aCheckBox)
        {
            dimentions.Add(aDimention);
            aDimention.SlabModel = slabModel;
            aDimention.IsVisible = true;
            aDimention.CheckBox = aCheckBox;
            aDimention.Dimention = systemDimentions.FirstOrDefault(d => d.Name.Equals(aDimention.GetDimentionName()));
            if (aDimention.Dimention != null) {
                aDimention.Result = slabDimentionsResults
                    .FirstOrDefault(r => r.DimentionId == aDimention.Dimention.Id);
            }
            aDimention.Color = SelectColorByDimentionResult(aDimention.Result);
            aCheckBox.CheckedChanged += CommanDimentionCheckChanged;
        }

        private void CommanDimentionCheckChanged(object sender, EventArgs e)
        {
            ShowModel();
        }

        private Color SelectColorByDimentionResult(DimentionResult aResult)
        {
            if (aResult == null || IsResultSatisfyRegulations(aResult)) {
                return NextGreenColor();
            }
            else {
                return NextRedColor();
            }
        }

        private bool IsResultSatisfyRegulations(DimentionResult aResult)
        {
            if (aResult == null) {
                return true;
            }

            var regulation =
                regulations.FirstOrDefault(
                    r => r.DimentionId == aResult.DimentionId && r.StandartSizeId == standartSizeId);
            if (regulation == null) {
                return true;
            }
            var value = aResult.Value;
            return value >= regulation.MinValue && value <= regulation.MaxValue;
        }

        private Color NextGreenColor()
        {
            var color = Color.FromArgb(255, Color.Green.R, Color.Green.G - greenDimentionsCount, Color.Green.B);
            greenDimentionsCount++;
            return color;
        }

        private Color NextRedColor()
        {
            var color = Color.FromArgb(255, Color.Red.R - redDimentionsCount, Color.Red.G, Color.Red.B);
            redDimentionsCount++;
            return color;
        }

        public void SetSlabModel(SlabModel3D aSlabModel)
        {
            slabModel = aSlabModel;
        }

        public void MoveModelToZeroPoint()
        {
            if (slabModel != null) {
                var moveTo = slabModel.TopLines[slabModel.TopLines.Length / 2].Last().Z / 2;
                for (var i = 0; i < slabModel.TopLines.Length; ++i) {
                    for (var j = 0; j < slabModel.TopLines[i].Length; ++j) {
                        slabModel.TopLines[i][j].Z -= moveTo;
                    }
                }
                for (var i = 0; i < slabModel.BottomLines.Length; ++i) {
                    for (var j = 0; j < slabModel.BottomLines[i].Length; ++j) {
                        slabModel.BottomLines[i][j].Z -= moveTo;
                    }
                }
                for (var i = 0; i < slabModel.LeftLines.Length; ++i) {
                    for (var j = 0; j < slabModel.LeftLines[i].Length; ++j) {
                        slabModel.LeftLines[i][j].Z -= moveTo;
                    }
                }
                for (var i = 0; i < slabModel.RightLines.Length; ++i) {
                    for (var j = 0; j < slabModel.RightLines[i].Length; ++j) {
                        slabModel.RightLines[i][j].Z -= moveTo;
                    }
                }
            }
        }

        public void SetDimentions(Dimention[] aDimentions)
        {
            systemDimentions = aDimentions;
        }

        public void SetDimentionResults(DimentionResult[] aDimentionResults)
        {
            slabDimentionsResults = aDimentionResults;
        }

        public void SetRegulations(Regulation[] aRegulations)
        {
            regulations = aRegulations;
        }

        public void InitializeGlObjects()
        {
            InitGridSurface();
            InitSensorValues();
            InitSlabDimentions();
        }

        public void ShowModel()
        {
            Gl.glPushMatrix();            
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glTranslated(translateX, translateY, translateZ);
            Gl.glRotated(angleX, 1, 0, 0);
            Gl.glRotated(angleY, 0, 1, 0);
            Gl.glRotated(angleZ, 0, 0, 1);
            {
                if (isShowGridSurface && objectsList.ContainsKey(KEY_SURFACE)) {
                    Gl.glCallList(objectsList[KEY_SURFACE]);
                }

                if (isShowSensorValues && objectsList.ContainsKey(KEY_SENSOR_VALUES)) {
                    Gl.glCallList(objectsList[KEY_SENSOR_VALUES]);
                }

                if (isShowSlabDimentions && objectsList.ContainsKey(KEY_SLAB_DIMENTIONS)) {
                    Gl.glCallList(objectsList[KEY_SLAB_DIMENTIONS]);
                }

                foreach (var dimention in dimentions) {
                    dimention.DrawDimention(isUseSmooth);
                }
            }
            Gl.glPopMatrix();
            Gl.glFlush();
            control.Invalidate();
        }

        public void MouseDown(MouseEventArgs e)
        {
            deltaX = e.X;
            deltaY = e.Y;
            lastPositionX = translateX;
            lastPositionY = translateY;
            lastAngleX = angleX;
            lastAngleY = angleY;
        }

        public void MouseMove(MouseEventArgs e)
        {
            var isStateChanged = false;

            if (e.Button == MouseButtons.Right) {
                translateX = lastPositionX + (e.X - deltaX) / 1;
                translateY = lastPositionY - (e.Y - deltaY) / 1;
                isStateChanged = true;
            }
            else if (e.Button == MouseButtons.Left) {
                angleX = lastAngleX + (e.Y - deltaY) / 10;
                angleY = lastAngleY + (e.X - deltaX) / 10;
                isStateChanged = true;
            }

            var selectedDimention = SelectedDimention(e.X, e.Y);
            if (selectedDimention != null) {
                if (!selectedDimention.IsSelected) {
                    isStateChanged = true;
                }
                selectedDimention.IsSelected = true;
                ShowDimentionControl(e.X, e.Y, selectedDimention);
            }
            else {
                HideDimentionControl();
                foreach (var dimention in dimentions) {
                    if (dimention.IsSelected) {
                        isStateChanged = true;
                    }
                    dimention.IsSelected = false;
                }
            }

            if (isStateChanged) {
                ShowModel();
            }
        }

        private void HideDimentionControl()
        {
            dimentionControl.Hide();
        }

        private DimentionGraphicPrimitiveBase SelectedDimention(int aX, int aY)
        {
            var pixel = new byte[4];
            Gl.glReadPixels(aX, control.Height - aY, 1, 1, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, pixel);
            var currentColor = Color.FromArgb(pixel[3], pixel[2], pixel[1], pixel[0]);
            return dimentions.FirstOrDefault(dimention => dimention.Color == currentColor);
        }

        private void ShowDimentionControl(int aX, int aY, DimentionGraphicPrimitiveBase aDimention)
        {
            dimentionControl.Top = aY;
            dimentionControl.Left = aX;
            dimentionControl.SetDimentionPrimitive(aDimention);
            dimentionControl.SetRegulation(GetRegulationOrDefault(aDimention));
            dimentionControl.Show();
        }

        private Regulation GetRegulationOrDefault(DimentionGraphicPrimitiveBase aDimention)
        {
            if (aDimention == null) {
                return null;
            }

            return regulations.FirstOrDefault(r => r.DimentionId == aDimention.Dimention.Id &&
                                                   r.StandartSizeId == standartSizeId);
        }

        public void ShowGridSurfaceChanged(bool aIsShowGridSurface)
        {
            isShowGridSurface = aIsShowGridSurface;
        }

        public void ShowDimentionsChanged(bool aIsShowDimentions)
        {
            isShowSlabDimentions = aIsShowDimentions;
        }

        public void ShowSensorValuesChanged(bool aIsShowSensorValues)
        {
            isShowSensorValues = aIsShowSensorValues;
        }

        private void InitGridSurface()
        {
            var strongLinesCount = 5;
            var slimLinesCount = 2;
            var distanceBetwenStrongLines = 1000;
            var depthY = -2000;
            var startPosition = distanceBetwenStrongLines * strongLinesCount / 2;

            var surfaceNumber = Gl.glGenLists(1);
            objectsList[KEY_SURFACE] = surfaceNumber;
            Gl.glNewList(surfaceNumber, Gl.GL_COMPILE);
            var color = Color.LightGray;
            Gl.glLineWidth(1f);
            Gl.glColor3d(
                Convert.ToDouble(color.R) / 255,
                Convert.ToDouble(color.G) / 255,
                Convert.ToDouble(color.B) / 255);
            Gl.glBegin(Gl.GL_LINES);
            {
                for (var i = 1; i <= strongLinesCount; ++i) {
                    Gl.glVertex3f(-startPosition, depthY, -startPosition + distanceBetwenStrongLines * i);
                    Gl.glVertex3f(startPosition, depthY, -startPosition + distanceBetwenStrongLines * i);
                    Gl.glVertex3f(startPosition - distanceBetwenStrongLines * i, depthY, -startPosition);
                    Gl.glVertex3f(startPosition - distanceBetwenStrongLines * i, depthY, startPosition);

                    Gl.glVertex3f(startPosition - distanceBetwenStrongLines * i, depthY, -startPosition);
                    Gl.glVertex3f(startPosition - distanceBetwenStrongLines * i, depthY + distanceBetwenStrongLines * (strongLinesCount - 1), -startPosition);

                    Gl.glVertex3f(startPosition, depthY, -startPosition + distanceBetwenStrongLines * i);
                    Gl.glVertex3f(startPosition, depthY + distanceBetwenStrongLines * (strongLinesCount-1), -startPosition + distanceBetwenStrongLines * i);

                    if (i != strongLinesCount) {
                        Gl.glVertex3f(-startPosition, depthY + distanceBetwenStrongLines * i, -startPosition);
                        Gl.glVertex3f(startPosition, depthY + distanceBetwenStrongLines * i, -startPosition);

                        Gl.glVertex3f(startPosition, depthY + distanceBetwenStrongLines * i, -startPosition);
                        Gl.glVertex3f(startPosition, depthY + distanceBetwenStrongLines * i, startPosition);
                    }
                }
            }
            Gl.glEnd();

            color = Color.Gray;
            Gl.glLineWidth(2f);
            Gl.glColor3d(
                Convert.ToDouble(color.R) / 255,
                Convert.ToDouble(color.G) / 255,
                Convert.ToDouble(color.B) / 255);
            Gl.glBegin(Gl.GL_LINES);
            {
                Gl.glVertex3f(startPosition, depthY, -startPosition);
                Gl.glVertex3f(-startPosition, depthY, -startPosition);

                Gl.glVertex3f(startPosition, depthY, -startPosition);
                Gl.glVertex3f(startPosition, depthY, startPosition);

                Gl.glVertex3f(startPosition, depthY, -startPosition);
                Gl.glVertex3f(startPosition, depthY + distanceBetwenStrongLines * (strongLinesCount - 1), -startPosition);
            }
            Gl.glEnd();
            Gl.glEndList();
        }

        private void InitSlabDimentions()
        {
            if (slabModel == null) {
                return;
            }
            var startPosition = 3;
            var endPosition = 3;
            var p1 = slabModel.TopLines[slabModel.TopLines.Length / 2][startPosition];
            var p2 = slabModel.RightLines[slabModel.RightLines.Length / 2][startPosition];
            var p3 = slabModel.BottomLines[slabModel.BottomLines.Length / 2][startPosition];
            var p4 = slabModel.LeftLines[slabModel.LeftLines.Length / 2][startPosition];
            var l1 = slabModel.TopLines[slabModel.TopLines.Length/2].Length;
            var p5 = slabModel.TopLines[slabModel.TopLines.Length / 2][l1 - endPosition];
            var l2 = slabModel.TopLines[slabModel.RightLines.Length / 2].Length;
            var p6 = slabModel.RightLines[slabModel.RightLines.Length / 2][l2 - endPosition];
            var l3 = slabModel.TopLines[slabModel.BottomLines.Length / 2].Length;
            var p7 = slabModel.BottomLines[slabModel.BottomLines.Length / 2][l3 - endPosition];
            var l4 = slabModel.TopLines[slabModel.LeftLines.Length / 2].Length;
            var p8 = slabModel.LeftLines[slabModel.LeftLines.Length / 2][l4 - endPosition];

            var slabDimentionsNumber = Gl.glGenLists(1);
            objectsList[KEY_SLAB_DIMENTIONS] = slabDimentionsNumber;
            Gl.glNewList(slabDimentionsNumber, Gl.GL_COMPILE);
            var color = Color.Blue;
            Gl.glColor3d(
                Convert.ToDouble(color.R) / 255,
                Convert.ToDouble(color.G) / 255,
                Convert.ToDouble(color.B) / 255);
            Gl.glLineWidth(2f);
            Gl.glEnable(Gl.GL_LINE_STIPPLE);
            Gl.glLineStipple(1, 0x00FF);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            {
                Gl.glVertex3d(p4.X, p1.Y, p1.Z);
                Gl.glVertex3d(p2.X, p1.Y, p1.Z);
                Gl.glVertex3d(p2.X, p3.Y, p1.Z);
                Gl.glVertex3d(p4.X, p3.Y, p1.Z);
                Gl.glVertex3d(p4.X, p1.Y, p1.Z);
                Gl.glVertex3d(p8.X, p5.Y, p5.Z);
                Gl.glVertex3d(p6.X, p5.Y, p5.Z);
                Gl.glVertex3d(p6.X, p7.Y, p5.Z);
                Gl.glVertex3d(p8.X, p7.Y, p5.Z);
                Gl.glVertex3d(p8.X, p5.Y, p5.Z);
            }
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINES);
            {
                Gl.glVertex3d(p2.X, p1.Y, p1.Z);
                Gl.glVertex3d(p6.X, p5.Y, p5.Z);
                Gl.glVertex3d(p2.X, p3.Y, p1.Z);
                Gl.glVertex3d(p6.X, p7.Y, p5.Z);
                Gl.glVertex3d(p4.X, p3.Y, p1.Z);
                Gl.glVertex3d(p8.X, p7.Y, p5.Z);
            }
            Gl.glEnd();
            Gl.glDisable(Gl.GL_LINE_STIPPLE);
            Gl.glEndList();
        }

        private void InitSensorValues()
        {
            if (slabModel == null) {
                return;
            }

            var sensorValuesNumber = Gl.glGenLists(1);
            objectsList[KEY_SENSOR_VALUES] = sensorValuesNumber;
            Gl.glNewList(sensorValuesNumber, Gl.GL_COMPILE);
            var color = Color.Brown;
            Gl.glColor3d(
                Convert.ToDouble(color.R) / 255,
                Convert.ToDouble(color.G) / 255,
                Convert.ToDouble(color.B) / 255);
            Gl.glLineWidth(2f);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            {
                for (var i = 0; i < slabModel.TopLines.Length; ++i) {
                    for (var j = 0; j < slabModel.TopLines[i].Length; ++j) {
                        var point = slabModel.TopLines[i][j];
                        Gl.glVertex3d(point.X, point.Y, point.Z);
                    }
                }
            }
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINE_STRIP);
            {
                for (var i = 0; i < slabModel.BottomLines.Length; ++i) {
                    for (var j = 0; j < slabModel.BottomLines[i].Length; ++j) {
                        var point = slabModel.BottomLines[i][j];
                        Gl.glVertex3d(point.X, point.Y, point.Z);
                    }
                }
            }
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINE_STRIP);
            {
                for (var i = 0; i < slabModel.LeftLines.Length; ++i) {
                    for (var j = 0; j < slabModel.LeftLines[i].Length; ++j) {
                        var point = slabModel.LeftLines[i][j];
                        Gl.glVertex3d(point.X, point.Y, point.Z);
                    }
                }
            }
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINE_STRIP);
            {
                for (var i = 0; i < slabModel.RightLines.Length; ++i) {
                    for (var j = 0; j < slabModel.RightLines[i].Length; ++j) {
                        var point = slabModel.RightLines[i][j];
                        Gl.glVertex3d(point.X, point.Y, point.Z);
                    }
                }
            }
            Gl.glEnd();
            Gl.glEndList();
        }

        public void ShowSmoothChanged(bool aIsUseSmooth)
        {
            isUseSmooth = aIsUseSmooth;
            if (aIsUseSmooth) {
                Gl.glEnable(Gl.GL_MULTISAMPLE_ARB);
                Gl.glEnable(Gl.GL_LINE_SMOOTH);
                Gl.glEnable(Gl.GL_BLEND);
            }
            else {
                Gl.glDisable(Gl.GL_MULTISAMPLE_ARB);
                Gl.glDisable(Gl.GL_LINE_SMOOTH);
                Gl.glDisable(Gl.GL_BLEND);
            }
        }

        public void Dispose()
        {
            if (objectsList.ContainsKey(KEY_SURFACE)) {
                Gl.glDeleteLists(objectsList[KEY_SURFACE], 1);
            }
            if (objectsList.ContainsKey(KEY_SENSOR_VALUES)) {
                Gl.glDeleteLists(objectsList[KEY_SENSOR_VALUES], 1);
            }
            if (objectsList.ContainsKey(KEY_SLAB_DIMENTIONS)) {
                Gl.glDeleteLists(objectsList[KEY_SLAB_DIMENTIONS], 1);
            }
        }

        public void MoveToFrontSide()
        {
            angleY = 0;
            angleX = 0;
            translateX = 0;
            translateY = 0;
        }

        public void MoveToTopSide()
        {
            angleY = 90;
            angleX = 90;
            translateX = 0;
            translateY = 0;
        }

        public void MoveToLeftSide()
        {
            angleY = 90;
            angleX = 0;
            translateX = 0;
            translateY = 0;
        }

        public void MoveToAngleSide()
        {
            angleX = 45;
            angleY = 45;
            translateX = 0;
            translateY = 0;
        }
    }
}
