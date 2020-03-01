using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    /* API routing rules 
     * The choice of Action() under the controller is based on following rules. 
     * At first it searches for Verb name within the controller. 
     * For example, if one GET HTTP request arrives at the controller then it searches 
     * for a Get() action defined in the controller. If it is present then it executes 
     * otherwise proceed to the second point.
       It searches for an action that starts with Get. For example if Get() is not present 
       in the controller then it searches for an action that starts with GetXXXXX() 
       for example GetData(). If there is no action that starts with even Get() then 
       please continue to the third point.
       It searches for an attribute over action. We can set [HttpGet] or this kind of 
       attribute over action that will help to map that specific action with a HTTP verb.
     */
    //this class is derrived from APIController class 
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;
        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        //In order enable filttering of customer based on 
        //Query parameter passed we need to pass an optional 
        //parameter called query which will come from NewRentals
        //Get api/customers
        public IHttpActionResult GetCustomers(string query = null)
        {
            //Here we will use link extension method called Select and pass delegate 
            //which does the mapping between customer and customer DTO
            //We have also eagerly load MembershipType as we want to show it along with customer
            /*
            return _context.Customers
                .Include(c=>c.MembershipType)
                .ToList() // This will immediately fire the query 
                .Select(Mapper.Map<Customer,CustomerDto>);
            //Replaced above query with below one*/
            //Below query will not fire as we are not calling ToList() immediately after it 
            //so at this point it is a CustomerQuery is IQuerable interface
            var customerQuery = _context.Customers.Include(c => c.MembershipType);
            //Now I can apply filter over customer query and make sure only desired records 
            //are pulled when query is fired 
            if (!string.IsNullOrWhiteSpace(query))
            {
                customerQuery = customerQuery.Where(c => c.Name.Contains(query));
            }
            var customerDtos = customerQuery
                .ToList()
                .Select(Mapper.Map<Customer, CustomerDto>);
            return Ok(customerDtos);
        }

        //Get api/customers/1 
        //Replace  public CustomerDto GetCustomer(int id) this with below to meet convention
        public IHttpActionResult GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                //Simply throw an exception 
                //throw new HttpResponseException(HttpStatusCode.NotFound);
                return NotFound();
            }
            //Here we are returning a single customer 
            //so we cannot use extension method select instead
            //we will called Mapper Map method returning CustomerDto
            return Ok( Mapper.Map<Customer,CustomerDto>(customer));
        }
        /*Post api/customers
          customer will come from request body
          In restful convention when we create a resource status code 
          by convention should be returned as 201 currently we are getting 200 – OK. 
          So we need modify our Customers WebApi so let us use IHttpActionResult as return
          type instead of customerdto. IHttpActionResult is similar to MVC ActionResult 
        */
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        {
            //Check whether customer object passed in is in correct state or not
            if (!ModelState.IsValid)
                //Instead of throwing exception we can use Helper method 
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
                return BadRequest();
            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();
            //Now after saving customer ID will get generated and we need to pass
            //it to CustomerDto
            customerDto.Id = customer.Id;
            //Instead returning customerdto we will use Creat method and pass the 
            //URI - Unified Resource Indentifier so example if Customer created with ID=10
            //then the URI will /api/customers + append newly created customer ID /10
            //so complete URI will be /api/customers/10
            return Created(new Uri(Request.RequestUri +"/"+ customer.Id), customerDto);

        }
        //PUT api/customer/1 
        //used for updating all fields of a resource. 
        //Customer Id will be passed as well as customer will come from request body
        [HttpPut]
        public void UpdateCustomer(int id, CustomerDto customerDto)
        {
            //Check whether customer object passed in is in correct state or not
            if (!ModelState.IsValid)

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            var customerInDB = _context.Customers.SingleOrDefault(c => c.Id == id);
            //client might sent wrong id so verify it 
            if (customerInDB == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            Mapper.Map(customerDto, customerInDB);
            _context.SaveChanges();
        }
        //DELETE api/customer/1
        [HttpDelete]
        public void DeteteCustomer(int id)
        {
            var customerInDB = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customerInDB == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            _context.Customers.Remove(customerInDB);
            _context.SaveChanges();
        }
    }

}
