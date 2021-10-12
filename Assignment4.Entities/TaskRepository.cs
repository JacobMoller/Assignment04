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
            UserDTO user = null;
            User userDTOResponse = null;
            if (task.AssignedToId != null)
            {
                userDTOResponse = _context.Users.SingleOrDefault(x => x.id == task.AssignedToId);

                user = new UserDTO(userDTOResponse.id, userDTOResponse.name, userDTOResponse.email);
            }

            //Handle Tags
            var cleanedTags = new List<Tag>();
            if (task.Tags != null)
            {
                foreach (var item in task.Tags)
                {
                    var tag = _context.Tags.Where(x => x.name == item);
                    if (tag.Count() == 0)
                    {
                        var newTag = new Tag
                        {
                            id = 0,
                            name = item,
                            tasks = null,
                        };
                        _context.Tags.Add(newTag);
                        _context.SaveChanges();
                        cleanedTags.Add(newTag);
                    }
                    else
                    {
                        cleanedTags.Add(tag.First());
                    }
                }
            }

            var taskElement = new Task
            {
                title = task.Title,
                assignedTo = userDTOResponse,
                description = task.Description,
                tags = cleanedTags,
                created = DateTime.UtcNow,
                state = State.New,
                stateUpdated = DateTime.UtcNow,
            };
            _context.Tasks.Add(taskElement);
            _context.SaveChanges();

            foreach (var item in cleanedTags)
            {
                var tagToBeUpdated = _context.Tags.Where(x => x.id == item.id).FirstOrDefault();
                List<Task> tasksList = tagToBeUpdated.tasks.ToList();
                tasksList.Add(taskElement);
                tagToBeUpdated.tasks = tasksList;
                _context.SaveChanges();
            }

            return (Response.Created, taskElement.id);
        }

        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            var tasks = new List<TaskDTO>();
            foreach (var item in _context.Tasks)
            {
                string assignedToName = null;
                if (item.assignedTo != null)
                {
                    assignedToName = item.assignedTo.name;
                }
                tasks.Add(new TaskDTO(item.id, item.title, assignedToName, getTagNameFromTag(item.tags), item.state));
            }
            return tasks;
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            var tasks = new List<TaskDTO>();
            foreach (var item in _context.Tasks)
            {
                if (item.state == State.Removed)
                {
                    string assignedToName = null;
                    if (item.assignedTo != null)
                    {
                        assignedToName = item.assignedTo.name;
                    }
                    tasks.Add(new TaskDTO(item.id, item.title, assignedToName, getTagNameFromTag(item.tags), item.state));
                }
            }
            return tasks;
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            var tasks = new List<TaskDTO>();
            foreach (var item in _context.Tasks)
            {
                foreach (var currentTag in item.tags)
                {
                    if (currentTag.name == tag)
                    {
                        string assignedToName = null;
                        if (item.assignedTo != null)
                        {
                            assignedToName = item.assignedTo.name;
                        }
                        tasks.Add(new TaskDTO(item.id, item.title, assignedToName, getTagNameFromTag(item.tags), item.state));
                    }
                }
            }
            return tasks;
        }

        private IReadOnlyCollection<string> getTagNameFromTag(ICollection<Tag> tagsInput)
        {
            var tags = new List<string>();
            if (tagsInput != null)
            {
                foreach (var tagItem in tagsInput)
                {
                    tags.Add(tagItem.name);
                }
            }
            return tags;
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int id)
        {
            var tasks = new List<TaskDTO>();
            foreach (var item in _context.Tasks)
            {
                if (item.assignedTo.id == id)
                {
                    string assignedToName = null;
                    if (item.assignedTo != null)
                    {
                        assignedToName = item.assignedTo.name;
                    }
                    tasks.Add(new TaskDTO(item.id, item.title, assignedToName, getTagNameFromTag(item.tags), item.state));
                }
            }
            return tasks;
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            var tasks = new List<TaskDTO>();
            foreach (var item in _context.Tasks)
            {
                if (item.state == state)
                {
                    string assignedToName = null;
                    if (item.assignedTo != null)
                    {
                        assignedToName = item.assignedTo.name;
                    }
                    var tags = new List<string>();
                    tasks.Add(new TaskDTO(item.id, item.title, assignedToName, getTagNameFromTag(item.tags), item.state));
                }
            }
            return tasks;
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
            //Create/update task must allow for editing tags.
            User user = null;
            if (task.AssignedToId != null)
            {
                var userDTOResponse = _context.Users.SingleOrDefault(x => x.id == task.AssignedToId);

                user = new User
                {
                    id = 0,
                    name = userDTOResponse.name,
                    email = userDTOResponse.email,
                    tasks = userDTOResponse.tasks,
                };
            }
            var taskElement = new Task
            {
                title = task.Title,
                assignedTo = user,
                description = task.Description,
                tags = (ICollection<Tag>)task.Tags,
                state = task.State,
                stateUpdated = DateTime.UtcNow,
            };
            var elementToBeUpdated = _context.Tasks.Single(x => x.id == task.Id);
            //Maybe use the _context.UpdateRange here?
            elementToBeUpdated = taskElement;
            _context.SaveChanges();
            return Response.Updated;
        }

        public Response Delete(int taskId)
        {
            Task task = _context.Tasks.FirstOrDefault(t => t.id == taskId);
            Response response;
            if (task == null)
            {
                return Response.NotFound;
            }
            switch (task.state)
            {
                case State.New:
                    _context.Tasks.Remove(task);
                    _context.SaveChanges();
                    response = Response.Deleted;
                    break;

                case State.Active:
                    task.state = State.Removed;
                    response = Response.Deleted;
                    _context.SaveChanges();
                    break;

                case State.Resolved:
                case State.Closed:
                case State.Removed:
                    response = Response.Conflict;
                    break;
                default:
                    response = Response.BadRequest;
                    break;
            }
            return response;
        }
    }
}
