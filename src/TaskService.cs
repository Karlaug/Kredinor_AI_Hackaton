using Microsoft.EntityFrameworkCore;
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

    public async Task<List<TaskItem>> GetAllTasksAsync()
    {
        return await _db.Tasks.ToListAsync();
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        return await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskItem> CreateTaskAsync(TaskItem task)
    {
        task.CreatedAt = DateTime.Now;
        task.Status = "Open";
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem?> UpdateTaskAsync(int id, TaskItem updated)
    {
        var existing = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (existing == null) return null;

        existing.Title = updated.Title;
        existing.Description = updated.Description;
        existing.Priority = updated.Priority;
        existing.DueDate = updated.DueDate;
        // NOTE: we deliberately do not update Status here, use the status endpoint
        await _db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return false;

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<TaskItem>> SearchTasksAsync(string query)
    {
        // Loads everything into memory first - known issue
        var all = await _db.Tasks.ToListAsync();
        return all.Where(t =>
            (t.Title != null && t.Title.ToLower().Contains(query.ToLower())) ||
            (t.Description != null && t.Description.ToLower().Contains(query.ToLower()))
        ).ToList();
    }

    public async Task<TaskItem?> AssignTaskAsync(int taskId, int userId)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return null;

        task.AssignedUserId = userId;
        task.AssignedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        // TODO: send notification email
        return task;
    }

    public async Task<List<TaskItem>> GetTasksByUserAsync(int userId)
    {
        return await _db.Tasks.Where(t => t.AssignedUserId == userId).ToListAsync();
    }

    // Duplicate-ish helper, used in two places
    public async Task<int> CountOpenTasksAsync(int userId)
    {
        var tasks = await _db.Tasks.Where(t => t.AssignedUserId == userId).ToListAsync();
        return tasks.Count(t => t.Status == "Open");
    }

    public async Task<int> CountActiveTasksAsync(int userId)
    {
        var all = await _db.Tasks.ToListAsync();
        var mine = all.Where(t => t.AssignedUserId == userId);
        return mine.Where(t => t.Status != "Closed" && t.Status != "Done").Count();
    }
}
