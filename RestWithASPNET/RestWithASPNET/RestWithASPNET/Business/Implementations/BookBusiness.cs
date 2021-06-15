using RestWithASPNET.Business.Implementations;
using RestWithASPNET.Data.Converter.Implementation;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Utils;
using RestWithASPNET.Model;
using RestWithASPNET.Repository.Implementations;
using System.Collections.Generic;

namespace RestWithASPNET.Business
{
    public class BookBusiness : IBookBusiness
    {
        private IRepository<Book> _repository;
        private BookConverter _converter ;
        public BookBusiness(IRepository<Book> repository)
        {
            _repository = repository;
            _converter = new BookConverter();
        }

        public BookVO Create(BookVO bookVO)
        {
            var book = _converter.Parse(bookVO);
            book = _repository.Create(book);
            return _converter.Parse(book);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);        }

        public List<BookVO> FindAll()
        {
            var books = _repository.FindAll();
            return _converter.Parse(books);
        }

        public BookVO FindById(long id)
        {
            var book = _repository.FindById(id);
            return _converter.Parse(book);
        }

        public PagedSearchVO<BookVO> FindWithPagedSearch(string title, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrWhiteSpace(sortDirection)) && !sortDirection.Equals("desc") ? "asc" : "desc";
            var size = (pageSize < 1) ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            var query = @"select * from books b where 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(title)) query += $"and b.title like '%{title}%' ";
            query += $"order by b.title {sort} limit {size} offset {offset}";

            var countQuery = @"select count(*) from books b where 1 = 1 ";
            if (string.IsNullOrWhiteSpace(countQuery)) countQuery += $"and b.title like '%{title}%'";

            var books = _repository.FindWithPagedSearch(query);
            var totalResults = _repository.GetCount(countQuery);

            return new PagedSearchVO<BookVO>
            {
                CurrentPage = page,
                List = _converter.Parse(books),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }

        public BookVO Update(BookVO bookVO)
        {
            var book = _converter.Parse(bookVO);
            book = _repository.Update(book);
            return _converter.Parse(book);
        }
    }
}
