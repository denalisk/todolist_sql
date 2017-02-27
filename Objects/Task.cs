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

        public Task(string newTask, string newDate, int newId = 0)
        {
            _id = newId;
            _name = newTask;
            _date = newDate;
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
                return (nameEquality && idEquality && dateEquality);
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
                Task newTask = new Task(newTaskName, newDate, id);
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
            List<int> categoryIds = new List<int> {};
            List<Category> thisTaskCategories = new List<Category> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT category_id FROM categories_tasks WHERE task_id = @TaskId;", conn);
            cmd.Parameters.Add(new SqlParameter("@TaskId", this.GetId()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int foundId = rdr.GetInt32(0);
                categoryIds.Add(foundId);
            }

            if (rdr != null)
            {
                rdr.Close();
            }

            foreach(int entry in categoryIds)
            {
                SqlCommand cmd2 = new SqlCommand("SELECT * FROM categories WHERE id=@CategoryId;", conn);
                cmd2.Parameters.Add(new SqlParameter("@CategoryId", entry));

                SqlDataReader rdr2 = cmd2.ExecuteReader();

                while (rdr2.Read())
                {
                    int newId = rdr2.GetInt32(0);
                    string newDescription = rdr2.GetString(1);
                    Category newCategory = new Category(newDescription, newId);
                    thisTaskCategories.Add(newCategory);
                }

                if (rdr2 != null)
                {
                    rdr2.Close();
                }
            }

            if (conn != null)
            {
                conn.Close();
            }
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
    }
}
