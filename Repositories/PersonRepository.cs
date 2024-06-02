using CRUD_Persons.Data;
using CRUD_Persons.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PersonRepository : IPersonRepository
{
    private readonly ApplicationDbContext _db;

    public PersonRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<Person>> GetAllAsync()
    {
        return await _db.Persons.ToListAsync();
    }

    public async Task<Person> GetByIdAsync(int id)
    {
        return await _db.Persons.FirstOrDefaultAsync(x => x.ID == id);
    }

    public async Task AddAsync(Person person)
    {
        await _db.Persons.AddAsync(person);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Person person)
    {
        _db.Persons.Update(person);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var person = await _db.Persons.FirstOrDefaultAsync(x => x.ID == id);
        if (person != null)
        {
            _db.Persons.Remove(person);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<Person> FindByNameAsync(string name, string lastName)
    {
        return await _db.Persons.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.LastName.ToLower() == lastName.ToLower());
    }
}
