using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace ConsoleApp1.Core
{
    internal class Colors
    {
        public static RLColor FloorBackground = RLColor.Black;
        public static RLColor Floor = Swatch.Blue5;
        public static RLColor FloorBackgroundFov = Swatch.Blue4;
        public static RLColor FloorFov = Swatch.Blue3;

        public static RLColor WallBackground = Swatch.Cyan5;
        public static RLColor Wall = Swatch.Cyan4;
        public static RLColor WallBackgroundFov = Swatch.Cyan3;
        public static RLColor WallFov = Swatch.Cyan2;

        public static RLColor TextHeading = Swatch.Gold3;

        public static RLColor Player = RLColor.White;
        public static RLColor Kobold = RLColor.Brown;

        public static RLColor DoorBackground = Swatch.ComplimentDarkest;
        public static RLColor Door = Swatch.ComplimentLighter;
        public static RLColor DoorBackgroundFov = Swatch.ComplimentDarker;
        public static RLColor DoorFov = Swatch.ComplimentLightest;
    }
}
