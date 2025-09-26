using MonPDLib;

namespace ParkirCctvDs
{
    public partial class ParkirCctv : Form
    {
        public ParkirCctv()
        {
            InitializeComponent();

            this.Load += ParkirCctv_Load;
        }

        private void ParkirCctv_Load(object sender, EventArgs e)
        {
            // Load data into ComboBox
            // Load Wilayah;
            var wilayahList = DataWilayah();
            cbWilayah.DataSource = wilayahList;
            cbWilayah.DisplayMember = "Text";
            cbWilayah.ValueMember = "Value";
            cbWilayah.SelectedIndex = -1;
        }


        public List<ComboBoxModel> DataWilayah()
        {
            var result = new List<ComboBoxModel>();
            var context = DBClass.GetContext();

            var wilayahList = context.MWilayahs
                .OrderBy(x => x.Uptd)
                .Select(x => x.Uptd)
                .Distinct().Select(x => new ComboBoxModel
                {
                    Value = x,
                    Text = x
                }).ToList();

            if(wilayahList.Any())
            {
                result.AddRange(wilayahList);
            }

            return result;
        }


        public class ComboBoxModel
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }
    }
}
