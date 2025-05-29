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
        //public bool IsPause { get; private set; }
        private bool IsPause;
        private bool IsSelectSkill;
        private readonly GameModel _model;
        private SelectSkillForm _uprgadeSelectForm;
        private readonly PauseForm _pauseForm;
        private readonly System.Windows.Forms.Timer _gameLoop;

        private GameRender render;
        public GameView(GameModel model, MainForm mainForm)
        {
            _model = model;
            render = new GameRender(_model);
            DoubleBuffered = true;
            IsPause = false;
            _pauseForm = new PauseForm(this, mainForm);
            Controls.Add(_pauseForm);
            _pauseForm.Hide();
            _gameLoop = new System.Windows.Forms.Timer { Interval = 16 };
            _gameLoop.Tick += GameLoop_Tick;
            _gameLoop.Start();
        }

        public void PauseGame()
        {
            IsPause = !IsPause;
            if (IsPause)
            {
                _model.IsPause = true;
                _gameLoop.Stop();
                _pauseForm.BringToFront();
                _pauseForm.Show();
            }
            else
            {
                _model.IsPause = false;
                _gameLoop.Start();
                _pauseForm.Hide();
            }
        }

        public void UpdateSelectSkillForm()
        {
            IsSelectSkill = !IsSelectSkill;
            if (IsSelectSkill)
            {            
                _uprgadeSelectForm = new SelectSkillForm(_model, this);
                Controls.Add(_uprgadeSelectForm);
                _gameLoop.Stop();
                _uprgadeSelectForm.BringToFront();
                _uprgadeSelectForm.Show();
            }
            else
            {
                _gameLoop.Start();
                _uprgadeSelectForm.Dispose();
            }

        }

        private void GameLoop_Tick(object sender, EventArgs e)
        {
            _model.Update();
            if (_model.IsSelectUpgrade)
            {
                UpdateSelectSkillForm();
            }
            if (_model.IsPause)
            {
                PauseGame();
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            //var render = new GameRender(_model);

            render.Render(e.Graphics);
        }

        protected override void Dispose(bool disposing)
        {
            _gameLoop.Stop();
            base.Dispose(disposing);
        }
    }
}
