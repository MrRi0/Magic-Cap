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
            InitializeComponent();
        }

        private void PlayButtonClick(object sender, EventArgs e)
        {
            _levelSelectForm = new LevelSelectForm(this);
            _levelSelectForm.Show();
            Hide();
        }

        private void ExitButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
