using RestWithASPNET.Business.Implementations;
using RestWithASPNET.Data.Converter.Implementation;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Utils;
using RestWithASPNET.Model;
using RestWithASPNET.Repository.Implementations;
using System.Collections.Generic;

namespace RestWithASPNET.Business
{
    public class PersonBusiness : IPersonBusiness
    {
        private IPersonRepository _repository;
        private PersonConverter _converter;
        public PersonBusiness(IPersonRepository repository)
        {
            _repository = repository;
            _converter = new PersonConverter();
        }

        public List<PersonVO> FindAll()
        {
            var personList = _repository.FindAll();
            return _converter.Parse(personList);
        }
        public PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrWhiteSpace(sortDirection)) && !sortDirection.Equals("desc") ? "asc" : "desc";
            var size = (pageSize < 1) ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            var query = @"select * from person p where 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(name)) query += $"and p.first_name like '%{name}%' ";
            query += $"order by p.first_name {sort} limit {size} offset {offset}";

            var countQuery = @"select count(*) from person p where 1 = 1 ";
            if (string.IsNullOrWhiteSpace(countQuery)) countQuery += $"and p.first_name like '%{name}%'";

            var persons = _repository.FindWithPagedSearch(query);
            var totalResults = _repository.GetCount(countQuery);

            return new PagedSearchVO<PersonVO>
            {
                CurrentPage = page,
                List = _converter.Parse(persons),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }

        public PersonVO FindById(long id)
        {
            var person = _repository.FindById(id);
            return _converter.Parse(person);
        }
        public List<PersonVO> FindByName(string firstName, string lastName)
        {
            var personList = _repository.FindByName(firstName, lastName);
            return _converter.Parse(personList);
        }
        public PersonVO Create(PersonVO personVO)
        {
            var person = _converter.Parse(personVO);
            person = _repository.Create(person);
            return _converter.Parse(person);
        }
        public PersonVO Update(PersonVO personVO)
        {
            var person = _converter.Parse(personVO);
            person = _repository.Update(person);
            return _converter.Parse(person);
        }
        public PersonVO Disable(long id)
        {
            var person = _repository.Disable(id);
            return _converter.Parse(person);
        }
        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
