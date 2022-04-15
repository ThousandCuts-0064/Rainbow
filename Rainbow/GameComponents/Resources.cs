using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public static class Resources
    {
        private static readonly string _folder;
        public static Image ColorWheel { get; }
        public static Image Birdy { get; }

        static Resources()
        {
            _folder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + $"\\{nameof(Resources)}\\";

            ColorWheel = Image.FromFile($"{_folder}{nameof(ColorWheel)}.png");
            //Birdy = Image.FromFile($"{_folder}{nameof(Birdy)}.png");
        }
    }
}
