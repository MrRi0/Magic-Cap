using GameWinForm.Model;

namespace GameWinForm.View
{
    partial class LevelSelectForm
    {
        private System.ComponentModel.IContainer components = null;
        private List<Button> _levelButtons;

        private int _windowHeight = Screen.PrimaryScreen.Bounds.Height;
        private int _windowWidth = Screen.PrimaryScreen.Bounds.Width;

        private Size _buttonSize = new Size(160, 160);
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
            InitializeBackButton();

            _indentVertical = (_windowHeight / 3 * 2 - _buttonSize.Height * 2) / 3;
            _indentHorizontal = (_windowWidth - _buttonSize.Width * 3) / 4;
            _levelButtons = new List<Button>();
            var levelCount = 1;
            for (int i = 0; i < levelCount; i++)
                _levelButtons.Add(new Button());

            SuspendLayout();

            InitializeLevelButtons(levelCount);
            Size = new Size(0, 0);
            MaximizeBox = false;
            Name = "LevelSelectForm";
            Text = "MenuForm";
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            ResumeLayout(false);
        }

        private void InitializeBackButton()
        {
            var backButton = new Button();
            backButton.Location = new Point(20, 20);
            backButton.Name = "_back";
            backButton.Size = new Size(80, 40);
            backButton.TabIndex = 0;
            backButton.Text = "Назад";
            backButton.UseVisualStyleBackColor = true;
            backButton.Click += BackButtonClick;
            Controls.Add(backButton);
        }

        public void InitializeLevelButtons(int levelCount)
        {
            for (int i = 0; i < levelCount; i++)
            {
                var level = _levelButtons[i];
                level.Name = "_level" + (i + 1).ToString();
                level.Size = _buttonSize;
                level.TabIndex = i;
                level.Text = (i + 1).ToString();
                level.Font = new Font("Microsoft Sans Serif", 16);
                level.UseVisualStyleBackColor = true;
                if (i < 3)
                {
                    level.Location = new Point(_indentHorizontal + (_indentHorizontal + _buttonSize.Width) * i, _windowHeight / 3);
                }
                else
                {
                    level.Location = new Point(_indentHorizontal + (_indentHorizontal + _buttonSize.Width) * (i % 3),
                        _windowHeight / 3 + _buttonSize.Height + _indentVertical);
                }
                Controls.Add(level);
                level.Click += LevelButtonClick;
            }
        }
    }
}