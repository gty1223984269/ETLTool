using Dapper;
using ETLTool.DataModel;
using ETLTool.TableModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ETLTool
{
    public class Repository
    {
        private string connectionString;
        public Repository()
        {
            connectionString = @"User ID=sa;Password=sa123;Server=CNSHHQ-L0011;Database=eldb;Pooling=true;";
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public void Add (List<WordRoot> list )
        {

            int id = 0;
            string sql1 = "";
            foreach (var model in list)
            {

                sql1 = @"INSERT INTO [dbo].[word_roots]
           ([created_date_time_utc]
           ,[created_by]
           ,[updated_date_time_utc]
           ,[updated_by]
           ,[is_active]
           ,[word]
           ,[chinese_meaning])
     VALUES(@created_date_time_utc,@created_by,@updated_date_time_utc,@updated_by,@is_active,@word,@chinese_meaning) SELECT CAST(SCOPE_IDENTITY() as int)";

                wordRoots wordRoots = new wordRoots();
                wordRoots.created_date_time_utc = DateTime.Now;
                wordRoots.created_by = "admin";
                wordRoots.updated_date_time_utc = DateTime.Now;
                wordRoots.updated_by = "admin";
                wordRoots.is_active =1;
                wordRoots.word = model.wordRoot;
                wordRoots.chinese_meaning = model.wordRootMeaning;

                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();

                     id = dbConnection.Query<int>(sql1, wordRoots).Single();
                }

                foreach (var model2 in model.relatedWord)
                {

                    string sql2 = @"INSERT INTO[dbo].[related_words]
        ([created_date_time_utc]
          ,[created_by]
          ,[updated_date_time_utc]
          ,[updated_by]
          ,[is_active]
          ,[root_id]
          ,[word]
          ,[chinese_meaning]
          ,[remember_logic])
   VALUES('" + DateTime.Now + "','admin','" + DateTime.Now + "','admin',1," + id + ",'" + model2.relatedWord + "','"+model2.releateWordMeaning+"','')";


                    using (IDbConnection dbConnection = Connection)
                    {
                        dbConnection.Open();
                        dbConnection.Execute(sql2);
                    }

                }




            }

               
          

          
        }

        public IEnumerable<object> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<object>("SELECT * FROM Products");
            }
        }

        public object GetByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Products"
                               + " WHERE ProductId = @Id";
                dbConnection.Open();
                return dbConnection.Query<object>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "DELETE FROM Products"
                             + " WHERE ProductId = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }

        public void Update(object model)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "UPDATE Products SET Name = @Name,"
                               + " Quantity = @Quantity, Price= @Price"
                               + " WHERE ProductId = @ProductId";
                dbConnection.Open();
                dbConnection.Query(sQuery, model);
            }
        }
    }
}
