using RestWithASPNET.Data.Converter.Contract;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNET.Data.Converter.Implementation
{
    public class BookConverter : IParser<BookVO, Book>, IParser<Book, BookVO>
    {
        public BookVO Parse(Book origin)
        {
            if (origin == null) return null;

            return new BookVO
            {
                Id = origin.Id,
                LaunchDate = origin.LaunchDate,
                Author = origin.Author,
                Price = origin.Price,
                Title = origin.Title
            };
        }

        public Book Parse(BookVO origin)
        {

            if (origin == null) return null;
            return new Book
            {
                Id = origin.Id,
                LaunchDate = origin.LaunchDate,
                Author = origin.Author,
                Price = origin.Price,
                Title = origin.Title
            };
        }

        public List<BookVO> Parse(List<Book> origins)
        {
            if (origins == null) return null;
            return origins.Select(origin => Parse(origin)).ToList();
        }

        public List<Book> Parse(List<BookVO> origins)
        {
            if (origins == null) return null;
            return origins.Select(origin => Parse(origin)).ToList();
        }
    }
}
