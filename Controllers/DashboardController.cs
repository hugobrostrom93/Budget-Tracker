using BudgetTrackerHugo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetTrackerHugo.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        // TEST
        [Authorize]
        // TEST
        public async Task<ActionResult> Index()
        {
            //Last 30 Days
            DateTime StartDate = DateTime.Today.AddDays(-29);
            DateTime EndDate = DateTime.Today;

            List<Transaction> SelectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate)
                .ToListAsync();

            //Total income
            int TotalIncome = SelectedTransactions
                .Where(x => x.Category.Type == "Income")
                .Sum(y => y.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("#,##0 kr");

            //Total expense
            int TotalExpense = SelectedTransactions
                .Where(x => x.Category.Type == "Expense")
                .Sum(y => y.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("#,##0 kr");

            //Balance
            int Balance = TotalIncome - TotalExpense;
            ViewBag.Balance = Balance.ToString("#,##0 kr");

            //Donut Chart - Expense By Category
            ViewBag.DonutChartData = SelectedTransactions
                .Where(x => x.Category.Type == "Expense")
                .GroupBy(y => y.Category.CategoryId)
                .Select(z => new
                {
                    categoryTitleWithIcon = z.First().Category.Icon + " " + z.First().Category.Title,
                    amount = z.Sum(l => l.Amount),
                    formattedAmount = z.Sum(y => y.Amount).ToString("0kr"),

                }).ToList();

            //Donut Chart - Income By Category
            ViewBag.DonutChartDataTwo = SelectedTransactions
                .Where(x => x.Category.Type == "Income")
                .GroupBy(y => y.Category.CategoryId)
                .Select(z => new
                {
                    categoryTitleWithIcon = z.First().Category.Icon + " " + z.First().Category.Title,
                    amount = z.Sum(l => l.Amount),
                    formattedAmount = z.Sum(y => y.Amount).ToString("0kr"),
                }).ToList();

            //10 Recent transactions
            ViewBag.RecentTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate)
                .OrderByDescending(c => c.Date)
                .Take(1000)
                .ToListAsync();


            return View();
        }
    }

    public class SplineChartData
    {
        public string day;
        public int income;
        public int expense;
    }
}
