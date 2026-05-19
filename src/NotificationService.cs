using System.Net.Mail;
using TaskApi.Models;

namespace TaskApi.Services;

public class NotificationService
{
    private readonly ILogger<NotificationService> _logger;

    // FIXME: hardcoded SMTP config, should be in appsettings
    private const string SmtpHost = "smtp.internal.local";
    private const int SmtpPort = 25;
    private const string FromAddress = "noreply@example.com";

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger;
    }

    public void SendTaskAssignedEmail(User user, TaskItem task)
    {
        try
        {
            var client = new SmtpClient(SmtpHost, SmtpPort);
            var message = new MailMessage(FromAddress, user.Email!)
            {
                Subject = "You have been assigned a new task: " + task.Title,
                Body = $"Hi {user.FullName},\n\nYou were assigned task '{task.Title}'.\n\nDue: {task.DueDate}\n\nDetails: {task.Description}"
            };
            client.Send(message);
        }
        catch (Exception ex)
        {
            // Silently swallow - we don't want to break the API
            _logger.LogError(ex, "Failed to send email");
        }
    }

    public void SendDueDateReminder(User user, TaskItem task)
    {
        try
        {
            var client = new SmtpClient(SmtpHost, SmtpPort);
            var message = new MailMessage(FromAddress, user.Email!)
            {
                Subject = "Reminder: task due soon",
                Body = $"Hi {user.FullName},\n\nYour task '{task.Title}' is due {task.DueDate}."
            };
            client.Send(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send reminder");
        }
    }

    // Called from a background job - runs every hour
    public void SendBulkReminders(List<(User user, TaskItem task)> items)
    {
        foreach (var item in items)
        {
            SendDueDateReminder(item.user, item.task);
        }
    }
}
