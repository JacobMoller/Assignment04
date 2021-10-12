using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assignment4.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Assignment4.Entities
{
    public class UserRepository : IUserRepository
    {
        KanbanContext _context;

        public UserRepository()
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
        public (Response Response, int UserId) Create(UserCreateDTO user)
        {
            var userResult = _context.Users.FirstOrDefault(t => t.email == user.Email);
            if (userResult == null)
            {
                var newUserElement = new User
                {
                    name = user.Name,
                    email = user.Email,
                };
                _context.Users.Add(newUserElement);
                _context.SaveChanges();
                return (Response.Created, newUserElement.id);
            }
            else
            {
                return (Response.Conflict, 0);
            }
        }

        public IReadOnlyCollection<UserDTO> ReadAll()
        {
            var users = new List<UserDTO>();
            foreach (var item in _context.Users)
            {
                users.Add(new UserDTO(item.id, item.name, item.email));
            }
            return users;
        }

        public UserDTO Read(int userId)
        {
            User user = _context.Users.FirstOrDefault(s => s.id == userId);
            if (user != null)
            {
                return new UserDTO(user.id, user.name, user.email);
            }
            else
            {
                return null;
            }

        }

        public Response Update(UserUpdateDTO user)
        {
            var userElement = new User
            {
                name = user.Name,
                email = user.Email,
            };
            var elementToBeUpdated = _context.Users.FirstOrDefault(x => x.id == user.Id);
            //Maybe use the _context.UpdateRange here?
            if (elementToBeUpdated != null)
            {
                elementToBeUpdated = userElement;
                _context.SaveChanges();
                return Response.Updated;
            }
            else
            {
                return Response.BadRequest;
            }
        }

        public Response Delete(int userId, bool force = false)
        {
            var userResult = _context.Users.FirstOrDefault(u => u.id == userId);
            if (userResult != null && userResult.tasks != null && !force)
                return Response.Conflict;
            else
            {
                _context.Remove(userResult);
                _context.SaveChanges();
                return Response.Deleted;
            }
        }
    }
}