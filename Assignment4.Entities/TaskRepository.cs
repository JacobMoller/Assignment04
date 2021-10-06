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
            //Check for additional info from TaskDTO
            var taskElement = new Task
            {
                title = task.Title,
                state = task.State,
            };
            context.Tasks.Add(taskElement);
            context.SaveChanges();

            return taskElement.taskId;
        }

        public void Delete(int taskId)
        {
            context.Tasks.Remove(context.Tasks.Single(s => s.taskId == taskId));
            context.SaveChanges();
        }

        public TaskDetailsDTO FindById(int id)
        {
            if (context.Tasks.Where(task => task.taskId == id).Count() > 0)
            {
                var result = from t in context.Tasks
                             join u in context.Users on t.assignedTo.userId equals u.userId into hej
                             from testnavn in hej.DefaultIfEmpty()
                             where t.taskId == id
                             select new TaskDetailsDTO()
                             {
                                 Id = t.taskId,
                                 Title = t.title,
                                 Description = t.description,
                                 AssignedToId = t.assignedTo.userId,
                                 AssignedToName = testnavn.name,
                                 AssignedToEmail = testnavn.email,
                                 Tags = null,
                                 State = t.state
                             };
                return (TaskDetailsDTO)result.Single();
            }
            else
            {
                return null;
            }
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
            context.Dispose();
        }


        public bool existsInDb(int taskId)
        {
            return FindById(taskId) != null;
        }

        public int getCount()
        {
            return context.Tasks.Count();
        }

        public void tagsTest()
        {
            //context.Entry(task).Collection(t => t.Tags).Query("")
        }
    }
}
