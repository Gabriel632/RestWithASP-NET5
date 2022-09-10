using RestWithASPNET5.Model;
using RestWithASPNET5.Repository.Generic;
using System.Collections.Generic;

namespace RestWithASPNET5.Repository
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person Disable(long id);
        List<Person> FindByName(string firstName, string lastName);
    }
}
