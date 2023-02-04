using System.Text;
using Newtonsoft.Json;
using xekoshop.Interfaces;

namespace xekoshop.Services;


public class DiscordWebhook : IDiscordWebhook 
{
    public DiscordWebhook(string webhookUrl, string webhookUser, string webhookAvatar)
    {
        WebhookUrl = webhookUrl;
        WebhookUser = webhookUser;
        WebhookAvatar = webhookAvatar;
        Client = new HttpClient();
    }

    private HttpClient Client { get; set; }
    public string WebhookUrl { get; set; }
    public string WebhookUser { get; set; }
    public string WebhookAvatar { get; set; }
    
    public async Task SendWebhook(string message)
    {
        await Client.PostAsync(WebhookUrl, new StringContent(JsonConvert.SerializeObject(new
        {
            username = WebhookUser,
            avatar_url = WebhookAvatar,
            content = message
        }), Encoding.UTF8, "application/json"));
    }
    
    
}