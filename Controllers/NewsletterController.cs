using Microsoft.AspNetCore.Mvc;
using CloudFluffInc.Models;

namespace CloudFluffInc.Controllers;

public class NewsletterController : Controller
{
    [HttpGet]
    public IActionResult Subscribe()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Subscribe(Subscriber subscriber)
    {
        // TODO: Add subscription logic here

        Console.WriteLine($"New subscription - Name: {subscriber.Name} Email: {subscriber.Email}");

        ViewBag.Message = $"Thank you {subscriber.Name} for subscribing to our newsletter!";
        return View("Subscribe");
    }
}