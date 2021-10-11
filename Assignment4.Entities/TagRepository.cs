using System;
using System.Collections.Generic;
using System.IO;
using Assignment4.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Assignment4.Entities
{
    public class TagRepository : ITagRepository
    {

        KanbanContext _context;

        public TagRepository()
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
        public (Response Response, int TagId) Create(TagCreateDTO tag)
        {
            var tagResult = _context.Tags.Where(t => t.name == tag.Name);
            if (tagResult.ToList().Count == 0)
            {
                var newTagElement = new Tag
                {
                    name = tag.Name,
                };
                _context.Tags.Add(newTagElement);
                _context.SaveChanges();
                return (Response.Created, newTagElement.id);
            }
            else
            {
                return (Response.Conflict, 0);
            }
            /*foreach (var item in _context.Tags)
            {
                Console.WriteLine("Hellooooo");
                if (item.name.ToString() == tag.Name)
                {
                    return (Response.Conflict, 0);
                }
            }
            var newTagElement = new Tag
            {
                name = tag.Name,
            };
            _context.Tags.Add(newTagElement);
            _context.SaveChanges();
            return (Response.Created, newTagElement.id);*/
        }

        public Response Delete(int tagId, bool force = false)
        {
            var tagResult = _context.Tags.FirstOrDefault(t => t.id == tagId);
            Console.WriteLine(tagResult.tasks == null);
            if (tagResult.tasks != null && !force)
                return Response.Conflict;
            else
            {
                _context.Remove(tagResult);
                _context.SaveChanges();
                return Response.Deleted;
            }
        }

        public TagDTO Read(int tagId)
        {
            var result = from t in _context.Tags
                         where t.id == tagId
                         select new TagDTO(t.id, t.name);

            return result.FirstOrDefault();
        }

        public IReadOnlyCollection<TagDTO> ReadAll()
        {
            var tags = new List<TagDTO>();
            foreach (var item in _context.Tags)
            {
                tags.Add(new TagDTO(item.id, item.name));
            }
            return tags;
        }

        public Response Update(TagUpdateDTO tag)
        {
            var tagElement = new Tag
            {
                name = tag.Name,
            };
            var elementToBeUpdated = _context.Tags.Single(x => x.id == tag.Id);
            //Maybe use the _context.UpdateRange here?
            elementToBeUpdated = tagElement;
            _context.SaveChanges();
            return Response.Updated;
        }
    }
}