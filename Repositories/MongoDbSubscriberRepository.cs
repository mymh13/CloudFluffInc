using CloudFluffInc.Models;
using MongoDB.Driver;

namespace CloudFluffInc.Repositories;

public class MongoDbSubscriberRepository : ISubscriberRepository
{
    private readonly IMongoCollection<Subscriber> _subscribers;

    public MongoDbSubscriberRepository(IMongoCollection<Subscriber> subscribers)
    {
        _subscribers = subscribers;
    }

    public async Task<IEnumerable<Subscriber>> GetAllSubscribersAsync()
    {
        return await _subscribers.Find(_ => true).ToListAsync();
    }
    
    public async Task<Subscriber?> GetSubscriberByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        return await _subscribers.Find(s => s.Email == email).FirstOrDefaultAsync();
    }
    
    public async Task<bool> AddSubscriberAsync(Subscriber subscriber)
    {
        if (subscriber == null || string.IsNullOrWhiteSpace(subscriber.Email))
        {
            return false;
        }

        var existingSubscriber = await GetSubscriberByEmailAsync(subscriber.Email);
        if (existingSubscriber != null)
        {
            return false;
        }

        try
        {
            await _subscribers.InsertOneAsync(subscriber);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<bool> UpdateSubscriberAsync(Subscriber subscriber)
    {
        if (subscriber == null || string.IsNullOrWhiteSpace(subscriber.Email))
        {
            return false;
        }

        try
        {
            var updateResult = await _subscribers.ReplaceOneAsync(s => s.Email == subscriber.Email, subscriber, new ReplaceOptions { IsUpsert = true });
            return updateResult.ModifiedCount > 0;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<bool> DeleteSubscriberAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var deleteResult = await _subscribers.DeleteOneAsync(s => s.Email == email);
            return deleteResult.DeletedCount > 0;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<bool> ExistsAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        return await _subscribers.CountDocumentsAsync(s => s.Email == email) > 0;
    }
}

