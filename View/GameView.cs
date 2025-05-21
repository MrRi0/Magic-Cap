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
        private readonly GameModel _model;
        private readonly System.Windows.Forms.Timer _gameLoop;
        private bool _isPause;
        private PauseForm _pauseForm;
        public GameView(GameModel model)
        {
            _model = model;
            DoubleBuffered = true;
            _isPause = false;
            _pauseForm = new PauseForm();
            _gameLoop = new System.Windows.Forms.Timer { Interval = 16 };
            _gameLoop.Tick += GameLoop_Tick;
            _gameLoop.Start();
            
        }

        public void PauseGame()
        {
            _isPause = !_isPause;
            if (_isPause)
            {
                _model.IsPause = true;
                _pauseForm.Show();
                _gameLoop.Stop();
            }
            else
            {
                _model.IsPause = false;
                _pauseForm.Hide();
                _gameLoop.Start();
            }
        }

        private void GameLoop_Tick(object sender, EventArgs e)
        {
            _model.Update();
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
