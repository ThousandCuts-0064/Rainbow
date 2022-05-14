using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public readonly struct ColorColumn
    {
        public const string NO_COLOR_STRING = "_";
        public const string EMPTY_STRING = " ";
        public const int NO_COLUMN = -1;
        private static readonly IReadOnlyMap<ColorColumn, string> _mapChars;

        public ColorCode ColorCode { get; }
        public int Column { get; }

        static ColorColumn()
        {
            var toInput = new Map<ColorColumn, string>()
            {
                { (ColorCode.I, 0), "Q" }, { (ColorCode.II, 0), "A" }, { (ColorCode.III, 0), "Z" },
                { (ColorCode.I, 1), "W" }, { (ColorCode.II, 1), "S" }, { (ColorCode.III, 1), "X" },
                { (ColorCode.I, 2), "E" }, { (ColorCode.II, 2), "D" }, { (ColorCode.III, 2), "C" },
                { (ColorCode.I, 3), "R" }, { (ColorCode.II, 3), "F" }, { (ColorCode.III, 3), "V" },
                { (ColorCode.I, 4), "T" }, { (ColorCode.II, 4), "G" }, { (ColorCode.III, 4), "B" },
                { (ColorCode.I, 5), "Y" }, { (ColorCode.II, 5), "H" }, { (ColorCode.III, 5), "N" },
                { (ColorCode.I, 6), "U" }, { (ColorCode.II, 6), "J" }, { (ColorCode.III, 6), "M" },
                { (ColorCode.I, 7), "I" }, { (ColorCode.II, 7), "K" }, { (ColorCode.III, 7), "," },
                { (ColorCode.I, 8), "O" }, { (ColorCode.II, 8), "L" }, { (ColorCode.III, 8), "." },
                { (ColorCode.I, 9), "P" }, { (ColorCode.II, 9), ";" }, { (ColorCode.III, 9), "/" },
            };

            for (int i = 0; i < 10; i++)
            {
                //toInput.Add(
                //    (ColorCode.None, i),
                //    EMPTY_COLOR_TO_STRING);

                toInput.Add(
                    (ColorCode.I_II, i),
                    toInput.Forward[(ColorCode.I, i)] +
                        toInput.Forward[(ColorCode.II, i)]);

                toInput.Add(
                    (ColorCode.I_III, i),
                    toInput.Forward[(ColorCode.I, i)] +
                        toInput.Forward[(ColorCode.III, i)]);

                toInput.Add(
                    (ColorCode.II_III, i),
                    toInput.Forward[(ColorCode.II, i)] +
                        toInput.Forward[(ColorCode.III, i)]);

                toInput.Add(
                    (ColorCode.All, i),
                    toInput.Forward[(ColorCode.I, i)] +
                        toInput.Forward[(ColorCode.II, i)] +
                        toInput.Forward[(ColorCode.III, i)]);
            }

            _mapChars = toInput;
        }

        public ColorColumn(ColorCode colorCode, int column)
        {
            ColorCode = colorCode;
            Column = column;
        }

        public static ColorColumn FromInput(string str, int defaultColumn = NO_COLUMN) =>
            str == EMPTY_STRING || str == NO_COLOR_STRING
                ? new ColorColumn(ColorCode.None, defaultColumn)
                : _mapChars.Reverse[str];
        public string ToInput() => ColorCode == ColorCode.None ? NO_COLOR_STRING : _mapChars.Forward[this];

        public static implicit operator ColorColumn((ColorCode code, int column) tuple) =>
            new ColorColumn(tuple.code, tuple.column);
    }
}
