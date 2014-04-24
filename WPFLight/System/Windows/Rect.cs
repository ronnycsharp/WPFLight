using System;
namespace System.Windows {
    public struct Rect {
        public Rect (float left, float top, float width, float height) {
            this.x = left;
            this.y = top;
            this.width = width;
            this.height = height;
        }

        public Rect (Point p1, Point p2) {
            var left = Math.Min(p1.X, p2.X);
            var top = Math.Min(p1.Y, p2.Y);
            var right = Math.Max(p1.X, p2.X);
            var bottom = Math.Max(p1.Y, p2.Y);

            this.x = left;
            this.y = top;
            this.width = right - left;
            this.height = bottom - top;
        }

        #region Eigenschaften

        public float X { get { return x; } set { x = value; } }
        public float Y { get { return y; } set { y = value; } }

        public float Left { get { return x; } }
        public float Top { get { return y; } }

        public float Width { get { return width; } set { width = value; } }
        public float Height { get { return height; } set { height = value; } }

        public float Right {
            get { return x + width; }
        }

        public float Bottom {
            get { return y + height; }
        }

        #endregion

        private float x;
        private float y;
        private float width;
        private float height;
    }
}