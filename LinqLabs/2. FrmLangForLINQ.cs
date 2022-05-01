using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Starter
{
    public partial class FrmLangForLINQ : Form
    {
        public FrmLangForLINQ()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Swap - int
            int n1 = 100;
            int n2 = 200;
            MessageBox.Show(n1 + "," + n2);
            Swap(ref n1, ref n2);
            MessageBox.Show(n1 + "," + n2);

            //Swap - string
            string s1 = "aaa";
            string s2 = "bbb";
            MessageBox.Show(s1 + "," + s2);
            Swap(ref s1, ref s2);
            MessageBox.Show(s1 + "," + s2);
        }

        //Swap多型方法
        void Swap(ref int A, ref int B)
        {
            int T = A;
            A = B;
            B = T;
        }
        void Swap(ref string A, ref string B)
        {
            string T = A;
            A = B;
            B = T;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Swap - Generic Method
            int n1 = 100;
            int n2 = 200;
            MessageBox.Show(n1 + "," + n2);
            //SwapAnyType<int>(ref n1, ref n2);
            SwapAnyType(ref n1, ref n2); //可自動推斷型別
            MessageBox.Show(n1 + "," + n2);

            string s1 = "aaa";
            string s2 = "bbb";
            MessageBox.Show(s1 + "," + s2);
            SwapAnyType(ref s1, ref s2);
            MessageBox.Show(s1 + "," + s2);
        }

        void SwapAnyType<T>(ref T A, ref T B)
        {
            T temp = B;
            B = A;
            A = temp;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Delegate
            //C# 1.0 具名方法
            buttonX.Click += ButtonX_Click;
            buttonX.Click += aaa; //可同時指向多個方法
            buttonX.Click += bbb;

            //C# 2.0 匿名方法
            buttonX.Click += delegate (object sender1, EventArgs e1) //僅buttonX.Click使用的委派事件方法，可寫成匿名方法
             {
                 MessageBox.Show("C# 2.0 匿名方法");
             };

            //C# 3.0 lambda運算式(匿名)
            buttonX.Click += (object sender2, EventArgs e2) => MessageBox.Show("C# 3.0 lambda運算式"); //goes to
            //{
            //      MessageBox.Show("C# 3.0 lambda運算式");
            //  };
        }

        private void bbb(object sender, EventArgs e)
        {
            MessageBox.Show("Method bbb");
        }

        private void aaa(object sender, EventArgs e)
        {
            MessageBox.Show("Method aaa");
        }

        private void ButtonX_Click(object sender, EventArgs e)
        {
            MessageBox.Show("buttonX click");
        }

        //Step1: create delegate 型別
        //Step2: create delegate object(new...)
        //Step3: invoke/call method
        delegate bool MyDelegate(int n); //Step1: create delegate 型別
        private void button9_Click(object sender, EventArgs e)
        {
            //直接call方法：Test()
            bool result = Test(6);
            MessageBox.Show("Result: " + result);

            //委派物件call方法
            MyDelegate delegateObj = new MyDelegate(Test); //Step2: create delegate object
            result = delegateObj(7); //Step3: invoke/call method
            MessageBox.Show("Result: " + result);

            //syntax sugar
            delegateObj = Test1; //委派指向的方法
            result=delegateObj(5);
            MessageBox.Show("Result: " + result);

            //C# 2.0 匿名方法
            delegateObj = delegate (int n)
              {
                  return n > 5;
              };
            result = delegateObj(6);
            MessageBox.Show("Result: " + result);

            //C# 3.0 lambda運算式(匿名)
            delegateObj = n => n > 5;
            result = delegateObj(1);
            MessageBox.Show("Result: " + result);
        }
        bool Test(int n)
        {
            return n > 5; //if n>5, return true
        }

        bool Test1(int n)
        {
            return n % 2 == 0;
        }

        //委派參數 Lambda Expression
        List<int> MyWhere(int[] nums, MyDelegate delegateObj) //判定int[]，符合委派物件的方法即傳回List
        {
            List<int> list = new List<int>();
            foreach (int n in nums)
            {
                if (delegateObj(n))
                {
                    list.Add(n);
                }
            }
            return list;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int> LargeList = MyWhere(nums, Test); 
            //foreach(int n in LargeList)
            //{
            //    listBox1.Items.Add(n);
            //}

            //匿名方法
            List<int> list = MyWhere(nums, n => n > 5);
            List<int> oddlist = MyWhere(nums, n =>n%2==1);
            List<int> evenlist = MyWhere(nums, n => n % 2 == 0);
            foreach (int n in list)
            {
                listBox1.Items.Add(n);
            }
            foreach (int n in oddlist)
            {
                listBox2.Items.Add(n);
            }
          
        }

        //Iterator - Yield return
        IEnumerable<int> MyIterator(int[] nums, MyDelegate delegateObj)
        {
            foreach (int n in nums)
            {
                if (delegateObj(n))
                {
                    yield return n;
                }
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<int> q = MyIterator(nums, n => n > 5); //此行定義方法
            foreach(int n in q) //此行才call方法
            {
                listBox1.Items.Add(n);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //C# 3.0
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var q = nums.Where(n => n > 5);
            foreach(int n in q)
            {
                listBox1.Items.Add(n);
            }

            //string
            string[] words = { "aaa", "bbbbb", "ccccccc" };
            IEnumerable<string> q1 = words.Where(w => w.Length > 3);
            foreach(string w in q1)
            {
                listBox2.Items.Add(w);
            }
            dataGridView1.DataSource = q1.ToList(); //顯示Length屬性(欄位)的資料

            //Northwind - Products
            productsTableAdapter1.Fill(nwDataSet1.Products);
            var q2 = nwDataSet1.Products.Where(p => p.UnitPrice > 30); //省略 from p in nwDataSet1.Products
            dataGridView2.DataSource = q2.ToList();
        }

        private void button45_Click(object sender, EventArgs e)
        {
            //var - 常用於匿名型別時
            var n = 100;
            var s = "var";
            var p = new Point(100, 100);
        }

        private void button41_Click(object sender, EventArgs e)
        {
            //物件初始化
            MyPoint pt1 = new MyPoint();
            pt1.p1 = 100; //set
            int w = pt1.p1; //get

            pt1.p2 = 200; //get
            List<MyPoint> list = new List<MyPoint>();
            list.Add(new MyPoint());
            list.Add(new MyPoint(100));
            list.Add(new MyPoint("aaa"));
            list.Add(new MyPoint(200, 300));

            //初始化
            list.Add(new MyPoint { p1 = 1, p2 = 2, Field1 = "aaa", Field2 = "bbb" });
            dataGridView1.DataSource = list;

            //集合初始化
            List<MyPoint> list2 = new List<MyPoint>
            {
                new MyPoint{p1=1,p2=2, Field1="111", Field2="222"},
                new MyPoint{p1=11,p2=22, Field1="11111", Field2="22222"},
            };
            dataGridView2.DataSource = list2;
        }

        class MyPoint
        {
            private int m_p1; //private避免初始值被修改
            public int p1
            {
                get
                {
                    return m_p1;
                }
                set
                {
                    m_p1 = value;
                }
            }

            public int p2 { get; set; } //自動套用get set

            public string Field1 = "xxx", Field2 = "yyy";

            public MyPoint()
            {

            }
            public MyPoint(int p1)
            {
                this.p1 = p1;
            }

            public MyPoint(int p1, int p2)
            {
                this.p1 = p1;
                this.p2 = p2;
            }

            public MyPoint(string Field)
            {

            }
        }

        private void button43_Click(object sender, EventArgs e)
        {
            //匿名型別
            var x = new { P1 = 99, P2 = 88, P3 = 77 };
            var y = new { P1 = 99, P2 = 88, P3 = 77 };
            var z = new { UserName = "xxx", Password = "yyy" };

            listBox1.Items.Add(x.GetType());
            listBox1.Items.Add(y.GetType());
            listBox1.Items.Add(z.GetType());

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //var q = from n in nums
            //        where n > 5
            //        select new { N = n, Square = n * n, Cube = n * n * n }; //可直接select匿名型別陣列

            var q = nums.Where(n => n > 5).Select(n => new { N = n, Square = n * n, Cube = n * n * n }); //Where(n => n > 5)作為Select的資料來源
            dataGridView1.DataSource = q.ToList();

            productsTableAdapter1.Fill(nwDataSet1.Products);
            var q1 = from p in nwDataSet1.Products
                     where p.UnitPrice > 30
                     select new
                     {
                         ID = p.ProductID,
                         產品名稱 = p.ProductName,
                         p.UnitPrice, //會自動推斷
                         p.UnitsInStock,
                         TotalPrice = $"{p.UnitPrice * p.UnitsInStock:c2}"//運算會自動推斷型別，要給屬性名
                     };
            dataGridView2.DataSource = q.ToList();
        }

        private void button40_Click(object sender, EventArgs e)
        {
            //隱含匿名型別陣列
         
        }

        private void button32_Click(object sender, EventArgs e)
        {
            //擴充方法
            string s = "abcd";
            int n = s.WordCount();
            MessageBox.Show("WordCount  = " + n);

            //WordCount
            string s2 = "123456789";
            n = s2.WordCount(); //也可寫成 n = MyStringExtend.WordCount(s2);
            MessageBox.Show("WordCount = " + n);
            
            //Chars
            char ch = s2.Chars(3);
            MessageBox.Show("Chars = " + ch);
        }
    }
    public static class MyStringExtend //擴充方法必為靜態類別
    {
        public static int WordCount(this string s) //及靜態方法(參數必須加this)
        {
            return s.Length;
        }

        public static char Chars(this string s, int index)
        {
            return s[index];
        }
    }
}
