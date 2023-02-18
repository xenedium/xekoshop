using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using xekoshop.Interfaces;
using xekoshop.Models;

namespace xekoshop.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDiscordWebhook _discordWebhook;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, IDiscordWebhook discordWebhook, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _discordWebhook = discordWebhook;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return RedirectToAction(controllerName: "Product", actionName: "Index");
    }
    
    public async Task<IActionResult> RequestAdmin()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();
        await _discordWebhook.SendWebhook($"@everyone\n```json\n{JsonConvert.SerializeObject(new
        {
            RequestType = "Admin Request",
            user.Id,
            user.UserName,
            user.Email,
            ClientIp = Request.Headers.TryGetValue("X-Forwarded-For", out var xForwardedFor)
                ? xForwardedFor.ToString()
                : Request.HttpContext.Connection.RemoteIpAddress?.ToString()
                  ?? "Unknown",
            UserAgent = Request.Headers.UserAgent.ToString()
        }, Formatting.Indented)}```");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
