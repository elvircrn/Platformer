using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLPlatformer
{
    class window
    {
        public static int Width, Height;

        public static void Set(int Width, int Height)
        {
            window.Width = Width;
            window.Height = Height;
        }
    }
}
