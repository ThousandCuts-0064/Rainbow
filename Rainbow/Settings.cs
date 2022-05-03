using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public struct Settings
    {
        private static readonly StringBuilder _stringBuilder = new StringBuilder();
        public bool AntiAliasing { get; set; }
        public bool PixelOffset { get; set; }
        public TextRenderingHint TextRenderingHint { get; set; }

        public override bool Equals(object obj) => 
            obj is Settings settings && settings == this;

        public override int GetHashCode() =>
            ((AntiAliasing ^ PixelOffset) ? 10 : 1) * (int)TextRenderingHint;

        public override string ToString() =>
            _stringBuilder.Clear()
            .Append(nameof(AntiAliasing)).Append(": ").Append(AntiAliasing).Append("; ")    
            .Append(nameof(PixelOffset)).Append(": ").Append(PixelOffset).Append("; ")
            .Append(nameof(TextRenderingHint)).Append(": ").Append(TextRenderingHint).Append("; ")
            .ToString();

        public static bool operator ==(Settings l, Settings r) =>
            l.AntiAliasing == r.AntiAliasing &&
            l.PixelOffset == r.PixelOffset &&
            l.TextRenderingHint == r.TextRenderingHint;
        public static bool operator !=(Settings l, Settings r) => !(r == l);
    }
}
