using Microsoft.EntityFrameworkCore;
using MonPDLib;

namespace CCTVParkirWorker
{
    public partial class Form1 : Form
    {
        List<ParkirView> parkirViews = new List<ParkirView>();
        ApiWorker _ApiWorker = new ApiWorker();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var _cont = DBClass.GetContext();
            var pl = _cont.MOpParkirCctvs.Include(x => x.MOpParkirCctvJasnita).Where(x => x.Vendor == 1).ToList();
            foreach (var item in pl)
            {
                foreach (var det in item.MOpParkirCctvJasnita)
                {
                    parkirViews.Add(new ParkirView()
                    {
                        Id = item.Nop + "-" + det.CctvId.ToString(),
                        NOP = item.Nop,
                        Nama = item.NamaOp,
                        Mode = det.CctvMode == 1 ? "IN" : det.CctvMode == 2 ? "OUT" : "HYBRID",
                        CCTVId = (int)det.CctvId,
                        Alamat = item.AlamatOp,
                        AccessPoint = det.AccessPoint
                    });
                }
            }
            


            _ApiWorker = new ApiWorker("http://202.146.133.26/grpc", "bapendasby", "surabaya2025!!",5,30);
            _ApiWorker.StartWorkers(parkirViews);
            dataGridView1.DataSource = parkirViews;
            dataGridView1.AutoResizeColumns();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text=="Gas")
            {
                button1.Text = "Hop";
                button1.Enabled = false;
            }
            else
            {
                button1.Text = "Gas";
                button1.Enabled = true;
            }
        }
    }
}
