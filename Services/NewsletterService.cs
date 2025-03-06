using CloudFluffInc.Models;

namespace CloudFluffInc.Services;

public class NewsletterService : INewsletterService
{
    private static readonly List<Subscriber> _subscribers = [];

    public async Task<OperationResult> SignUpForNewsletterAsync(Subscriber subscriber)
    {
        return await Task.Run(() =>
        {
            if (subscriber == null || string.IsNullOrWhiteSpace(subscriber.Email))
            {
                return OperationResult.Failure("Invalid subscriber data");
            }

            if (IsAlreadySubscribed(subscriber.Email))
            {
                return OperationResult.Failure("You are already subscribed to the newsletter");
            }

            _subscribers.Add(subscriber);
            
            return OperationResult.Success($"Welcome to the newsletter, {subscriber.Name}!");
        });
    }

    public async Task<OperationResult> OptOutFromNewsletterAsync(string email)
    {
        return await Task.Run(() =>
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return OperationResult.Failure("Invalid email address");
            }

            var subscriber = FindSubscriberByEmail(email);
            if (subscriber == null)
            {
                return OperationResult.Failure("You are not subscribed to the newsletter");
            }
            
            _subscribers.Remove(subscriber);
            
            return OperationResult.Success("You have been removed from the newsletter");
        });
    }

    public async Task<IEnumerable<Subscriber>> GetActiveSubscribersAsync()
    {
        return await Task.Run(() => _subscribers.ToList());
    }

    private static bool IsAlreadySubscribed(string email)
    {
        return _subscribers.Any(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    private static Subscriber? FindSubscriberByEmail(string email)
    {
        return _subscribers.FirstOrDefault(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }
}
