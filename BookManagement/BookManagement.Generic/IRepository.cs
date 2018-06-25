using BookManagement.Domain;
using System.Collections.Generic;

namespace BookManagement.Generic
{
    public interface IRepository
    {
        /// <summary>
        /// Add the specified book.
        /// </summary>
        /// <param name="book">Book.</param>
        void Add(Book book);

        /// <summary>
        /// Remove the specified book.
        /// </summary>
        /// <param name="id">BookId.</param>
        void Remove(int id);

        /// <summary>
        /// Modify the specified book.
        /// </summary>
        /// <param name="book">Book.</param>
        void Modify(Book book);

        /// <summary>
        /// Finds all book.
        /// </summary>
        /// <returns>All books.</returns>
        IList<Book> FindAll();

        /// <summary>
        /// Finds book by identifier.
        /// </summary>
        /// <returns>Book eneity.</returns>
        Book FindById(int id);
    }
}
