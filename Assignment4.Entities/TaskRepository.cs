using System;
using Assignment4.Core;
using System.Collections.Generic;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {
        public IReadOnlyCollection<TaskDTO> All()
        {
            throw new NotImplementedException();
            //SELECT * FROM Tasks
        }
        public int Create(TaskDTO task)
        {
            throw new NotImplementedException();
        }

        public void Delete(int taskId)
        {
            throw new NotImplementedException();
        }

        public TaskDetailsDTO FindById(int id)
        {
            throw new NotImplementedException();
            //SELECT * FROM Tasks WHERE taskId = id
            //^Remember to avoid sql injections here
        }

        public void Update(TaskDTO task)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
