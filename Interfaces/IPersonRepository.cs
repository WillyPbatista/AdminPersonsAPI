using System.Collections.Generic;
using System.Threading.Tasks;
using CRUD_Persons.Models;

public interface IPersonRepository
{
    Task<List<Person>> GetAllAsync();
    Task<Person> GetByIdAsync(int id);
    Task AddAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(int id);
    Task<Person> FindByNameAsync(string name, string lastName);
}
