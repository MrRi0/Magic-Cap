using GameWinForm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.View
{
    public class SelectSkillForm : UserControl
    {
        private GameModel _model;
        private GameView _view;
        private Size _buttonSize = new Size(200, 75);
        private Size _iconSize = new Size(200, 200);
        private Button[] _buttons;
        private PictureBox[] _icons;
        private Upgrades[] _upgrades;
        private Skills[] _skills;
        private int _windowHeight = Screen.PrimaryScreen.Bounds.Height;
        private int _windowWidth = Screen.PrimaryScreen.Bounds.Width;

        public SelectSkillForm(GameModel model, GameView view)
        {
            _model = model;
            _view = view;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Size = new Size(900, 450);
            BackColor = Color.FromArgb(234, 192, 147);
            Location = new Point((_windowWidth - Size.Width) / 2, (_windowHeight - Size.Height) / 2);
            BorderStyle = BorderStyle.FixedSingle;
            Padding = new Padding(5);
            Name = "ChangeUprgadeForm";
            Text = "Upgrade";

            BackgroundImage = GetSprite("selectUpgradeBg.png");
            BackgroundImageLayout = ImageLayout.Stretch;

            _upgrades = _model.RandomUpgrades;
            _skills = _model.RandomSkills;

            if (_upgrades == null || _upgrades == Array.Empty<Upgrades>())
            {
                _icons = new PictureBox[3];
                for (int i = 0; i < _icons.Length; i++)
                {
                    if (_skills[i] != Skills.NoSkill)
                        _icons[i] = InitializeIcon(_skills[i].ToString(), new Point(45 + (_iconSize.Width + 105) * i, 50));
                }
                CreateButtons<Skills>(_skills);
                _buttons[0].Click += SelectSkillNum1;
                _buttons[1].Click += SelectSkillNum2;
                _buttons[2].Click += SelectSkillNum3;
            }
            else if (_skills == null || _skills == Array.Empty<Skills>())
            {
                _icons = new PictureBox[3];
                for (int i = 0; i < _icons.Length; i++)
                {
                    _icons[i] = InitializeIcon(_upgrades[i].ToString(), new Point(45 + (_iconSize.Width + 105) * i, 50));
                }
                CreateButtons<Upgrades>(_upgrades);
                _buttons[0].Click += SelectUpgradeNum1;
                _buttons[1].Click += SelectUpgradeNum2;
                _buttons[2].Click += SelectUpgradeNum3;
            }
        }

        private void CreateButtons<T>(T[] buttonContent)
        {
            var indentHorizontal = 105;
            _buttons = new Button[3];
            for (int i = 0; i < _buttons.Length; i++)
            {
                var text = buttonContent[i].ToString();
                _buttons[i] = InitializeButton(
                    "button_" + i.ToString(),
                    text,
                    new Point(45 + (_buttonSize.Width + indentHorizontal) * i, 300));
            }
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

        private PictureBox InitializeIcon(string name, Point position)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = GetSprite(name + "Icon.png");
            pictureBox.Location = position;
            pictureBox.Size = _iconSize;
            Controls.Add(pictureBox);
            return pictureBox;
        }

        private void SelectUpgradeNum1(object sender, EventArgs e)
        { 
            _model.UpgradePlayer(_upgrades[0]);
            _view.UpdateSelectSkillForm();
        }
        private void SelectUpgradeNum2(object sender, EventArgs e)
        {
            _model.UpgradePlayer(_upgrades[1]);
            _view.UpdateSelectSkillForm();
        }
        private void SelectUpgradeNum3(object sender, EventArgs e)
        {
            _model.UpgradePlayer(_upgrades[2]);
            _view.UpdateSelectSkillForm();
        }

        private void SelectSkillNum1(object sender, EventArgs e)
        {
            _model.SetPlayerSkill(_skills[0]);
            _view.UpdateSelectSkillForm();
        }
        private void SelectSkillNum2(object sender, EventArgs e)
        {
            _model.SetPlayerSkill(_skills[1]);
            _view.UpdateSelectSkillForm();
        }
        private void SelectSkillNum3(object sender, EventArgs e)
        {
            _model.SetPlayerSkill(_skills[2]);
            _view.UpdateSelectSkillForm();
        }

        private static Bitmap GetSprite(string fileName)
        {
            var fullPath = Path.Combine(
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
                "View", "Image", fileName
            );
            var result = new Bitmap(fullPath);
            result.MakeTransparent(Color.White);
            return result;
        }
    }
}
