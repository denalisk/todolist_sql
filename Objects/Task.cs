using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ToDoList.Objects //note namespace .Objects
{
    public class Task
    {
        private int _id;
        private string _name;
        private int _category_id;
        private string _date;

        public Task(string newTask, int newCategoryId, string newDate = "2000-01-01", int newId = 0)
        {
            _id = newId;
            _name = newTask;
            _category_id = newCategoryId;
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
                bool categoryIdEquality = this.GetCategoryId() == newTask.GetCategoryId();
                return (nameEquality && idEquality && categoryIdEquality);
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
                int newCategoryId = rdr.GetInt32(2);
                string newDate = rdr.GetString(3);
                Task newTask = new Task(newTaskName, newCategoryId, newDate, id);
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

            SqlCommand cmd = new SqlCommand("INSERT INTO tasks (name, category_id) OUTPUT INSERTED.id VALUES (@TaskName, @CategoryId);", connection);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@TaskName";
            nameParameter.Value = this.GetName();

            SqlParameter categoryIdParameter = new SqlParameter();
            categoryIdParameter.ParameterName = "@CategoryId";
            categoryIdParameter.Value = this.GetCategoryId();

            cmd.Parameters.Add(categoryIdParameter);
            cmd.Parameters.Add(nameParameter);

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
            int foundTaskCategoryId = 0;
            while(rdr.Read())
            {
                foundTaskId = rdr.GetInt32(0);
                foundTaskName = rdr.GetString(1);
                foundTaskCategoryId = rdr.GetInt32(2);
                foundTaskDate = rdr.GetString(3);
            }
            Task foundTask = new Task(foundTaskName, foundTaskCategoryId, foundTaskDate, foundTaskId);

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

        public int GetCategoryId()
        {
            return _category_id;
        }
        public void SetCategoryId(int newCategoryId)
        {
            _category_id = newCategoryId;
        }
    }
}
