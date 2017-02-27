using Nancy;
using ToDoList.Objects;
using System.Collections.Generic;

namespace ToDoList
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      // Get["/"] = _ => {
      //   return View["index.cshtml"];
      // };
      //
      // Get["/categories"] = _ => {
      //   var allCategories = Category.GetAll();
      //   return View["categories.cshtml", allCategories];
      // };
      // Post["/categories"] = _ => {
      //   var newCategory = new Category(Request.Form["category-name"]);
      //   newCategory.Save();
      //   var allCategories = Category.GetAll();
      //   return View["categories.cshtml", allCategories];
      // };
      //
      // Get["/categories/new"] = _ => {
      //   return View["category_form.cshtml"];
      // };
      //
      // Get["/categories/{id}"] = parameters => {
      //   Dictionary<string, object> model = new Dictionary<string, object>();
      //   var selectedCategory = Category.Find(parameters.id);
      //   var categoryTasks = selectedCategory.GetTasks();
      //   model.Add("category", selectedCategory);
      //   model.Add("tasks", categoryTasks);
      //   return View["category.cshtml", model];
      // };
      //
      // Get["/categories/{id}/tasks/new"] = parameters => {
      //   Dictionary<string, object> model = new Dictionary<string, object>();
      //   Category selectedCategory = Category.Find(parameters.id);
      //   List<Task> allTasks = selectedCategory.GetTasks();
      //   model.Add("category", selectedCategory);
      //   model.Add("tasks", allTasks);
      //   return View["category_tasks_form.cshtml", model];
      // };
      //
      // Post["/delete/task/{categoryId}/{taskId}"] = parameters => {
      //     Task targetTask = Task.Find(parameters.taskId);
      //     targetTask.Delete();
      //     Dictionary<string, object> model = new Dictionary<string, object>();
      //     var selectedCategory = Category.Find(parameters.categoryId);
      //     var categoryTasks = selectedCategory.GetTasks();
      //     model.Add("category", selectedCategory);
      //     model.Add("tasks", categoryTasks);
      //     return View["category.cshtml", model];
      // };
      //
      // Post["/delete/category/{categoryId}"] = parameters => {
      //     Category targetCategory = Category.Find(parameters.categoryId);
      //     targetCategory.Delete();
      //     List<Category> allCategories = Category.GetAll();
      //     return View["categories.cshtml", allCategories];
      // };
      //
      // Post["/tasks"] = _ => {
      //   Dictionary<string, object> model = new Dictionary<string, object>();
      //   Category selectedCategory = Category.Find(Request.Form["category-id"]);
      //   string taskDescription = Request.Form["task-description"];
      //   int newCategoryId = int.Parse(Request.Form["category-id"]);
      //   string newDate = Request.Form["date"];
      //   Task newTask = new Task(taskDescription, newCategoryId, newDate);
      //   newTask.Save();
      //   List<Task> categoryTasks = selectedCategory.GetTasks();
      //   model.Add("tasks", categoryTasks);
      //   model.Add("category", selectedCategory);
      //   return  View["category.cshtml", model];
      // };
    }
  }
}
