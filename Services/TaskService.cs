using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{

  
    public interface ITaskService
    {
        IQueryable<TaskItem> GetAll();
        TaskItem? GetTasksOfId(int id);
        TaskItem? Create(TaskItem createTaskItem);
        void Update(int id, TaskItem updateTaskItem);
        void Delete(int id);
        TaskItem? GetTitle(string titleTask);
        List<TaskItem> GetIsCompleted(bool completedTask);

        event Action<TaskItem>? TaskAdd;
    }
    public class TaskService:ITaskService
    {
        private readonly AppDbContext _context;

        public event Action<TaskItem>? TaskAdd;
        
        public TaskService(AppDbContext context)
        {
            _context = context;
        }
     
        public IQueryable<TaskItem> GetAll()
        {
            return _context.TasksItems;
        }

        public TaskItem? GetTasksOfId(int id)
        {
            var task = new TaskItem();
            try
            {
                task = _context.TasksItems.FirstOrDefault(x => x.Id == id);
              
            }
            catch (IndexOutOfRangeException ex)
            {
                if (task == null)
                {
                    Console.WriteLine(ex.Message);
                  
                }
            }
            return task;
        }

        public TaskItem? Create(TaskItem taskItem) 
        {
            var createTask = taskItem; 
            try
            {
                _context.TasksItems.Add(createTask);
                _context.SaveChanges();
                TaskAdd?.Invoke(createTask);
             
            }
            catch(InvalidOperationException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
              
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            return createTask;
        }

        public void Update(int id, TaskItem updateTaskItem)
        {
                 var updateTask = new TaskItem();
           
                updateTask = _context.TasksItems.FirstOrDefault(x => x.Id == id);

                  if (updateTask == null)
                  {
                     return;
                  }
               
                updateTask.Title = updateTaskItem.Title;
                updateTask.Title = updateTaskItem.Title; 
                updateTask.Description = updateTaskItem.Description;
                updateTask.IsCompleted = updateTaskItem.IsCompleted;
                updateTask.CreatedAt = updateTaskItem.CreatedAt;
                _context.SaveChanges();
                return;

        }
           
        public void Delete(int id)
        {
            var deleteTask = _context.TasksItems.FirstOrDefault(x => x.Id == id);
            if (deleteTask == null)
            {
                return;
            }

            _context.TasksItems.Remove(deleteTask);
            _context.SaveChanges();
            return;
        }

       public TaskItem? GetTitle(string titleTask)
        {
            TaskItem? taskTitle = null;
            foreach (var taskItem in _context.TasksItems)
            {
                if (taskItem.Title == null)
                {
                    return taskTitle;
                }
                if (taskItem.Title.Contains(titleTask, StringComparison.OrdinalIgnoreCase))
                {
                    taskTitle = taskItem;

                }
            }
            return taskTitle;
        }

        public List<TaskItem> GetIsCompleted(bool completedTask)
        {
            var taskIsCompleted = _context.TasksItems.Where(x => x.IsCompleted == completedTask).ToList();

            return taskIsCompleted;
        }

    }
}
