using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rainbow
{
    class GameString : GameObject
    {
        private static readonly StringFormat _defaultFormat = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        private readonly StringFormat _format;
        private readonly SolidBrush _brush;
        private readonly Font _font;
        public Color Color { get => _brush.Color; set => _brush.Color = value; }
        public RectangleF Rectangle { get; set; }
        public string String { get; set; }

        public GameString(string @string, RectangleF rectangle, Color color, Layer layer) : base(layer)
        {
            _format = (StringFormat)_defaultFormat.Clone();
            String = @string;
            Rectangle = rectangle;
            _brush = new SolidBrush(color);
            _font = new Font(
                FontFamily.GenericMonospace, 
                Math.Min(rectangle.Width * 0.9f / @string.Length, rectangle.Height * 0.5f));
        }

        public override void Draw(Graphics graphics) =>
            graphics.DrawString(String, _font, _brush, Rectangle, _format);

        public override void Dispose()
        {
            base.Dispose();
            _format.Dispose();
            _font.Dispose();
            _brush.Dispose();
        }
    }
}
