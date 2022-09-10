using RestWithASPNET5.Data.VO;
using RestWithASPNET5.Hypermedia.Utils;
using System.Collections.Generic;

namespace RestWithASPNET5.Business
{
    public interface IPersonBussines
    {
        PersonVO Create(PersonVO person);
        PersonVO FindByID(long id);
        List<PersonVO> FindByName(string firstName, string lastName);
        List<PersonVO> FindAll();
        PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
        PersonVO Update(PersonVO person);
        PersonVO Disable(long id);
        void Delete(long id);
    }
}
