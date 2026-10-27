using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("Tasks")]
    public class TasksController : Controller
    {
        
         private readonly ITaskService _taskService;
         private readonly FileLogger? _logger;

       public TasksController(ITaskService taskService, FileLogger logger)
        {
            _taskService = taskService;
            _logger = logger;

            _taskService.TaskAdd += task => _logger.Log($"Задача добавлена: {task.Title}");
        }
     
        [HttpGet]
        public IActionResult GetAllTasks()
        {
            return Ok(_taskService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetIdTasks(int id)
        {
              var  task = _taskService.GetTasksOfId(id);
              return Ok(task);
        }

        [HttpPost]
        public IActionResult CreateTasks(TaskItem task)
        {
            _taskService.Create(task);
            return CreatedAtAction(nameof(GetIdTasks), new { Id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public IActionResult RefreshTasks(int id, TaskItem refreshTask) 
        {
            var findId = _taskService.GetTasksOfId(id);
            if (findId == null)
            {
                return NotFound($"Книги с таким ID: {id}, не существует!");
            }
            
                _taskService.Update(id, refreshTask);
                return Ok("Изменения применены!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var findId = _taskService.GetTasksOfId(id);
            if (findId == null)
            {
                return NotFound($"Задачи с таким ID: {id}, не существует!");
            }
            _taskService.Delete(id);
            return NoContent();
        }

        [HttpGet("searchOfTitle/{title}")]
        public IActionResult GetTaskOfTitle(string title) 
        {
            TaskItem? taskTitle = _taskService.GetTitle(title);
            if (taskTitle == null)
            {
               return NotFound($"Нет задачи с таким названием: {title}");
                
            }
            return Ok(taskTitle);
        }

        [HttpGet("searchOfIsCompleted/{isCompleted}")]
        public IActionResult GetTasksOfIsComplected(bool isCompleted)
        {
            var taskIsCompleted = _taskService.GetIsCompleted(isCompleted);

            return Ok(taskIsCompleted);
        }
    }
}
