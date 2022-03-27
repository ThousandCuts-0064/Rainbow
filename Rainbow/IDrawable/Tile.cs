using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rainbow
{
    public class Tile : GameplayElement
    {
        private static readonly StringFormat _stringFormat = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        private static readonly Brush _brushText = new SolidBrush(Color.DarkGray);
        private readonly Font _font;
        private readonly SolidBrush _solidBrush;
        private readonly Pen _pen = new Pen(Color.Black, Game.Unit);
        private readonly string _text;
        private readonly int _column;
        public readonly IColorModel _colorModel;
        public ColorCode ColorCode { get; }
        public Color Color { get; }
        public PointF Location { get; private set; }

        public Tile(IColorModel colorModel, ColorCode colorCode, int column, bool hintButtons)
        {
            _colorModel = colorModel;
            ColorCode = colorCode;
            Color = colorModel.CodeToColor(colorCode);
            _solidBrush = new SolidBrush(Color);
            _column = column;
            Location = Game.SpawnLocations[column];
            if (!hintButtons) return;
            if (colorCode.HasFlag(ColorCode.I)) 
                _text += InputManager.MapKeys.Reverse[(ColorCode.I, column)].ToString();
            if (colorCode.HasFlag(ColorCode.II)) 
                _text += InputManager.MapKeys.Reverse[(ColorCode.II, column)].ToString();
            if (colorCode.HasFlag(ColorCode.III)) 
                _text += InputManager.MapKeys.Reverse[(ColorCode.III, column)].ToString();
            _font = new Font(
                FontFamily.GenericMonospace, // Chars are same width and calculations are easy
                Math.Min(Game.TileHeight * 0.5f, Game.TileWidth / _text.Length));
        }

        public override void Draw(Graphics graphics)
        {
            RectangleF rectangleF = new RectangleF(Location.X, Location.Y, Game.TileWidth, Game.TileHeight);
            graphics.FillRectangle(_solidBrush, rectangleF);

            rectangleF.Inflate(-Game.HalfUnit, -Game.HalfUnit);
            graphics.DrawRectangle(_pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);

            graphics.DrawString(_text, _font, _brushText, rectangleF.X + rectangleF.Width * 0.5f, rectangleF.Y + rectangleF.Height * 0.5f, _stringFormat);
        }

        public override void Dispose()
        {
            base.Dispose();
            _solidBrush.Dispose();
            _pen.Dispose();
            _font?.Dispose();
        }

        protected override void Update() => Location = new PointF(Location.X,
            Location.Y + Game.TileUnitsPerSecond * Game.Unit * Game.DeltaTime);
    }
}

//private void DrawText()
//{

//    // text and font
//    string text = "one two three four five six seven eight nine ten eleven twelve";

//    Font font = new System.Drawing.Font("Arial", 30, FontStyle.Regular, GraphicsUnit.Point);

//    switch (i)
//    {
//        case 0:
//            picbox.Image = TextDrawing.DrawTextToBitmap(text, font, Color.Red, TextDrawing.DrawMethod.AutosizeAccordingToText, new RectangleF(0, 0, picbox.Width, picbox.Height));
//            break;
//        case 1:
//            picbox.Image = TextDrawing.DrawTextToBitmap(text, font, Color.Red, TextDrawing.DrawMethod.AutoFitInConstantRectangleWithoutWarp, new RectangleF(0, 0, picbox.Width, picbox.Height));
//            break;
//        case 2:
//            picbox.Image = TextDrawing.DrawTextToBitmap(text, font, Color.Red, TextDrawing.DrawMethod.AutoWarpInConstantRectangle, new RectangleF(0, 0, picbox.Width, picbox.Height));
//            break;
//        case 3:
//            picbox.Image = TextDrawing.DrawTextToBitmap(text, font, Color.Red, TextDrawing.DrawMethod.AutoFitInConstantRectangleWithWarp, new RectangleF(0, 0, picbox.Width, picbox.Height));
//            break;
//    }
//    this.Text = ((TextDrawing.DrawMethod)(i)).ToString() + "                      Please resize window size by mouse to see drawing methods differences";
//}
