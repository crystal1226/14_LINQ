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
    public partial class Frm作業_3 : Form
    {
        public Frm作業_3()
        {
            InitializeComponent();
        }

        #region Group by
        private void button4_Click(object sender, EventArgs e)
        {
            //int[] 分三群 -No LINQ
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            treeView1.Nodes.Clear();
            TreeNode nodeS = treeView1.Nodes.Add("Small");
            TreeNode nodeM = treeView1.Nodes.Add("Medium");
            TreeNode nodeL = treeView1.Nodes.Add("Large");
            foreach (int i in nums)
            {
                if (i <= 5)
                    nodeS.Nodes.Add(i.ToString());
                else if (i <= 10)
                    nodeM.Nodes.Add(i.ToString());
                else
                    nodeL.Nodes.Add(i.ToString());
            }
        }
        #endregion

        #region LINQ to FileInfo[]
        private void button38_Click(object sender, EventArgs e)
        {
            //依檔案大小分組檔案 (大=>小)
            DirectoryInfo dir = new DirectoryInfo(@"c:\windows");
            FileInfo[] files = dir.GetFiles();
            dataGridView1.DataSource = files;

            var q = from f in files
                    orderby f.Length descending //可先orderby
                    group f by FileLength(f.Length) into g
                    select new
                    {
                        LengthGroup = g.Key,
                        Count = g.Count(),
                        Group = g
                    };
            dataGridView2.DataSource = q.ToList();

            //TreeView
            treeView1.Nodes.Clear();
            foreach (var group in q)
            {
                string s = $"{group.LengthGroup}({group.Count})";
                TreeNode node = treeView1.Nodes.Add(group.LengthGroup, s);
                foreach (var item in group.Group)
                {
                    node.Nodes.Add(item.ToString());
                }
            }
        }
        //public class LengthCompare : IComparer<string>
        //{
        //    public int Compare(string s1, string s2)
        //    {
        //        Dictionary<string, int> LengthDic = new Dictionary<string, int>()
        //        {
        //            {"大",1 },{"中",2},{"小",3},{"極小",4}
        //        };

        //        int val1 = LengthDic[s1];
        //        int val2 = LengthDic[s2];
        //        return val1 - val2; //若小於0(等於s1<s2)則回傳
        //    }
        //}
        private string FileLength(long n)
        {
            if (0 <= n && n <= 16384) //0-16KB
                return "極小";
            else if (16384 < n && n <= 1048576) //16KB-1MB
                return "小";
            else if (1048576 < n && n <= 134217728) //1MB-128MB
                return "中";
            else
                return "大";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //依年分組檔案 (大=>小)
            DirectoryInfo dir = new DirectoryInfo(@"c:\windows");
            FileInfo[] files = dir.GetFiles();
            dataGridView1.DataSource = files;

            var q = from f in files
                    group f by f.CreationTime.Year into g
                    orderby g.Count() descending
                    select new
                    {
                        Year=g.Key,
                        Count = g.Count(),
                        Group =g
                    };
            dataGridView2.DataSource = q.ToList();

            //TreeView
            treeView1.Nodes.Clear();
            foreach(var group in q)
            {
                string s = $"{group.Year}({group.Count})";
                TreeNode node= treeView1.Nodes.Add(group.Year.ToString(), s);
                foreach(var item in group.Group)
                {
                    node.Nodes.Add(item.ToString());
                }
            }
        }

        #endregion

        #region LINQ to Northwind Entity
        NorthwindEntities dbContext = new NorthwindEntities();
        private void button8_Click(object sender, EventArgs e)
        {
            //NW Products - 低中高價產品
            var q0 = from p in dbContext.Products
                     select p;
            dataGridView1.DataSource = q0.ToList();

            var q = from p in dbContext.Products.AsEnumerable()
                    orderby p.UnitPrice descending
                    group p by PriceGroup(p.UnitPrice.GetValueOrDefault()) into g
                    select new 
                    {
                        Group=g.Key, 
                        Count=g.Count(),
                        Products=g
                    };
            dataGridView2.DataSource = q.ToList();

            treeView1.Nodes.Clear();
            foreach(var group in q)
            {
                string s = $"{group.Group} ({group.Count})";
                TreeNode node = treeView1.Nodes.Add(group.Group.ToString(),s);
                foreach(var item in group.Products)
                {
                    string s2 = $"{item.ProductName} - {item.UnitPrice.GetValueOrDefault():c2}";
                    node.Nodes.Add(s2);
                }
            }
        }

        private string PriceGroup(decimal n)
        {
            if (n <= 20)
                return "低價";
            else if (21 < n && n <= 80)
                return "中價";
            else
                return "高價";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //Orders - Group by Year
            var q = from o in dbContext.Orders
                    group o by o.OrderDate.Value.Year into g
                    orderby g.Key descending
                    select new { Year = g.Key, Count = g.Count() };
            dataGridView1.DataSource = q.ToList();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //Orders - Group by Year & Month
            var q0 = from p in dbContext.Orders
                     select p;
            dataGridView1.DataSource = q0.ToList();

            var q = from o in dbContext.Orders.AsEnumerable()
                    group o by new { o.OrderDate.Value.Year, o.OrderDate.Value.Month } into g
                    orderby g.Key.Year, g.Key.Month
                    select new
                    {
                        Date = $"{g.Key.Year}/{g.Key.Month}",
                        Count=g.Count(),
                        Orders=g
                    };
            dataGridView2.DataSource = q.ToList();

            foreach(var group in q)
            {
                string s = $"{group.Date} ({group.Count})";
                TreeNode node = treeView1.Nodes.Add(group.Date, s);
                foreach (var item in group.Orders)
                {
                    string s2 = $"{item.OrderID} - {item.OrderDate}";
                    node.Nodes.Add(s2);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //總銷售金額
            var q1 = (from od in dbContext.Order_Details.AsEnumerable()
                     select  od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)).Sum();
            MessageBox.Show($"{q1:c2}");

            //每年總銷售金額
            var q2 = from od in dbContext.Order_Details.AsEnumerable()
                      group od by od.Order.OrderDate.Value.Year into g
                      select new
                      {
                          Year=g.Key,
                          Sum = $"{g.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)):c2}"
                      };
            dataGridView2.DataSource = q2.ToList();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //銷售最好的Top5業務員
            var q = (from od in dbContext.Order_Details.AsEnumerable()
                     orderby od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount) descending
                     select new
                     {
                         EmpFirstName = od.Order.Employee.FirstName,
                         EmpLastName = od.Order.Employee.LastName,
                         TotalSales=$"{od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount):c2}"
                     }).Take(5);
            dataGridView1.DataSource = q.ToList();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            //產品最高單價前 5 筆 (包括類別名稱)
            var q = (from p in dbContext.Products.AsEnumerable()
                     orderby p.UnitPrice descending
                     select new
                     {
                         CategoryName = p.Category.CategoryName,
                         p.ProductName,
                         UnitPrice=$"{ p.UnitPrice:c2}"
                     }).Take(5);
            dataGridView1.DataSource = q.ToList();
        }
      
        private void button7_Click(object sender, EventArgs e)
        {
            //產品有任何一筆單價大於300?
            bool result;
            result = dbContext.Products.Any(p => p.UnitPrice > 300);
            MessageBox.Show(result.ToString());

            var q= from p in dbContext.Products
                    where p.UnitPrice > 300
                    select p;
            dataGridView1.DataSource = q.ToList();
        }
        #endregion
    }
}
