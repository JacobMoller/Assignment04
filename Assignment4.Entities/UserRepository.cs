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
            throw new System.NotImplementedException();
        }

        public Response Delete(int userId, bool force = false)
        {
            throw new System.NotImplementedException();
        }

        public UserDTO Read(int userId)
        {
            var user = _context.Users.FirstOrDefault(s => s.id == userId);
            return new UserDTO(user.id, user.name, user.email);
        }

        public IReadOnlyCollection<UserDTO> ReadAll()
        {
            throw new System.NotImplementedException();
        }

        public Response Update(UserUpdateDTO user)
        {
            throw new System.NotImplementedException();
        }
    }
}