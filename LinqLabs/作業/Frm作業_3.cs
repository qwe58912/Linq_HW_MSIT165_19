using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LinqLabs.作業
{
    public partial class Frm作業_3 : Form
    {

        List<Student> students_scores = new List<Student>()
            {
            new Student{ Name = "aaa", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Male" },
            new Student{ Name = "bbb", Class = "CS_102", Chi = 80, Eng = 80, Math = 100, Gender = "Male" },
            new Student{ Name = "ccc", Class = "CS_101", Chi = 60, Eng = 50, Math = 75, Gender = "Female" },
            new Student{ Name = "ddd", Class = "CS_102", Chi = 80, Eng = 70, Math = 85, Gender = "Female" },
            new Student{ Name = "eee", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Female" },
            new Student{ Name = "fff", Class = "CS_102", Chi = 80, Eng = 80, Math = 80, Gender = "Female" },

            };
        public class Student
        {
            public string Name { get; set; }
            public string Class { get; set; }
            public int Chi { get; set; }
            public int Eng { get; set; }
            public int Math { get; set; }
            public string Gender { get; set; }
        }
        NorthwindEntities dbContext = new NorthwindEntities();
        public Frm作業_3()
        {
            InitializeComponent();

            //hint
            //students_scores = new List<Student>()
            {
                //new Student { Name = "aaa", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Male" },
                //new Student { Name = "bbb", Class = "CS_102", Chi = 80, Eng = 80, Math = 100, Gender = "Male" },
                //new Student { Name = "ccc", Class = "CS_101", Chi = 60, Eng = 50, Math = 75, Gender = "Female" },
                //new Student { Name = "ddd", Class = "CS_102", Chi = 80, Eng = 70, Math = 85, Gender = "Female" },
                //new Student { Name = "eee", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Female" },
                //new Student { Name = "fff", Class = "CS_102", Chi = 80, Eng = 80, Math = 80, Gender = "Female" },

            };
        }

        private void button36_Click(object sender, EventArgs e)
        {
            #region 搜尋 班級學生成績

            // 
            // 共幾個 學員成績 ?						
            var q = from s in students_scores
                    select s;
            this.dataGridView1.DataSource = students_scores;
            MessageBox.Show($"共{q.Count().ToString()}個學員");


            //=====================================================================
            // 找出 前面三個 的學員所有科目成績		
            var q1 = from s in students_scores.Take(3)
                     select s;
            //this.dataGridView1.DataSource = q1.ToList();


            //=====================================================================
            // 找出 後面兩個 的學員所有科目成績					
            var q2 = from s in students_scores.Skip(students_scores.Count - 2)
                     select s;
            //this.dataGridView1.DataSource = q2.ToList();


            //=====================================================================
            // 找出 Name 'aaa','bbb','ccc' 的學員國文英文科目成績						
            var q3 = from s in students_scores
                     where s.Name == "aaa" || s.Name == "bbb" || s.Name == "ccc"
                     select new { s.Name, 國文成績 = s.Chi, 英文成績 = s.Eng };
            //this.dataGridView1.DataSource = q3.ToList();


            //=====================================================================
            // 找出學員 'bbb' 的成績	                          
            var q4 = from s in students_scores
                     where s.Name == "bbb"
                     select s;
            //this.dataGridView1.DataSource = q4.ToList();


            //=====================================================================
            // 找出除了 'bbb' 學員的學員的所有成績 ('bbb' 退學)	
            var q5 = from s in students_scores
                     where s.Name != "bbb"
                     select s;
            //this.dataGridView1.DataSource = q5.ToList();


            //=====================================================================
            // 找出 'aaa', 'bbb' 'ccc' 學員 國文數學兩科 科目成績  |	
            var q6 = from s in students_scores
                     where s.Name == "aaa" || s.Name == "bbb" || s.Name == "ccc"
                     select new { s.Name, 國文成績 = s.Chi, 數學成績 = s.Math };
            //this.dataGridView1.DataSource = q6.ToList();


            // 數學不及格 ... 是誰 
            var q7 = from s in students_scores
                     where s.Math < 60
                     select s;
            //this.dataGridView1.DataSource = q7.ToList();
            #endregion
        }

        private void button37_Click(object sender, EventArgs e)
        {
            //個人 sum, min, max, avg
            // 統計 每個學生個人成績 並排序
            var Scores = students_scores.Select(s => new
            {
                Name = s.Name,
                sumScore = new[] { s.Chi, s.Eng, s.Math }.Sum(),
                minScore = new[] { s.Chi, s.Eng, s.Math }.Min(),
                maxScore = new[] { s.Chi, s.Eng, s.Math }.Max(),
                avgScore = new[] { s.Chi, s.Eng, s.Math }.Average()

            }).ToList();

            this.dataGridView1.DataSource = Scores;
        }

        private void button33_Click(object sender, EventArgs e)
        {
            // split=> 分成 三群 '待加強'(60~69) '佳'(70~89) '優良'(90~100) 
            // print 每一群是哪幾個 ? (每一群 sort by 分數 descending)
            var q = from s in students_scores
                    group s by (s.Chi + s.Eng + s.Math) / 3 >= 90 ? "優良" : (s.Chi >= 70 ? "佳" : "待加強") into n
                    select new
                    {
                        AverageScoreRank = n.Key,
                        Count = n.Count(),
                        Mygroup = n.OrderByDescending(s => (s.Chi + s.Eng + s.Math) / 3)
                    };

            this.dataGridView1.DataSource = q.ToList();
            this.treeView1.Nodes.Clear();
            foreach (var group in q)
            {
                string s = $"{group.AverageScoreRank} ({group.Count})";
                TreeNode node = this.treeView1.Nodes.Add(s);

                foreach (var item in group.Mygroup)
                {
                    node.Nodes.Add($"{item.Name} - {(item.Chi+item.Eng+item.Math)/3}分");
                }
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();

            var file = from f in files
                       orderby f.Length descending
                       group f by f.Length > 100000 ? ">100000" : "<=100000" into n
                       select new { Length = n.Key, Count = n.Count(), Mygroup = n };

            this.dataGridView1.DataSource = file.ToList();
            this.treeView1.Nodes.Clear();
            foreach (var group in file)
            {
                TreeNode node = this.treeView1.Nodes.Add(group.Length);

                foreach (var item in group.Mygroup)
                {
                    node.Nodes.Add($"{item.ToString()}  {item.Length}");
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();

            var file = from f in files
                       orderby f.CreationTime descending
                       group f by f.CreationTime.Year into n
                       select new { CreationTime = n.Key, Count = n.Count(), Mygroup = n };

            this.dataGridView1.DataSource = file.ToList();
            this.treeView1.Nodes.Clear();
            foreach (var group in file)
            {
                TreeNode node = this.treeView1.Nodes.Add(group.CreationTime.ToString());

                foreach (var item in group.Mygroup)
                {
                    node.Nodes.Add($"{item.ToString()}   {item.CreationTime}");
                }

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var product = from p in this.dbContext.Products
                          group p by p.UnitPrice < 10 ? "低":(p.UnitPrice < 50 ? "中":"高") into p2
                          select new { p2.Key, Count = p2.Count(), Mygroup = p2};

            dataGridView1.DataSource = product.ToList();
            this.treeView1.Nodes.Clear();
            foreach (var group in product)
            {
                TreeNode node = this.treeView1.Nodes.Add($"{group.Key} ({group.Count})");
                foreach (var item in group.Mygroup)
                {
                    node.Nodes.Add($"{item.ProductName} - {item.UnitPrice:c2}");
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            var order1 = from o1 in this.dbContext.Orders
                         select o1;

            dataGridView1.DataSource = order1.ToList();

            var order = from o in this.dbContext.Orders
                        group o by o.OrderDate.Value.Year into yearGroup
                        select new { yearGroup.Key, Count = yearGroup.Count(),Mygroup = yearGroup };

            this.treeView1.Nodes.Clear();
            foreach (var group in order)
            {
                TreeNode node = this.treeView1.Nodes.Add($"{group.Key} ({group.Count})");
                foreach (var item in group.Mygroup)
                {
                    node.Nodes.Add($"OrderID:{item.OrderID} - OrderDate:{item.OrderDate:yyyy/MM/dd}");
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var od = from o1 in this.dbContext.Orders
                         select o1;

            dataGridView1.DataSource = od.ToList();

            var order = from o in this.dbContext.Orders
                        group o by o.OrderDate.Value.Year into yearGroup
                        select new
                        {
                            Year = yearGroup.Key,
                            YearCount = yearGroup.Count(),
                            Mygroup = (from m in yearGroup
                                       group m by m.OrderDate.Value.Month into monthGroup
                                       select new
                                       {
                                           Month = monthGroup.Key,
                                           MonthCount = monthGroup.Count(),
                                           Order = monthGroup
                                       })

                        };


            this.treeView1.Nodes.Clear();
            foreach (var yearGroup in order)
            {
                TreeNode yearNode = new TreeNode($"{yearGroup.Year} ({yearGroup.YearCount})");
                treeView1.Nodes.Add(yearNode);

                foreach (var monthGroup in yearGroup.Mygroup)
                {
                    TreeNode monthNode = new TreeNode($"{monthGroup.Month}月 - ({monthGroup.MonthCount})");
                    yearNode.Nodes.Add(monthNode);

                    foreach (var order1 in monthGroup.Order)
                    {
                        monthNode.Nodes.Add($"OrderID:{order1.OrderID} - OrderDate:{order1.OrderDate:yyyy/MM/dd}");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var order = from o in this.dbContext.Orders.ToList()
                          join d in this.dbContext.Order_Details
                          on o.OrderID equals d.OrderID
                          group d by o.EmployeeID into em
                          select new { EmployeeID =em.Key, TotalSales = em.Sum(d => d.Quantity * d.UnitPrice) };

            dataGridView1.DataSource = order.ToList();

            var totalSales = (from d in this.dbContext.Order_Details
                              select d.Quantity * d.UnitPrice).Sum();

            MessageBox.Show($"總銷售金額: {totalSales}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var order = from o in this.dbContext.Orders.ToList()
                          join d in this.dbContext.Order_Details
                          on o.OrderID equals d.OrderID
                          group d by o.EmployeeID into em
                          select new { EmployeeID = em.Key, TotalSales = em.Sum(d => d.Quantity * d.UnitPrice)  } into top
                          orderby top.TotalSales descending
                          select top;

            dataGridView1.DataSource = order.Take(5).ToList();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var product = from p in this.dbContext.Products.ToList()
                          join c in this.dbContext.Categories
                          on p.CategoryID equals c.CategoryID
                          group p by new { c.CategoryName, p.ProductName, p.UnitPrice } into g
                          select new
                          {
                              ProductName = g.Key.ProductName,
                              Category = g.Key.CategoryName,
                              UnitPrice = g.Key.UnitPrice
                          } into p2
                          orderby p2.UnitPrice descending
                          select p2;

            dataGridView1.DataSource = product.Take(5).ToList();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var product = from p in this.dbContext.Products.ToList()
                          join c in this.dbContext.Categories
                          on p.CategoryID equals c.CategoryID
                          group p by new { c.CategoryName, p.ProductName, p.UnitPrice } into g
                          select new
                          {
                              ProductName = g.Key.ProductName,
                              Category = g.Key.CategoryName,
                              UnitPrice = g.Key.UnitPrice
                          } into p2
                          where p2.UnitPrice > 300
                          orderby p2.UnitPrice descending
                          select p2;

            
            dataGridView1.DataSource = product.ToList();
            if (dataGridView1.Rows.Count==0)
                MessageBox.Show("無任何商品單價>300");
        }
    }
}
