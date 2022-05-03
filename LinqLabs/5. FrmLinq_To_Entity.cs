﻿using LinqLabs;
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Test Entity model
            NorthwindEntities dbContext = new NorthwindEntities();
            var q = from p in dbContext.Products
                    where p.UnitPrice > 30
                    select p;
            dataGridView1.DataSource = q.ToList();
        }
    }
}
