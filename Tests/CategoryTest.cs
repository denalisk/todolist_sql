using Xunit;
using ToDoList.Objects;
using System;
using System.Collections.Generic;

namespace ToDoList
{
    public class CategoryTest : IDisposable
    {
        public CategoryTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_CategoryDatabaseFirstEmpty()
        {

            //Arrange
            int result = Task.GetAll().Count;

            //Assert
            Assert.Equal(0, result);

        }

        [Fact]
        public void Test_Save_SaveCategoryToDatabase()
        {
            // Arrange
                Category newCategory = new Category("Lawn Chores");

            // Act
                newCategory.Save();
                List<Category> allCategories = Category.GetAll();
                List<Category> testList = new List<Category>{newCategory};

            // Assert
                Assert.Equal(testList, allCategories);

        }
        //
        // [Fact]
        // public void Test_Save_AssignsIdToCategory()
        // {
        //     // Arrange
        //     Category newCategory = new Category("Chores");
        //
        //     // Act
        //     newCategory.Save();
        //     Category savedCategory = Category.GetAll()[0];
        //
        //     int resultId = savedCategory.GetId();
        //     int testId = newCategory.GetId();
        //
        //     // Assert
        //     Assert.Equal(testId, resultId);
        // }
        //
        // [Fact]
        // public void Test_Find_FindCategoryInDatabase()
        // {
        //     //Arrange
        //     Category newCategory = new Category("Chores");
        //     newCategory.Save();
        //
        //     //Act
        //     Category foundCategory = Category.Find(newCategory.GetId());
        //
        //     //Assert
        //     Assert.Equal(newCategory, foundCategory);
        // }
        //
        // [Fact]
        // public void Test_GetTasks_RetrieveAllTasksInCategory()
        // {
        //     //Arrange
        //     Category newCategory = new Category("Kitchen");
        //     newCategory.Save();
        //
        //     Task firstTask = new Task("Wash Dishes", newCategory.GetId(), "1999-01-01");
        //     firstTask.Save();
        //     Task secondTask = new Task("Empty Dishwasher", newCategory.GetId(), "2000-01-01");
        //     secondTask.Save();
        //
        //     //Act
        //     List<Task> testTaskList = new List<Task> {firstTask, secondTask};
        //     List<Task> resultTaskList = newCategory.GetTasks();
        //
        //     //Assert
        //     Assert.Equal(testTaskList, resultTaskList);
        // }
        //
        // [Fact]
        // public void Test_DeleteCategory_DeletesEntireCategory()
        // {
        //     // Arrange
        //     Category newCategory = new Category("Kitchen");
        //     newCategory.Save();
        //     int targetId = newCategory.GetId();
        //
        //     Task firstTask = new Task("Wash Dishes", newCategory.GetId(), "1999-01-01");
        //     firstTask.Save();
        //     Task secondTask = new Task("Empty Dishwasher", newCategory.GetId(), "2000-01-01");
        //     secondTask.Save();
        //
        //     //Act
        //     newCategory.Delete();
        //
        //     //Assert
        //     Assert.Equal(0, Category.GetAll().Count);
        // }
        //
        //
        // [Fact]
        // public void Test_TaskDatabaseFirstEmpty()
        // {
        //
        //     //Arrange
        //     int result = Task.GetAll().Count;
        //
        //     //Assert
        //     Assert.Equal(0, result);
        //
        // }
        //
        // [Fact]
        // public void Test_Save_SaveTaskToDatabase()
        // {
        //     // Arrange
        //         Task newTask = new Task("Lawn Chores", 0, "2000-01-01");
        //
        //     // Act
        //         newTask.Save();
        //         List<Task> allTasks = Task.GetAll();
        //         List<Task> testList = new List<Task>{newTask};
        //
        //     // Assert
        //         Assert.Equal(testList, allTasks);
        //
        // }
        //
        // [Fact]
        // public void Test_Save_AssignsIdToTask()
        // {
        //     // Arrange
        //     Task newTask = new Task("Chores", 0, "2000-01-01");
        //
        //     // Act
        //     newTask.Save();
        //     Task savedTask = Task.GetAll()[0];
        //
        //     int resultId = savedTask.GetId();
        //     int testId = newTask.GetId();
        //
        //     // Assert
        //     Assert.Equal(testId, resultId);
        // }
        //
        // [Fact]
        // public void Test_Find_FindTaskInDatabase()
        // {
        //     //Arrange
        //     Task newTask = new Task("Chores", 0, "2000-01-01");
        //     newTask.Save();
        //
        //     //Act
        //     Task foundTask = Task.Find(newTask.GetId());
        //
        //     //Assert
        //     Assert.Equal(newTask, foundTask);
        // }
        //
        // [Fact]
        // public void Test_Delete_RemoveTaskFromDatabase()
        // {
        //     // Arrange
        //
        //     Task firstTask = new Task("Wash Dishes", 1, "1999-01-01");
        //     firstTask.Save();
        //     Task secondTask = new Task("Empty Dishwasher", 1, "2000-01-01");
        //     secondTask.Save();
        //
        //
        //     // Act
        //     List<Task> testTaskList = new List<Task> {secondTask};
        //     firstTask.Delete();
        //     List<Task> resultTaskList = Task.GetAll();
        //
        //     //Assert
        //     Assert.Equal(testTaskList, resultTaskList);
        //
        // }
        //

        public void Dispose()
        {
            Task.DeleteAll();
            Category.DeleteAll();
        }
    }
}
