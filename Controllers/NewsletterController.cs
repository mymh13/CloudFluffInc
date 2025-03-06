using Microsoft.AspNetCore.Mvc;
using CloudFluffInc.Models;
using CloudFluffInc.Services;

namespace CloudFluffInc.Controllers;

public class NewsletterController : Controller
{
    private readonly INewsletterService _newsletterService;

    public NewsletterController(INewsletterService newsletterService)
    {
        _newsletterService = newsletterService;
    }

    [HttpGet]
    public IActionResult Subscribe()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe(Subscriber subscriber)
    {
        if (!ModelState.IsValid)
        {
            return View(subscriber);
        }

        var result = await _newsletterService.SignUpForNewsletterAsync(subscriber);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError("Email", result.Message);
            return View(subscriber);
        }

        Console.WriteLine($"New subscription - Name: {subscriber.Name} Email: {subscriber.Email}");

        TempData["SuccessMessage"] = result.Message;

        return RedirectToAction(nameof(Subscribe));
    }

    [HttpGet]
    public async Task<IActionResult> Subscribers()
    {
        var subscribers = await _newsletterService.GetActiveSubscribersAsync();
        return View(subscribers);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unsubscribe(string email)
    {
        var result = await _newsletterService.OptOutFromNewsletterAsync(email);
        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = result.Message;
        }

        return RedirectToAction(nameof(Subscribers));
    }
}