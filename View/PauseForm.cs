using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.View
{
    public class PauseForm : UserControl
    {
        private GameView _gameView;
        private MainForm _mainForm;
        private Button _continueButton;
        private Button _settingsButton;
        private Button _exitMenuButton;

        private Size _buttonSize = new Size(200, 75);
        private int _indentVertical;
        private int _indentHorizontal;

        private int _windowHeight = Screen.PrimaryScreen.Bounds.Height;
        private int _windowWidth = Screen.PrimaryScreen.Bounds.Width;

        public PauseForm(GameView gameView, MainForm mainForm)
        {
            _mainForm = mainForm;
            _gameView = gameView;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Size = new Size(300, 450);
            BackColor = Color.PeachPuff;
            Location = new Point((_windowWidth - Size.Width) / 2, (_windowHeight - Size.Height) / 2);
            BorderStyle = BorderStyle.FixedSingle;
            Padding = new Padding(5);
            Name = "PauseForm";
            Text = "Pause";

            _indentVertical = ((Size.Height - 150) - _buttonSize.Height * 3) / 4;
            _indentHorizontal = (Size.Width - _buttonSize.Width) / 2;

            _continueButton = new Button();
            _settingsButton = new Button();
            _exitMenuButton = new Button();

            InitializeButton(_continueButton, "_continueButton", "Продолжить",
                new Point(_indentHorizontal, 100 + _indentHorizontal), ContinueButtonClick);
            InitializeButton(_settingsButton, "_settingsButton", "Настройки",
                new Point(_indentHorizontal, _continueButton.Location.Y + _buttonSize.Height + _indentVertical), SettingsButtonClick);
            InitializeButton(_exitMenuButton, "_exitButton", "Выйти в меню",
                new Point(_indentHorizontal, _settingsButton.Location.Y + _buttonSize.Height + _indentVertical), ExitMenuButtonClick);
        }

        private void InitializeButton(Button button, string name, string text, Point position, EventHandler action)
        {
            button.Location = position;
            button.Name = name;
            button.Size = _buttonSize;
            button.TabIndex = 0;
            button.Text = text;
            button.UseVisualStyleBackColor = true;
            button.Click += action;
            Controls.Add(button);
        }

        private void ContinueButtonClick(object sender, EventArgs e) => _gameView.PauseGame();

        private void SettingsButtonClick(object sender, EventArgs e)
        { }

        private void ExitMenuButtonClick(object sender, EventArgs e) => _mainForm.BackToMenu();
    }
}
