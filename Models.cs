namespace TaskApi.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }       // "Open", "InProgress", "Done", "Closed"
    public string? Priority { get; set; }     // "Low", "Medium", "High", "Critical"
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? AssignedUserId { get; set; }
    public DateTime? AssignedAt { get; set; }
    public int? ProjectId { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? Role { get; set; }
    public bool IsActive { get; set; }
}

public class Project
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AssignRequest
{
    public int UserId { get; set; }
}
