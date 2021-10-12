using Xunit;
using Assignment4.Core;

namespace Assignment4.Entities.Tests
{
    public class TagRepositoryTests
    {
        TagRepository ts = new TagRepository();

        [Fact]
        public void CreateDTO()
        {
            var tagCreateDTO = new TagCreateDTO
            {
                Name = "Test"
            };

            var response = ts.Create(tagCreateDTO);

            Assert.Equal(Response.Created, response.Response);

            var expected = new TagDTO(response.TagId, "Test");
            var actual = ts.Read(response.TagId);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            ts.Delete(response.TagId, false);
        }


        [Fact]
        public void Read()
        {
            TagCreateDTO tag = new TagCreateDTO
            {
                Name = "Innovation"
            };

            var response = ts.Create(tag);

            TagDTO tagDTO = new TagDTO(response.TagId, tag.Name);

            Assert.Equal(tagDTO, ts.Read(response.TagId));
            ts.Delete(response.TagId, false);
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
            var response = ts.Create(tagDTO);
            Assert.Equal(Response.Created, response.Response);

            var newTagDTO = new TagUpdateDTO
            {
                Id = response.TagId,
                Name = "UI",
            };
            var newTaskResponse = ts.Update(newTagDTO);
            Assert.Equal(Response.Updated, newTaskResponse);
            ts.Delete(response.TagId, false);
        }

        [Fact]
        public void Delete()
        {
            TagCreateDTO tag = new TagCreateDTO
            {
                Name = "User Interface",
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
            var response = ts.Create(tag);

            ts.ConnectToTask(response.TagId, 1);

            var tagDeleteResponse = ts.Delete(response.TagId, false);
            Assert.Equal(Response.Conflict, tagDeleteResponse);
            ts.Delete(response.TagId, true);
        }

        [Fact]
        public void DeleteSuccesful_WithForce()
        {
            TagCreateDTO tag = new TagCreateDTO
            {
                Name = "User Interface",
            };
            var response = ts.Create(tag);

            ts.ConnectToTask(response.TagId, 1);

            var tagDeleteResponse = ts.Delete(response.TagId, true);
            Assert.Equal(Response.Deleted, tagDeleteResponse);
        }

    }
}