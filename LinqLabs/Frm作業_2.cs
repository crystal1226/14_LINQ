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
            ControlsBinding();
            LoadYearToComboBox();
        }

        private void ControlsBinding()
        {
            productPhotoTableAdapter1.Fill(aW2019DataSet1.ProductPhoto);
            bindingSource1.DataSource = aW2019DataSet1.ProductPhoto;
            dataGridView1.DataSource = bindingSource1;
            pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
        }

        private void LoadYearToComboBox()
        {
            var q = from p in aW2019DataSet1.ProductPhoto
                    orderby p.ModifiedDate.Year ascending
                    select p.ModifiedDate.Year;
            var modifiedYear=q.Distinct();
            foreach (var year in modifiedYear)
            {
                comboBox3.Items.Add(year);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //All 腳踏車
            var q = aW2019DataSet1.ProductPhoto;
            bindingSource1.DataSource = q.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //區間腳踏車
            DateTime startTime = (DateTime)dateTimePicker1.Value;
            DateTime endTime = (DateTime)dateTimePicker2.Value;
            var q = aW2019DataSet1.ProductPhoto.Where(p => p.ModifiedDate > startTime && p.ModifiedDate < endTime);
            bindingSource1.DataSource = q.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //某年腳踏車
            if (comboBox3.Text == "")
            {
                MessageBox.Show("請選擇年份");
            }
            else
            {
                string year = comboBox3.Text;
                var q = aW2019DataSet1.ProductPhoto.Where(p => p.ModifiedDate.Year.ToString() == year);
                bindingSource1.DataSource = q.ToList();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //某季腳踏車 & 有幾筆
            if (comboBox3.Text == "")
            {
                MessageBox.Show("請選擇年份");
            }
            else if (comboBox2.Text == "")
            {
                MessageBox.Show("請選擇季度");
            }
            else
            {
                string qtr = comboBox2.Text;
                if (qtr == "第一季")
                {
                    QtrQuery(1, 3);
                }
                else if (qtr == "第二季")
                {
                    QtrQuery(4,6);
                }
                else if (qtr == "第三季")
                {
                    QtrQuery(7, 9);
                }
                else
                {
                    QtrQuery(10, 12);
                }
            }
        }

        void QtrQuery(int startMon, int endMon)
        {
            string year = comboBox3.Text;
            var q = aW2019DataSet1.ProductPhoto.Where(p => p.ModifiedDate.Year.ToString() == year &&
                    p.ModifiedDate.Month >= startMon && p.ModifiedDate.Month <= endMon);
            bindingSource1.DataSource = q.ToList();
            lblMaster.Text = "Master: 共" + q.ToList().Count + "筆";
        }
    }
}
