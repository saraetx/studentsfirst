using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Infrastructure;

namespace StudentsFirst.Api.Monolithic.Tests
{
    public class InMemoryApplicationDbContextProvider : IDbContextProvider<StudentsFirstContext>
    {
        public InMemoryApplicationDbContextProvider()
        {
            _dbConnection = new SqliteConnection("Filename=:memory:");
            _dbConnection.Open();

            _dbContextOptions = new DbContextOptionsBuilder()
                .UseSqlite(_dbConnection)
                .Options;

            using (StudentsFirstContext context = Context) { context.Database.Migrate(); }
        }

        public void Dispose()
        {
            _dbConnection.Close();
        }

        private readonly SqliteConnection _dbConnection;
        private readonly DbContextOptions _dbContextOptions;

        public StudentsFirstContext Context => new StudentsFirstContext(_dbContextOptions);
    }
}
