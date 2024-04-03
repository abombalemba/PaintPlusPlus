using System.Drawing;

namespace KPFU_2_sem_programming_PaintPlusPlus {
    public class ArrayPoints {
        private int index = 0;
        private Point[] points;

        public ArrayPoints(int size) {
            if (size <= 0) {
                size = 2;
            }

            points = new Point[size];
        }

        public void setPoint(int x, int y) {
            if (index >= points.Length) {
                index = 0;
            }

            points[index] = new Point(x, y);
            ++index;
        }

        public void resetPoints() {
            index = 0;
        }

        public int getCountPoints() {
            return index;
        }

        public Point[] getPoints() {
            return points;
        }
    }
}
