using NorthwindLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new NorthwindEntities())
            {
              // 1. Show all the employees, except those who live in UK
                var employees = context.Employees.Where(e => e.City!= "London").ToArray();
                //foreach (var employee in employees)
                //{
                //    Console.WriteLine($"{ employee.FirstName} {employee.LastName}".ToString());
                //}

                // 2. Show all product details for products whose unit price is greater than $10
                //and quantity in stock greater than 2.Sort by product price.

                var products = context.Products.Where(p => p.UnitPrice > 10 && p.UnitsInStock > 2).OrderByDescending(p=>p.UnitPrice);

                // 3. Show the profit for each product (how much money the company will earn
                //if they sell all the units in stock). Sort by the profit.

                var profit = context.Products.Select
                    (p => (p.UnitPrice * p.UnitsInStock)).OrderByDescending(p => p.Value);


            }
        }
    }
}