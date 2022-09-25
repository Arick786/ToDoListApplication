using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Todoapp.Models;
using ToDoList.Model;

namespace Todoapp.Controllers
{
    public class HomeController : Controller
    {
        private ToDoListRepository _todoRepository = new ToDoListRepository();

        private readonly ILogger<HomeController> _logger;

        private bool IsDiscanding = false;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string sortOrder)
        {
            var order = string.Empty;

            if (string.IsNullOrEmpty(sortOrder) == true) order = "desc";
            else
            {
                order = sortOrder == "desc" ? "asc" : "desc";
            }

            ViewBag.NameSortParm = order;

            IsDiscanding = sortOrder == "desc";

            if (_todoRepository.ToDoLists.Count == 0)
            {
                var todos = _todoRepository.GetAll(IsDiscanding);

                return View(todos);
            }
            else
            {
                if (IsDiscanding == true) return View(_todoRepository.ToDoLists.OrderByDescending(o => o.TodoDateTime));
                else return View(_todoRepository.ToDoLists.OrderBy(o => o.TodoDateTime));
            }
        }

        public ActionResult Details(int id)
        {
            var findTodo = _todoRepository.FindById(id);

            return View(findTodo);
        }

        public ActionResult Create()
        {
            var maxId = DBR.ToDoMaxId();

            var todo = new ToDoListModel { Index = maxId, Name = "", Status = false, ToDoStatus = "", TodoDateTime = DateTime.Now };

            return View(todo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ToDoListModel collection)
        {
            try
            {
                var maxId = collection.Index;
                var name = collection.Name;
                var status = collection.Status;
                var isComplete = status == true ? "Completed" : "Not Copleted";

                var saved = Appendage.ToSaveUpdate(new ToDoListModel { Index = maxId, Name = name, Status = status, ToDoStatus = isComplete, TodoDateTime = collection.TodoDateTime });

                if (saved == true) return RedirectToAction(nameof(Index));
                else return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var findTodo = _todoRepository.FindById(id);

            return View(findTodo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var name = collection["Name"].ToString();
                var status = collection["Status"].ToString().ToObjBool();
                var isComplete = status == true ? "Completed" : "Not Copleted";

                var saved = Appendage.ToSaveUpdate(new ToDoListModel { Index = id, Name = name, Status = status, ToDoStatus = isComplete, TodoDateTime = DateTime.Now });

                if (saved == true) return RedirectToAction(nameof(Index));
                else return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Update(int id)
        {
            var findTodo = _todoRepository.FindById(id);

            return View(findTodo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, ToDoListModel collection)
        {
            try
            {
                //var name = collection["Name"].ToString();
                //var status = collection["Status"].ToString().ToObjBool();
                //var datetime = collection["TodoDateTime"].ToString().ToObjDateTime();
                //var isComplete = collection["ToDoStatus"].ToString();

                var saved = Appendage.ToSaveUpdate(collection);

                if (saved == true) return RedirectToAction(nameof(Index));
                else return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var findTodo = _todoRepository.FindById(id);

            return View(findTodo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ToDoListModel collection)
        {
            try
            {
                //var name = collection["Name"].ToString();
                //var status = collection["Status"].ToString().ToObjBool();
                //var isComplete = status == true ? "Completed" : "Not Copleted";

                if (collection.Index <= 0) collection = _todoRepository.FindById(id);

                var delete = Appendage.ToDelete(collection);

                if (delete == true) return RedirectToAction(nameof(Index));
                else return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Home()
        {
            ViewData["Message"] = "Your web page.";

            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["Message"] = "Your privacy page.";

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
