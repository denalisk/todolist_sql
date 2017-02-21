using Xunit;
using ToDoList.Objects;
using System;
using System.Collections.Generic;

namespace ToDoList
{
    public class ToDoTest : IDisposable
    {
        public ToDoTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_TaskDatabaseFirstEmpty()
        {

            //Arrange
            int result = Task.GetAll().Count;

            //Assert
            Assert.Equal(result, 0);

        }

        [Fact]
        public void Test_Save_SaveTaskToDatabase()
        {
            // Arrange
                Task newTask = new Task("Lawn Chores", 0, "2000-01-01");

            // Act
                newTask.Save();
                List<Task> allTasks = Task.GetAll();
                List<Task> testList = new List<Task>{newTask};

            // Assert
                Assert.Equal(testList, allTasks);

        }

        [Fact]
        public void Test_Save_AssignsIdToTask()
        {
            // Arrange
            Task newTask = new Task("Chores", 0, "2000-01-01");

            // Act
            newTask.Save();
            Task savedTask = Task.GetAll()[0];

            int resultId = savedTask.GetId();
            int testId = newTask.GetId();

            // Assert
            Assert.Equal(testId, resultId);
        }

        [Fact]
        public void Test_Find_FindTaskInDatabase()
        {
            //Arrange
            Task newTask = new Task("Chores", 0, "2000-01-01");
            newTask.Save();

            //Act
            Task foundTask = Task.Find(newTask.GetId());

            //Assert
            Assert.Equal(newTask, foundTask);
        }

        [Fact]
        public void Test_Delete_RemoveTaskFromDatabase()
        {
            // Arrange

            Task firstTask = new Task("Wash Dishes", 1, "1999-01-01");
            firstTask.Save();
            Task secondTask = new Task("Empty Dishwasher", 1, "2000-01-01");
            secondTask.Save();


            // Act
            List<Task> testTaskList = new List<Task> {secondTask};
            firstTask.Delete();
            List<Task> resultTaskList = Task.GetAll();

            //Assert
            Assert.Equal(testTaskList, resultTaskList);

        }

        public void Dispose()
        {
            Task.DeleteAll();
        }
    }
}
