using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabaCupTest
{
    public partial class MainForm : Form
    {
        private const int Width = 600;
        private const int Height = 700;
        private const int CircleCenterX = 300;
        private const int CircleCenterY = 250;
        private const int LineStep = 40;
        private const int LineOverlay = 12;
        private const int MiniCircleRadiusMin = 3;
        private const int MiniCircleRadiusMax = 7;
        private const int BigRadius = 50;
        private int bigCircleRadius = 150; //150
        private int cubeSize = 40;
        private Point cubeStartPoint = new Point();

        private Random random = new Random();
        private ToolTip toolTip = new ToolTip();

        public MainForm()
        {
            this.Paint += new PaintEventHandler(MainFormPaint);
            this.MouseMove += new MouseEventHandler(OnMouseMove);
            this.Size = new Size(Width, Height);
        }

        private void MainFormPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            cubeStartPoint = GetPointCircle(CircleCenterX, CircleCenterY, bigCircleRadius);
            FillBackground(g);

            DrawCircle(g, CircleCenterX, CircleCenterY, bigCircleRadius, Color.FromArgb(252, 206, 170));
            DrawRectangles(g, cubeStartPoint, cubeSize);
            DrawCircle(g, CircleCenterX, CircleCenterY, bigCircleRadius - bigCircleRadius / 4, Color.FromArgb(254, 245, 204)); //110
            DrawRotatedRectangle(g);
            DrawCircle(g, CircleCenterX, CircleCenterY, bigCircleRadius - bigCircleRadius / 3, Color.FromArgb(162, 106, 76)); //90

            DrawLines(g, Color.Black, Color.White);
            DrawMiniCircles(g);
            DrawAndFillPath(g, Color.FromArgb(86, 50, 24));
        }

        private void FillBackground(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(238, 28, 37)), 0, 0, Width, Height);
        }

        private void DrawCircle(Graphics g, int posX, int posY, int radius, Color color)
        {
            g.FillEllipse(new SolidBrush(color), posX - radius, posY - radius, radius * 2, radius * 2);
        }

        private Point GetPointCircle(int x, int y, int radius)
        {
            int posX = x - radius;
            
            return new Point(posX, y);
        }

        private void DrawRotatedRectangle(Graphics g)
        {
            Matrix myMatrix = new Matrix();
            myMatrix.RotateAt(-45, new PointF(200, 200));
            g.Transform = myMatrix;
            SolidBrush myBrush = new SolidBrush(Color.FromArgb(254, 245, 204));
            Rectangle rect = new Rectangle(330, 300, 70, 30);
            g.FillRectangle(myBrush, rect);
            g.ResetTransform();
        }

        private void DrawLines(Graphics g, Color color1, Color color2)
        {
            Pen pen = new Pen(color1, 20);
            int startPosX = -2;
            int startPosY = 450;
            int numZigZags = 11;
            int zigZagSpacing = -60;

            int startX = startPosX;
            int startY = startPosY;
            int step = LineStep;
            int overlay = LineOverlay;
            int zigZagHeight = step * 2;

            for (int zigZag = 0; zigZag < numZigZags; zigZag++)
            {
                int currentX = startX;
                int currentY = startPosY + zigZag * (zigZagHeight + zigZagSpacing);

                pen.Color = (zigZag % 2 == 0) ? color1 : color2;

                for (int i = 0; i < 15; i++)
                {
                    if (i % 2 == 0)
                    {
                        g.DrawLine(pen, new Point(currentX, currentY), new Point(currentX + step + overlay, currentY + step));
                        currentX += step;
                        currentY += step;
                    }
                    else
                    {
                        g.DrawLine(pen, new Point(currentX, currentY), new Point(currentX + step + overlay, currentY - step));
                        currentX += step;
                        currentY -= step;
                    }
                }
            }
        }

        private void DrawMiniCircles(Graphics g)
        {
            SolidBrush brush = new SolidBrush(Color.FromArgb(87, 51, 25));
            int centerX = 290;
            int centerY = 170;

            for (int i = 0; i < 10; i++)
            {
                double angle = random.NextDouble() * Math.PI;
                double radius = random.NextDouble() * BigRadius;

                int x = (int)(centerX + radius * Math.Cos(angle));
                int y = (int)(centerY + radius * Math.Sin(angle));

                int r = random.Next(MiniCircleRadiusMin, MiniCircleRadiusMax + 1);

                if (y - r >= centerY - BigRadius)
                {
                    g.FillEllipse(brush, x - r, y - r, r * 2, r * 2);
                }
            }
        }

        private void DrawAndFillPath(Graphics g, Color color)
        {
            GraphicsPath path = new GraphicsPath();
            Brush brush = new SolidBrush(color);

            Rectangle rect = new Rectangle(220, 150, 160, 180);
            float startAngle = 0;
            float sweepAngle = 180;
            path.AddArc(rect, startAngle, sweepAngle);

            PointF p1 = new PointF(rect.Left, 220);
            PointF cp1 = new PointF(rect.X + 15, 170);
            PointF cp2 = new PointF(rect.X + 40, 220);
            PointF p2 = new PointF(rect.X + 60, 220);
            path.AddBezier(p1, cp1, cp2, p2);

            cp1.X = rect.X + 125;
            cp1.Y = 160;
            cp2.X = rect.X + 160;
            cp2.Y = 210;
            p1.X = rect.Right;
            p1.Y = 220;
            path.AddBezier(p2, cp1, cp2, p1);

            path.CloseFigure();

            g.FillPath(brush, path);
        }

        private void DrawRectangles(Graphics g, Point point, int cubeSize)
        {
            Color color1 = Color.White;
            Color color2 = Color.Brown;

            DrawRectangle(g, point.X, point.Y, cubeSize, cubeSize, color1, color2);
            DrawRectangle(g, point.X, point.Y+ cubeSize + cubeSize / 10, cubeSize, cubeSize, color1, color2);
            DrawRectangle(g, point.X - cubeSize / 8, point.Y + cubeSize / 2, cubeSize, cubeSize, color1, color2);
        }

        private void DrawRectangles(Graphics g, int posX, int posY, int cubeSize)
        {
            Color color1 = Color.White;
            Color color2 = Color.Brown;

            DrawRectangle(g, posX, posY, cubeSize, cubeSize, color1, color2);
            DrawRectangle(g, posX, posY + cubeSize + cubeSize / 10, cubeSize, cubeSize, color1, color2);
            DrawRectangle(g, posX - cubeSize / 8, posY + cubeSize / 2, cubeSize, cubeSize, color1, color2);
        }

        private void DrawRectangle(Graphics g, int x, int y, int width, int height, Color color1, Color color2)
        {
            g.FillRectangle(new SolidBrush(color1), x, y, width, height);
            g.DrawRectangle(new Pen(color2, 1), x, y, width, height);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            int mouseX = e.X;
            int mouseY = e.Y;
            if (IsPointInCircle(mouseX, mouseY, CircleCenterX, CircleCenterY, BigRadius))
            {
                toolTip.Show("Сoffee mug", this, e.X, e.Y, 1000);
            }

            else if (IsPointInCircle(mouseX, mouseY, cubeStartPoint.X, cubeStartPoint.Y + cubeStartPoint.Y / 4, cubeSize))
            {
                toolTip.Show("Shugar", this, e.X, e.Y, 1000);
            }
        }

        private bool IsPointInCircle(int mouseX, int mouseY, int circleCenterX, int circleCenterY, int bigRadius)
        {
            return Math.Pow(mouseX - circleCenterX, 2) + Math.Pow(mouseY - circleCenterY, 2)
                <= Math.Pow(bigRadius, 2);
        }
    }
}



