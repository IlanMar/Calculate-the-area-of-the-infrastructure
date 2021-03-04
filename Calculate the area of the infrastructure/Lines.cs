using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Calculate_the_area_of_the_infrastructure
{
    class Lines
    {
        public Lines() { }
        public Lines(int lineNumber, int[] arrayWariable)
        {
            this.lineNumber = lineNumber;
            w = arrayWariable[0];
            A = arrayWariable[1];
            B = arrayWariable[2];
            C = arrayWariable[3];
        }

        public int lineNumber;
        public float w;
        public float A, B, C;
        public PointF firstSection;
        public PointF firstSectionIntersectionPoint;
        public float angleBetweenFeirstSection;
        public PointF secondSection;
        public PointF secondSectionIntersectionPoint;
        public float angleBetweenSecondSection;
        public float area;
        public List<PointF> intersectLinesNumList = new List<PointF>();        
    }
}
