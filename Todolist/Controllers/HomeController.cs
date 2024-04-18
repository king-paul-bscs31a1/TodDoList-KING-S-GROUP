using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Service;
using TodoList.Domain;
using Todolist.Models;
using TodoList.Domain.Interface;
using System.Security.Cryptography.X509Certificates;
using static Todolist.Controllers.HomeController;



namespace Todolist.Controllers
{
    public class HomeController(ITodoService service) : Controller
    {
        private readonly dynamic _categories = Enum.GetValues(typeof(Category)).OfType<dynamic>().Select(x => new { Key = x, Value = Enum.GetName(x) });
        private readonly dynamic _statuses = Enum.GetValues(typeof(Status)).OfType<dynamic>().Select(x => new { Key = x, Value = Enum.GetName(x) });
        //private readonly dynamic _categories = Enum.GetValues(typeof(Category)).OfType<dynamic>().Select(x => new { Key = x, Value = Enum.GetName(x) });


        public IActionResult Index(string id)
        {
            var temp = service.GetAll();


            var filters = new Filters(id);
            ViewBag.Filters = filters;
            ViewBag.Categories = _categories;
            ViewBag.Statuses = _statuses;
            ViewBag.DueFilters = Filters.DueFilterValues;

            //IQueryable<ToDo> query = context.ToDoS
            //    .Include(t => t.Category)
            //    .Include(t => t.Status);

            //if (filters.HasCategory)
            //{
            //    query = query.Where(t => t.CategoryId == filters.CategoryId);
            //}

            //if (filters.HasStatus)
            //{
            //    query = query.Where(t => t.StatusId == filters.StatusId);
            //}

            //if (filters.HasDue)
            //{
            //    var today = DateTime.Today;
            //    if (filters.IsPast)
            //    {
            //        query = query.Where(t => t.DueDate < today);
            //    }
            //    else if (filters.IsFuture)
            //    {
            //        query = query.Where(t => t.DueDate > today);

            //    }
            //    else if (filters.IsToday)
            //    {
            //        query = query.Where(t => t.DueDate == today);
            //    }
            //}
            //var tasks = query.OrderBy(t => t.DueDate).ToList();

            //return View(tasks);

            return View(temp);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = _categories;
            ViewBag.Statuses = _statuses;

            return View();
        }

        [HttpPost]
        public IActionResult Add(AddTodoModel task)
        {
            if (ModelState.IsValid)
            {
                service.Add(task);

                return RedirectToAction("Index");
            }


            ViewBag.Categories = _categories;
            ViewBag.Statuses = _statuses;
            return View(task);

        }

        //[HttpPost]
        //public IActionResult Filter(string[] filter)
        //{
        //    string id = string.Join("-", filter);
        //    return RedirectToAction("Index", new { ID = id });
        //}
        //[HttpPost]
        //public IActionResult MarkComplete([FromRoute] string id, ToDo selected)
        //{
        //    selected = context.ToDoS.Find(selected.Id);

        //    if (selected != null)
        //    {
        //        selected.StatusId = "closed";
        //        context.SaveChanges();
        //    }
        //    return RedirectToAction("Index", new { ID = id });
        //}

        [HttpPost]
        public async Task<IActionResult> MarkComplete([FromRoute] int id, ToDo selected)
        {
            var markedAsDone = await service.MarkAsDone((id));

            return RedirectToAction("Index", new { ID = id });
        }
        [HttpPost]
        public IActionResult DeleteComplete(int id)
        {
            service.Delete(id);
            return RedirectToAction("Index", new { ID = id });
        }
    }
}