using System.Collections.Generic;
using System.Data.SqlClient;
using System;


namespace ToDoList.Objects
{
    public class Category
    {

        private int _id;
        private string _description;

        public Category(string categoryDescription, int newId = 0)
        {
            _id = newId;
            _description = categoryDescription;
        }

        public override bool Equals(System.Object otherCategory)
        {
            if (!(otherCategory is Category))
            {
                return false;
            }
            else
            {
                Category newCategory = (Category) otherCategory;
                bool descritionEquality = this.GetDescription() == newCategory.GetDescription();
                bool idEquality = this.GetId() == newCategory.GetId();
                return (descritionEquality && idEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetId().GetHashCode();
        }

        public string GetDescription()
        {
            return _description;
        }
        public int GetId()
        {
            return _id;
        }

        public static List<Category> GetAll()
        {
            List<Category> allCategories = new List<Category>{};

            SqlConnection connection = DB.Connection();
            connection.Open();

            SqlCommand cmd = new SqlCommand("SELECT * From categories;", connection);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string categoryDescription = rdr.GetString(1);
                Category newCategory = new Category(categoryDescription, id);
                allCategories.Add(newCategory);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (connection != null)
            {
                connection.Close();
            }
            return allCategories;
        }


        public void Save()
        {
            SqlConnection connection = DB.Connection();
            connection.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO categories (description) OUTPUT INSERTED.id VALUES (@CategoryDescription);", connection);

            SqlParameter descriptionParameter = new SqlParameter();
            descriptionParameter.ParameterName = "@CategoryDescription";
            descriptionParameter.Value = this.GetDescription();
            cmd.Parameters.Add(descriptionParameter);
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

        public static Category Find(int id)
        {
            //Connection
            SqlConnection connection = DB.Connection();
            connection.Open();

            //Command
            SqlCommand cmd = new SqlCommand("SELECT * FROM categories WHERE id=@CategoryId;", connection);

            //Parameter
            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@CategoryId";
            idParameter.Value = id.ToString();
            cmd.Parameters.Add(idParameter);

            //Reader
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundCategoryId = 0;
            string foundCategoryDescription = null;
            while(rdr.Read())
            {
                foundCategoryId = rdr.GetInt32(0);
                foundCategoryDescription = rdr.GetString(1);
            }
            Category foundCategory = new Category(foundCategoryDescription, foundCategoryId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (connection != null)
            {
                connection.Close();
            }
            return foundCategory;
        }

        // public List<Task> GetTasks()
        // {
        //     SqlConnection conn = DB.Connection();
        //     conn.Open();
        //
        //     SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE category_id = @CategoryId ORDER BY date;", conn);
        //     SqlParameter categoryIdParameter = new SqlParameter();
        //     categoryIdParameter.ParameterName = "@CategoryId";
        //     categoryIdParameter.Value = this.GetId();
        //     cmd.Parameters.Add(categoryIdParameter);
        //     SqlDataReader rdr = cmd.ExecuteReader();
        //     List<Task> tasks = new List<Task> {};
        //     while (rdr.Read())
        //     {
        //         int taskId = rdr.GetInt32(0);
        //         string taskDescription = rdr.GetString(1);
        //         int taskCategoryId = rdr.GetInt32(2);
        //         string newDate = rdr.GetString(3);
        //         Task newTask = new Task(taskDescription, newDate, taskId);
        //         tasks.Add(newTask);
        //     }
        //     if (rdr != null)
        //     {
        //         rdr.Close();
        //     }
        //     if (conn != null)
        //     {
        //         conn.Close();
        //     }
        //     return tasks;
        // }

        public void Delete()
        {
            SqlConnection connection = DB.Connection();
            connection.Open();

            // List<Task> targetTasks = this.GetTasks();
            // foreach(Task entry in targetTasks)
            // {
            //     entry.Delete();
            // }
            SqlCommand cmd = new SqlCommand("DELETE FROM categories WHERE id=@CategoryId;", connection);

            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@CategoryId";
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

            SqlCommand cmd = new SqlCommand("DELETE FROM categories;", connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
