using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.App_Start
{
    //dervie this class from Profile available in AutoMapper. 
    public class MappingProfile:Profile
    {
        //Mapping profile which determines how objects of different types 
        //can be mapped to each other 
        public MappingProfile()
        {
            /*when we call createMap method the Automapper 
              uses Reflection to scan these types, it finds these properties 
              and map them based on its name that is why autmatter is called 
              Convention based mapping tool because it uses property names as
              the convention to map the objects 
              During update you may get error/exception called 
              Id is the key property for the Movie/customer class, and a 
              key property should not be changed.
              To resolve this, you need to tell AutoMapper to ignore
              Id property during mapping of a MovieDto to Movie or customerDto to customer
              .ForMember(m=>m.Id,opt=>opt.Ignore());
           */
            Mapper.CreateMap<Customer, CustomerDto>();
            Mapper.CreateMap<CustomerDto,Customer>().ForMember(c => c.Id, opt => opt.Ignore());
            Mapper.CreateMap<Movie, MovieDto>();
            Mapper.CreateMap<MovieDto, Movie>().ForMember(m=>m.Id,opt => opt.Ignore());
            Mapper.CreateMap<MembershipType, MembershipTypeDto>();
            Mapper.CreateMap<Genre, GenreDto>();
        }
    }
}