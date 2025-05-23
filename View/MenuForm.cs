using GameWinForm.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameWinForm.View
{
    public partial class MenuForm : Form
    {
        private LevelSelectForm _levelSelectForm;
        public MenuForm()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            _levelSelectForm = new LevelSelectForm(this);
            _levelSelectForm.Hide();
            InitializeComponent();
        }

        private void PlayButtonClick(object sender, EventArgs e)
        {
            this.SwitchForms(_levelSelectForm);
        }

        private void ExitButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
