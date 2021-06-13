using RestWithASPNET.Business.Implementations;
using RestWithASPNET.Data.Converter.Implementation;
using RestWithASPNET.Data.VO;
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

        public BookVO Update(BookVO bookVO)
        {
            var book = _converter.Parse(bookVO);
            book = _repository.Update(book);
            return _converter.Parse(book);
        }
    }
}
