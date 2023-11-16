using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SysPet.Data;
using SysPet.Exception;
using SysPet.Models;
using System.Diagnostics;

namespace SysPet.Controllers
{
    [ServiceFilter(typeof(ManageExceptionFilter))]

    public class HomeController : Controller
    {
        private readonly SalesData _salesData;
        private readonly InternmentsData _internmentsData;
        private readonly DatingData _dingData;
        private readonly ProductsData _productsData;
        public HomeController()
        {
            _salesData = new SalesData();
            _internmentsData = new InternmentsData();
            _productsData = new ProductsData();
            _dingData = new DatingData();
        }

        public IActionResult Index2()
        {
            var user = HttpContext.Session.GetString("User");
            ViewBag.User = user;

            var model = new ChartViewModel
            {
                Labels = new List<string> { "January", "February", "March", "April", "May", "June" },
                Values = new List<int> { 10, 20, 15, 25, 30, 22 }
            };

            var stackedBar = new StackedBarChartModel
            {
                Labels = new List<string> { "January", "February", "March", "April", "May", "June" },
                Datasets = new List<StackedBarDataset>
                {
            new StackedBarDataset
            {
                Label = "Dataset 1",
                Data = new List<int> { 10, 20, 15, 25, 30, 22 },
                BackgroundColor = "rgba(75, 192, 192, 0.5)"
            },
            new StackedBarDataset
            {
                Label = "Dataset 2",
                Data = new List<int> { 5, 15, 10, 20, 25, 18 },
                BackgroundColor = "rgba(255, 99, 132, 0.5)"
            }
        }
            };

            var pieChart = new PieChartViewModel
            {
                Labels = new List<string> { "Red", "Blue", "Yellow", "Green", "Purple", "Orange" },
                Data = new List<int> { 10, 20, 15, 25, 30, 22 },
                BackgroundColors = new List<string>
        {
            "rgba(255, 99, 132, 0.5)",
            "rgba(54, 162, 235, 0.5)",
            "rgba(255, 206, 86, 0.5)",
            "rgba(75, 192, 192, 0.5)",
            "rgba(153, 102, 255, 0.5)",
            "rgba(255, 159, 64, 0.5)"
        }
            };



            var multiLineChartModel = new MultiLineChartModel
            {
                Labels = new List<string> { "January", "February", "March", "April", "May", "June" },
                Datasets = new List<LineDataset>
        {
            new LineDataset
            {
                Label = "Dataset 1",
                Data = new List<int> { 10, 20, 15, 25, 30, 22 },
                BorderColor = "rgba(75, 192, 192, 1)",
                BorderWidth = 2
            },
            new LineDataset
            {
                Label = "Dataset 2",
                Data = new List<int> { 5, 15, 10, 20, 25, 18 },
                BorderColor = "rgba(255, 99, 132, 1)",
                BorderWidth = 2
            }
        }
            };

            var radarChart = new RadarChartModel
            {
                Labels = new List<string> { "Eating", "Drinking", "Sleeping", "Designing", "Coding", "Cycling", "Running" },
                Datasets = new List<RadarDataset>
        {
            new RadarDataset
            {
                Label = "Dataset 1",
                Data = new List<int> { 65, 59, 90, 81, 56, 55, 40 },
                BorderColor = "rgba(75, 192, 192, 1)",
                BackgroundColor = "rgba(75, 192, 192, 0.2)",
                BorderWidth = 2
            },
            new RadarDataset
            {
                Label = "Dataset 2",
                Data = new List<int> { 28, 48, 40, 19, 96, 27, 100 },
                BorderColor = "rgba(255, 99, 132, 1)",
                BackgroundColor = "rgba(255, 99, 132, 0.2)",
                BorderWidth = 2
            }
        }
            };

            var bubbleChart = new BubbleChartModel
            {
                Datasets = new List<BubbleDataset>
            {
                new BubbleDataset
                {
                    Label = "Dataset 1",
                    Data = GenerateRandomData(10),
                    BackgroundColor = "rgba(255, 99, 132, 0.5)"
                },
                new BubbleDataset
                {
                    Label = "Dataset 2",
                    Data = GenerateRandomData(10),
                    BackgroundColor = "rgba(54, 162, 235, 0.5)"
                }
            }
            };

            var doughnutChart = new DoughnutChartModel
            {
                Datasets = new List<DoughnutDataset>
                {
                    new DoughnutDataset
                    {
                        Data = new List<int> { 30, 40, 30 },
                        BackgroundColor = new List<string> { "rgba(255, 99, 132, 0.5)", "rgba(54, 162, 235, 0.5)", "rgba(255, 206, 86, 0.5)" },
                        HoverBackgroundColor = new List<string> { "rgba(255, 99, 132, 1)", "rgba(54, 162, 235, 1)", "rgba(255, 206, 86, 1)" }
                    }
                }
            };

            model.StackedBarChart = stackedBar;
            model.PieChart = pieChart;
            model.MultiLineChart = multiLineChartModel;
            model.RadarChart = radarChart;
            model.BubbleChart = bubbleChart;
            model.DoughnutChart = doughnutChart;

            return View(model);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            var user = HttpContext.Session.GetString("User");
            ViewBag.User = user;

            var lastFiveDays = DateTime.Now.AddDays(-5);
            var result = await _salesData.GetOnlySales();
            var saleList = result.Where(x => x.FechaVenta.Date >=  lastFiveDays.Date && x.FechaVenta.Date < DateTime.Now.Date).ToList();
            var labels = saleList.Select(x => x.FechaVenta.ToString()).ToList();
            var values = saleList.Select(x => (int)x.Cantidad).ToList();
            var model = new ChartViewModel
            {
                Labels = labels,
                Values = values,
            };

            var internments = await _internmentsData.GetOnlyInternments();
            var internmentList = internments.Where(x => x.Fecha.Date >= lastFiveDays.Date && x.Fecha.Date < DateTime.Now.Date).ToList();
            var internmentLabels = internmentList.Select(x => x.Fecha.ToString()).ToList();
            var internmentValues = internmentList.Select(x => (int)x.CantidadRegistros).ToList();
            var internmentChart = new InternmentChartViewModel
            {
                Labels = internmentLabels,
                Values = internmentValues,
            };

            var appointments = await _dingData.GetOnlyAppointments();
            var appointmentList = appointments.Where(x => x.Fecha.Date >= lastFiveDays.Date && x.Fecha.Date < DateTime.Now.Date).ToList();
            var appointmentLabels = appointmentList.Select(x => x.Fecha.ToString()).ToList();
            var appointmentValues = appointmentList.Select(x => (int)x.CantidadRegistros).ToList();
            var appointmentChart = new AppointmentChartViewModel
            {
                Labels = appointmentLabels,
                Values = appointmentValues,
            };

            var productsToExpired = await _productsData.GetExpiredProducts();
            var productLabels = productsToExpired.Select(x => x.Nombre).ToList();
            var productValues = productsToExpired.Select(x => x.Stock).ToList();
            var productChart = new ProductExpiredChartModel
            {
                Labels = productLabels,
                Datasets = new List<DoughnutDataset>
                {
                    new DoughnutDataset
                    {
                        Data = productValues,
                        BackgroundColor = new List<string> { "rgba(255, 99, 132, 0.5)", "rgba(54, 162, 235, 0.5)", "rgba(255, 206, 86, 0.5)","rgba(153,255,102,0.6)" },
                        HoverBackgroundColor = new List<string> { "rgba(255, 99, 132, 1)", "rgba(54, 162, 235, 1)", "rgba(255, 206, 86, 1)", "rgba(153,255,102, 1)" }
                    }
                },
            };

            var productsInStock = await _productsData.GetExpirdeStok();
            var stockLabels = productsInStock.Select(x => x.Nombre).ToList();
            var stockValues = productsInStock.Select(x => x.Stock).ToList();
            var productStockChart = new ProductInStockChartModel
            {
                Labels = stockLabels,
                Datasets = new List<DoughnutDataset>
                {
                    new DoughnutDataset
                    {
                        Data = stockValues,
                        BackgroundColor = new List<string> { "rgba(255, 99, 132, 0.5)", "rgba(54, 162, 235, 0.5)", "rgba(255, 206, 86, 0.5)","rgba(153,255,102,0.6)" },
                        HoverBackgroundColor = new List<string> { "rgba(255, 99, 132, 1)", "rgba(54, 162, 235, 1)", "rgba(255, 206, 86, 1)", "rgba(153,255,102, 1)" }
                    }
                },
            };

            var saleDetailList = await _salesData.GetOnlySalesDetail();
            var labelDetails = saleDetailList.Select(x => x.Articulo).ToList();
            var valueDetails = saleDetailList.Select(x => (int)x.Cantidad).ToList();

            var doughnutChart = new DoughnutChartModel
            {
                Datasets = new List<DoughnutDataset>
                {
                    new DoughnutDataset
                    {
                        Data = valueDetails,
                        BackgroundColor = new List<string> { "rgba(255, 99, 132, 0.5)", "rgba(54, 162, 235, 0.5)", "rgba(255, 206, 86, 0.5)","rgba(153,255,102,0.6)" },
                        HoverBackgroundColor = new List<string> { "rgba(255, 99, 132, 1)", "rgba(54, 162, 235, 1)", "rgba(255, 206, 86, 1)", "rgba(153,255,102, 1)" }
                    }
                },
                Labels = labelDetails,
            };

            var totalSalesByMonth = await _salesData.GetOnlyTotalSales();
            var totalSalesLabels = totalSalesByMonth.OrderBy(x => x.MesNumero).Select(x => x.Mes).ToList();
            var totalSalesValues = totalSalesByMonth.OrderBy(x => x.MesNumero).Select(x => x.TotalVentas).ToList();

            var multiLineChartModel = new MultiLineChartModel
            {
                Labels = totalSalesLabels,
                Datasets = new List<LineDataset>
                {
                    new LineDataset
                    {
                        Label = "Dataset 1",
                        Data = totalSalesValues,
                        BorderColor = "rgba(75, 192, 192, 1)",
                        BorderWidth = 2
                    }
                }
            };

            model.DoughnutChart = doughnutChart;
            model.InternmentChart = internmentChart;
            model.AppointmentChart = appointmentChart;
            model.ProductExpiredChart = productChart;
            model.ProductInStockChart = productStockChart;
            model.MultiLineChart = multiLineChartModel;

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View();
        }

        private List<BubbleData> GenerateRandomData(int count)
        {
            var random = new Random();
            var data = new List<BubbleData>();

            for (int i = 0; i < count; i++)
            {
                data.Add(new BubbleData
                {
                    X = random.Next(1, 100),
                    Y = random.Next(1, 100),
                    Radius = random.Next(5, 20)
                });
            }

            return data;
        }
    }
}