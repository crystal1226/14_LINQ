using LinqLabs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Starter
{
    public partial class FrmHelloLinq : Form
    {
        public FrmHelloLinq()
        {
            InitializeComponent();
            productsTableAdapter1.Fill(nwDataSet1.Products);
            ordersTableAdapter1.Fill(nwDataSet1.Orders);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //IEnumerable<T> - int[]
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //syntaz sugar - foreach
            foreach (int n in nums) 
            {
                listBox1.Items.Add(n);
            }
            listBox1.Items.Add("----------------------------------------------------");

            //使用GetEnumerator()
            IEnumerator en = nums.GetEnumerator();
            while (en.MoveNext()) //往列舉的下一個值
            {
                listBox1.Items.Add(en.Current); //加入目前列舉值
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //IEnumerable<T> - List<T>
            List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //syntaz sugar - foreach
            foreach (int n in list)
            {
                listBox1.Items.Add(n);
            }
            listBox1.Items.Add("----------------------------------------------------");

            //使用GetEnumerator()
            int n1 = 100;
            var n2 = 100; //var可自動判斷型別

            List<int>.Enumerator en = list.GetEnumerator(); //等於 var en= list.GetEnumerator();
            while (en.MoveNext())
            {
                listBox1.Items.Add(en.Current);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //LINQ三步驟
            //Step1: define Data Source
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //Step2: define query
            IEnumerable<int> q = from n in nums //IEnumerable<int>可寫var
                                                where (n>=1&&n<=4) && (n%2==0) //query使用C#語法
                                                select n;
            //Step3: execute query
            foreach (int n in q)
            {
                listBox1.Items.Add(n);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //LINQ三步驟 - 使用方法
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<int> q = from n in nums
                                                where IsEven(n)
                                                select n;
            foreach (int n in q)
            {
                listBox1.Items.Add(n);
            }
        }

        bool IsEven(int n)
        {
            //if (n % 2 == 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            //return n % 2 == 0;
            return (n % 2 == 0) ? true : false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //IEnumerable任意型別
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<Point>/*同結果型別*/ q = from n in nums
                                                                            where n >= 6
                                                                            select new Point(n, n * n); //結果可為任意型別
            //execute query - foreach
            foreach (Point pt in q)
            {
                listBox1.Items.Add(pt.X + ", " + pt.Y);
            }

            //execute query - ToList()
            List<Point> list = q.ToList(); //背後仍是foreach迴圈
            dataGridView1.DataSource = list;

            chart1.DataSource = list;
            chart1.Series[0].XValueMember = "X";
            chart1.Series[0].YValueMembers = "Y";
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string[]
            string[] words = { "apple", "Apple", "pineApple", "xxxapple" };
            IEnumerable<string> q = from w in words
                                    where (w.ToLower().Contains("apple")) && (w.Length>5)
                                    select w;
            foreach(string s in q)
            {
                listBox1.Items.Add(s);
            }
            dataGridView1.DataSource = q.ToList();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //LinQ to DataSet - Products
            //dataGridView1.DataSource = nwDataSet1.Products;
            IEnumerable<NWDataSet.ProductsRow> q = from p in nwDataSet1.Products //ProductsRow(空值會發生DBnull)
                                                   where !p.IsUnitPriceNull() && p.UnitPrice > 30 && p.UnitPrice < 50 && p.ProductName.StartsWith("M")
                                                   select p;
            dataGridView1.DataSource = q.ToList();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //LinQ to DataSet - Orders
            IEnumerable<NWDataSet.OrdersRow> q = from o in nwDataSet1.Orders
                                                 where o.OrderDate.Month==1 || o.OrderDate.Month == 2 || o.OrderDate.Month == 3
                                                 select o;
            dataGridView1.DataSource = q.ToList();
        }
    }
}
