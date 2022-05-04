using LinqLabs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Starter
{
    public partial class FrmLinq_To_Entity : Form
    {
        public FrmLinq_To_Entity()
        {
            InitializeComponent();
            dbContext.Database.Log = Console.Write; //for列印SQL query(Log為委派物件)，非必要時可註解，避免耗效能
        }
       
        NorthwindEntities dbContext = new NorthwindEntities();

        #region Intro
        private void button1_Click(object sender, EventArgs e)
        {
            //Test Entity model
            var q = from p in dbContext.Products
                    where p.UnitPrice > 30
                    select p;
            dataGridView1.DataSource = q.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //導覽屬性 - 若Table沒關聯要Join
            dataGridView1.DataSource= dbContext.Categories.First().Products.ToList(); //父找子
            MessageBox.Show(dbContext.Products.First().Category.CategoryName); //子找父
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //預存程序 - 在DbContext變方法
            dataGridView1.DataSource= dbContext.Sales_by_Year(new DateTime(1996, 1, 1), DateTime.Now).ToList();
        }
        #endregion

        #region OrderBy
        private void button22_Click(object sender, EventArgs e)
        {
            //OrderBy / ThenBy
            //By query
            var q1 = from p in dbContext.Products.AsEnumerable() //將IQueryable轉換回IEnumerable(IQueryable)
                     orderby p.UnitsInStock descending, p.ProductID
                     select new
                     {
                         p.ProductID,
                         p.ProductName,
                         p.UnitPrice,
                         p.UnitsInStock,
                         TotalPrice =$"{p.UnitPrice * p.UnitsInStock:c2}"
                     };
            dataGridView1.DataSource = q1.ToList();

            //By Method
            var q2 = dbContext.Products.OrderByDescending(p => p.UnitsInStock).ThenByDescending(p => p.ProductID);
            dataGridView2.DataSource = q2.ToList();
        }
        private void button23_Click(object sender, EventArgs e)
        {
            //new MyComparer()) - 自訂compare logic
            var q = dbContext.Products.AsEnumerable().OrderBy(p => p, new MyComparer()).ToList();
            dataGridView1.DataSource = q.ToList();
        }

        class MyComparer : IComparer<Product>
        {
            public int Compare(Product x, Product y)
            {
                if (x.UnitPrice < y.UnitPrice)
                    return -1;
                else if (x.UnitPrice > y.UnitPrice)
                    return 1;
                else
                    return string.Compare(x.ProductName[0].ToString(), y.ProductName[0].ToString(), true);
            }
        }
        #endregion

        #region Join
        private void button20_Click(object sender, EventArgs e)
        {
            //join(inner join) / join into..(left outer join)
            var q = from c in dbContext.Categories
                     join p in dbContext.Products
                     on c.CategoryID equals p.CategoryID
                     select new { p.ProductID, p.Category.CategoryName, p.ProductName, p.UnitPrice };
            dataGridView1.DataSource = q.ToList();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //點關連標記法(left out join)
            var q = from p in dbContext.Products
                    select new { p.ProductID, p.Category.CategoryName, p.ProductName, p.UnitPrice };
            dataGridView2.DataSource = q.ToList();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            //SelectMany - inner join / Cross join
            var q1 = from c in dbContext.Categories
                    from p in c.Products
                    select new { c.CategoryID, c.CategoryName, p.ProductName, p.UnitPrice };
            dataGridView1.DataSource = q1.ToList();

            var q2=dbContext.Categories.SelectMany(c => c.Products, (c, p) => new { c.CategoryID, c.CategoryName, p.ProductName, p.UnitPrice });
            dataGridView2.DataSource = q2.ToList();
        }
        #endregion

        #region Join/GroupBy
        private void button11_Click(object sender, EventArgs e)
        {
            //AvgUnitPrice of each Category
            var q = from p in dbContext.Products
                    group p by p.Category.CategoryName into g
                    select new { CategoryName = g.Key, AvgUnitPrice = g.Average(p => p.UnitPrice) };
            dataGridView1.DataSource = q.ToList();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //Orders -  Group by 日期 / 大小
            var q1 = from o in dbContext.Orders
                    group o by o.OrderDate.Value.Year into g //DateTime? - ?表示可為null的數值型別，取值方式--.Value
                    orderby g.Key
                    select new { Year = g.Key, Count = g.Count() };
            dataGridView1.DataSource = q1.ToList();

            var q2 = from o in this.dbContext.Orders
                     group o by new { o.OrderDate.Value.Year, o.OrderDate.Value.Month } into g
                     select new { Year = g.Key, Count = g.Count() };
            dataGridView2.DataSource = q2.ToList();
        }
        #endregion

        #region CRUD
        private void button55_Click(object sender, EventArgs e)
        {
            //Create(Insert)
            Product pro = new Product { ProductName ="Test"+DateTime.Now.ToLongTimeString(), Discontinued=true};
            dbContext.Products.Add(pro);
            dbContext.SaveChanges();
            Read_RefreshDataGridView();
        }

        private void button56_Click(object sender, EventArgs e)
        {
            //Update
            var product = (from p in dbContext.Products
                           where p.ProductName.Contains("Test")
                           select p).FirstOrDefault();
            if (product == null) return; //exit method
            product.ProductName = "Test" + product.ProductName;
            dbContext.SaveChanges();
            Read_RefreshDataGridView();
        }

        private void button53_Click(object sender, EventArgs e)
        {
            //Delete
            var product = (from p in this.dbContext.Products
                           where p.ProductName.Contains("Test")
                           select p).FirstOrDefault();

            if (product == null) return;
            dbContext.Products.Remove(product);
            dbContext.SaveChanges();
            Read_RefreshDataGridView();
        }

        void Read_RefreshDataGridView()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = this.dbContext.Products.ToList();
        }
        #endregion
    }
}
