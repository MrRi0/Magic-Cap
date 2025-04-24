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

        public GameView(GameModel model)
        {
            _model = model;
            DoubleBuffered = true;

            _gameLoop = new System.Windows.Forms.Timer { Interval = 16 };
            _gameLoop.Tick += GameLoop_Tick;
            _gameLoop.Start();
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
