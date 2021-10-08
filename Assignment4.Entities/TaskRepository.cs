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

        KanbanContext _context;

        public TaskRepository()
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..//..//..//..//")))
                    .AddUserSecrets<TaskRepository>()
                    .AddJsonFile("appsettings.json")
                    .Build();

            var connectionString = configuration.GetConnectionString("KanbanBoard");

            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>().UseSqlServer(connectionString);

            var context = new KanbanContext(optionsBuilder.Options);

            _context = context;
        }

        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {
            User user = null;
            if (task.AssignedToId != null)
            {
                var userDTOResponse = new UserRepository().Read((int)task.AssignedToId);

                user = new User
                {
                    id = userDTOResponse.Id,
                    name = userDTOResponse.Name,
                    email = userDTOResponse.Email,
                    tasks = null,
                };
            }

            var taskElement = new Task
            {
                title = task.Title,
                assignedTo = user,
                description = task.Description,
                tags = (ICollection<Tag>)task.Tags,
                created = DateTime.UtcNow,
                state = State.New,
                stateUpdated = DateTime.UtcNow,
            };
            _context.Tasks.Add(taskElement);
            _context.SaveChanges();
            Console.WriteLine(taskElement.id);

            return (Response.Created, taskElement.id);
        }


        /*
        MAYBE FOR TESTS??
        public bool existsInDb(int taskId)
        {
            return FindById(taskId) != null;
        }


        public int getCount()
        {
            return _context.Tasks.Count();
        }
        */

        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            /*List<TaskDTO> collection = new List<TaskDTO>();
            foreach (var task in _context.Tasks.ToList())
            {
                var element = new TaskDTO
                {
                    Id = task.id,
                    Title = task.title,
                    AssignedToName = task.assignedTo == null ? null : task.assignedTo.name,
                    Tags = null,
                    State = task.state,
                };
                collection.Add(element);
            }
            return collection; */
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int id)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            throw new NotImplementedException();
        }

        public TaskDetailsDTO Read(int id)
        {
            var result = from t in _context.Tasks
                         join u in _context.Users on t.assignedTo.id equals u.id into taskuser
                         from testnavn in taskuser.DefaultIfEmpty()
                         where t.id == id
                         select new TaskDetailsDTO(t.id, t.title, t.description, t.created, t.assignedTo.name, null, t.state, t.stateUpdated);

            return (TaskDetailsDTO)result.FirstOrDefault();
        }

        public Response Update(TaskUpdateDTO task)
        {
            /*
            var result = context.Tasks.Single(s => s.id == task.Id);
            result.title = task.Title;
            if (task.AssignedToName != null)
            {
                result.assignedTo = context.Users.Single(s => s.userName == task.AssignedToId);
            }
            result.description = task.Description;
            result.state = task.State;

            //update tags HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            context.SaveChanges();
            */
            throw new NotImplementedException();
        }

        Response ITaskRepository.Delete(int id)
        {
            /*_context.Tasks.Remove(_context.Tasks.Single(s => s.id == taskId));
            _context.SaveChanges();*/
            throw new NotImplementedException();
        }
    }
}
