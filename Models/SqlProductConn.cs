using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using POC;


/*namespace SQLPRODUCTCONN.Models
{
    public class SqlProductConn : DbContext
    {
        public string Conn;
        public SqlProductConn(IConfiguration configuration)
        {
            Conn = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }
        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            SqlConnection sqlConnection = new SqlConnection(Conn);
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM product");
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        Product_noa = reader.GetString(reader.GetOrdinal("noa")),
                        Product_product = reader.GetString(reader.GetOrdinal("product")),
                        Product_price = reader.GetValue(reader.GetOrdinal("price")),
                    };
                    products.Add(product);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！");
            }
            sqlConnection.Close();
            return products;
        }
        
    }
    public class Product
    {
        public string Product_noa { get; set; }
        public string Product_product { get; set; }
        public object Product_price { get; set; }
    }
}
*/