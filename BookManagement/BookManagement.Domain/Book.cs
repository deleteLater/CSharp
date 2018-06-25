namespace BookManagement.Domain
{
    public class Book
    {
        /// <summary>
        /// Gets or sets the book identifier.
        /// </summary>
        /// <value>Book id.</value>
        public int Id { set; get; }

        /// <summary>
        /// Gets or sets the book title.
        /// </summary>
        /// <value>Book title.</value>
        public string Title { set; get; }

        /// <summary>
        /// Gets or sets the book author.
        /// </summary>
        /// <value>Book author.</value>
        public string Author { set; get; }

        /// <summary>
        /// Gets or sets the book press.
        /// </summary>
        /// <value>Book press.</value>
        public string Press { set; get; }

        /// <summary>
        /// ISBN : International Standard Book Number,13-digit
        /// Gets or sets the ISBN.
        /// </summary>
        /// <value>book ISBN.</value>
        public string ISBN { set; get; }

        /// <summary>
        /// Gets or sets the book price.
        /// </summary>
        /// <value>Book price.</value>
        public double Price { set; get; }
    }
}