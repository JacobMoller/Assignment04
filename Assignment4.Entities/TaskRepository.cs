using System;
using Assignment4.Core;
using System.Collections.Generic;
using Assignment4;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IO;
using System.Linq;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {
        KanbanContext context = getContext();
        public static KanbanContext getContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..//..//..//..//")))
                .AddUserSecrets<TaskRepository>()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("KanbanBoard");

            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>().UseSqlServer(connectionString);
            return new KanbanContext(optionsBuilder.Options);
        }

        public IReadOnlyCollection<TaskDTO> All()
        {
            List<TaskDTO> collection = new List<TaskDTO>();
            foreach (var item in context.Tasks.ToList())
            {
                var element = new TaskDTO
                {
                    Id = item.taskId,
                    Title = item.title,
                    Description = item.description,
                    AssignedToId = item.assignedTo == null ? null : item.assignedTo.userId,
                    State = item.state,
                };
                collection.Add(element);
            }
            return collection;
        }
        public int Create(TaskDTO task)
        {
            var taskElement = new Task
            {
                title = task.Title,
                state = task.State,
            };
            context.Tasks.Add(taskElement);
            context.SaveChanges();

            return (int)taskElement.taskId;
        }

        public void Delete(int taskId)
        {
            context.Tasks.Remove(context.Tasks.Single(s => s.taskId == taskId));
            context.SaveChanges();
        }

        public TaskDetailsDTO FindById(int id)
        {
            var result = context.Users.Join(context.Tasks,
            user => user.userId,
            task => task.assignedTo.userId,
            (user, task) => new { user, task })
            .Where(task => task.task.taskId == id)
            .Select(z => new { user = z.user, task = z.task });

            var taskResult = result.First().task;
            var userResult = result.First().user;
            var taskDTO = new TaskDetailsDTO
            {
                Id = taskResult.taskId,
                Title = taskResult.title,
                Description = taskResult.description,
                AssignedToId = userResult.userId,
                AssignedToName = userResult.name,
                AssignedToEmail = userResult.email,
                Tags = null,
                State = taskResult.state,
            };
            //update tags HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //(IEnumerable<string>) taskResult.tags.SelectMany(tag => tag.name),

            return taskDTO;
        }

        public void Update(TaskDTO task)
        {
            var result = context.Tasks.Single(s => s.taskId == task.Id);
            result.title = task.Title;
            if (task.AssignedToId != null)
            {
                result.assignedTo = context.Users.Single(s => s.userId == task.AssignedToId);
            }
            result.description = task.Description;
            result.state = task.State;
            //update tags HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
