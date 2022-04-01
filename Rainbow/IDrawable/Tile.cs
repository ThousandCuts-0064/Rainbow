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
        private static readonly SolidBrush _brushText = new SolidBrush(Color.DarkGray);
        private static readonly Color _colorBlend = Color.Gray;
        private readonly Font _font;
        private readonly SolidBrush _solidBrush;
        private readonly Pen _pen = new Pen(Color.Black, Game.Unit);
        private readonly string _text;
        private readonly GameModifiers _gameModifiers;
        private readonly int _column;
        public readonly IColorModel _colorModel;
        private float _blendRation;
        public ColorCode ColorCode { get; }
        public Color Color => _solidBrush.Color;
        public PointF Location { get; private set; }

        public Tile(IColorModel colorModel, ColorCode colorCode, GameModifiers gameModifiers, int column)
        {
            _colorModel = colorModel;
            ColorCode = colorCode;
            _column = column;
            _gameModifiers = gameModifiers;
            _solidBrush = new SolidBrush(colorModel.CodeToColor(colorCode));
            _blendRation = 0;
            Location = Game.SpawnLocations[column];
            if (!gameModifiers.HasFlag(GameModifiers.HintButtons)) return;
            if (colorCode.HasFlag(ColorCode.I)) _text += InputManager.MapKeys.Reverse[(ColorCode.I, column)].ToString();
            if (colorCode.HasFlag(ColorCode.II)) _text += InputManager.MapKeys.Reverse[(ColorCode.II, column)].ToString();
            if (colorCode.HasFlag(ColorCode.III)) _text += InputManager.MapKeys.Reverse[(ColorCode.III, column)].ToString();
            _font = new Font(
                FontFamily.GenericMonospace, // Chars are same width and calculations are easy
                Math.Min(Game.TileHeight * 0.5f, Game.TileWidth / _text.Length));
        }

        public override void Draw(Graphics graphics)
        {
            Color colorBase = _solidBrush.Color;

            if (_gameModifiers.HasFlag(GameModifiers.FadingColors))
            {
                _solidBrush.Color = Color.Blend(_colorBlend, _blendRation);
                _blendRation += _blendRation < 1 ? 0.002f : 0;
                _brushText.Color = _colorBlend;
            }

            if (_gameModifiers.HasFlag(GameModifiers.InvertedColors))
                _solidBrush.Color = _solidBrush.Color.Invert();

            RectangleF rectangleF = new RectangleF(Location.X, Location.Y, Game.TileWidth, Game.TileHeight);
            graphics.FillRectangle(_solidBrush, rectangleF);

            rectangleF.Inflate(-Game.HalfUnit, -Game.HalfUnit);
            graphics.DrawRectangle(_pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);

            graphics.DrawString(_text, _font, _brushText, rectangleF.X + rectangleF.Width * 0.5f, rectangleF.Y + rectangleF.Height * 0.5f, _stringFormat);

            _solidBrush.Color = colorBase;
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
