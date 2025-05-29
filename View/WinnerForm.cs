using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.View
{
    public class WinnerForm : UserControl
    {
        private MainForm _mainForm;
        private Button _exitMenuButton;

        private Size _buttonSize = new Size(200, 75);
        private int _indentVertical;
        private int _indentHorizontal;

        private int _windowHeight = Screen.PrimaryScreen.Bounds.Height;
        private int _windowWidth = Screen.PrimaryScreen.Bounds.Width;

        public WinnerForm(MainForm mainForm)
        {
            _mainForm = mainForm;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Size = new Size(300, 450);
            BackColor = Color.PeachPuff;
            Location = new Point((_windowWidth - Size.Width) / 2, (_windowHeight - Size.Height) / 2);
            BorderStyle = BorderStyle.FixedSingle;
            Padding = new Padding(5);
            Name = "WinnerForm";
            Text = "YouWin";

            BackgroundImage = GetSprite("winnerBg.png");
            BackgroundImageLayout = ImageLayout.Stretch;

            _indentHorizontal = (Size.Width - _buttonSize.Width) / 2;
            _indentVertical = 300;

            _exitMenuButton = new Button();
            InitializeButton(_exitMenuButton, "Exit", "Выйти в меню", new Point(_indentHorizontal, _indentVertical), ExitMenuButtonClick);
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

        private void ExitMenuButtonClick(object sender, EventArgs e) => _mainForm.BackToMenu();

        private static Bitmap GetSprite(string fileName)
        {
            var fullPath = Path.Combine(
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
                "View", "Image", fileName
            );
            var result = new Bitmap(fullPath);
            return result;
        }
    }
}
