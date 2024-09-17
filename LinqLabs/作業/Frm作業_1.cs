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

namespace MyHomeWork
{
    public partial class Frm作業_1 : Form
    {
        public Frm作業_1()
        {
            InitializeComponent();


        }
        private void Frm作業_1_Load(object sender, EventArgs e)
        {
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
            this.order_DetailsTableAdapter1.Fill(this.nwDataSet1.Order_Details);
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            var orderYear = from odYear in this.nwDataSet1.Orders
                            select odYear.OrderDate.Year;
            this.comboBox1.DataSource = orderYear.Distinct().ToList();
        }
        private void button14_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            var logFile = from n in files
                          where n.Extension == ".log"
                          select n;
            //files[0].CreationTime
            //this.dataGridView1.DataSource = files;
            this.dataGridView1.DataSource = logFile.ToList();
        }
        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            var created2019File = from n in files
                                  where n.CreationTime.Year == 2019
                                  select n;
            this.dataGridView1.DataSource = created2019File.ToList();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            var bigFile = from n in files
                          where Convert.ToInt32(n.Length) >= 10000 //自定義10000以上為大檔案
                          select n;
            this.dataGridView1.DataSource = bigFile.ToList();
            //this.dataGridView1.DataSource = files;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            DisplayOrderInfo();
        }

        private void DisplayOrderInfo()
        {
            this.lblDetails.Text = "訂單明細";


            var orders = from order in this.nwDataSet1.Orders
                         where !(order.IsShipRegionNull() || order.IsShippedDateNull())
                         select order;
            this.dataGridView1.DataSource = orders.ToList();

            Display1stOrderDetail();
        }
        private void Display1stOrderDetail()
        {
            int orderID = Convert.ToInt32(dataGridView1.Rows[0].Cells
                            [dataGridView1.Columns["OrderID"].Index].Value);

            var orderDetail = from detail in this.nwDataSet1.Order_Details
                              where detail.OrderID == orderID
                              select detail;

            this.dataGridView2.DataSource = orderDetail.ToList();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                DisplayOrderInfo();
            }

            else
            {
                this.lblDetails.Text = "訂單明細";

                var orders = from order in this.nwDataSet1.Orders
                             where !(order.IsShipRegionNull() || order.IsShippedDateNull())
                             & order.OrderDate.Year == Convert.ToInt32(comboBox1.Text)
                             select order;
                this.dataGridView1.DataSource = orders.ToList();

                Display1stOrderDetail();
            }
        }
        int _pages = -1;
        private void button12_Click(object sender, EventArgs e)
        {
            this.lblDetails.Text = "Products";

            _pages -= 1;
            if (_pages < 0)
                _pages = 0;
            int t = _pages * Convert.ToInt32(textBox1.Text);

            var products = from product in this.nwDataSet1.Products.Skip(t).Take(Convert.ToInt32(textBox1.Text))
                           select product;

            this.dataGridView2.DataSource = products.ToList();
        }
        private void button13_Click(object sender, EventArgs e)
        {
            this.lblDetails.Text = "Products";

            _pages += 1;
            int num = Convert.ToInt32(textBox1.Text);
            int count = (from product in this.nwDataSet1.Products
                         select product).ToArray().Length / num;
            if (_pages >= count)
                _pages = count;

            var products = from product in this.nwDataSet1.Products.Skip(_pages * num).Take(num)
                           select product;
            this.dataGridView2.DataSource = products.ToList();

            //this.nwDataSet1.Products.Take(10);//Top 10 Skip(10)

            //Distinct()
        }


        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int orderID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

                var orderDetail = from detail in this.nwDataSet1.Order_Details
                                  where detail.OrderID == orderID
                                  select detail;
                this.dataGridView2.DataSource = orderDetail.ToList();
            }
            catch 
            {
                
            }
        }


    }
}
