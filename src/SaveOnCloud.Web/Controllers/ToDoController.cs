using SaveOnCloud.Core;
using SaveOnCloud.Core.Entities;
using SaveOnCloud.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SaveOnCloud.Web.Models;

namespace SaveOnCloud.Web.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IRepository _repository;

        public ToDoController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var items = _repository.List<ToDoItem>()
                            .Select(ToDoItemModel.FromToDoItem);
            return View(items);
        }

        public IActionResult Populate()
        {
            int recordsAdded = DatabasePopulator.PopulateDatabase(_repository);
            return Ok(recordsAdded);
        }
    }
}
