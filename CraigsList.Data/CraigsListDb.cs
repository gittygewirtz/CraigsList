using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CraigsList.Data
{
    public class CraigsListDb
    {
        private string _conStr;
        public CraigsListDb(string conStr)
        {
            _conStr = conStr;
        }

        public List<Post> GetAllPosts()
        {
            using (var conn = new SqlConnection(_conStr))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM POSTS";
                var result = new List<Post>();
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Post
                    {
                        Id = (int)reader["Id"],
                        UserId = (int)reader["UserId"],
                        Title = (string)reader["Title"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        Text = (string)reader["Text"]
                    });
                }
                return result;
            }
        }

        public void AddUser(User user, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            using (var connection = new SqlConnection(_conStr))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Users (Name, Email, PasswordHash) " +
                                  "VALUES (@name, @email, @hash) SELECT SCOPE_IDENTITY()";
                cmd.Parameters.AddWithValue("@name", user.Name);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@hash", hash);
                connection.Open();
                user.Id = (int)(decimal)cmd.ExecuteScalar();
            }
        }

        public List<Post> GetAllPosts(int userId)
        {
            using (var conn = new SqlConnection(_conStr))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM POSTS
                                    WHERE UserId = @userId";
                cmd.Parameters.AddWithValue("@userId", userId);
                var result = new List<Post>();
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Post
                    {
                        Id = (int)reader["Id"],
                        UserId = userId,
                        Title = (string)reader["Title"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        Text = (string)reader["Text"]
                    });
                }
                return result;
            }
        }

        public void DeletePost(int id)
        {
            using (var conn = new SqlConnection(_conStr))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM Posts
                                    WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddNewPost(Post p)
        {
            using (var conn = new SqlConnection(_conStr))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO Posts (UserId, Title, PhoneNumber, Text)
                                    VALUES (@userId, @title, @phoneNumber, @text)";
                cmd.Parameters.AddWithValue("@userId", p.UserId);
                cmd.Parameters.AddWithValue("@title", p.Title);
                cmd.Parameters.AddWithValue("@phoneNumber", p.PhoneNumber);
                cmd.Parameters.AddWithValue("@text", p.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public User Login(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (isValidPassword)
            {
                return user;
            }

            return null;
        }

        public User GetUserByEmail(string email)
        {
            using (var conn = new SqlConnection(_conStr))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Users
                                    WHERE Email = @email";
                cmd.Parameters.AddWithValue("@email", email);
                var result = new User();
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Id = (int)reader["Id"];
                    result.Name = (string)reader["Name"];
                    result.Email = email;
                    result.PasswordHash = (string)reader["PasswordHash"];
                };
                return result;
            }
        }
        public int GetUserIdByEmail(string email)
        {
            using (var conn = new SqlConnection(_conStr))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT Id FROM Users
                                    WHERE Email = @email";
                cmd.Parameters.AddWithValue("@email", email);
                var result = new User();
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

    }
}

