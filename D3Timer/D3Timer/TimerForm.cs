using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace D3Timer
{
    public partial class TimerForm : Form
    {
        private StopWatch stopWatch = new StopWatch();
        
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        private KeyboardHook keyboardHook = new KeyboardHook();

        private State state = State.Stopped;

        public TimerForm()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Location = calcWindowPosition();
            registerHotkeys();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetWindowPos(Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = stopWatch.GetElapsedTimeString();
        }

        private Point calcWindowPosition()
        {
            int x = Screen.PrimaryScreen.Bounds.Width / 2 + 300;
            return new Point(x, 0);
        }

        private void startTimer()
        {
            Show();
            timer.Enabled = true;
            stopWatch.Start();
            state = State.Running;
        }

        private void stopTimer()
        {
            Hide();
            timer.Enabled = false;
            stopWatch.Stop();
            state = State.Stopped;
        }

        private void pauseTimer() {
            timer.Enabled = false;
            state = State.Paused;
        }

        private void registerHotkeys()
        {
            try
            {
                keyboardHook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
                keyboardHook.RegisterHotKey(D3Timer.ModifierKeys.Control | D3Timer.ModifierKeys.Shift, Keys.A);
            } catch (InvalidOperationException e)
            {
                MessageBox.Show("Unable to register unique hotkey :(");
            }
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            switch (state)
            {
                case State.Stopped:
                    startTimer();
                    break;
                case State.Running:
                    pauseTimer();
                    break;
                case State.Paused:
                    stopTimer();
                    break;
            }
        }


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    }

    public enum State
    {
        Running, Paused, Stopped
    }
}
