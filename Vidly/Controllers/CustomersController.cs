using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        /*
         * ApplicationDBContext object is a disposable object 
         * so we should properly dispose it by overriding the dispose 
         * method of the base controller class  
         */
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //Will create a new customer based customer data entered in form
        //We need MembershipTypes for form Dropdown so we use ViewModel to 
        //get MembershipType for our view
        public ActionResult New()
        {
            var MembershipTypes = _context.MembershipTypes.ToList();
            var viewmodel = new CustomerFormViewModel {
                //We are basically initializing customer so that we get 
                //Customer Id = 0 initially indicating its new customer all 
                //other properties will be initallize with default values
                //this will help us show Validation message correctly 
                Customer = new Customer(),
                MembershipTypes = MembershipTypes
            };
            return View("CustomerForm",viewmodel);
        }
        /*Below method will receive the user entered data i.e. viewmodel attached as parameter
        //which passed on Clicking of Submit button in the view and Save it inside DB. 
        //Since this will create a customer so we should decorate this method with HttpPost attribute
        //As we don't want it to be called by Get method
        //Now suppose we change the Create method signature and Customer type Customer parameter 
        //even then MVC framework is smart enough to map/bind the form data to Customer model properties as 
        //Majority properties are of customer type only. 
        //Renamed Create to Save to use it for both cases for creation as well as saving existing customer
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            /*
             * When ASP.NET MVC populate this Customer object passed as parameter with request data it checks to 
             * see if the object is valid based on the data annotations applied on various 
             * properties of this customer class so at this point we can use Model state 
             * property to get access to Validation data. It has a property called IsValid 
             * which we can use to change the application flow
             */
            if (!ModelState.IsValid)
            {
                //Model state is invalid so render the same view again to user inorder enable re-entry
                var viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };
                return View("CustomerForm", viewModel);

            }
            //If the customer Id is not passed then it is a new record
            if (customer.Id == 0)
            {
                //Below line will not immediately add it to database. It is just in the memory.
                //DB context has a change detection mechanism so any time you add / modify/ remove 
                //one of its existing objects it will mark them as Added/ Modifed or deleted
                _context.Customers.Add(customer);
            }
            else
            {
                //Existing customer modified so customer Id is already available
                //Please note we use Single in context as we want to throw an exeption 
                //in case customer is not available since we don't want this line to execute
                var customerInDB = _context.Customers.Single(c => c.Id == customer.Id);
                //Now let us set the properties of the object to update with the passed object
                customerInDB.Name = customer.Name;
                customerInDB.Birthdate = customer.Birthdate;
                customerInDB.MembershipTypeId = customer.MembershipTypeId;
                customerInDB.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
                //The above mapping can be done with Auto Mapper library which map by convention
                //Mapper.Map(customer, customerInDB)
            }
            //To persist these changes we need to call DB COntext save changes method 
            //this will go through all the marked modified objects and based on the kind of modification
            //generate SQL Statements at run time and run them on the Database. All these statements are
            //wrapped in to transactions i.e. either all or no changes will be presisted to database 
            _context.SaveChanges();
            //Finally we redirect to Customers list page by calling Index method of Customers controller
            return RedirectToAction("Index","Customers");

        }
        // GET: Customers
        public ActionResult Index()
        {
            // if you keep var customers = _context.Customers line then
            //EntityFramework (EF) will not execute query immediately over DB
            //To execute query immediately you need to call .ToList() at the end of this line
            //Eagerloading of MembershipType with customers by using .Include(c=>c.MembershipType)
            //Since .Include is an extention method and is defined in different namespace called 
            //System.data.Entity
            var customers = _context.Customers.Include(c => c.MembershipType).ToList();
            return View(customers);
            //since we are getting data from our api directly so we can commend above two lines 
            //return View();
        }
        //Index data table will generate a data table using webapi
        public ActionResult Indexdt()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            var customer = _context.Customers.Include(c=>c.MembershipType).SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //Below method will be used to edit the Selected/Clicked customer link
        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            //Check whether Dbcontext returned a customer with given Id or not
            if (customer == null)
            {
                return HttpNotFound();
            }
            //Let us create a viewModel object of CustomerFormViewModel type 
            //and pass customer and as well as pass membershiptypes 
            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };
            return View("CustomerForm", viewModel);
        }
    }
}