using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NerdToDo.Models
{
    public interface ITodo
    {
        string Title { get; set; }
        string Description { get; set; }
        DateTime DueDate { get; set; }
        Priority Priority { get; set; }
    }

    public class TodoBase(string title, string description, DateTime duedate, Priority priority):ITodo
    {
        public string Title { get; set; } = title;
        public string Description { get; set; } = description;
        public DateTime DueDate { get; set; } = duedate;
        public Priority Priority { get; set; } = priority;
    }

    public class Todo(string title, string description, DateTime duedate, Priority priority)
        :TodoBase(title, description, duedate, priority)
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public static Todo MapToTodo<T>(T from) where T:ITodo =>
            new(from.Title, from.Description, from.DueDate, from.Priority);
        
    }

    public class CreateTodoDto(string title, string description, DateTime duedate, Priority priority)
        :TodoBase(title, description, duedate, priority) { }

    public class UpdateTodoDto(string title, string description, DateTime duedate, Priority priority)
        :TodoBase(title, description, duedate, priority) { }

    public enum Priority
    {
        Low,
        Medium,
        High
    }
}
