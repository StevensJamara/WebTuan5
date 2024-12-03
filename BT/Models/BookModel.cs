namespace BT.Models
{
    //Create model books
    public class BookModel
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public DateTime PublishedDate { get; set; }
    }

}
