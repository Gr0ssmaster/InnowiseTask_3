using Dapper;

namespace Task_3
{
    public interface ITaskRepository
    {
        IEnumerable<Tasks> GetAllTasks();
        Tasks GetTaskById(int id);
        int AddTask(Tasks task);
        bool DeleteTask(int id);
        bool UpdateTask(int id, bool isCompleted);

    }

    public class TaskRepository : ITaskRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TaskRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public IEnumerable<Tasks> GetAllTasks()
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            string sql = "SELECT * FROM Tasks";
            return connection.Query<Tasks>(sql);
        }
        public Tasks GetTaskById(int id) { 
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            string sql = "SELECT * FROM Tasks WHERE Id = @Id";
            return connection.QueryFirstOrDefault<Tasks>(sql, new { Id = id });  
        }
        public int AddTask(Tasks task)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            string sql = "INSERT INTO Tasks(Title, Description, IsCompleted, CreatedAt) " +
                "VALUES(@Title, @Description, @IsCompleted, @CreatedAt)";
            return connection.ExecuteScalar<int>(sql, task);
        }

        public bool DeleteTask(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            string sql = "DELETE FROM Tasks WHERE Id = @Id";
            int rows = connection.Execute(sql, new { id });
            return rows > 0;
        }

        public bool UpdateTask(int id, bool isCompleted)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            string sql = "UPDATE Tasks SET IsCompleted = @IsCompleted WHERE Id = @Id";
            int rows = connection.Execute(sql, new { Id = id, IsCompleted = isCompleted });
            return rows > 0;
        }
    }
}