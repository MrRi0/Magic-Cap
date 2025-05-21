using System.Net.Sockets;

namespace GameWinForm.View
{
    partial class PauseForm
    {
        private System.ComponentModel.IContainer components = null;
        private Button _continueButton;
        private Button _settingsButton;
        private Button _exitMenuButton;

        private Size _buttonSize = new Size(200, 75);
        private int _indentVertical;
        private int _indentHorizontal;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Size = new Size(300, 450);
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            BackColor = Color.PeachPuff;
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
    }
}