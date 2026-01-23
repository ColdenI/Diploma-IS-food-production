using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace Program.scr.forms
{

    public partial class AnalyticsForm : Form
    {
        private Button btnGenerateReport;
        private Button btnSaveHtml;
        private RichTextBox rtbReport;
        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;

        public AnalyticsForm()
        {
            InitializeComponent();
            InitializeComponent_();
        }

        private void InitializeComponent_()
        {
            this.Size = new Size(1000, 700);
            this.Text = "Аналитика";
            this.StartPosition = FormStartPosition.CenterScreen;

            // Controls
            var lblPeriod = new Label { Text = "Период:", Location = new Point(20, 20) };
            dtpStart = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(120, 20) };
            dtpEnd = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(340, 20) };
            dtpEnd.Value = DateTime.Today;
            dtpStart.Value = DateTime.Today.AddMonths(-1);

            btnGenerateReport = new Button { Text = "Сформировать отчет", Location = new Point(550, 18), Size = new Size(120, 30) };
            btnGenerateReport.Click += BtnGenerateReport_Click;

            btnSaveHtml = new Button { Text = "Сохранить в HTML", Location = new Point(700, 20), Size = new Size(120, 30) };
            btnSaveHtml.Click += BtnSaveHtml_Click;
            btnSaveHtml.Visible = false;

            rtbReport = new RichTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(950, 600),
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Font = new Font("Consolas", 10)
            };

            this.Controls.AddRange(new Control[] { lblPeriod, dtpStart, dtpEnd, btnGenerateReport, btnSaveHtml, rtbReport });
        }

        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            var start = dtpStart.Value.Date;
            var end = dtpEnd.Value.Date.AddDays(1).AddSeconds(-1); // Включительно

            var report = GenerateFullReport(start, end);

            rtbReport.Text = report;
            btnSaveHtml.Enabled = true;
        }

        private string GenerateFullReport(DateTime start, DateTime end)
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendLine($"=== Отчет по анализу ===");
            sb.AppendLine($"Период: {start:dd.MM.yyyy} — {end:dd.MM.yyyy}");
            sb.AppendLine();

            sb.AppendLine("--- Лучшие менеджеры ---");
            var bestManagers = GetBestManagers(start, end);
            foreach (var item in bestManagers)
                sb.AppendLine($"{item.FullName}: {item.Count} заказов");
            if (!bestManagers.Any()) sb.AppendLine("Нет данных.");
            sb.AppendLine();

            sb.AppendLine("--- Лучшие повара ---");
            var bestCooks = GetBestCooks(start, end);
            foreach (var item in bestCooks)
                sb.AppendLine($"{item.FullName}: {item.Count} заказов");
            if (!bestCooks.Any()) sb.AppendLine("Нет данных.");
            sb.AppendLine();

            sb.AppendLine("--- Лучшие админы ---");
            var bestAdmins = GetBestAdmins(start, end);
            foreach (var item in bestAdmins)
                sb.AppendLine($"{item.FullName}: {item.Count} заказов");
            if (!bestAdmins.Any()) sb.AppendLine("Нет данных.");
            sb.AppendLine();

            sb.AppendLine("--- Прибыль ---");
            var profit = GetProfit(start, end);
            sb.AppendLine($"Общая прибыль: {profit:C}");
            sb.AppendLine();

            sb.AppendLine("--- Самые популярные товары ---");
            var popularProducts = GetPopularProducts(start, end);
            foreach (var item in popularProducts)
                sb.AppendLine($"{item.Name}: {item.Quantity} шт.");
            if (!popularProducts.Any()) sb.AppendLine("Нет данных.");
            sb.AppendLine();

            sb.AppendLine("--- Расход сырья ---");
            var rawUsage = GetRawMaterialUsage(start, end);
            foreach (var item in rawUsage)
                sb.AppendLine($"{item.Name}: {item.Quantity:F3} ед.");
            if (!rawUsage.Any()) sb.AppendLine("Нет данных.");
            sb.AppendLine();

            sb.AppendLine("--- Анализ заказов ---");
            var orderStats = GetOrderStats(start, end);
            sb.AppendLine($"Всего заказов: {orderStats.Total}");
            sb.AppendLine($"Завершено: {orderStats.Completed}");
            sb.AppendLine($"Среднее время выполнения: {orderStats.AvgTime:g}");
            if (orderStats.Total == 0) sb.AppendLine("Нет данных.");
            sb.AppendLine();

            return sb.ToString();
        }

        private List<RankedEmployee> GetBestManagers(DateTime start, DateTime end)
        {
            var list = new List<RankedEmployee>();
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    SELECT e.FullName, COUNT(*) AS Count
                    FROM SalesOrders s
                    INNER JOIN Employees e ON s.ManagerID = e.ID
                    WHERE s.OrderDate BETWEEN @Start AND @End
                    GROUP BY e.FullName
                    ORDER BY Count DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@Start", start);
                    cmd.Parameters.AddWithValue("@End", end);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new RankedEmployee
                            {
                                FullName = reader.GetString("FullName"),
                                Count = reader.GetInt32("Count")
                            });
                        }
                    }
                }
            }
            return list.Take(5).ToList();
        }

        private List<RankedEmployee> GetBestCooks(DateTime start, DateTime end)
        {
            var list = new List<RankedEmployee>();
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    SELECT e.FullName, COUNT(*) AS Count
                    FROM SalesOrders s
                    INNER JOIN Employees e ON s.CookID = e.ID
                    WHERE s.OrderDate BETWEEN @Start AND @End
                    GROUP BY e.FullName
                    ORDER BY Count DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@Start", start);
                    cmd.Parameters.AddWithValue("@End", end);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new RankedEmployee
                            {
                                FullName = reader.GetString("FullName"),
                                Count = reader.GetInt32("Count")
                            });
                        }
                    }
                }
            }
            return list.Take(5).ToList();
        }

        private List<RankedEmployee> GetBestAdmins(DateTime start, DateTime end)
        {
            var list = new List<RankedEmployee>();
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    SELECT e.FullName, COUNT(*) AS Count
                    FROM SalesOrders s
                    INNER JOIN Employees e ON s.ManagerID = e.ID
                    INNER JOIN Auth a ON e.ID = a.EmployeeID
                    WHERE a.AccessLevel = 0 AND s.OrderDate BETWEEN @Start AND @End
                    GROUP BY e.FullName
                    ORDER BY Count DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@Start", start);
                    cmd.Parameters.AddWithValue("@End", end);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new RankedEmployee
                            {
                                FullName = reader.GetString("FullName"),
                                Count = reader.GetInt32("Count")
                            });
                        }
                    }
                }
            }
            return list.Take(5).ToList();
        }

        private decimal GetProfit(DateTime start, DateTime end)
        {
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    SELECT SUM(TotalAmount) AS Profit
                    FROM SalesOrders
                    WHERE OrderDate BETWEEN @Start AND @End
                      AND Status IN ('Завершено')", conn))
                {
                    cmd.Parameters.AddWithValue("@Start", start);
                    cmd.Parameters.AddWithValue("@End", end);
                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }
        }

        private List<RankedProduct> GetPopularProducts(DateTime start, DateTime end)
        {
            var list = new List<RankedProduct>();
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    SELECT p.Name, SUM(oi.Quantity) AS Quantity
                    FROM OrderItems oi
                    INNER JOIN Products p ON oi.ProductID = p.ID
                    INNER JOIN SalesOrders s ON oi.OrderID = s.ID
                    WHERE s.OrderDate BETWEEN @Start AND @End
                    GROUP BY p.Name
                    ORDER BY Quantity DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@Start", start);
                    cmd.Parameters.AddWithValue("@End", end);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new RankedProduct
                            {
                                Name = reader.GetString("Name"),
                                Quantity = reader.GetDecimal("Quantity")
                            });
                        }
                    }
                }
            }
            return list.Take(10).ToList();
        }

        private List<RankedRawMaterial> GetRawMaterialUsage(DateTime start, DateTime end)
        {
            var list = new List<RankedRawMaterial>();
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    SELECT rm.Name, SUM(r.Quantity * oi.Quantity) AS Quantity
                    FROM OrderItems oi
                    INNER JOIN Products p ON oi.ProductID = p.ID
                    INNER JOIN Recipes r ON p.ID = r.ProductID
                    INNER JOIN RawMaterials rm ON r.RawMaterialID = rm.ID
                    INNER JOIN SalesOrders s ON oi.OrderID = s.ID
                    WHERE s.OrderDate BETWEEN @Start AND @End
                    GROUP BY rm.Name
                    ORDER BY Quantity DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@Start", start);
                    cmd.Parameters.AddWithValue("@End", end);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new RankedRawMaterial
                            {
                                Name = reader.GetString("Name"),
                                Quantity = reader.GetDecimal("Quantity")
                            });
                        }
                    }
                }
            }
            return list.Take(10).ToList();
        }

        private OrderStats GetOrderStats(DateTime start, DateTime end)
        {
            var stats = new OrderStats();
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    SELECT COUNT(*) AS Total,
                           SUM(CASE WHEN Status = 'Завершён' THEN 1 ELSE 0 END) AS Completed,
                           AVG(CAST(DATEDIFF(SECOND, OrderDate, CompletionDate) AS FLOAT)) AS AvgSecs
                    FROM SalesOrders
                    WHERE OrderDate BETWEEN @Start AND @End
                      AND CompletionDate IS NOT NULL", conn))
                {
                    cmd.Parameters.AddWithValue("@Start", start);
                    cmd.Parameters.AddWithValue("@End", end);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.Total = reader.GetInt32("Total");
                            stats.Completed = reader.IsDBNull("Completed") ? 0 : reader.GetInt32("Completed");
                            var avgSecs = reader.IsDBNull("AvgSecs") ? 0 : reader.GetDouble("AvgSecs");
                            stats.AvgTime = TimeSpan.FromSeconds(avgSecs);
                        }
                    }
                }
            }
            return stats;
        }

        private void BtnSaveHtml_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "HTML Files (*.html)|*.html",
                FileName = $"Аналитика_{dtpStart.Value:yyyy-MM-dd}_to_{dtpEnd.Value:yyyy-MM-dd}.html"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var html = GenerateHtmlReport();
                File.WriteAllText(saveFileDialog.FileName, html, System.Text.Encoding.UTF8);
                MessageBox.Show("Отчет сохранен в HTML.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GenerateHtmlReport()
        {
            var start = dtpStart.Value.Date;
            var end = dtpEnd.Value.Date.AddDays(1).AddSeconds(-1);

            var report = GenerateFullReport(start, end);

            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Отчет по аналитике</title>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; }}
        h1, h2 {{ color: #2c3e50; }}
        pre {{ background-color: #f4f4f4; padding: 10px; border-radius: 5px; }}
    </style>
</head>
<body>
    <h1>Отчет по аналитике</h1>
    <p><strong>Период:</strong> {start:dd.MM.yyyy} — {end:dd.MM.yyyy}</p>
    <pre>{report}</pre>
</body>
</html>";

            return html;
        }
    }

    // Вспомогательные классы
    public class RankedEmployee
    {
        public string FullName { get; set; }
        public int Count { get; set; }
    }

    public class RankedProduct
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
    }

    public class RankedRawMaterial
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
    }

    public class OrderStats
    {
        public int Total { get; set; } = 0;
        public int Completed { get; set; } = 0;
        public TimeSpan AvgTime { get; set; } = TimeSpan.Zero;
    }
}
