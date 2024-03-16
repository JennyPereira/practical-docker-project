using task_api.Interfaces;
using task_api.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace task_api.Repositories;

public class WorkRepository : IWorkRepository
{

    private readonly IMongoCollection<Work> _workCollection;
    private readonly IMongoDatabase _database;

    public WorkRepository(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        _database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _workCollection = _database.GetCollection<Work>(mongoDBSettings.Value.CollectionName);
    }

    public async Task<List<Work>> GetAsync()
    {
        //Console.WriteLine("aqui si entre");
        return await _workCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<Work> GetWorkByIdAsync(string id)
    {
        FilterDefinition<Work> filter = Builders<Work>.Filter.Eq("Id", id);
        Console.WriteLine("PARA ENCONTRAR");
        return await _workCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Work work)
    {
        Console.WriteLine(work.user);
        await _workCollection.InsertOneAsync(work);
    }

    public async Task DeleteAsync(string id)
    {
        FilterDefinition<Work> filter = Builders<Work>.Filter.Eq("Id", id);
        await _workCollection.DeleteOneAsync(filter);
        return;
    }

    public async Task<Work> ComprobateUserByTaskIdAsync(string userId, string workId)
    {
        /*FilterDefinition<Work> filter = Builders<Work>.Filter.Eq("user", id);*/
        FilterDefinition<Work> filter = Builders<Work>.Filter.And(
            Builders<Work>.Filter.Eq("user", userId),
            Builders<Work>.Filter.Eq("Id", workId)
        );

        return await _workCollection.Find(filter).FirstOrDefaultAsync();
    }
}