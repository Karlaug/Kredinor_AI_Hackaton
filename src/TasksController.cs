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
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaskItem task)
    {
        // TODO: validation
        var created = await _taskService.CreateTaskAsync(task);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaskItem task)
    {
        var updated = await _taskService.UpdateTaskAsync(id, task);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _taskService.DeleteTaskAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string query)
    {
        // FIXME: this is slow on large datasets
        var results = await _taskService.SearchTasksAsync(query);
        return Ok(results);
    }

    [HttpPost("{id}/assign")]
    public async Task<IActionResult> Assign(int id, [FromBody] AssignRequest request)
    {
        var result = await _taskService.AssignTaskAsync(id, request.UserId);
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var tasks = await _taskService.GetTasksByUserAsync(userId);
        return Ok(tasks);
    }

    [HttpPost("bulk-import")]
    public async Task<IActionResult> BulkImport([FromBody] List<TaskItem> tasks)
    {
        foreach (var t in tasks)
        {
            await _taskService.CreateTaskAsync(t);
        }
        return Ok(new { count = tasks.Count });
    }
}
