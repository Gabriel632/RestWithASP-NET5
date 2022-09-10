using RestWithASPNET5.Data.Converter.Implementations;
using RestWithASPNET5.Data.VO;
using RestWithASPNET5.Hypermedia.Utils;
using RestWithASPNET5.Repository;
using System.Collections.Generic;
using System.Text;

namespace RestWithASPNET5.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBussines
    {
        private readonly IPersonRepository _repository;
        private readonly PersonConverter _converter;

        public PersonBusinessImplementation(IPersonRepository repository)
        {
            _repository = repository;
            _converter = new PersonConverter();
        }

        public PersonVO Create(PersonVO person)
        {
            var personEntity = _converter.Parse(person);
            personEntity = _repository.Create(personEntity);
            return _converter.Parse(personEntity);
        }

        public PersonVO Disable(long id)
        {
            var personEntity = _repository.Disable(id);
            return _converter.Parse(personEntity);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }

        public List<PersonVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        public PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var sort = !string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("desc")
                ? "asc" : "desc";
            var size = pageSize < 1 ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            var query = new StringBuilder();
            query.AppendLine("SELECT * FROM person p WHERE 1=1 ");
            if (!string.IsNullOrWhiteSpace(name))
                query.AppendLine($"AND p.first_name LIKE '%{name}%' ");            
            query.AppendLine($"ORDER BY p.first_name {sort} limit {size} offset {offset} ");

            var persons = _repository.FindWithPagedSearch(query.ToString());

            var countQuery = new StringBuilder();
            countQuery.AppendLine("SELECT COUNT(*) FROM person p WHERE 1=1 ");
            if (!string.IsNullOrWhiteSpace(name))
                countQuery.AppendLine($"AND p.first_name LIKE '%{name}%' ");

            int totalResults = _repository.GetCount(countQuery.ToString());

            return new PagedSearchVO<PersonVO>
            {
                CurrentPage = page,
                List = _converter.Parse(persons),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }

        public PersonVO FindByID(long id)
        {
            return _converter.Parse(_repository.FindByID(id));
        }

        public List<PersonVO> FindByName(string firstName, string lastName)
        {
            return _converter.Parse(_repository.FindByName(firstName, lastName));
        }

        public PersonVO Update(PersonVO person)
        {
            var personEntity = _converter.Parse(person);
            personEntity = _repository.Update(personEntity);
            return _converter.Parse(personEntity);
        }        
    }
}
