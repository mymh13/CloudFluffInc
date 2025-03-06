using CloudFluffInc.Models;

namespace CloudFluffInc.Repositories;

public interface ISubscriberRepository
{
    Task<IEnumerable<Subscriber>> GetAllSubscribersAsync();
    Task<Subscriber?> GetSubscriberByEmailAsync(string email);
    Task<bool> AddSubscriberAsync(Subscriber subscriber);
    Task<bool> UpdateSubscriberAsync(Subscriber subscriber);
    Task<bool> DeleteSubscriberAsync(string email);
    Task<bool> ExistsAsync(string email);
}