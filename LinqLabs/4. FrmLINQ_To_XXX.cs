using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace Starter
{
    public partial class FrmLINQ_To_XXX : Form
    {
        public FrmLINQ_To_XXX()
        {
            InitializeComponent();
        }

        #region //分組彙總運算子 - Group  / Aggregate
        private void button6_Click(object sender, EventArgs e)
        {
            //Group by
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //IEnumerable<IGrouping<int, int>> q = from n in nums
            //                                     group n by n % 2; //split as 2 keys: 1 (n%2=1), 0 (n%2=0)
            IEnumerable<IGrouping<string, int>> q = from n in nums
                                                 group n by n % 2==0?"偶數":"奇數"; //Key變為string

            dataGridView1.DataSource = q.ToList();

            //Add TreeView
            treeView1.Nodes.Clear();
            foreach (var group in q) //先加group為節點
            {
                TreeNode node = treeView1.Nodes.Add(group.Key.ToString());
                foreach(var item in group) //再加group中的item為子節點
                {
                    node.Nodes.Add(item.ToString());
                }
            }

            //Add ListView
            foreach(var group in q)
            {
                ListViewGroup lvg= listView1.Groups.Add(group.Key.ToString(), group.Key.ToString());
                foreach(var item in group)
                {
                    listView1.Items.Add(item.ToString()).Group = lvg;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Group / Aggregation
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,12,13 };
            var q = from n in nums
                    group n by n % 2 == 0 ? "偶數" : "奇數" into g
                    select new
                    {
                        MyKey = g.Key,
                        MyCount = g.Count(),
                        MyMin=g.Min(),
                        MyMax=g.Max(),
                        MyGroup=g
                    };
            dataGridView1.DataSource = q.ToList();

            //Add TreeView
            treeView1.Nodes.Clear();
            foreach (var group in q)
            {
                string s = $"{group.MyKey}({group.MyCount})"; //格式化node標題
                TreeNode node = treeView1.Nodes.Add(group.MyKey.ToString(), s); //MyKey
                foreach (var item in group.MyGroup) //MyGroup
                {
                    node.Nodes.Add(item.ToString());
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Group into
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            var q = from n in nums
                    group n by MyKey(n) into g //自定義group的方法
                    select new
                    {
                        MyKey = g.Key,
                        MyCount = g.Count(),
                        MyMin = g.Min(),
                        MyMax = g.Max(),
                        MyGroup = g
                    };
            dataGridView1.DataSource = q.ToList();

            //Add TreeView
            treeView1.Nodes.Clear();
            foreach (var group in q)
            {
                string s = $"{group.MyKey}({group.MyCount})";
                TreeNode node = treeView1.Nodes.Add(group.MyKey.ToString(), s);
                foreach (var item in group.MyGroup)
                {
                    node.Nodes.Add(item.ToString());
                }
            }

            //Chart
            chart1.DataSource = q.ToList();
            chart1.Series[0].XValueMember = "MyKey";
            chart1.Series[0].YValueMembers = "MyCount";
            chart1.Series[0].ChartType = SeriesChartType.Column;

            chart1.Series[1].XValueMember = "MyKey";
            chart1.Series[1].YValueMembers = "MyCount";
            chart1.Series[1].ChartType = SeriesChartType.Column;
        }

        private string MyKey(int n)
        {
            if (n < 5)
                return "Small";
            else if (n < 10)
                return "Medium";
            else
                return "Large";
        }
        #endregion

        #region //LinQ to 檔案目錄
        private void button38_Click(object sender, EventArgs e)
        {
            //依副檔名分組
            DirectoryInfo dir = new DirectoryInfo(@"c:\windows");
            FileInfo[] files = dir.GetFiles();
            dataGridView1.DataSource = files;

            var q = from f in files
                    group f by f.Extension into ext
                    orderby ext.Count() descending
                    select new
                    {
                        ExtName=ext.Key,
                        ExtCount = ext.Count()
                    };
            dataGridView2.DataSource = q.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //查詢具有指定屬性或名稱的檔案 (用 Let)
            DirectoryInfo dir = new DirectoryInfo(@"c:\windows");
            FileInfo[] files = dir.GetFiles();
            int count = (from f in files
                     let s = f.Extension
                     where s == ".exe"
                     select f).Count();
            MessageBox.Show(".exe Count= "+count);     
        }
        #endregion

        #region //LinQ to string
        private void button1_Click(object sender, EventArgs e)
        {
            //統計某個字在字串中出現的次數
            string s = "This is a book. This is a pen. This is a apple.";
            char[] chars = { ' ',  ',' ,  '?' ,  '.' };
            string[] words = s.Split(chars); //按照chars中的元素切割string
            var q = from w in words
                    group w by w into g
                    select new { MyKey = g.Key, MyCount = g.Count() };
            dataGridView1.DataSource = q.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Linq to string - 查詢字串出現的字數
            string s = "This is a book. This is a pen. This is a apple.";
            char[] chars = { ' ', ',', '?', '.' };
            string[] words = s.Split(chars,StringSplitOptions.RemoveEmptyEntries); //移除空白項
            var q = from w in words
                    group w by w.ToUpper() into g
                    select new { MyKey = g.Key, MyCount = g.Count() };
            dataGridView1.DataSource = q.ToList();
        }
        #endregion

        #region //LinQ 其他語法
        private void button15_Click(object sender, EventArgs e)
        {
            //Linq 其他語法
            //集合運算子 - Distinct / Union / Intersect / Except
            int[] nums1 = { 1, 2, 3, 5, 11, 2 };
            int[] nums2 = { 1, 3, 66, 77, 111 };
            IEnumerable<int> q = nums1.Intersect(nums2); //交集
            q = nums1.Distinct(); //不重複
            q = nums1.Union(nums2); //聯集

            //數量詞作業 - Any / All / Contains (回傳bool)
            bool result;
            result = nums1.Any(n => n > 100);
            result = nums2.All(n => n >= 1);
            result = nums2.Contains(66);

            //切割運算子 - Take / TakeWhile / Skip / SkipWhile
            q = nums1.Take(2);

            //元素運算子 - First / Last / Single / ElementAt
            //                      FirstOrDefault / LastOrDefault / SingleOrDefault / ElementAtOrDefault
            int n1;
            n1 = nums1.First();
            n1 = nums1.Last();
            n1 = nums2.ElementAt(3);
            n1 = nums2.ElementAtOrDefault(13); //若索引超出範圍，傳回預設值

            //產生作業 - Generation-Range / Repeat / Empty DefaultIfEmpty
            var q1 = Enumerable.Range(1, 100).Select(n=>new { n}); //+select new{n}
            dataGridView1.DataSource = q1.ToList();

            var q2 = Enumerable.Repeat(60, 100).Select(n => new { n });
            dataGridView2.DataSource = q2.ToList();
        }
        #endregion

        #region //LinQ to DataSet
        private void button10_Click(object sender, EventArgs e)
        {
            //Linq to DataSet - Northwind各分類平均單價
            productsTableAdapter1.Fill(nwDataSet1.Products);
            categoriesTableAdapter1.Fill(nwDataSet1.Categories);

            var q1 = from p in nwDataSet1.Products
                    group p by p.CategoryID into g
                    select new { CategoryID = g.Key, AvgPrice=$"{g.Average(p=>p.UnitPrice):c2}"};
            dataGridView1.DataSource = q1.ToList();

            //Join
            var q2 = from c in nwDataSet1.Categories
                     join p in nwDataSet1.Products
                     on c.CategoryID equals p.CategoryID
                     group p by c.CategoryName into g
                     select new { CategoryName = g.Key, AvgPrice = $"{g.Average(p => p.UnitPrice):c2}" };
            dataGridView2.DataSource = q2.ToList();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //NorthWind - Group by OrderYear
            ordersTableAdapter1.Fill(nwDataSet1.Orders);
            dataGridView1.DataSource = nwDataSet1.Orders;

            var q = from o in nwDataSet1.Orders
                    group o by o.OrderDate.Year into g
                    select new
                    {
                        OrderYear = g.Key,
                        OrderCount = g.Count()
                    };
            dataGridView2.DataSource = q.ToList();

            //+篩選條件
            int count = (from o in nwDataSet1.Orders
                         where o.OrderDate.Year == 1996
                         select o).Count(); //將query直接執行Count()，回傳int
            MessageBox.Show("Order count of 1996= " + count);
        }
        #endregion

        #region //LinQ to XML
        private void button8_Click(object sender, EventArgs e)
        {
            //Select 選取投影-轉型成 XML 文件
        }
        #endregion
    }
}
