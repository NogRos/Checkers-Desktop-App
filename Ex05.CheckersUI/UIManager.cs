using System;
using System.Runtime.InteropServices;

namespace Ex05.CheckersUI
{
    internal class UIManager
    {
        private readonly BoardForm r_BoardForm;

        public UIManager()
        {
            GameSettingsForm gameSettingsForm = new GameSettingsForm();
            GameSettingsForm.InitPackage initPackage = gameSettingsForm.GetInitPackage();
            if (initPackage != null)
            {
                r_BoardForm = new BoardForm(initPackage);
                AnimateWindow(r_BoardForm.Handle, 1000, AnimateWindowFlags.AW_CENTER);
                r_BoardForm.ShowDialog();
            }
        }

        [DllImport("user32")]
        internal static extern bool AnimateWindow(IntPtr handle, int time, AnimateWindowFlags flag);

        internal enum AnimateWindowFlags : uint
        {
            AW_ACTIVATE = 0x00020000,
            AW_BLEND = 0x00080000,
            AW_CENTER = 0x00000010,
            AW_HIDE = 0x00010000,
            AW_HOR_POSITIVE = 0x00000001,
            AW_HOR_NEGATIVE = 0x00000002,
            AW_SLIDE = 0x00040000,
            AW_VER_POSITIVE = 0x00000004,
            AW_VER_NEGATIVE = 0x00000008,
        }
    }
}