using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;

// Code-behind logic for the Products Razor page
public class ProductsModel : PageModel
{
    // Holds a list of Product objects that we will show on the page
    public List<Product> Products { get; set; }

    // Runs when a user opens the Products page (HTTP GET request)
    public void OnGet()
    {
        Products = new List<Product>();

        // Connection string required to connect to SQL Server database
        string connectionString = "Server=localhost;Database=Northwind;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=True;";
       
        // Creates a connection to SQL Server
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open(); // Opens the database connection

            // SQL query: gets product name, category name, and price
            string sql = @"SELECT p.ProductName, c.CategoryName, p.UnitPrice
                           FROM Products p
                           JOIN Categories c ON p.CategoryID = c.CategoryID";

            // Sends the SQL command to SQL Server
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                // Reads the results
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Loop through each row in the database result
                    while (reader.Read())
                    {
                        // Add a new Product object to the list
                        Products.Add(new Product
                        {
                            ProductName = reader.GetString(0), // first column
                            CategoryName = reader.GetString(1), // second column
                            UnitPrice = reader.GetDecimal(2)   // third column
                        });
                    }
                }
            }
        }
    }
}

// Product class represents a single product item
public class Product
{
    public string ProductName { get; set; }  // product name
    public string CategoryName { get; set; } // product category
    public decimal UnitPrice { get; set; }   // price of the product
}
