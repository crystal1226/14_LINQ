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

        #region //Group by
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

        #region //LINQ to FileInfo[]
        private void button38_Click(object sender, EventArgs e)
        {
            //依檔案大小分組檔案 (大=>小)
            DirectoryInfo dir = new DirectoryInfo(@"c:\windows");
            FileInfo[] files = dir.GetFiles();
            dataGridView1.DataSource = files;

            var q = from f in files
                    group f by FileLength(f.Length) into g
                    //orderby //todo
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
        public class LengthCompare : IComparer<string>
        {
            public int Compare(string s1, string s2)
            {
                Dictionary<string, int> LengthDic = new Dictionary<string, int>()
                {
                    {"大",1 },{"中",2},{"小",3},{"極小",4}
                };

                int val1 = LengthDic[s1];
                int val2 = LengthDic[s2];
                return val1 - val2; //若小於0(等於s1<s2)則回傳
            }
        }
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
    }
}
