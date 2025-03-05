using System.ComponentModel.DataAnnotations;

namespace CloudFluffInc.Models;

public class NewsletterViewModel
{
    public Subscriber NewSubscriber { get; set; } = new();
    public List<Subscriber> Subscribers { get; set; } = [];
} 