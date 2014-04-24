using Microsoft.Xna.Framework;

namespace System.Windows.Media {
    public class GradientStop {
        public GradientStop () { }

        public GradientStop (float offset, Color color) {
            Offset = offset;
            Color = color;
        }

        public override bool Equals (object obj) {
            var gs = obj as GradientStop;
            return gs != null
                && gs.Offset == this.Offset
                && gs.Color == this.Color;
        }

        public override int GetHashCode () {
            return (this.Offset.ToString() + ";"
                        + this.Color.PackedValue.ToString() + ";").GetHashCode();
        }

        public float Offset { get; set; }
        public Color Color { get; set; }
    }
}
