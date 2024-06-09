using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NerdToDo.Models;

namespace NerdToDo.Services
{
    public class TodosService
    {
        private readonly IMongoCollection<Todo> _todosCollection;

        public TodosService(
            IOptions<NerdTodoDatabaseSettings> nerdTodoDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                nerdTodoDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                nerdTodoDatabaseSettings.Value.DatabaseName);

            _todosCollection = mongoDatabase.GetCollection<Todo>(
                nerdTodoDatabaseSettings.Value.TodosCollectionName);
        }

        public async Task<List<Todo>> GetAsync() =>
            await _todosCollection.Find(_ => true).ToListAsync();

        public async Task<Todo?> GetAsync(string id) =>
            await _todosCollection.Find(todo => todo.Id == id).FirstOrDefaultAsync();

        public async Task<Todo?> CreateAsync(CreateTodoDto newTodo)
        {
            var todoing = Todo.MapToTodo(newTodo);
            await _todosCollection.InsertOneAsync(todoing);
            return todoing;
        }

        public async Task<Todo?> UpdateAsync(string id, UpdateTodoDto updatedTodo)
        {
            var todoing = Todo.MapToTodo(updatedTodo);
            await _todosCollection.ReplaceOneAsync(todo => todo.Id == id, todoing);
            return todoing;
        }

        public async Task RemoveAsync(string id) =>
            await _todosCollection.DeleteOneAsync(todo => todo.Id == id);
    }
}
