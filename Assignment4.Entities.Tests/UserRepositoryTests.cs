using Xunit;
using Assignment4.Core;

namespace Assignment4.Entities.Tests
{
    public class UserRepositoryTests
    {

        UserRepository us = new UserRepository();

        [Fact]
        public void Create()
        {
            var userCreateDTO = new UserCreateDTO
            {
                Name = "John Doe",
                Email = "john@doe.com",
            };

            var createResponse = us.Create(userCreateDTO);
            var expected = new UserDTO(createResponse.UserId, "John Doe", "john@doe.com");
            var actual = us.Read(createResponse.UserId);

            Assert.Equal(Response.Created, createResponse.Response);
            us.Delete(createResponse.UserId, true);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Email, actual.Email);
        }

        [Fact]
        public void ReadAll()
        {
            var userDTO = new UserCreateDTO
            {
                Name = "Jenny Doe",
                Email = "jenny@doe.com",
            };
            int a = us.ReadAll().Count;
            var user = us.Create(userDTO);
            int b = us.ReadAll().Count;
            us.Delete(user.UserId);
            int c = us.ReadAll().Count;
            Assert.Equal(a, b - 1);
            Assert.Equal(a, c);
        }

        [Fact]
        public void Read()
        {
            var userDTO = new UserCreateDTO
            {
                Name = "John Doe",
                Email = "john@doe.com",
            };
            var user = us.Create(userDTO);
            var actual = us.Read(user.UserId);

            var expected = new UserDTO(user.UserId, "John Doe", "john@doe.com");
            us.Delete(user.UserId, true);

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Email, actual.Email);
        }

        [Fact]
        public void Update()
        {
            var userDTO = new UserCreateDTO
            {
                Name = "John Doe",
                Email = "john@doe.com",
            };
            (Response oldTaskResponse, int id) = us.Create(userDTO);
            Assert.Equal(Response.Created, oldTaskResponse);

            var newUserDTO = new UserUpdateDTO
            {
                Id = id,
                Name = "John Doe",
                Email = "john@doe.dk",
            };
            var newTaskResponse = us.Update(newUserDTO);
            Assert.Equal(Response.Updated, newTaskResponse);
            us.Delete(id, true);
        }

        [Fact]
        public void Delete()
        {
            var us = new UserRepository();

            var userDTO = new UserCreateDTO
            {
                Name = "John Doe",
                Email = "john@doe.com",
            };
            var createdResponse = us.Create(userDTO);

            Assert.Equal(Response.Created, createdResponse.Response);

            var deleteResponse = us.Delete(createdResponse.UserId, true);

            Assert.Equal(Response.Deleted, deleteResponse);

            Assert.Null(us.Read(createdResponse.UserId));
        }
    }
}