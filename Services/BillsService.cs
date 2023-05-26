namespace BillsClientApi.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BillsClientApi.Models;
public class BillsService
    {
    private readonly IMongoCollection<Bills> _billsCollection;

    public BillsService(
        IOptions<BillsStoreDatabaseSettings> billsStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            billsStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            billsStoreDatabaseSettings.Value.DatabaseName);

        _billsCollection = mongoDatabase.GetCollection<Bills>(
            billsStoreDatabaseSettings.Value.BillsCollectionName);
    }

    public async Task<List<Bills>> GetAsync() =>
        await _billsCollection.Find(x => true).ToListAsync();

    public async Task<Bills?> GetAsync(string id) =>
        await _billsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Bills newBills) =>
        await _billsCollection.InsertOneAsync(newBills);

    public async Task UpdateAsync(string id, Bills updatedBills) =>
        await _billsCollection.ReplaceOneAsync(x => x.Id == id, updatedBills);

    public async Task RemoveAsync(string id) =>
        await _billsCollection.DeleteOneAsync(x => x.Id == id);
}

