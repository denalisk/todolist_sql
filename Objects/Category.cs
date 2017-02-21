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
            SqlCommand cmd = new SqlCommand("SELECT * FROM categories WHERE id =@CategoryId;", connection);

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
