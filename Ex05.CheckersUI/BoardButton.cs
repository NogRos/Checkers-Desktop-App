using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Ex05.CheckersUI
{
    internal class BoardButton : Button
    {
        private static Dictionary<eBoardButtonType, Color> s_ColorMap = new Dictionary<eBoardButtonType, Color>()
        {
            { eBoardButtonType.White, Color.LightGoldenrodYellow },
            { eBoardButtonType.Black, Color.SaddleBrown },
        };

        internal BoardButton(eBoardButtonType i_Type, int i_xCord, int i_yCord)
            : base()
        {
            BackColor = s_ColorMap[i_Type];
            Type = i_Type;
            X = i_xCord;
            Y = i_yCord;
        }

        internal enum eBoardButtonType
        {
            White = 1,
            Black,
        }

        internal eBoardButtonType Type { get; }

        internal int X { get; }

        internal int Y { get; }

        internal void Highlight()
        {
            BackColor = Color.FromArgb(127, 181, 255);
        }

        internal void Dehighlight()
        {
            BackColor = s_ColorMap[Type];
            FlatStyle = FlatStyle.Standard;
            FlatAppearance.BorderSize = 0;
        }

        internal void HighlightAsDestination()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderColor = Color.FromArgb(202, 130, 255);
            FlatAppearance.BorderSize = 5;
        }
    }
}
