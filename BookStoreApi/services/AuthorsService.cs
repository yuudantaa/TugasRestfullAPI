using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Services;

public class AuthorsService
{
    private readonly IMongoCollection<Authors> _booksCollection;

    public AuthorsService(
        IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
        bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Authors>(
            bookStoreDatabaseSettings.Value.BooksCollectionName);
    }

    public async Task<List<Authors>> GetAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();

    public async Task<Authors?> GetAsync(string id) =>
        await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Authors newAuthors) =>
        await _booksCollection.InsertOneAsync(newAuthors);

    public async Task UpdateAsync(string id, Authors updateAuthors) =>
        await _booksCollection.ReplaceOneAsync(x => x.Id == id, updateAuthors);

    public async Task RemoveAsync(string id) =>
        await _booksCollection.DeleteOneAsync(x => x.Id == id);
}