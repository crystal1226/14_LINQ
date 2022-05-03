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
    public partial class FrmLINQ架構介紹_InsideLINQ : Form
    {
        public FrmLINQ架構介紹_InsideLINQ()
        {
            InitializeComponent();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            //定義資料來源 - 非泛用集合 - ArrayList
            ArrayList arrlist = new ArrayList();
            arrlist.Add(3);
            arrlist.Add(4);

            var q = from n in arrlist.Cast<int>() //非泛用集合的擴充功能，轉換為IEnumerable<T>
                    where n > 2
                    select new { N = n }; //select必須建立屬性(new匿名型別)，否則DataSource無法繫結
            dataGridView1.DataSource = q.ToList();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //定義query
            productsTableAdapter1.Fill(nwDataSet1.Products);
            var q = (from p in nwDataSet1.Products
                     orderby p.UnitsInStock descending
                     select p).Take(5); //Take(x) 查詢前x筆
            dataGridView1.DataSource = q.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Aggr.
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            listBox1.Items.Add("Sum= " + nums.Sum()); //馬上執行query
            listBox1.Items.Add("Min= " + nums.Min());
            listBox1.Items.Add("Max= " + nums.Max());
            listBox1.Items.Add("Avg= " + nums.Average());
            listBox1.Items.Add("Count= " + nums.Count());

            productsTableAdapter1.Fill(nwDataSet1.Products);
            listBox1.Items.Add("Max UnitPrice= " + $"{nwDataSet1.Products.Max(p => p.UnitPrice):c2}");
            listBox1.Items.Add("Min UnitPrice= " + $"{nwDataSet1.Products.Min(p => p.UnitPrice):c2}");
            listBox1.Items.Add("Avg UnitPrice= " + $"{nwDataSet1.Products.Average(p => p.UnitPrice):c2}");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //延遲查詢估算
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int i = 0;
            var q = from n in nums
                    select ++i;
            foreach(var v in q)
            {
                listBox1.Items.Add(string.Format("v={0}, i={1}", v, i));
            }

            var q1 = (from n in nums
                      select ++i).ToList() ;
            foreach (var v in q1)
            {
                listBox1.Items.Add(string.Format("v={0}, i={1}", v, i));
            }
        }
    }
}