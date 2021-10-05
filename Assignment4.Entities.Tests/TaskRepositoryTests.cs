using System;
using Assignment4.Entities;
using Xunit;
using Assignment4.Core;
using Assignment4;
using System.Linq;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests
    {
        [Fact]
        public void All()
        {
            Assert.True(true);
        }

        [Fact]
        public void Create()
        {

            /*var ts = new TaskRepository();

            var taskDTO = new TaskDTO
            {
                Title = "Make UI",
                State = State.New,
            };
            ts.Create(taskDTO);
            Console.WriteLine();

            Assert.True(false);*/
            Assert.True(true);
        }

        [Fact]
        public void Delete()
        {
            Assert.True(true);
        }

        [Fact]
        public void FindById()
        {
            var ts = new TaskRepository();
            var expected = new Task
            {
                taskId = 1,
                title = "24/7",
                assignedTo = new User
                {
                    userId = 860,
                    name = "Anatol Crosseland",
                    email = "acrosselandqm@squidoo.com",
                },
                description = "ut ultrices vel augue vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae donec pharetra magna vestibulum aliquet ultrices erat tortor sollicitudin mi sit amet lobortis sapien sapien non mi integer ac neque duis bibendum morbi non quam nec dui luctus rutrum nulla tellus in sagittis dui vel nisl duis ac nibh fusce lacus purus aliquet at feugiat non pretium quis lectus suspendisse potenti in eleifend quam a odio in hac habitasse platea dictumst maecenas ut massa quis augue luctus tincidunt nulla mollis molestie lorem quisque ut erat curabitur gravida nisi at nibh in hac habitasse platea dictumst aliquam augue quam sollicitudin vitae consectetuer eget rutrum at lorem integer tincidunt ante vel ipsum praesent blandit lacinia erat vestibulum sed magna at nunc commodo placerat praesent blandit nam nulla integer pede justo",
                state = State.Active,
            };

            var actual = ts.FindById(expected.taskId);
            Console.WriteLine(expected.SequenceEqual(actual));
            Assert.True(false);
        }

        [Fact]
        public void Update()
        {
            Assert.True(true);
        }

        [Fact]
        public void Dispose()
        {
            Assert.True(true);
        }
    }
}
