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
        private static readonly Brush _textBrush = new SolidBrush(Color.DarkGray);
        private readonly SolidBrush _solidBrush;
        private readonly Pen _pen = new Pen(Color.Black, Game.Unit);
        private readonly Font _font = new Font(FontFamily.GenericSansSerif, Math.Min(Game.TileHeight * 0.6f, Game.TileWidth * 0.25f));
        private readonly string _text;
        public Color Color { get; private set; }
        public PointF Location { get; private set; }

        public Tile(Color fill, PointF location, bool hintButtons)
        {
            Color = fill;
            _solidBrush = new SolidBrush(fill);
            Location = location;
            _text = hintButtons ? "Hello!" : null;
        }

        public override void Draw(Graphics graphics)
        {
            RectangleF rectangleF = new RectangleF(Location.X, Location.Y, Game.TileWidth, Game.TileHeight);
            graphics.FillRectangle(_solidBrush, rectangleF);

            rectangleF.Inflate(-Game.HalfUnit, -Game.HalfUnit);
            graphics.DrawRectangle(_pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
            
            graphics.DrawString(_text, _font, _textBrush, rectangleF.X, rectangleF.Y);
        }

        public override void Dispose()
        {
            base.Dispose();
            _solidBrush.Dispose();
            _pen.Dispose();
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
