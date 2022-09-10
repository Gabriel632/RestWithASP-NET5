using RestWithASPNET5.Data.VO;
using System.Collections.Generic;

namespace RestWithASPNET5.Business
{
    public interface IBookBussines
    {
        BookVO Create(BookVO book);
        BookVO FindByID(int id);
        List<BookVO> FindAll();
        BookVO Update(BookVO book);
        void Delete(int id);
    }
}
