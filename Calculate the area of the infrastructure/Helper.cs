using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Calculate_the_area_of_the_infrastructure
{
    static class Helper
    {
        public static PointF GetNextPointOnLine(float Y, float X, Lines line, int dir) //Метод берет следующую точку на прямой, dir указывает направление +-1
        {

            float x;
            if (line.A != 0)//данное условие требуется для случая когда линия горизонтальна и А = 0
            {
                Y = Y + 0.1f * dir;
                x = (((-1) * line.B * Y - line.C) / line.A);
            }
            else
            {
                x = X + 0.1f * dir;               
            }
            PointF temp = new PointF(x, Y);
            return temp;
        }

        public static PointF GetNextPointOnSection(PointF a, PointF b, int dir)//метод возвращает следующую точку на отрезке
        {
            PointF temp = new PointF();
            float rac = 0.1f;
            float rab = (float)(Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2)));
            float k = (rac / rab) * dir;
            temp.X = a.X + (b.X - a.X) * k;
            temp.Y = a.Y + (b.Y - a.Y) * k;

            return temp;
        }

        public static float CountDist(PointF a, PointF b)//метод возвращает расстояние между двумя точками
        {
            float dist = (float)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
            return dist;
        }

        public static float GetRectAres(float segmentLenght, float w)
        {
            return segmentLenght * w / 2;
        }

        public static PointF GetDefinitePointOnSection(PointF a, PointF b, int dir, float step)
        {
            PointF temp = new PointF();
            float rab = (float)(Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2)));
            float k = (step / rab) * dir;
            temp.X = a.X + (b.X - a.X) * k;
            temp.Y = a.Y + (b.Y - a.Y) * k;

            return temp;
        }
    }
}
