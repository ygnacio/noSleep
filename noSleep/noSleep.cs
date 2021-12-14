using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace noSleep
{
    public partial class noSleep : Form
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        private const uint KEYEVENTF_KEYUP = 0x2;

        private const int TIME_FACTOR = 1000 * 60;
        private const int DEFAULT_TIME = 10;

        //[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        //public static extern short GetKeyState(int keyCode);

        public noSleep()
        {
            InitializeComponent();
            tbInterval.Text = DEFAULT_TIME.ToString();
            WakeUpTimer.Enabled = false;
            label2.Text = "Timer Stopped";
        }

        private async void WakeUpTimer_Tick(object sender, EventArgs e)
        {
            int i = 0;
            // Activar / desactivar la tecla scroll lock con un intervalo de 1/2 sec
            do
            {
                keybd_event((byte)Keys.Scroll, 0, 0, 0);
                keybd_event((byte)Keys.Scroll, 0, KEYEVENTF_KEYUP, 0);
                ScrollLockStatus.Checked = Control.IsKeyLocked(Keys.Scroll);
                i++;
                await PutTaskDelay(500);
            } while (i < 2);
        }

        private void noSleep_FormClosed(object sender, FormClosedEventArgs e)
        {
            WakeUpTimer.Stop();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (int.TryParse(tbInterval.Text, out int number))
            {
                WakeUpTimer.Interval = number * TIME_FACTOR;
            }
            else
            {
                WakeUpTimer.Interval = DEFAULT_TIME * TIME_FACTOR;
                tbInterval.Text = DEFAULT_TIME.ToString();
            }

            WakeUpTimer.Start();
            label2.Text = "Timer Started";
            tbInterval.Enabled = false;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            WakeUpTimer.Stop();
            label2.Text = "Timer Stopped";
            tbInterval.Enabled = true;
        }

        private async Task PutTaskDelay(int ms)
        {
            await Task.Delay(ms);
        }
    }
}