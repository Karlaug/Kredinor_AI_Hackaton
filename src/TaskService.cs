using TaskApi.Models;
using TaskApi.Data;

namespace TaskApi.Services;

public class TaskService
{
    private readonly AppDbContext _db;

    public TaskService(AppDbContext db)
    {
        _db = db;
    }

    public List<TaskItem> GetAllTasks()
    {
        return _db.Tasks.ToList();
    }

    public TaskItem? GetTaskById(int id)
    {
        return _db.Tasks.FirstOrDefault(t => t.Id == id);
    }

    public TaskItem CreateTask(TaskItem task)
    {
        task.CreatedAt = DateTime.Now;
        task.Status = "Open";
        _db.Tasks.Add(task);
        _db.SaveChanges();
        return task;
    }

    public TaskItem? UpdateTask(int id, TaskItem updated)
    {
        var existing = _db.Tasks.FirstOrDefault(t => t.Id == id);
        if (existing == null) return null;

        existing.Title = updated.Title;
        existing.Description = updated.Description;
        existing.Priority = updated.Priority;
        existing.DueDate = updated.DueDate;
        // NOTE: we deliberately do not update Status here, use the status endpoint
        _db.SaveChanges();
        return existing;
    }

    public bool DeleteTask(int id)
    {
        var task = _db.Tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return false;

        _db.Tasks.Remove(task);
        _db.SaveChanges();
        return true;
    }

    public List<TaskItem> SearchTasks(string query)
    {
        // Loads everything into memory first - known issue
        var all = _db.Tasks.ToList();
        return all.Where(t =>
            (t.Title != null && t.Title.ToLower().Contains(query.ToLower())) ||
            (t.Description != null && t.Description.ToLower().Contains(query.ToLower()))
        ).ToList();
    }

    public TaskItem? AssignTask(int taskId, int userId)
    {
        var task = _db.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null) return null;

        task.AssignedUserId = userId;
        task.AssignedAt = DateTime.Now;
        _db.SaveChanges();

        // TODO: send notification email
        return task;
    }

    public List<TaskItem> GetTasksByUser(int userId)
    {
        return _db.Tasks.Where(t => t.AssignedUserId == userId).ToList();
    }

    // Duplicate-ish helper, used in two places
    public int CountOpenTasks(int userId)
    {
        var tasks = _db.Tasks.Where(t => t.AssignedUserId == userId).ToList();
        return tasks.Count(t => t.Status == "Open");
    }

    public int CountActiveTasks(int userId)
    {
        var all = _db.Tasks.ToList();
        var mine = all.Where(t => t.AssignedUserId == userId);
        return mine.Where(t => t.Status != "Closed" && t.Status != "Done").Count();
    }
}
