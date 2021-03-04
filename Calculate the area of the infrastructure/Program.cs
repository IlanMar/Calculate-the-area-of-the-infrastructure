using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Calculate_the_area_of_the_infrastructure
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Lines> LinesList = new List<Lines>();//список объектов типа линия
            List<PointF> vertexList = new List<PointF>();//список координат многоугольника
            List<Point> linesCoord = new List<Point>();
            Console.WriteLine("K");//количество вершин
            int coordinatesCount = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Pairs of coordinates");
            var pairsOfCoordinates = Console.ReadLine().Split(" ");

            for (int i = 0; i < pairsOfCoordinates.Length - 1; i = i + 2)
            {
                PointF temp = new PointF(float.Parse(pairsOfCoordinates[i]), float.Parse(pairsOfCoordinates[i + 1]));
                vertexList.Add(temp);
            }

            Console.WriteLine("N");//количество прямых линий
            int linearObjectsCount = Int32.Parse(Console.ReadLine());
            List<int[]> linearObjectsList = new List<int[]>();//список вершин многоугольника

            for (int i = 0; i < linearObjectsCount; i++)
            {
                int[] ArrayOfPoints = new int[4];
                ArrayOfPoints = Array.ConvertAll(Console.ReadLine().Split(" "), int.Parse);
                Lines lines = new Lines(i, ArrayOfPoints);
                LinesList.Add(lines);
            }

            for (int i = 0; i < LinesList.Count; i++)
            {
                float lineAngle;

                if (LinesList[i].A == 0) lineAngle = 0;
                else if (LinesList[i].B == 0) lineAngle = 90;
                else
                {
                    lineAngle = (float)(((Math.Atan((-1) * (LinesList[i].A / LinesList[i].B))) * 180) / Math.PI);
                }

                bool isParalel = false;//паралельна ли прямая отрезку
                bool segmentCoincide = false;//совпадает ли прямая и отрезок

                for (int j = 0; j < vertexList.Count; j++)//проходим по каждой вершине многоугольника
                {
                    int jj = j;
                    //Находим наклон текущего отрезка
                    float X1 = vertexList[j].X;
                    float Y1 = vertexList[j].Y;

                    if ((j + 1) == vertexList.Count) jj = -1;//на тот случае если мы дошли до последней вершины

                    float X2 = vertexList[jj + 1].X;
                    float Y2 = vertexList[jj + 1].Y;
                    var x = X2 - X1; var y = Y2 - Y1;
                    float sectionAngle;

                    if (x == 0) sectionAngle = 90 * (y > 0 ? 1 : -1);
                    else if (y == 0) sectionAngle = 0;
                    else sectionAngle = (float)((Math.Atan(x / y)) * 180 / Math.PI);//угол наклона отрезка

                    isParalel = lineAngle == sectionAngle ? true : false;//если прямая линия паралельна текущему отрезку то true

                    var temp1 = LinesList[i].A * X1 + LinesList[i].B * Y1 + LinesList[i].C;//проверяем не касаются ли прямая и отрезок           
                    var temp2 = LinesList[i].A * X2 + LinesList[i].B * Y2 + LinesList[i].C;

                    if (temp1 == 0 && temp2 == 0) segmentCoincide = true;

                    if (segmentCoincide)//если прямая паралельна отрезку то можно сразу посчитать площадь прямоугольника
                    {
                        if ((j + 1) == vertexList.Count) jj = -1;//на тот случай если мы дошли до последней вершины то надо чтобы j+1 ссылался на первую вершину
                        var segmentLenght = Helper.CountDist(vertexList[j], vertexList[jj + 1]);
                        var rectArea = Helper.GetRectAres(segmentLenght, LinesList[i].w);
                        LinesList[i].area = rectArea;
                        LinesList[i].firstSection = vertexList[j];
                        LinesList[i].secondSection = vertexList[j];
                    }

                    Lines tempLine = new Lines();

                    //Ищем точку пересечения отрезка и прямой
                    if (!segmentCoincide)
                    {
                        var z = IntersectionsCheck.CheckIntersectionPoint(LinesList[i], vertexList[j], vertexList[jj + 1]); //метод возвращает точку пересечения или возвращает 999
                        if (z.X != 999)
                        {
                            if (LinesList[i].firstSection.IsEmpty && LinesList[i].firstSectionIntersectionPoint.IsEmpty)
                            {
                                LinesList[i].firstSection = vertexList[j];
                                LinesList[i].firstSectionIntersectionPoint = z;
                                LinesList[i].angleBetweenFeirstSection = lineAngle - sectionAngle;
                            }
                            else
                            {
                                LinesList[i].secondSection = vertexList[j];
                                LinesList[i].secondSectionIntersectionPoint = z;
                                LinesList[i].angleBetweenSecondSection = lineAngle - sectionAngle;
                            }
                        }
                    }
                }
            }


            float totalArea = 0;

            for (int i = 0; i < LinesList.Count; i++) //Перебираем все линии и считаем площадь 
            {
                if (!LinesList[i].firstSectionIntersectionPoint.IsEmpty && !LinesList[i].secondSectionIntersectionPoint.IsEmpty)
                {
                    var figureLenght = Helper.CountDist(LinesList[i].firstSectionIntersectionPoint, LinesList[i].secondSectionIntersectionPoint);
                    PointF p1 = Helper.GetDefinitePointOnSection(LinesList[i].firstSection, LinesList[i].firstSectionIntersectionPoint, 1, LinesList[i].w / 2);
                    PointF p2 = Helper.GetDefinitePointOnSection(LinesList[i].firstSection, LinesList[i].firstSectionIntersectionPoint, -1, LinesList[i].w / 2);
                    var t = Helper.CountDist(p1, p2);

                    PointF p3 = Helper.GetDefinitePointOnSection(LinesList[i].secondSection, LinesList[i].secondSectionIntersectionPoint, 1, LinesList[i].w / 2);
                    PointF p4 = Helper.GetDefinitePointOnSection(LinesList[i].secondSection, LinesList[i].secondSectionIntersectionPoint, -1, LinesList[i].w / 2);
                    var t2 = Helper.CountDist(p3, p4);

                    LinesList[i].area = ((t + t2) / 2) * figureLenght;
                    totalArea = totalArea + LinesList[i].area;
                }
                if (LinesList[i].firstSection == LinesList[i].secondSection)
                {
                    totalArea = totalArea + LinesList[i].area;
                }
            }

            for (int i = 0; i < LinesList.Count; i++)//Подсчитываем площадь пересечений под линиями внутри выпуклого многоугольника
            {
                for (int j = 0; j < LinesList.Count - 1; j++)
                {
                    float delta = LinesList[i].A * LinesList[j].B - LinesList[j].A * LinesList[i].B;
                    // if (delta == 0)//линии паралельны  
                    PointF intersect = new PointF();
                    intersect.X = (LinesList[j].B * LinesList[i].C - LinesList[i].B * LinesList[j].C) / delta;
                    intersect.Y = (LinesList[i].A * LinesList[j].C - LinesList[j].A * LinesList[i].C) / delta;
                    LinesList[i].intersectLinesNumList.Add(intersect);
                    var aX = LinesList[i].firstSectionIntersectionPoint.X;
                    var aY = LinesList[i].firstSectionIntersectionPoint.Y;
                    var bX = LinesList[j].firstSectionIntersectionPoint.X;
                    var bY = LinesList[j].firstSectionIntersectionPoint.Y;
                    var isOnSegment = ((intersect.X - aX) / (bX - aX) != (intersect.Y - aY) / (bY - aY));

                    if (isOnSegment)
                    {
                        float interArea = (LinesList[i].w * LinesList[j].w) / 2;
                        totalArea = totalArea - interArea;
                    }
                }
            }

            Console.WriteLine("Area under linear objects");
            Console.WriteLine((int)totalArea);
        }
    }


}
