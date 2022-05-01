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
    }
}