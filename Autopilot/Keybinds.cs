
using InputSimulatorStandard;
using InputSimulatorStandard.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

namespace Autopilot
{
    class Keybinds
    {
        private HotkeyListener listener = new HotkeyListener();
        public Keybinds()
        {
            Hotkey screenshootButton = new Hotkey(Keys.Control, Keys.P);
            listener.Add(screenshootButton);
            listener.HotkeyPressed += TakeScreenshot;
        }

        private void TakeScreenshot(object sender, HotkeyEventArgs e)
        {
            var simulator = new InputSimulator();
            simulator.Keyboard.KeyPress(VirtualKeyCode.VK_1);
            System.Windows.Forms.MessageBox.Show("Should have pressed button!");
        }
    }
}