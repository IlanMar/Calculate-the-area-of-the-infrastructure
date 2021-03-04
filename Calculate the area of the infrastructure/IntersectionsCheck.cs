using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Calculate_the_area_of_the_infrastructure
{
    static class IntersectionsCheck
    {
        public static PointF CheckIntersectionPoint(Lines Line, PointF p1, PointF p2)
        {   
            //Ищем пересечения отрезка и прямой линии
            //сначла находим случайную координату на линии в произвольной точке
            int counter = 0;
            float X, Y;
            PointF pointOnLine = new PointF(999, 999);
            PointF pointOnSection = p1;

            if (Line.A != 0)//линия горизонтальна
            {
                pointOnLine.X = (((-1) * Line.B * 1 - Line.C) / Line.A);
                pointOnLine.Y = 1;
            }
            else
            {
                pointOnLine.Y = (-1) * Line.C / Line.B;
                pointOnLine.X = 1;
            }
            
            var dist = Helper.CountDist(pointOnLine, p1);//определяем расстояние между случайной точкой на линии и одним из концов отрезка

            while (dist > 0.2)//в этом цикле осуществляется перебор точек на прямой и отрезке в поиске минимального расстояния между ними
            {
                counter++;
                bool key = true;
                int dir = -1;
                PointF nextPointLine = Helper.GetNextPointOnLine(pointOnLine.Y, pointOnLine.X, Line, dir);
                var newDist = Helper.CountDist(nextPointLine, pointOnSection);

                if (newDist > dist)//если расстояние увеличилось то берем точку в другом направлении
                {
                    dir = dir * (-1);
                    nextPointLine = Helper.GetNextPointOnLine(pointOnLine.Y, pointOnLine.X, Line, dir);
                    newDist = Helper.CountDist(nextPointLine, pointOnSection);
                }

                if (newDist < dist)
                {
                    while (key)
                    {
                        pointOnLine = nextPointLine;
                        float lastDist = newDist;
                        nextPointLine = Helper.GetNextPointOnLine(nextPointLine.Y, pointOnLine.X, Line, dir);
                        newDist = Helper.CountDist(nextPointLine, pointOnSection);

                        if (newDist > lastDist)
                        {
                            dir = dir * (-1);
                            key = false;
                            dist = lastDist;
                        }
                        else
                        {
                            pointOnLine = nextPointLine;
                            dist = newDist;
                        }
                    }
                }
              
                dir = 1;
                pointOnSection = Helper.GetNextPointOnSection(pointOnSection, p2, dir);
                newDist = Helper.CountDist(pointOnSection, pointOnLine);

                if (newDist > dist)
                {
                    dir = dir * (-1);
                    pointOnSection = Helper.GetNextPointOnSection(pointOnSection, p2, dir);
                    newDist = Helper.CountDist(pointOnSection, pointOnLine);
                    dist = newDist;
                }
                else
                {
                    key = true;
                    while (key)
                    {
                        pointOnSection = Helper.GetNextPointOnSection(pointOnSection, p2, dir);
                        newDist = Helper.CountDist(pointOnSection, pointOnLine);

                        if (newDist < dist) dist = newDist;
                        else
                        {
                            dir = dir * (-1);
                            key = false;
                        }
                    }
                }

                if (dist < 0.1) return pointOnLine;

                if (counter > 500)
                {
                    PointF temp = new PointF(999, 999);
                    return temp;
                }
            }
            return pointOnLine;
        }
    }
}
