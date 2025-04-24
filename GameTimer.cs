using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace GameWinForm
{
    public class GameTimer
    {
        private Timer timer;
        private Action updateAction;

        public GameTimer(Action action)
        {
            timer = new Timer { Interval = 16 };
            updateAction = action;
            timer.Tick += (s, e) => updateAction();
        }

        public void Start(int fps)
        {
            timer.Interval = 1000 / fps;
            timer.Start();
        }
    }
}
