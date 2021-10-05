using System;
using Assignment4.Entities;
using Assignment4.Core;

namespace Assignment4
{
    class Program
    {
        static void Main(string[] args)
        {
            var ts = new TaskRepository();
            /*All
            ts.All();
            */

            /*Create
            var taskdto = new TaskDTO
            {
                Title = "lav ui",
                State = State.New,
            };
            ts.Create(taskdto);
            */

            /*Delete
            ts.Delete(910);
            */

            /*FindById
            ts.FindById(1);
            */

            /*Update
            var taskdto = new TaskDTO
            {
                Id = 912,
                Title = "UI Design",
                State = State.Closed,
                AssignedToId = 467,
            };
            ts.Update(taskdto);
            */

            /*Dispose
            ts.Dispose();
            */
        }
    }
}
