using Xunit;
using Assignment4.Core;
using System.Collections.Generic;

namespace Assignment4.Entities.Tests
{
    public class TagRepositoryTests
    {
        /*TagRepository ts = new TagRepository();

        [Fact]
        public void CreateDTO()
        {
            var tagCreateDTO = new TagCreateDTO
            {
                Name = "test"
            };

            (Response response, int id) = ts.Create(tagCreateDTO);

            Assert.Equal(Response.Created, response);

            var expected = new TagDTO(id, "test");
            var actual = ts.Read(id);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
        }


        [Fact]
        public void Read()
        {
            TagCreateDTO tag = new TagCreateDTO
            {
                Name = "Innovation"
            };

            (Response tagResponse, int tagId) = ts.Create(tag);

            TagDTO tagDTO = new TagDTO(tagId, tag.Name);

            Assert.Equal(tagDTO, ts.Read(tagId));
        }

        [Fact]
        public void ReadAll()
        {
            int a = ts.ReadAll().Count;
            var tagDTO = new TagCreateDTO
            {
                Name = "Make UI",
            };
            var tag = ts.Create(tagDTO);
            int b = ts.ReadAll().Count;
            Assert.Equal(a, b - 1);
            ts.Delete(tag.TagId);
            int c = ts.ReadAll().Count;
            Assert.Equal(a, c);
        }

        [Fact]
        public void Update()
        {
            var tagDTO = new TagCreateDTO
            {
                Name = "Make UI",
            };
            (Response oldTaskResponse, int id) = ts.Create(tagDTO);
            Assert.Equal(Response.Created, oldTaskResponse);

            var newTagDTO = new TagUpdateDTO
            {
                Name = "UI",
            };
            var newTaskResponse = ts.Update(newTagDTO);
            Assert.Equal(Response.Updated, newTaskResponse);
        }

        [Fact]
        public void Delete()
        {
            TagCreateDTO tag = new TagCreateDTO
            {
                Name = "UI",
            };
            (Response tagResponse, int id) = ts.Create(tag);

            var tagDeleteResponse = ts.Delete(id);

            Assert.Equal(Response.Deleted, tagDeleteResponse);
            Assert.Null(ts.Read(id));
        }

        [Fact]
        public void DeleteUnsuccesful_ForceMissing()
        {
            TagCreateDTO tag = new TagCreateDTO
            {
                Name = "User Interface",
            };
            (Response tagResponse, int id) = ts.Create(tag);

            var taskCreateDTO = new TaskCreateDTO
            {
                Title = "Make UI ect.",
                AssignedToId = 4,
                Description = "We're making UI and other things.",
                Tags = new List<string> { "User Interface" },
            };

            var tagDeleteResponse = ts.Delete(id);
            Assert.Equal(Response.Conflict, tagDeleteResponse);
        }

        [Fact]
        public void DeleteSuccesful_WithForce()
        {
            TagCreateDTO tag = new TagCreateDTO
            {
                Name = "UI ect.",
            };
            (Response tagResponse, int id) = ts.Create(tag);

            var taskCreateDTO = new TaskCreateDTO
            {
                Title = "Make UI ect.",
                AssignedToId = 5,
                Description = "We're making UI and other things.",
                Tags = new List<string> { "UI ect." },
            };

            var tagDeleteResponse = ts.Delete(id, true);
            Assert.Equal(Response.Deleted, tagDeleteResponse);
            Assert.Null(ts.Read(id));
        }*/

    }
}