using GameWinForm.Controller;
using GameWinForm.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.View
{
    public class GameView : UserControl
    {
        public bool IsPause { get; private set; }
        private readonly GameModel _model;
        private UprgadeSelectForm _uprgadeSelectForm;
        private readonly System.Windows.Forms.Timer _gameLoop;
        public GameView(GameModel model)
        {
            _model = model;
            DoubleBuffered = true;
            IsPause = false;
            _gameLoop = new System.Windows.Forms.Timer { Interval = 16 };
            _gameLoop.Tick += GameLoop_Tick;
            _gameLoop.Start();
        }

        public void PauseGame(PauseForm pauseForm)
        {
            IsPause = !IsPause;
            if (IsPause)
            {
                _model.IsPause = true;
                _gameLoop.Stop();
                pauseForm.BringToFront();
                pauseForm.Show();
            }
            else
            {
                _model.IsPause = false;
                _gameLoop.Start();
                pauseForm.Hide();
            }
        }

        private void GameLoop_Tick(object sender, EventArgs e)
        {
            _model.Update();
            if (_model.IsSelectUpgrade)
            { 
                _uprgadeSelectForm = new UprgadeSelectForm(_model);
                Controls.Add(_uprgadeSelectForm);
                _gameLoop.Stop();
                _uprgadeSelectForm.BringToFront();
                _uprgadeSelectForm.Show();
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var render = new GameRender(_model);

            render.Render(e.Graphics);
        }

        protected override void Dispose(bool disposing)
        {
            _gameLoop.Stop();
            base.Dispose(disposing);
        }
    }
}
