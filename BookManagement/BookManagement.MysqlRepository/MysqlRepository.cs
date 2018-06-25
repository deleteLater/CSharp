using BookManagement.Generic;
using BookManagement.Domain;
using System.Configuration;
using Simple.Data;
using System.Collections.Generic;
using System.Linq;

namespace BookManagement.MysqlRepository
{
    public class BookRepository : IRepository
    {
        private readonly string _connectionString
                        = ConfigurationManager.ConnectionStrings["MysqlDb"].ConnectionString;
        //dataset.table.operation
        protected dynamic Db
        {
            get
            {
                return Database.OpenConnection(_connectionString);
            }
        }

        public void Add(Book book)
        {
            Db.Book.Insert(book);
        }

        public IList<Book> FindAll()
        {
            IEnumerable<Book> books = Db.Book.All();
            return books.ToList();
        }

        public Book FindById(int id)
        {
            var book = Db.Book.Get(id);
            return book;
        }

        public void Modify(Book book)
        {
            Db.Book.Update(book);
        }

        public void Remove(int id)
        {
            Db.Book.DeleteById(id);
        }
    }
}
