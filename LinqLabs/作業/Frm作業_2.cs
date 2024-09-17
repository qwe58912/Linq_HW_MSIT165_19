using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_2 : Form
    {
        public Frm作業_2()
        {
            InitializeComponent();
            this.productPhotoTableAdapter1.Fill(this.awDataSet1.ProductPhoto);
            var productYear = from pdYear in this.awDataSet1.ProductPhoto
                              select pdYear.ModifiedDate.Year;
            this.comboBox3.DataSource = productYear.Distinct().ToList();

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            displayProductInfo();

            //byte[] photo = Convert.ToByte( dataGridView1.Rows[0].Cells
            //    [dataGridView1.Columns["LargePhoto"].Index].Value);

            //Stream streamImage = new MemoryStream(photo);
            //this.pictureBox1.Image = Bitmap.FromStream(streamImage);

            //this.pictureBox1.Image = (Image)dataGridView1.Rows[0].Cells[3].Value;
        }

        private void displayProductInfo()
        {
            var productPhotos = from photo in this.awDataSet1.ProductPhoto
                                select photo;
            this.dataGridView1.DataSource = productPhotos.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var productPhotos = from photo in this.awDataSet1.ProductPhoto
                                where photo.ModifiedDate > Convert.ToDateTime(dateTimePicker1.Value)
                                & photo.ModifiedDate < Convert.ToDateTime(dateTimePicker2.Value)
                                select photo;
            this.dataGridView1.DataSource = productPhotos.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox3.Text))
            {
                displayProductInfo();

            }
            else
            {
                var productPhotos = from photo in this.awDataSet1.ProductPhoto
                                    where photo.ModifiedDate.Year.ToString() == comboBox3.Text
                                    select photo;
                this.dataGridView1.DataSource = productPhotos.ToList();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox2.Text))
            {
                displayProductInfo();
            }
            else
            {
                int quarter = (this.comboBox2.SelectedIndex + 1) * 3;
                var productPhotos = from photo in this.awDataSet1.ProductPhoto
                                    where photo.ModifiedDate.Month == quarter
                                    || photo.ModifiedDate.Month == quarter - 1
                                    || photo.ModifiedDate.Month == quarter - 2
                                    select photo;

                this.label1.Text = $"{comboBox2.Text} 共{productPhotos.ToList().Count}筆";
                this.dataGridView1.DataSource = productPhotos.ToList();
            }
        }


        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                if (row.Cells["LargePhoto"].Value is byte[] imageBytes)
                {
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        this.pictureBox1.Image = Image.FromStream(ms);
                    }
                }
        }
    }
}
