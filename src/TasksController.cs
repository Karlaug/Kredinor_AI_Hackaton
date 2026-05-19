using Microsoft.AspNetCore.Mvc;
using TaskApi.Models;
using TaskApi.Services;

namespace TaskApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskService _taskService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(TaskService taskService, ILogger<TasksController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var tasks = _taskService.GetAllTasks();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var task = _taskService.GetTaskById(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public IActionResult Create([FromBody] TaskItem task)
    {
        // TODO: validation
        var created = _taskService.CreateTask(task);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] TaskItem task)
    {
        var updated = _taskService.UpdateTask(id, task);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _taskService.DeleteTask(id);
        if (!deleted)
        {
            _logger.LogWarning("Delete failed: task {TaskId} not found", id);
            return NotFound();
        }

        _logger.LogInformation("Task {TaskId} deleted successfully", id);
        return NoContent();
    }

    [HttpGet("search")]
    public IActionResult Search(string query)
    {
        // FIXME: this is slow on large datasets
        var results = _taskService.SearchTasks(query);
        return Ok(results);
    }

    [HttpPost("{id}/assign")]
    public IActionResult Assign(int id, [FromBody] AssignRequest request)
    {
        var result = _taskService.AssignTask(id, request.UserId);
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public IActionResult GetByUser(int userId)
    {
        var tasks = _taskService.GetTasksByUser(userId);
        return Ok(tasks);
    }

    [HttpPost("bulk-import")]
    public IActionResult BulkImport([FromBody] List<TaskItem> tasks)
    {
        foreach (var t in tasks)
        {
            _taskService.CreateTask(t);
        }
        return Ok(new { count = tasks.Count });
    }
}
