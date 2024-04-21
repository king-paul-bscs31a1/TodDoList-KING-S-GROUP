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

        public IActionResult Index(string id)
        {
            var temp = service.GetAll();


            var filters = new Filters(id);
            ViewBag.Filters = filters;
            ViewBag.Categories = _categories;
            ViewBag.Statuses = _statuses;
            ViewBag.DueFilters = Filters.DueFilterValues;

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