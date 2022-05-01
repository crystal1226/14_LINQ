using LinqLabs;
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
    public partial class Frm作業_1 : Form
    {
        public Frm作業_1()
        {
            InitializeComponent();
            ordersTableAdapter1.Fill(nwDataSet1.Orders);
            order_DetailsTableAdapter1.Fill(nwDataSet1.Order_Details);
            productsTableAdapter1.Fill(nwDataSet1.Products);
            LoadYearToComboBox();
        }

        #region //LINQ to FileInfo[]
        private void button14_Click(object sender, EventArgs e)
        {
           //FileInfo[] - .log檔
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files =  dir.GetFiles();
            //this.dataGridView1.DataSource = files;

            IEnumerable<FileInfo> q = from f in files
                                      where f.Extension == ".log"
                                      select f;
            dataGridView1.DataSource = q.ToList();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //FileInfo[] - 2019 created - order
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();

            IEnumerable<FileInfo> q = from f in files
                                      where f.CreationTime.Year == 2019
                                      orderby f.CreationTime ascending
                                      select f;
            dataGridView1.DataSource = q.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //FileInfo[] - 大檔案
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();
            this.dataGridView1.DataSource = files;

            IEnumerable<FileInfo> q = from f in files
                                      where f.Length > 30000
                                      select f;
            dataGridView1.DataSource = q.ToList();
        }
        #endregion

        #region //LINQ to Northwind DataSet - Orders
        private void LoadYearToComboBox()
        {
            var q = from o in nwDataSet1.Orders
                    group o by o.OrderDate.Year into orderYear
                    select orderYear;

            foreach (var year in q)
            {
                comboBox1.Items.Add(year.Key);
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            //All 訂單
            IEnumerable<NWDataSet.OrdersRow> q = from o in nwDataSet1.Orders
                                                 select o;
            dataGridView1.DataSource = q.ToList();
            //Click後顯示10248 order details
            IEnumerable<NWDataSet.Order_DetailsRow> q1 = from od in nwDataSet1.Order_Details
                                                         where od.OrderID == (int)dataGridView1.Rows[0].Cells[0].Value
                                                         select od;
            dataGridView2.DataSource = q1.ToList();
            dataGridView1.CellClick += DataGridView1_CellClick;
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            IEnumerable<NWDataSet.Order_DetailsRow> q = from od in nwDataSet1.Order_Details
                                                        where od.OrderID == (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value
                                                        select od;
            dataGridView2.DataSource = q.ToList();
        }
 
        private void button1_Click(object sender, EventArgs e)
        {
            //某年訂單
            if (comboBox1.Text=="")
            {
                MessageBox.Show("請選擇年份");
            }
            else
            {
                var q = from o in nwDataSet1.Orders
                        where o.OrderDate.Year == Convert.ToInt32(comboBox1.Text)
                        select o;
                dataGridView1.DataSource = q.ToList();

                //某年訂單明細
                var q1 = from o in nwDataSet1.Orders
                         join od in nwDataSet1.Order_Details
                         on o.OrderID equals od.OrderID
                         where o.OrderDate.Year.ToString() == comboBox1.Text
                         select new { od.OrderID, od.ProductID, od.UnitPrice, od.Quantity, od.Discount };
                dataGridView2.DataSource = q1.ToList();
                dataGridView1.CellClick += DataGridView1_CellClick;
            }
        }
        #endregion

        #region //LINQ to Northwind DataSet - Products(一頁幾筆)
        int count = 0;
        private void button13_Click(object sender, EventArgs e)
        {
            int page = int.Parse(txtPage.Text);
            //下一頁
            if (count < (nwDataSet1.Products.Rows.Count / page) + 1)
            {
                var q = from p in nwDataSet1.Products.Skip(count * page).Take(page)
                        select p;
                dataGridView1.DataSource = q.ToList();
                count += 1;
                dataGridView1.CellClick += DataGridView1_CellClick1;
            }
            else
            {
                MessageBox.Show("已為最後一頁");
            }
        }

        private void DataGridView1_CellClick1(object sender, DataGridViewCellEventArgs e)
        {
            var q = from p in nwDataSet1.Products
                    join od in nwDataSet1.Order_Details
                    on p.ProductID equals od.ProductID
                    where p.ProductID == (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value
                    select od;
            dataGridView2.DataSource = q.ToList();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int page = int.Parse(txtPage.Text);
            //上一頁
            if (count >1)
            {
                var q = from p in nwDataSet1.Products.Skip((count - 2) * page).Take(page)
                        select p;
                dataGridView1.DataSource = q.ToList();
                count -= 1;
                dataGridView1.CellClick += DataGridView1_CellClick1;
            }
            else
            {
                MessageBox.Show("已為第一頁");
            }
        }

       
    }
        #endregion
    }

