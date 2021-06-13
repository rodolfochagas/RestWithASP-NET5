using RestWithASPNET.Model;
using System.Collections.Generic;

namespace RestWithASPNET.Repository.Implementations
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person Disable(long id);
        List<Person> FindByName(string firstName, string lastName);
    }
}
