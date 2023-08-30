using Azure;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.ViewModels.Task;
using ToDoList.Service.Interfaces;

namespace ToDoList.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> EndDay()
        {
            var response = await _taskService.EndDay();

            if (response.StatusCode == Domain.Enum.StatusCode.OK) return Ok(new { description = response.Description });
            return BadRequest(new { description = response.Description });
        }
        public async Task<IActionResult> GetCompletedTasks()
        {
            var result = await _taskService.GetCompletedTasks();
            return Json(new { data = result.Data });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskViewModel viewModel)
        {
            var response = await _taskService.Create(viewModel);

            if (response.StatusCode == Domain.Enum.StatusCode.OK) return Ok(new { description = response.Description });
            return BadRequest(new { description = response.Description });
        }

       

        [HttpPost]
        public async Task<IActionResult> EndTask(int id)
        {
            var response = await _taskService.EndTask(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK) return Ok(new { description = response.Description });
            return BadRequest(new { description = response.Description });
        }

        public async Task<IActionResult> TaskHandler()
        {
            var response = await _taskService.GetTasks();
            return Json(new { data = response.Data });
        }
    }
}