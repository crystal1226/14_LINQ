using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LinqLabs
{
    public partial class Frm考試 : Form
    {
        public Frm考試()
        {
            InitializeComponent();
            students_scores = new List<Student>()
                                         {
                                            new Student{ Name = "aaa", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Male" },
                                            new Student{ Name = "bbb", Class = "CS_102", Chi = 80, Eng = 80, Math = 100, Gender = "Male" },
                                            new Student{ Name = "ccc", Class = "CS_101", Chi = 60, Eng = 50, Math = 75, Gender = "Female" },
                                            new Student{ Name = "ddd", Class = "CS_102", Chi = 80, Eng = 70, Math = 85, Gender = "Female" },
                                            new Student{ Name = "eee", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Female" },
                                            new Student{ Name = "fff", Class = "CS_102", Chi = 80, Eng = 80, Math = 80, Gender = "Female" },
                                          };
        }
        List<Student> students_scores;
        public class Student
        {
            public string Name { get; set; }
            public string Class { get;  set; }
            public int Chi { get; set; }
            public int Eng { get; internal set; }
            public int Math { get;  set; }
            public string Gender { get; set; }
        }
        #region 搜尋 班級學生成績
        private void button36_Click(object sender, EventArgs e)
        {
            // 共幾個學員成績 ?
            int i = students_scores.Count();
            MessageBox.Show("共" + i + "個學員");

            // 找出前面三個的學員所有科目成績
            var q1 = (from s in students_scores
                      select new {s.Name, s.Chi, s.Eng, s.Math }).Take(3);
            dataGridView1.DataSource = q1.ToList();
            label1.Text= "前三個學員的所有科目成績";

            // 找出後面兩個的學員所有科目成績
            var q2 = (from s in students_scores
                      select new { s.Name, s.Chi, s.Eng, s.Math }).Skip(i-2).Take(2);
            dataGridView2.DataSource = q2.ToList();
            label2.Text = "後兩個學員的所有科目成績";

            //找出 Name 'aaa','bbb','ccc' 的學員國文英文科目成績
            var q3 = from s in students_scores
                     //where s.Name.Contains("aaa") || s.Name.Contains("bbb") || s.Name.Contains("ccc")
                     where s.Name=="aaa" || s.Name=="bbb" || s.Name=="ccc"
                     select new {s.Name, s.Chi, s.Eng};
            dataGridView3.DataSource = q3.ToList();
            label3.Text = " Name 'aaa','bbb','ccc' 的學員國文英文科目成績";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //找出學員 'bbb' 的成績
            var q1 = from s in students_scores
                     where s.Name == "bbb"
                     select s;
            dataGridView1.DataSource = q1.ToList();
            label1.Text = "學員 'bbb' 的成績";

            //找出除了 'bbb' 學員的學員的所有成績('bbb' 退學)
            var q2 = from s in students_scores
                     where !s.Name.Contains("bbb")
                     select s;
            dataGridView2.DataSource = q2.ToList();
            label2.Text = "除了 'bbb' 學員的學員的所有成績('bbb' 退學)";

            // 找出 'aaa', 'bbb' 'ccc' 學員國文數學兩科科目成績
            var q3=from s in students_scores
                   where s.Name == "aaa" || s.Name == "bbb" || s.Name == "ccc"
                   select new { s.Name, s.Chi, s.Math };
            dataGridView3.DataSource = q3.ToList();
            label3.Text = "'aaa', 'bbb' 'ccc' 學員國文數學兩科科目成績";

            // 數學不及格... 是誰
            var q4 = from s in students_scores
                     where s.Math < 60
                     select s.Name;

            List<string> StuName = new List<string>();
            foreach (string name in q4)
            {
                StuName.Add(name);
            }
            string result = String.Join(", ", StuName.ToArray());
            MessageBox.Show("數學不及格："+result);
        }
        #endregion
        #region 每個學生個人成績
        private void button37_Click(object sender, EventArgs e)
        {
            //個人 sum, min, max, avg
            //Sum
            var q1 = from s in students_scores
                     select new
                     {
                         s.Name,
                         Sum = s.Chi+s.Eng+s.Math,
                         Min=Math.Min((Math.Min(s.Chi,s.Eng)),s.Math),
                         Max = Math.Max((Math.Max(s.Chi, s.Eng)), s.Math),
                         Avg=( s.Chi + s.Eng + s.Math)/3
                     };
            dataGridView1.DataSource = q1.ToList();
            label1.Text = "個人成績 Sum, Min, Max, Avg";

            //各科 sum, min, max, avg
            //Sum
            List<Student> list = new List<Student>();
            //Sum
            list.Add(new Student
            {
                Name="各科Sum",
                Chi = students_scores.Sum(s => s.Chi),
                Math = students_scores.Sum(s => s.Math),
                Eng = students_scores.Sum(s => s.Eng)
            });

            //Min
            list.Add(new Student
            {
                Name = "各科Min",
                Chi = students_scores.Min(s => s.Chi),
                Math = students_scores.Min(s => s.Math),
                Eng = students_scores.Min(s => s.Eng)
            });

            //Max
            list.Add(new Student
            {
                Name = "各科Max",
                Chi = students_scores.Max(s => s.Chi),
                Math = students_scores.Max(s => s.Math),
                Eng = students_scores.Max(s => s.Eng)
            });

            //Avg
            list.Add(new Student
            {
                Name = "各科Avg",
                Chi = (int)students_scores.Average(s => Convert.ToDouble(s.Chi)),
                Math = (int)students_scores.Average(s => Convert.ToDouble(s.Math)),
                Eng = (int)students_scores.Average(s => Convert.ToDouble(s.Eng))
            });
            var q2 = list;
            dataGridView2.DataSource = q2.ToList();
            label2.Text = "各科 Sum, Min, Max, Avg";
        }
      
        #endregion
        private void button33_Click(object sender, EventArgs e)
        {
            // split=> 分成 三群 '待加強'(60~69) '佳'(70~89) '優良'(90~100) 
            // print 每一群是哪幾個 ? (每一群 sort by 分數 descending)
        }

        private void button35_Click(object sender, EventArgs e)
        {
            // 統計 :　所有隨機分數出現的次數/比率; sort ascending or descending
            // 63     7.00%
            // 100    6.00%
            // 78     6.00%
            // 89     5.00%
            // 83     5.00%
            // 61     4.00%
            // 64     4.00%
            // 91     4.00%
            // 79     4.00%
            // 84     3.00%
            // 62     3.00%
            // 73     3.00%
            // 74     3.00%
            // 75     3.00%
        }

        #region 銷售分析 & Plot
        NorthwindEntities dbContext = new NorthwindEntities();
        private void button34_Click(object sender, EventArgs e)
        {
            // 年度最高銷售金額 / 年度最低銷售金額
            var q1 = from o in dbContext.Orders.AsEnumerable()
                     group o by o.OrderDate.Value.Year into g
                     select new
                     {
                         Year = g.Key,
                         MaxSales = $"{g.Max(o=>o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount))):c2}",
                         MinSales = $"{g.Min(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount))):c2}"
                     };
            dataGridView1.DataSource = q1.ToList();
            label1.Text = "年度最高銷售金額 / 年度最低銷售金額";

            // 那一年總銷售最好 ? 那一年總銷售最不好 ?  
            var q2 = from od in dbContext.Order_Details.AsEnumerable()
                     group od by od.Order.OrderDate.Value.Year into g
                     select new
                     {
                         Year=g.Key,
                         Sales= $"{g.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)):c2}"
                     };
            dataGridView2.DataSource = q2.ToList();
            label2.Text = "年總銷售最好為 " + q2.First().Year + " 年，最差為 " + q2.Last().Year + " 年";

            // 那一個月總銷售最好 ? 那一個月總銷售最不好 ?
            var q3 = from od in dbContext.Order_Details.AsEnumerable()
                     group od by od.Order.OrderDate.Value.Month into g
                     orderby g.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)) descending
                     select new
                     {
                         Month = g.Key,
                         Sales = $"{g.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)):c2}"
                     };
            dataGridView3.DataSource = q3.ToList();
            label3.Text = "月總銷售最好為 " +q3.First().Month+" 月，最差為 "+q3.Last().Month+" 月";

            // 每年總銷售分析圖
            chart1.DataSource = q2.ToList();
            chart1.Series[0].XValueMember = "Year";
            chart1.Series[0].YValueMembers = "Sales";
            chart1.Series[0].ChartType = SeriesChartType.Column;

            // 每月總銷售分析圖
            chart2.DataSource = q3.ToList();
            chart2.Series[0].XValueMember = "Month";
            chart2.Series[0].YValueMembers = "Sales";
            chart2.Series[0].ChartType = SeriesChartType.Column;
        }
        #endregion

        private void button6_Click(object sender, EventArgs e)
        {
            //年銷售成長率
            var q = from o in dbContext.Orders.AsEnumerable()
                    group o by o.OrderDate.Value.Year into g
                    orderby g.Key
                    select new
                    {
                        Year=g.Key,
                        Sales=g.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)))
                    };
            dataGridView1.DataSource = q.ToList();
            label1.Text = "年度銷售";
            label2.Text = "年銷售成長率";

            List<YearOnYear> list = new List<YearOnYear>();
            var Q = q.ToList(); //先存進List
            for (int i = 1; i < Q.Count(); i++)
            {
                decimal rate = (Q[i].Sales - Q[i - 1].Sales) / Q[i - 1].Sales;
                YearOnYear yearOnYear = new YearOnYear
                {
                    year=Q[i].Year,
                    YoY=rate
                };
                list.Add(yearOnYear);
            }
            dataGridView2.DataSource = list;
            chart2.DataSource = list;
            chart2.Series[0].XValueMember = "year";
            chart2.Series[0].YValueMembers = "YoY";
            chart2.Series[0].ChartType = SeriesChartType.Column;
            chart2.Series[0].LegendText = "年銷售成長率";

            chart1.Series.Add("年銷售成長率");
            foreach(var item in list)
            {
                chart1.Series[1].Points.AddXY(item.year,item.YoY);
            }
            chart1.Series[1].XAxisType = AxisType.Secondary;
            chart1.Series[1].YAxisType = AxisType.Secondary;
            chart1.Series[1].ChartType = SeriesChartType.Line;
        }
        public class YearOnYear
        {
            public int year { get; set; }
            public decimal YoY { get; set; }
        }
    }
}
