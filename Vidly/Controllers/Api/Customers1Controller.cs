using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    //Please this class is derrived from APIController class 
    public class Customers1Controller : ApiController
    {
        private ApplicationDbContext _context;
        public Customers1Controller()
        {
            _context = new ApplicationDbContext();
        }

        //Get api/customers
        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        //Get api/customers/1 
        public Customer GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                //Simply throw an exception 
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return customer;
        }
        //Post api/customers
        //customer will come from request body
        [HttpPost]
        public Customer CreateCustomer(Customer customer)
        {
            //Check whether customer object passed in is in correct state or not
            if (!ModelState.IsValid)

                throw new HttpResponseException(HttpStatusCode.BadRequest);

            _context.Customers.Add(customer);
            _context.SaveChanges();
            return customer;

        }
        //PUT api/customer/1 
        //used for updating all fields of a resource. 
        //Customer Id will be passed as well as customer will come from request body
        [HttpPut]
        public void UpdateCustomer(int id, Customer customer)
        {
            //Check whether customer object passed in is in correct state or not
            if (!ModelState.IsValid)

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            var customerInDB = _context.Customers.SingleOrDefault(c => c.Id == id);
            //client might sent wrong id so verify it 
            if (customerInDB == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            //if no exception simply set the properties 
            customerInDB.Name = customer.Name;
            customerInDB.MembershipTypeId = customer.MembershipTypeId;
            customerInDB.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            customerInDB.Birthdate = customer.Birthdate;
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
