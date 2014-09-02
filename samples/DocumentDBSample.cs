using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents;  

namespace DocumentDbSample
{
    class Program
    {
        private static string URI = "<your endpoint URI>";
        private static string Key = "<your key>";
        private static DocumentClient client = new DocumentClient(new Uri(URI), Key);
        static void Main(string[] args)
        {
            // create or read database 
            Database database = CreateOrReadDatabase("EmployeesDatabase").Result;
            // create or read collection
            DocumentCollection collection = CreateOrReadCollection(database,"EmployeeList").Result;
            
            // create two employees objects
            Employee Mohamed = new Employee() { Name = "Mohamed Naguib", Email = "mohamed.naguib.92@hotamil.com", Address = "Cairo, Egypt"};
            Employee Allen = new Employee() { Name = "Allen Roger", Email = "allen.roger@awsc.com", Address = "London, England"};

            // insert documents into the collection
            client.CreateDocumentAsync(collection.SelfLink, Mohamed);
            client.CreateDocumentAsync(collection.SelfLink, Allen);

            // listing all documents in the collection
            foreach (Employee employee in client.CreateDocumentQuery(collection.SelfLink, "select * from EmployeeList"))
            {
                Console.WriteLine("Employee Name: " + employee.Name + ", Address: " + employee.Address + " , Email: " + employee.Email);
            }

            
        }
        public static async Task<Database> CreateOrReadDatabase(string databaseName)
        {
            if (client.CreateDatabaseQuery().Where(x => x.Id == databaseName).AsEnumerable().Any())
            {
                return client.CreateDatabaseQuery().Where(x => x.Id == databaseName).AsEnumerable().FirstOrDefault();
            }
            return await client.CreateDatabaseAsync(new Database { Id = databaseName });
        }
        public static async Task<DocumentCollection> CreateOrReadCollection(Database database,string collectionName)
        {
            if (client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id == collectionName).ToArray().Any())
            {
                return client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id == collectionName).ToArray().FirstOrDefault();
            }
            return await client.CreateDocumentCollectionAsync(database.SelfLink, new DocumentCollection { Id = collectionName });
        }
    }
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
