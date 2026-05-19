using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.Models;
using TaskApi.Services;

namespace TaskApi.Tests;

public class DeleteTaskTests : IDisposable
{
    private readonly AppDbContext _db;
    private readonly TaskService _service;

    public DeleteTaskTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        _db = new AppDbContext(options);
        _db.Database.OpenConnection();
        _db.Database.EnsureCreated();
        _service = new TaskService(_db);
    }

    public void Dispose()
    {
        _db.Database.CloseConnection();
        _db.Dispose();
    }

    [Fact]
    public void DeleteTask_ExistingTask_ReturnsTrue()
    {
        var task = new TaskItem { Title = "Test task" };
        _db.Tasks.Add(task);
        _db.SaveChanges();

        var result = _service.DeleteTask(task.Id);

        Assert.True(result);
        Assert.Null(_db.Tasks.FirstOrDefault(t => t.Id == task.Id));
    }

    [Fact]
    public void DeleteTask_NonExistentId_ReturnsFalse()
    {
        var result = _service.DeleteTask(999999);

        Assert.False(result);
    }

    [Fact]
    public void DeleteTask_RepeatedDelete_ReturnsFalseSecondTime()
    {
        var task = new TaskItem { Title = "Test task" };
        _db.Tasks.Add(task);
        _db.SaveChanges();
        var id = task.Id;

        _service.DeleteTask(id);
        var result = _service.DeleteTask(id);

        Assert.False(result);
    }
}
