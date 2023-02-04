namespace xekoshop.Interfaces;

public interface IDiscordWebhook
{
    public string WebhookUrl { get; set; }
    public string WebhookUser { get; set; }
    public string WebhookAvatar { get; set; }
    
    public Task SendWebhook(string message);
    
}