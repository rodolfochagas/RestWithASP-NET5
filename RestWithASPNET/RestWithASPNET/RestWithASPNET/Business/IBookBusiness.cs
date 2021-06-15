using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Utils;
using System.Collections.Generic;

namespace RestWithASPNET.Business.Implementations
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO book);
        BookVO FindById(long id);
        List<BookVO> FindAll();
        PagedSearchVO<BookVO> FindWithPagedSearch(string title, string sortDirection, int pageSize, int page);
        BookVO Update(BookVO book);
        void Delete(long id);
    }
}
