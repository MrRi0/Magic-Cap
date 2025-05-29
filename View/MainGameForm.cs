using GameWinForm.Controller;
using GameWinForm.Core;
using GameWinForm.Model;
using System.Windows.Forms;

namespace GameWinForm.View
{
    public class MainForm : Form
    {
        private readonly GameModel _model;
        private readonly InputController _inputController;
        private readonly MouseController _mouseController;
        private readonly GameView _gameView;
        private readonly MenuForm _menuForm;
        private readonly PauseForm _pauseForm;

        public MainForm(MenuForm menuForm, int levelNumber)
        {
            _menuForm = menuForm;

            _model = new GameModel(levelNumber);
            _inputController = new InputController(_model);
            _mouseController = new MouseController(_model);
            _gameView = new GameView(_model, this)
            {
                Dock = DockStyle.Fill
            };
            _pauseForm = new PauseForm(_gameView, this);
            Controls.Add(_gameView);

            _gameView.MouseDown += OnMouseDown;
            _gameView.MouseUp += OnMouseUp;
            _gameView.MouseMove += OnMouseMove;

            KeyPreview = true;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _mouseController.HandleMouseDown(e);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            _mouseController.HandleMouseUp(e);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            _mouseController.HandleMouseMove(e);
        }

        public void BackToMenu()
        {
            this.SwitchForms(_menuForm);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            _inputController.HandleKeyDown(e.KeyCode);
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            _inputController.HandleKeyUp(e.KeyCode);
            base.OnKeyUp(e);
        }


    }
}

