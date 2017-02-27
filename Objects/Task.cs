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

        public void Delete()
        {
            SqlConnection connection = DB.Connection();
            connection.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM tasks WHERE id=@TaskId", connection);

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
