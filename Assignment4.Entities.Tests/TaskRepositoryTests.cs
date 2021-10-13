using System;
using Xunit;
using Assignment4.Core;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests
    {
        private readonly KanbanContext _context;
        private readonly TaskRepository _repo;
        public TaskRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();
            context.Users.Add(new User
            {
                id = 1,
                name = "John Taylor",
                email = "john.L.T@email.dk",
                tasks = null
            });
            context.Users.Add(new User
            {
                id = 2,
                name = "Dee Doe",
                email = "deedoe@gmail.com",
                tasks = null
            });
            context.Users.Add(new User
            {
                id = 3,
                name = "Philip P. Cyan",
                email = "ppcyan@itu.dk",
                tasks = null
            });
            context.Users.Add(new User
            {
                id = 4,
                name = "FirstName LastName",
                email = "first.last@gmail.co",
                tasks = null
            });
            context.SaveChanges();

            _context = context;
            _repo = new TaskRepository(_context);

        }

        [Fact]
        public void CreatingTask_ValidatingThatAttributesAreSet()
        {
            var taskCreateDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 2,
                Description = "Lorem Ipsum",
                Tags = null
            };

            (Response response, int id) = _repo.Create(taskCreateDTO);

            Assert.Equal(Response.Created, response);

            var expected = new TaskDetailsDTO(id, "Make UI", "Lorem Ipsum", DateTime.UtcNow, "Dee Doe", null, State.New, DateTime.UtcNow);
            var actual = _repo.Read(id);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Created, actual.Created, precision: TimeSpan.FromSeconds(5));
            Assert.Equal(expected.AssignedToName, actual.AssignedToName);
            Assert.Null(actual.Tags);
            Assert.Equal(expected.State, actual.State);
            Assert.Equal(expected.StateUpdated, actual.StateUpdated, precision: TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void CountNumberOfTasks_AddOne_ValidateIncremention()
        {
            int a = _repo.ReadAll().Count();
            var taskDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 3,
                Description = "hej",
                Tags = null,
            };
            var task = _repo.Create(taskDTO);
            int b = _repo.ReadAll().Count();
            Assert.Equal(a, b - 1);
            _repo.Delete(task.TaskId);
            int c = _repo.ReadAll().Count();
            Assert.Equal(a, c);
        }


        [Fact]
        public void CountNumberOfRemovedTasks_RemoveOne_ValidateIncremention()
        {
            int a = _repo.ReadAllRemoved().Count();
            var taskDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 4,
                Description = "hej",
                Tags = null,
            };

            (Response oldTaskResponse, int id) = _repo.Create(taskDTO);
            Assert.Equal(Response.Created, oldTaskResponse);

            var updateTask = new TaskUpdateDTO
            {
                Id = id,
                Title = "Making UI",
                AssignedToId = 2,
                Description = "farvel",
                Tags = null,
                State = State.Active,
            };

            var updatedtask = _repo.Update(updateTask);

            Assert.Equal(Response.Updated, updatedtask);

            _repo.Delete(updateTask.Id);
            int b = _repo.ReadAllRemoved().Count();
            Assert.Equal(a, b - 1);
        }

        [Fact]
        public void CountNumberOfTasksWithTag_AddOne_ValidateIncremention()
        {
            var taskDTOOne = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 4,
                Description = "hej",
                Tags = new List<string>() { "UI" },
            };
            _repo.Create(taskDTOOne);

            int a = _repo.ReadAllByTag("UI").Count();

            var taskDTOTwo = new TaskCreateDTO
            {
                Title = "Make Other UI",
                AssignedToId = 4,
                Description = "hej",
                Tags = new List<string> { "UI" },
            };
            _repo.Create(taskDTOTwo);

            int b = _repo.ReadAllByTag("UI").Count();
            Assert.Equal(a, b - 1);
        }

        [Fact]
        public void CountNumberOfTasksAssignedToUser_AddOne_ValidateIncremention()
        {
            var taskDTOOne = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 4,
                Description = "hej",
                Tags = new List<string>() { "UI" },
            };
            _repo.Create(taskDTOOne);

            int a = _repo.ReadAllByUser(4).Count();

            var taskDTOTwo = new TaskCreateDTO
            {
                Title = "Make Other UI",
                AssignedToId = 4,
                Description = "hej",
                Tags = new List<string> { "UI" },
            };
            _repo.Create(taskDTOTwo);

            int b = _repo.ReadAllByUser(4).Count();
            Assert.Equal(a, b - 1);
        }

        [Fact]
        public void CountNumberOfTasksWithStateNew_AddOne_ValidateIncremention()
        {
            var test = new List<string>().Count();
            int a = _repo.ReadAllByState(State.New).Count();
            var taskDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 3,
                Description = "hej",
                Tags = null,
            };
            var task = _repo.Create(taskDTO);
            int b = _repo.ReadAllByState(State.New).Count();
            Assert.Equal(a, b - 1);
            _repo.Delete(task.TaskId);
            int c = _repo.ReadAllByState(State.New).Count();
            Assert.Equal(a, c);
        }

        [Fact]
        public void CreateTask_ValidateAttributes()
        {
            var taskDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 2,
                Description = "hej",
                Tags = null,
            };
            var task = _repo.Create(taskDTO);
            var actual = _repo.Read(task.TaskId);

            var expected = new TaskDetailsDTO(task.TaskId, "Make UI", "hej", DateTime.UtcNow, "Dee Doe", null, State.New, DateTime.UtcNow);

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Created, actual.Created, precision: TimeSpan.FromSeconds(5));
            Assert.Equal(expected.AssignedToName, actual.AssignedToName);
            Assert.Null(actual.Tags);
            Assert.Equal(expected.State, actual.State);
            Assert.Equal(expected.StateUpdated, actual.StateUpdated, precision: TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void CreateTask_UpdateTask_ValidateAttributes()
        {
            var taskDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 1,
                Description = "hej",
                Tags = null,

            };
            (Response oldTaskResponse, int id) = _repo.Create(taskDTO);
            Assert.Equal(Response.Created, oldTaskResponse);

            var newTaskDTO = new TaskUpdateDTO
            {
                Title = "Making UI",
                AssignedToId = 2,
                Description = "farvel",
                Tags = null,
                Id = id,
                State = State.Active,
            };
            var newTaskResponse = _repo.Update(newTaskDTO);
            Assert.Equal(Response.Updated, newTaskResponse);

            var actual = _repo.Read(newTaskDTO.Id);

            Assert.Equal("Making UI", actual.Title);
            Assert.Equal("farvel", actual.Description);
            Assert.Equal(State.Active, actual.State);
            Assert.Equal(DateTime.UtcNow, actual.StateUpdated, precision: TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void CreateTaskAndValidateCreation_DeleteTaskAndValidateDeletion_TaskIsNull()
        {
            var taskDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 1,
                Description = "hej",
                Tags = null,
            };
            var createdResponse = _repo.Create(taskDTO);

            Assert.Equal(Response.Created, createdResponse.Response);

            var deleteResponse = _repo.Delete(createdResponse.TaskId);

            Assert.Equal(Response.Deleted, deleteResponse);

            Assert.Null(_repo.Read(createdResponse.TaskId));
        }
    }
}
