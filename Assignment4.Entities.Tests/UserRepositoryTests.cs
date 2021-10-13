using Xunit;
using Assignment4.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Assignment4.Entities.Tests
{
    public class UserRepositoryTests
    {

        private readonly KanbanContext _context;
        private readonly UserRepository _repo;
        public UserRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();

            _context = context;
            _repo = new UserRepository(_context);

        }

        [Fact]
        public void CreatingUser_ValidatingThatAttributesAreSet()
        {
            var userCreateDTO = new UserCreateDTO
            {
                Name = "John Doe",
                Email = "john@doe.com",
            };

            var createResponse = _repo.Create(userCreateDTO);
            var expected = new UserDTO(createResponse.UserId, "John Doe", "john@doe.com");
            var actual = _repo.Read(createResponse.UserId);

            Assert.Equal(Response.Created, createResponse.Response);
            _repo.Delete(createResponse.UserId, true);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Email, actual.Email);
        }

        [Fact]
        public void CountNumberOfUser_AddOne_ValidateIncremention()
        {
            var userDTO = new UserCreateDTO
            {
                Name = "Jenny Doe",
                Email = "jenny@doe.com",
            };
            int a = _repo.ReadAll().Count;
            var user = _repo.Create(userDTO);
            int b = _repo.ReadAll().Count;
            _repo.Delete(user.UserId);
            int c = _repo.ReadAll().Count;
            Assert.Equal(a, b - 1);
            Assert.Equal(a, c);
        }

        [Fact]
        public void CreateUser_ValidateAttributes()
        {
            var userDTO = new UserCreateDTO
            {
                Name = "John Doe",
                Email = "john@doe.com",
            };
            var user = _repo.Create(userDTO);
            var actual = _repo.Read(user.UserId);

            var expected = new UserDTO(user.UserId, "John Doe", "john@doe.com");
            _repo.Delete(user.UserId, true);

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Email, actual.Email);
        }

        [Fact]
        public void CreateUser_UpdateUser_ValidateAttributes()
        {
            var userDTO = new UserCreateDTO
            {
                Name = "John Doe",
                Email = "john@doe.com",
            };
            (Response oldTaskResponse, int id) = _repo.Create(userDTO);
            Assert.Equal(Response.Created, oldTaskResponse);

            var newUserDTO = new UserUpdateDTO
            {
                Id = id,
                Name = "John Doe",
                Email = "john@doe.dk",
            };
            var newTaskResponse = _repo.Update(newUserDTO);
            Assert.Equal(Response.Updated, newTaskResponse);
        }

        [Fact]
        public void CreateUserAndValidateCreation_DeleteUserAndValidateDeletion_UserIsNull()
        {
            var userDTO = new UserCreateDTO
            {
                Name = "John Doe",
                Email = "john@doe.com",
            };
            var createdResponse = _repo.Create(userDTO);

            Assert.Equal(Response.Created, createdResponse.Response);

            var deleteResponse = _repo.Delete(createdResponse.UserId, true);

            Assert.Equal(Response.Deleted, deleteResponse);

            Assert.Null(_repo.Read(createdResponse.UserId));
        }
    }
}