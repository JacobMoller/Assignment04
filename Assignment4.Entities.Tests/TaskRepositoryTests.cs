using System;
using Assignment4.Entities;
using Xunit;
using Assignment4.Core;
using Assignment4;
using System.Linq;
using System.Collections.Generic;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests
    {

        TaskRepository ts = new TaskRepository();

        [Fact]
        public void All()
        {
            //Compare rows in the Task table with the size of the List of tasks
            var expected = ts.getCount();

            var actual = ts.All().Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create()
        {
            var taskDTO = new TaskDTO
            {
                Title = "Make UI",
                State = State.New,
            };
            int taskId = ts.Create(taskDTO);
            //Check if the new task was succesfully inserted into the database
            Assert.True(ts.existsInDb(taskId));
            ts.Delete(taskId);
        }

        [Fact]
        public void FindById()
        {
            var ts = new TaskRepository();
            var expected = new TaskDetailsDTO
            {
                Id = 1,
                Title = "24/7",
                Description = "ut ultrices vel augue vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae donec pharetra magna vestibulum aliquet ultrices erat tortor sollicitudin mi sit amet lobortis sapien sapien non mi integer ac neque duis bibendum morbi non quam nec dui luctus rutrum nulla tellus in sagittis dui vel nisl duis ac nibh fusce lacus purus aliquet at feugiat non pretium quis lectus suspendisse potenti in eleifend quam a odio in hac habitasse platea dictumst maecenas ut massa quis augue luctus tincidunt nulla mollis molestie lorem quisque ut erat curabitur gravida nisi at nibh in hac habitasse platea dictumst aliquam augue quam sollicitudin vitae consectetuer eget rutrum at lorem integer tincidunt ante vel ipsum praesent blandit lacinia erat vestibulum sed magna at nunc commodo placerat praesent blandit nam nulla integer pede justo",
                AssignedToId = 860,
                AssignedToName = "Anatol Crosseland",
                AssignedToEmail = "acrosselandqm@squidoo.com",
                Tags = null,
                State = State.Active,
            };

            var actual = ts.FindById(expected.Id);
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Delete()
        {
            var ts = new TaskRepository();

            var taskDTO = new TaskDTO
            {
                Title = "Make UI",
                State = State.New,
            };
            var taskId = ts.Create(taskDTO);

            ts.Delete(taskId);
            Assert.False(ts.existsInDb(taskId));
        }

        [Fact]
        public void Update()
        {
            //New task
            var newTaskDTO = new TaskDTO
            {
                Title = "Make UI",
                State = State.New,
            };
            int taskId = ts.Create(newTaskDTO);
            //update task
            var updatedTaskDTO = new TaskDTO
            {
                Id = taskId,
                Title = "Redesign UI",
                State = State.Active,
            };

            ts.Update(updatedTaskDTO);

            TaskDetailsDTO findByResult = ts.FindById(updatedTaskDTO.Id);

            var actual = new TaskDTO
            {
                Id = findByResult.Id,
                Title = findByResult.Title,
                Description = findByResult.Description,
                AssignedToId = findByResult.AssignedToId,
                Tags = (IReadOnlyCollection<string>)findByResult.Tags,
                State = findByResult.State,
            };

            Assert.Equal(updatedTaskDTO, actual);
        }

        /*[Fact]
        public void Dispose()
        {
            Assert.True(false);
        }*/
    }
}
