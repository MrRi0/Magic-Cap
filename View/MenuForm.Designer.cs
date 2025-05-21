namespace GameWinForm.View
{
    partial class MenuForm
    {
        private System.ComponentModel.IContainer components = null;
        private Button _playButton;
        private Button _settingsButton;
        private Button _exitButton;

        private int _windowHeight = Screen.PrimaryScreen.Bounds.Height;
        private int _windowWidth = Screen.PrimaryScreen.Bounds.Width;

        private Size _bigButtonSize = new Size(280, 70);
        private Size _smallButtonSize = new Size(260, 60);


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
            _playButton = new Button();
            _settingsButton = new Button();
            _exitButton = new Button();
            SuspendLayout();

            InitializePlayButton();

            InitializeSettingsButton();

            InitializeExitButton();

            Controls.Add(_exitButton);
            Controls.Add(_settingsButton);
            Controls.Add(_playButton);
            Size = new Size(0, 0);
            Name = "MenuForm";
            Text = "MenuForm";
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximizeBox = false;
            ResumeLayout(false);
        }

        private void InitializeExitButton()
        {
            _exitButton.Location = new Point((_windowWidth - _smallButtonSize.Width) / 2, _settingsButton.Location.Y + _settingsButton.Size.Height / 2 + 40);
            _exitButton.Name = "exitButton";
            _exitButton.Size = _smallButtonSize;
            _exitButton.TabIndex = 2;
            _exitButton.Text = "Выход";
            _exitButton.UseVisualStyleBackColor = true;
            _exitButton.Click += ExitButtonClick;
        }

        private void InitializeSettingsButton()
        {
            _settingsButton.Location = new Point((_windowWidth - _smallButtonSize.Width) / 2, _playButton.Location.Y + _playButton.Size.Height / 2 + 40);
            _settingsButton.Name = "settingButton";
            _settingsButton.Size = _smallButtonSize;
            _settingsButton.TabIndex = 1;
            _settingsButton.Text = "Настройки";
            _settingsButton.UseVisualStyleBackColor = true;
        }

        private void InitializePlayButton()
        {
            _playButton.Location = new Point((_windowWidth - _bigButtonSize.Width) / 2, (_windowHeight - _bigButtonSize.Height) / 2);
            _playButton.Name = "playButton";
            _playButton.Size = _bigButtonSize;
            _playButton.TabIndex = 0;
            _playButton.Text = "Играть";
            _playButton.UseVisualStyleBackColor = true;
            _playButton.Click += PlayButtonClick;
        }
    }
}