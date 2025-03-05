using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using CloudFluffInc.Models;

namespace CloudFluffInc.Controllers;

public class NewsletterController : Controller
{
    private static List<Subscriber> _subscribers = [];

    [HttpGet]
    public IActionResult Subscribe()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Subscribe(Subscriber subscriber)
    {
        if (!ModelState.IsValid)
        {
            return View(subscriber);
        }

        if (_subscribers.Any(s => s.Email == subscriber.Email))
        {
            ModelState.AddModelError("Email", "You are already subscribed to our newsletter.");
            return View(subscriber);
        }

        _subscribers.Add(subscriber);

        Console.WriteLine($"New subscription - Name: {subscriber.Name} Email: {subscriber.Email}");

        TempData["SuccessMessage"] = $"Thank you for subscribing, {subscriber.Name}! You will receive our newsletter at {subscriber.Email}";

        return RedirectToAction(nameof(Subscribe));
    }

    [HttpGet]
    public IActionResult Subscribers()
    {
        return View(_subscribers);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Unsubscribe(string email)
    {
        var subscriber = _subscribers.FirstOrDefault(s => s.Email == email);
        if (subscriber != null)
        {
            _subscribers.Remove(subscriber);
            TempData["SuccessMessage"] = $"Successfully removed {email} from the newsletter list.";
        }
        return RedirectToAction(nameof(Subscribers));
    }
}