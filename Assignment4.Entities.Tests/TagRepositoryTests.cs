using Xunit;
using Assignment4.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Assignment4.Entities.Tests
{
    public class TagRepositoryTests
    {

        private readonly KanbanContext _context;
        private readonly TagRepository _repo;
        public TagRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();
            context.Tasks.Add(new Task
            {
                id = 1,
                title = "Kort",
                state = State.New,
            });
            context.SaveChanges();
            _context = context;
            _repo = new TagRepository(_context);

        }



        [Fact]
        public void CreatingTag_ValidatingThatAttributesAreSet()
        {
            var tagCreateDTO = new TagCreateDTO
            {
                Name = "Test"
            };

            var response = _repo.Create(tagCreateDTO);

            Assert.Equal(Response.Created, response.Response);

            var expected = new TagDTO(response.TagId, "Test");
            var actual = _repo.Read(response.TagId);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            _repo.Delete(response.TagId, false);
        }


        [Fact]
        public void CreateTag_ValidateAttributes()
        {
            TagCreateDTO tag = new TagCreateDTO
            {
                Name = "Innovation"
            };

            var response = _repo.Create(tag);

            TagDTO tagDTO = new TagDTO(response.TagId, tag.Name);

            Assert.Equal(tagDTO, _repo.Read(response.TagId));
            _repo.Delete(response.TagId, false);
        }

        [Fact]
        public void CountNumberOfTags_AddOne_ValidateIncremention()
        {
            int a = _repo.ReadAll().Count;
            var tagDTO = new TagCreateDTO
            {
                Name = "Make UI",
            };
            var tag = _repo.Create(tagDTO);
            int b = _repo.ReadAll().Count;
            Assert.Equal(a, b - 1);
            _repo.Delete(tag.TagId);
            int c = _repo.ReadAll().Count;
            Assert.Equal(a, c);
        }

        [Fact]
        public void CreateTag_UpdateTag_ValidateAttributes()
        {
            var tagDTO = new TagCreateDTO
            {
                Name = "Make UI",
            };
            var response = _repo.Create(tagDTO);
            Assert.Equal(Response.Created, response.Response);

            var newTagDTO = new TagUpdateDTO
            {
                Id = response.TagId,
                Name = "UI",
            };
            var newTaskResponse = _repo.Update(newTagDTO);
            Assert.Equal(Response.Updated, newTaskResponse);
            _repo.Delete(response.TagId, false);
        }

        [Fact]
        public void CreateTagAndValidateCreation_DeleteTagAndValidateDeletion_TagIsNull()
        {
            TagCreateDTO tag = new TagCreateDTO
            {
                Name = "User Interface",
            };
            (Response tagResponse, int id) = _repo.Create(tag);

            Assert.Equal(Response.Created, tagResponse);

            var tagDeleteResponse = _repo.Delete(id);

            Assert.Equal(Response.Deleted, tagDeleteResponse);
            Assert.Null(_repo.Read(id));
        }

        [Fact]
        public void CreateTagAndValidateCreation_ConnectToUser_DeleteTagExpectForceMissing_ReturnsConflict()
        {
            TagCreateDTO tag = new TagCreateDTO
            {
                Name = "User Interface",
            };
            var response = _repo.Create(tag);
            Assert.Equal(Response.Created, response.Response);

            _repo.ConnectToTask(response.TagId, 1);

            var tagDeleteResponse = _repo.Delete(response.TagId, false);
            Assert.Equal(Response.Conflict, tagDeleteResponse);
            _repo.Delete(response.TagId, true);
        }

        [Fact]
        public void CreateTagAndValidateCreation_ConnectToUser_DeleteTagUseForce_ReturnsDeleted()
        {
            TagCreateDTO tag = new TagCreateDTO
            {
                Name = "User Interface",
            };
            var response = _repo.Create(tag);
            Assert.Equal(Response.Created, response.Response);

            _repo.ConnectToTask(response.TagId, 1);

            var tagDeleteResponse = _repo.Delete(response.TagId, true);
            Assert.Equal(Response.Deleted, tagDeleteResponse);
        }

    }
}