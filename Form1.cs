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

            FillBackground(g);
            DrawCircles(g);
            DrawRotatedRectangle(g);
            DrawLines(g, Color.Black, Color.White);
            DrawMiniCircles(g);
            DrawAndFillPath(g, Color.FromArgb(86, 50, 24));
            DrawRectangles(g);
        }

        private void FillBackground(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(238, 28, 37)), 0, 0, Width, Height);
        }

        private void DrawCircles(Graphics g)
        {
            DrawCircle(g, CircleCenterX, CircleCenterY, 150, Color.FromArgb(252, 206, 170));
            DrawCircle(g, CircleCenterX, CircleCenterY, 110, Color.FromArgb(254, 245, 204));
            DrawCircle(g, CircleCenterX, CircleCenterY, 90, Color.FromArgb(162, 106, 76));
        }

        private void DrawCircle(Graphics g, int posX, int posY, int radius, Color color)
        {
            g.FillEllipse(new SolidBrush(color), posX - radius, posY - radius, radius * 2, radius * 2);
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
        private void DrawCube(Graphics g, int x, int y, int size)
        {
            // Определение координат вершин куба
            Point[] points = new Point[]
            {
        new Point(x, y), // Верхний левый передний угол
        new Point(x + size, y), // Верхний правый передний угол
        new Point(x + size, y + size), // Нижний правый передний угол
        new Point(x, y + size), // Нижний левый передний угол
        new Point(x + size / 4, y - size / 4), // Верхний левый задний угол (с учетом перспективы)
        new Point(x + size + size / 4, y - size / 4), // Верхний правый задний угол (с учетом перспективы)
        new Point(x + size + size / 4, y + size - size / 4), // Нижний правый задний угол (с учетом перспективы)
        new Point(x + size / 4, y + size - size / 4)  // Нижний левый задний угол (с учетом перспективы)
            };

            // Нарисовать линии, соединяющие вершины
            Pen pen = new Pen(Color.White, 2);

            // Рисуем переднюю грань
            g.DrawLine(pen, points[0], points[1]); // Верхняя линия
            g.DrawLine(pen, points[1], points[2]); // Правая линия
            g.DrawLine(pen, points[2], points[3]); // Нижняя линия
            g.DrawLine(pen, points[3], points[0]); // Левая линия

            // Рисуем заднюю грань
            g.DrawLine(pen, points[4], points[5]); // Верхняя линия
            g.DrawLine(pen, points[5], points[6]); // Правая линия
            g.DrawLine(pen, points[6], points[7]); // Нижняя линия
            g.DrawLine(pen, points[7], points[4]); // Левая линия

            // Соединяем переднюю и заднюю грани
            g.DrawLine(pen, points[0], points[4]); // Верхний левый угол
            g.DrawLine(pen, points[1], points[5]); // Верхний правый угол
            g.DrawLine(pen, points[2], points[6]); // Нижний правый угол
            g.DrawLine(pen, points[3], points[7]); // Нижний левый угол
        }
        private void DrawRectangles(Graphics g)
        {
            int x = 150;
            int y = 195;
            int size = 40;
            Color color1 = Color.White;
            Color color2 = Color.Brown;

            DrawRectange(g, x, y, size, size, color1, color2);
            DrawRectange(g, x, y + size + size / 10, size, size, color1, color2);
            DrawRectange(g, x - size / 8, y + size / 2, size, size, color1, color2);
        }
        private void DrawRectange(Graphics g, int x, int y, int width, int height, Color color1, Color color2)
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
                toolTip.Show("This is the circle", this, e.X, e.Y, 1000);
            }
            else if (IsPointInCircle(mouseX, mouseY, 150, 195, 100))
            {
                toolTip.Show("lox", this, e.X, e.Y, 1000);
            }
        }

        private bool IsPointInCircle(int mouseX, int mouseY, int circleCenterX, int circleCenterY, int bigRadius)
        {
            return Math.Pow(mouseX - circleCenterX, 2) + Math.Pow(mouseY - circleCenterY, 2)
                <= Math.Pow(bigRadius, 2);
        }
    }

}



