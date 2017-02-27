using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ToDoList.Objects //note namespace .Objects
{
    public class Task
    {
        private int _id;
        private string _name;
        private string _date;
        private bool _isComplete;

        public Task(string newTask, string newDate, int newId = 0, bool isComplete = false)
        {
            _id = newId;
            _name = newTask;
            _date = newDate;
            _isComplete = isComplete;
        }

        public override bool Equals(System.Object otherTask)
        {
            if (!(otherTask is Task))
            {
                return false;
            }
            else
            {
                Task newTask = (Task) otherTask;
                bool nameEquality = this.GetName() == newTask.GetName();
                bool idEquality = this.GetId() == newTask.GetId();
                bool dateEquality= this.GetDate() == newTask.GetDate();
                bool completeStatusEquality = this.GetIsComplete() == newTask.GetIsComplete();
                return (nameEquality && idEquality && dateEquality && completeStatusEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetId().GetHashCode();
        }

        public static List<Task> GetAll()
        {
            List<Task> allTasks = new List<Task>{};

            SqlConnection connection = DB.Connection();
            connection.Open();

            SqlCommand cmd = new SqlCommand("SELECT * From tasks;", connection);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string newTaskName = rdr.GetString(1);
                string newDate = rdr.GetString(2);
                bool status = rdr.GetBoolean(3);
                Task newTask = new Task(newTaskName, newDate, id, status);
                allTasks.Add(newTask);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (connection != null)
            {
                connection.Close();
            }
            return allTasks;
        }


        public void Save()
        {
            SqlConnection connection = DB.Connection();
            connection.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO tasks (name, date) OUTPUT INSERTED.id VALUES (@TaskName, @Date);", connection);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@TaskName";
            nameParameter.Value = this.GetName();

            SqlParameter dateParameter = new SqlParameter();
            dateParameter.ParameterName = "@Date";
            dateParameter.Value = this.GetDate();

            cmd.Parameters.Add(nameParameter);
            cmd.Parameters.Add(dateParameter);


            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (connection != null)
            {
                connection.Close();
            }
        }

        public static Task Find(int id)
        {
            //Connection
            SqlConnection connection = DB.Connection();
            connection.Open();

            //Command
            SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE id =@TaskId;", connection);

            //Parameter
            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@TaskId";
            idParameter.Value = id.ToString();
            cmd.Parameters.Add(idParameter);

            //Reader
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundTaskId = 0;
            string foundTaskName = null;
            string foundTaskDate = null;
            while(rdr.Read())
            {
                foundTaskId = rdr.GetInt32(0);
                foundTaskName = rdr.GetString(1);
                foundTaskDate = rdr.GetString(2);
            }
            Task foundTask = new Task(foundTaskName, foundTaskDate, foundTaskId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (connection != null)
            {
                connection.Close();
            }
            return foundTask;
        }

        public void AddCategory(Category newCategory)
        {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("INSERT INTO categories_tasks (category_id, task_id) VALUES (@CategoryId, @TaskId);", conn);

          SqlParameter categoryIdParameter = new SqlParameter();
          categoryIdParameter.ParameterName = "@CategoryId";
          categoryIdParameter.Value = newCategory.GetId();
          cmd.Parameters.Add(categoryIdParameter);

          SqlParameter taskIdParameter = new SqlParameter();
          taskIdParameter.ParameterName = "@TaskId";
          taskIdParameter.Value = this.GetId();
          cmd.Parameters.Add(taskIdParameter);

          cmd.ExecuteNonQuery();

          if (conn != null)
          {
            conn.Close();
          }
        }

        public List<Category> GetCategories()
        {
            List<Category> thisTaskCategories = new List<Category> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT categories.* FROM categories JOIN categories_tasks ON (categories.id = categories_tasks.category_id) JOIN tasks ON (categories_tasks.task_id = tasks.id) WHERE tasks.id = @TaskId;", conn);
            cmd.Parameters.Add(new SqlParameter("@TaskId", this.GetId()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int newId = rdr.GetInt32(0);
                string newDescription = rdr.GetString(1);
                Category newCategory = new Category(newDescription, newId);
                thisTaskCategories.Add(newCategory);
            }

            DB.CloseSqlConnection(rdr, conn);
            return thisTaskCategories;
        }

        public void Delete()
        {
            SqlConnection connection = DB.Connection();
            connection.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM tasks WHERE id=@TaskId; DELETE FROM categories_tasks WHERE task_id=@TaskId;", connection);

            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@TaskId";
            idParameter.Value = this.GetId();
            cmd.Parameters.Add(idParameter);
            cmd.ExecuteNonQuery();

            if (connection != null)
            {
                connection.Close();
            }
        }

        public void ToggleComplete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE tasks SET is_complete=@CompleteValue OUTPUT INSERTED.is_complete WHERE id=@TaskId;", conn);
            cmd.Parameters.Add(new SqlParameter("@TaskId", this.GetId()));
            cmd.Parameters.Add(new SqlParameter("@CompleteValue", (this.GetIsComplete() != true)));
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this.SetIsComplete(rdr.GetBoolean(0));
            }

            DB.CloseSqlConnection(rdr, conn);
        }

        public static List<Task> GetComplete(bool status)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE is_complete=@CompleteStatus;", conn);
            cmd.Parameters.Add(new SqlParameter("@CompleteStatus", status));
            SqlDataReader rdr = cmd.ExecuteReader();
            List<Task> allTasks = new List<Task>{};
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string newTaskName = rdr.GetString(1);
                string newDate = rdr.GetString(2);
                bool completeStatus = rdr.GetBoolean(3);
                Task newTask = new Task(newTaskName, newDate, id, completeStatus);
                allTasks.Add(newTask);
            }
            DB.CloseSqlConnection(rdr, conn);
            return allTasks;
        }

        public static void DeleteAll()
        {
            SqlConnection connection = DB.Connection();
            connection.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM tasks;", connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public string GetName()
        {
            return _name;
        }
        public void SetName(string newName)
        {
            _name = newName;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetDate()
        {
            return _date;
        }
        public void SetDate(string newDate)
        {
            _date = newDate;
        }

        public bool GetIsComplete()
        {
            return _isComplete;
        }
        public void SetIsComplete(bool status)
        {
            _isComplete = status;
        }
    }
}
