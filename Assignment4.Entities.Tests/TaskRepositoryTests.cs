using System;
using Xunit;
using Assignment4.Core;
using System.Linq;
using System.Collections.Generic;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests
    {

        TaskRepository ts = new TaskRepository();

        [Fact]
        public void CreateDTO()
        {
            var taskCreateDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 2,
                Description = "Lorem Ipsum",
                Tags = null
            };

            (Response response, int id) = ts.Create(taskCreateDTO);

            Assert.Equal(Response.Created, response);

            var expected = new TaskDetailsDTO(id, "Make UI", "Lorem Ipsum", DateTime.UtcNow, "Lola Carrodus", null, State.New, DateTime.UtcNow);
            var actual = ts.Read(id);
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
        public void ReadAll()
        {
            int a = ts.ReadAll().Count();
            var taskDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 3,
                Description = "hej",
                Tags = null,
            };
            var task = ts.Create(taskDTO);
            int b = ts.ReadAll().Count();
            Assert.Equal(a, b - 1);
            ts.Delete(task.TaskId);
            int c = ts.ReadAll().Count();
            Assert.Equal(a, c);
        }


        /*[Fact]
        public void ReadAllRemoved()
        {
            int a = ts.ReadAllRemoved().Count();
            var taskDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 4,
                Description = "hej",
                Tags = null,
            };

            (Response oldTaskResponse, int id) = ts.Create(taskDTO);
            Assert.Equal(Response.Created, oldTaskResponse);

            var updateTask = new TaskUpdateDTO
            {
                Title = "Making UI",
                AssignedToId = 2,
                Description = "farvel",
                Tags = null,
                Id = id,
                State = State.Active,
            };

            var updatedtask = ts.Update(updateTask);

            int b = ts.ReadAllRemoved().Count();
            ts.Delete(updateTask.Id);
            Assert.Equal(a, b + 1);
        }*/

        /*[Fact]
        public void ReadAllByTag()
        {
            var taskDTOOne = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 4,
                Description = "hej",
                Tags = new List<string>() { "UI" },
            };
            ts.Create(taskDTOOne);

            int a = ts.ReadAllByTag("UI").Count();

            var taskDTOTwo = new TaskCreateDTO
            {
                Title = "Make Other UI",
                AssignedToId = 4,
                Description = "hej",
                Tags = new List<string> { "UI" },
            };
            ts.Create(taskDTOTwo);

            int b = ts.ReadAllByTag("UI").Count();
            Assert.Equal(a, b - 1);
        }*/

        /*[Fact]
        public void ReadAllByUser()
        {

        }*/

        [Fact]
        public void ReadAllByState()
        {
            var test = new List<string>().Count();
            int a = ts.ReadAllByState(State.New).Count();
            var taskDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 3,
                Description = "hej",
                Tags = null,
            };
            var task = ts.Create(taskDTO);
            int b = ts.ReadAllByState(State.New).Count();
            Assert.Equal(a, b - 1);
            ts.Delete(task.TaskId);
            int c = ts.ReadAllByState(State.New).Count();
            Assert.Equal(a, c);
        }

        [Fact]
        public void Read()
        {
            var taskDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 2,
                Description = "hej",
                Tags = null,
            };
            var task = ts.Create(taskDTO);
            var actual = ts.Read(task.TaskId);

            var expected = new TaskDetailsDTO(task.TaskId, "Make UI", "hej", DateTime.UtcNow, "Lola Carrodus", null, State.New, DateTime.UtcNow);

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
        public void Update()
        {
            var taskDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 1,
                Description = "hej",
                Tags = null,

            };
            (Response oldTaskResponse, int id) = ts.Create(taskDTO);
            Assert.Equal(Response.Created, oldTaskResponse);

            var newTaskDTO = new TaskUpdateDTO
            {
                Title = "Making UI",
                AssignedToId = 2,
                Description = "farvel",
                Tags = null, //TODO: Use different tags
                Id = id,
                State = State.Active,
            };
            var newTaskResponse = ts.Update(newTaskDTO);
            Assert.Equal(Response.Updated, newTaskResponse);
        }

        [Fact]
        public void Delete()
        {
            var taskDTO = new TaskCreateDTO
            {
                Title = "Make UI",
                AssignedToId = 1,
                Description = "hej",
                Tags = null,
            };
            var createdResponse = ts.Create(taskDTO);

            Assert.Equal(Response.Created, createdResponse.Response);

            var deleteResponse = ts.Delete(createdResponse.TaskId);

            Assert.Equal(Response.Deleted, deleteResponse);

            Assert.Null(ts.Read(createdResponse.TaskId));
        }
    }
}
