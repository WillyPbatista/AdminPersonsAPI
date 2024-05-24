using AutoMapper;
using CRUD_Persons.DTOs;
using CRUD_Persons.Models;

namespace CRUD_Persons
{
    public class MappingConfig : Profile
    {

        public MappingConfig()
        {
            CreateMap<Person, PersonDTO>().ReverseMap();

            CreateMap<Person, CreatePersonDTO>().ReverseMap();

            CreateMap<Person, UpdatePersonDTO>().ReverseMap();  
        }

    }
}
