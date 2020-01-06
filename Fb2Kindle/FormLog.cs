using System.Windows.Forms;

namespace Fb2Kindle
{
    public partial class FormLog : Form
    {
        public FormLog()
        {
            InitializeComponent();
        }

        public void SetText(string text)
        {
            textLog.Text = text;
            textLog.Select(0, 0);
        }
    }
}
