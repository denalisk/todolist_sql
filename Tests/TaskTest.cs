using Xunit;
using ToDoList.Objects;
using System;
using System.Collections.Generic;

namespace ToDoList
{
  public class TaskTest : IDisposable
  {
    [Fact]
    public void Test1_GetDescription_ReturnsDescription()
    {
      //Arrange
      string description01 = "walk the dog";
       Task checkIf = new Task(description01);

      //Act
      string result = checkIf.GetDescription();

      //Assert
      Assert.Equal(description01, result);
    }
    [Fact]
    public void Test2_GetAll_ReturnsAllTasks()
    {
      //Arrange
      string description01 = "walk the dog";
      string description02 = "wash dishes";
      Task taskInstance01 = new Task(description01);
      Task taskInstance02 = new Task(description02);
      List<Task> checkIfTaskList = new List<Task> {taskInstance01, taskInstance02};

      //Act
      List<Task> result = Task.GetAll();

      foreach (Task currently in result)
      {
        Console.WriteLine("Output: " + currently.GetDescription());
      }
      //Assert
      Assert.Equal(checkIfTaskList, result);
    }
    public void Dispose()
    {
      Task.DeleteAll();
    }
  }
}
