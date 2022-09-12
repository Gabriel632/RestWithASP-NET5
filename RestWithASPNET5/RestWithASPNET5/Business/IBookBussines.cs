using RestWithASPNET5.Data.VO;
using RestWithASPNET5.Hypermedia.Utils;
using System.Collections.Generic;

namespace RestWithASPNET5.Business
{
    public interface IBookBussines
    {
        BookVO Create(BookVO book);
        BookVO FindByID(int id);
        List<BookVO> FindAll();
        PagedSearchVO<BookVO> FindWithPagedSearch(string title, string sortDirection, int pageSize, int page);
        BookVO Update(BookVO book);
        void Delete(int id);
    }
}
