using CloudFluffInc.Models;
using System.Collections.Concurrent;

namespace CloudFluffInc.Repositories;

public class InMemorySubscriberRepository : ISubscriberRepository
{
    private readonly ConcurrentDictionary<string, Subscriber> _subscribers = new(StringComparer.OrdinalIgnoreCase);

    public Task<IEnumerable<Subscriber>> GetAllSubscribersAsync()
    {
        return Task.FromResult(_subscribers.Values.AsEnumerable());
    }

    public Task<Subscriber?> GetSubscriberByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Task.FromResult<Subscriber?>(null);
        }

        _subscribers.TryGetValue(email, out var subscriber);
        return Task.FromResult(subscriber);
    }

    public Task<bool> AddSubscriberAsync(Subscriber subscriber)
    {
        if (subscriber == null || string.IsNullOrWhiteSpace(subscriber.Email))
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(_subscribers.TryAdd(subscriber.Email, subscriber));
    }

    public Task<bool> UpdateSubscriberAsync(Subscriber subscriber)
    {
        if (subscriber == null || string.IsNullOrWhiteSpace(subscriber.Email))
        {
            return Task.FromResult(false);
        }

        if (!_subscribers.ContainsKey(subscriber.Email))
        {
            return Task.FromResult(false);
        }

        _subscribers.AddOrUpdate(subscriber.Email, subscriber, (key, oldValue) => subscriber);

        return Task.FromResult(true);
    }

    public Task<bool> DeleteSubscriberAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(_subscribers.TryRemove(email, out _));
    }
    
    public Task<bool> ExistsAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Task.FromResult(false);
        }
        return Task.FromResult(_subscribers.ContainsKey(email));
    }
}