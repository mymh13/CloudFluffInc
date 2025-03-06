using CloudFluffInc.Models;
using CloudFluffInc.Repositories;

namespace CloudFluffInc.Services;

public class NewsletterService : INewsletterService
{
    private readonly ISubscriberRepository _subscriberRepository;

    public NewsletterService(ISubscriberRepository subscriberRepository)
    {
        _subscriberRepository = subscriberRepository;
    }

    public async Task<OperationResult> SignUpForNewsletterAsync(Subscriber subscriber)
    {
        if (subscriber == null || string.IsNullOrWhiteSpace(subscriber.Email))
            {
                return OperationResult.Failure("Invalid subscriber data");
            }

            if (await _subscriberRepository.ExistsAsync(subscriber.Email))
            {
                return OperationResult.Failure("You are already subscribed to the newsletter");
            }

            var success = await _subscriberRepository.AddSubscriberAsync(subscriber);
            if (!success)
            {
                return OperationResult.Failure("Failed to add subscriber");
            }

            return OperationResult.Success($"Welcome to the newsletter, {subscriber.Name}!");
    }

    public async Task<OperationResult> OptOutFromNewsletterAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            {
                return OperationResult.Failure("Invalid email address");
            }

            var subscriber = await _subscriberRepository.GetSubscriberByEmailAsync(email);
            if (subscriber == null)
            {
                return OperationResult.Failure("You are not subscribed to the newsletter");
            }
            
            var success = await _subscriberRepository.DeleteSubscriberAsync(email);
            if (!success)
            {
                return OperationResult.Failure("Failed to remove subscriber");
            }
            
            return OperationResult.Success("You have been removed from the newsletter");
    }

    public async Task<IEnumerable<Subscriber>> GetActiveSubscribersAsync()
    {
        var subscribers = await _subscriberRepository.GetAllSubscribersAsync();
        return subscribers.ToList();
    }
}
