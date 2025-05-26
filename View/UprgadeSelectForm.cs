using GameWinForm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.View
{
    public class UprgadeSelectForm : UserControl
    {
        private GameModel _model;
        private Size _buttonSize = new Size(200, 75);
        private Button[] _buttons;
        private Upgrades[] _upgrades = new Upgrades[3];
        private int _windowHeight = Screen.PrimaryScreen.Bounds.Height;
        private int _windowWidth = Screen.PrimaryScreen.Bounds.Width;

        public UprgadeSelectForm(GameModel model)
        {
            _model = model;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Size = new Size(900, 450);
            BackColor = Color.PeachPuff;
            Location = new Point((_windowWidth - Size.Width) / 2, (_windowHeight - Size.Height) / 2);
            BorderStyle = BorderStyle.FixedSingle;
            Padding = new Padding(5);
            Name = "ChangeUprgadeForm";
            Text = "Upgrade";

            var _upgrades = _model.RandomUpgrades.ToArray();

            var indentHorizontal = (Size.Width - _buttonSize.Width * 3) / 4;
            _buttons = new Button[3];
            for (int i = 0; i < _buttons.Length; i++)
            {
                var text = _upgrades[i].ToString();
                _buttons[i] = InitializeButton(
                    "button_" + i.ToString(),
                    text,
                    new Point(indentHorizontal + (_buttonSize.Width + indentHorizontal) * i, 300));
            }
            _buttons[0].Click += SelectUpgradeNum1;
            _buttons[1].Click += SelectUpgradeNum2;
            _buttons[2].Click += SelectUpgradeNum3;
        }

        private Button InitializeButton(string name, string text, Point position)
        {
            var button = new Button();
            button.Location = position;
            button.Name = name;
            button.Size = _buttonSize;
            button.TabIndex = 0;
            button.Text = text;
            button.UseVisualStyleBackColor = true;
            Controls.Add(button);
            return button;
        }
        //сделать выход из формы
        private void SelectUpgradeNum1(object sender, EventArgs e) => _model.Player.UpgradePlayer(_upgrades[0]);
        private void SelectUpgradeNum2(object sender, EventArgs e) => _model.Player.UpgradePlayer(_upgrades[1]);
        private void SelectUpgradeNum3(object sender, EventArgs e) => _model.Player.UpgradePlayer(_upgrades[2]);
    }
}
