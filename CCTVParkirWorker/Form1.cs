using Microsoft.EntityFrameworkCore;
using MonPDLib;

namespace CCTVParkirWorker
{
    public partial class Form1 : Form
    {
        List<ParkirView> parkirViews = new List<ParkirView>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var _cont = DBClass.GetContext();
            //var pl= _cont.MOpParkirCctvs.Include(x=>x.jasni)
            //parkirViews= 
        }
    }
}
