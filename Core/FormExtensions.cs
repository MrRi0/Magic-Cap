using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.Core
{
    public static class FormExtensions
    {
        public static async Task SwitchForms(this Form currentForm, Form newForm)
        {
            newForm.Show();
            await Task.Delay(60);
            currentForm.Hide();
        }
    }
}
