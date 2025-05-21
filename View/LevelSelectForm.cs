using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GameWinForm.View
{
    public partial class LevelSelectForm: Form
    {
        private MainForm _mainForm;
        private MenuForm _menu;
        public LevelSelectForm(MenuForm menu)
        {
            _menu = menu;
            InitializeComponent();
        }

        private void LevelButtonClick(object sender, EventArgs e)
        {
            _mainForm = new MainForm(this);
            _mainForm.Show();
            Close();
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            _menu.Show();
            Close();
        }
    }
}
