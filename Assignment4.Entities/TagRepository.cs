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

        public TagRepository(KanbanContext context)
        {
            _context = context;
        }
        public (Response Response, int TagId) Create(TagCreateDTO tag)
        {
            var tagResult = _context.Tags.Where(t => t.name == tag.Name);
            if (tagResult.Count() == 0)
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
                return (Response.Conflict, tagResult.First().id);
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
            var tagResult = _context.Tags.Where(t => t.id == tagId);
            if (tagResult.Count() > 0)
            {
                if (tagResult.First().tasks != null)
                {
                    if (force)
                    {
                        _context.Remove(tagResult.First());
                        _context.SaveChanges();
                        return Response.Deleted;
                    }
                    else
                    {
                        return Response.Conflict;
                    }
                }
                else
                {
                    _context.Remove(tagResult.First());
                    _context.SaveChanges();
                    return Response.Deleted;
                }
            }
            else
            {
                return Response.BadRequest;
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
            elementToBeUpdated = tagElement;
            _context.SaveChanges();
            return Response.Updated;
        }

        public Response ConnectToTask(int tagId, int taskId)
        {
            var tag = _context.Tags.FirstOrDefault(x => x.id == tagId);
            var task = _context.Tasks.FirstOrDefault(x => x.id == taskId);
            if (tag != null && task != null)
            {
                tag.tasks = new List<Task>() { task };
                _context.SaveChanges();
                return Response.Updated;
            }
            else
            {
                return Response.BadRequest;
            }
        }
    }
}