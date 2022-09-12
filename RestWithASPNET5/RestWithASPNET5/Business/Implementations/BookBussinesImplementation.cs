using RestWithASPNET5.Data.Converter.Implementations;
using RestWithASPNET5.Data.VO;
using RestWithASPNET5.Hypermedia.Utils;
using RestWithASPNET5.Model;
using RestWithASPNET5.Repository.Generic;
using System.Collections.Generic;
using System.Text;

namespace RestWithASPNET5.Business.Implementations
{
    public class BookBussinesImplementation : IBookBussines
    {
        private readonly IRepository<Book> _repository;
        private readonly BookConverter _converter;

        public BookBussinesImplementation(IRepository<Book> repository)
        {
            _repository = repository;
            _converter = new BookConverter();
        }


        public BookVO Create(BookVO book)
        {
            var bookEntity = _converter.Parse(book);
            bookEntity = _repository.Create(bookEntity);
            return _converter.Parse(bookEntity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public List<BookVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        public PagedSearchVO<BookVO> FindWithPagedSearch(string title, string sortDirection, int pageSize, int page)
        {
            var sort = !string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("desc")
                ? "asc" : "desc";
            var size = pageSize < 1 ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            var query = new StringBuilder();
            query.AppendLine("SELECT * FROM books b WHERE 1=1 ");
            if (!string.IsNullOrWhiteSpace(title))
                query.AppendLine($"AND b.title LIKE '%{title}%' ");
            query.AppendLine($"ORDER BY b.title {sort} limit {size} offset {offset} ");

            var books = _repository.FindWithPagedSearch(query.ToString());

            var countQuery = new StringBuilder();
            countQuery.AppendLine("SELECT COUNT(*) FROM books b WHERE 1=1 ");
            if (!string.IsNullOrWhiteSpace(title))
                countQuery.AppendLine($"AND b.title LIKE '%{title}%' ");

            int totalResults = _repository.GetCount(countQuery.ToString());

            return new PagedSearchVO<BookVO>
            {
                CurrentPage = page,
                List = _converter.Parse(books),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }

        public BookVO FindByID(int id)
        {
            return _converter.Parse(_repository.FindByID(id));
        }        

        public BookVO Update(BookVO book)
        {
            var bookEntity = _converter.Parse(book);
            bookEntity = _repository.Update(bookEntity);
            return _converter.Parse(bookEntity);
        }
    }
}
