using GameWinForm.Controller;
using GameWinForm.View;
using System.Runtime.CompilerServices;

namespace GameWinForm
{
    static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var startForm = new MenuForm();
            Application.Run(startForm);
        }
    }
}