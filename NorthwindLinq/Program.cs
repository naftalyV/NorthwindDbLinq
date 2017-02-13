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
                var employees = context.Employees.Where(e => e.City!= "London").ToList();

                // 2. Show all product details for products whose unit price is greater than $10
                //and quantity in stock greater than 2.Sort by product price.

                var products = context.Products.Where(p => p.UnitPrice > 10 && p.UnitsInStock > 2).OrderBy(p=>p.UnitPrice);

                // 3. Show the profit for each product (how much money the company will earn
                //if they sell all the units in stock). Sort by the profit.

                var profit = context.Products.Select
                    (p => (p.UnitPrice * p.UnitsInStock)).OrderBy(p => p.Value).ToList();

                // 4. How much each customer paid for all the orders he had committed together ?

                var customerPaid = context.Orders.Join
                    (context.Order_Details, o => o.OrderID,
                    od => od.OrderID, (o, od) => new { od, o })
                    .Join(context.Customers,
                    c => c.o.CustomerID, c => c.CustomerID,
                    (o, c) => new { o, c })
                    .Where(oc => oc.c.CustomerID == oc.o.o.CustomerID &&
                    oc.o.o.OrderID == oc.o.od.OrderID)
                    .Select(customer => new
                    {
                        Name = customer.c.ContactName,
                        paid = customer.o.od.Quantity * customer.o.od.UnitPrice })
                        .GroupBy(c=>c.Name)
                        .ToList();

                // 5. Show how many Employees live in each country and their average age.

                
                var EmployeeCountries = context.Employees.GroupBy(e => e.EmployeeID).Count();

                //.6 for each employee display the total price paid on all of his orders that hasn’t shipped yet

                var employeeOrders = context.Employees
                    .Join(context.Orders, e => e.EmployeeID,
                    o => o.EmployeeID, (e, o) => new { e, o })
                    .Join(context.Order_Details, od => od.o.OrderID,
                    od => od.OrderID, (o, od) => new { o, od })
                       .Where(eo => eo.o.e.EmployeeID == eo.o.e.EmployeeID)
                        .Where(eo => eo.o.o.ShippedDate == null)
                        .Select(employee => new
                        {
                            Name = employee.o.e.FirstName,
                            totalPrice = (employee.od.UnitPrice * employee.od.Quantity)
                        })
                            .GroupBy(e => e.Name).ToList();
                // .7 for each category display the total sales revenue, every year.


                //               select distinct CategoryName,
                //sum(od.Quantity * od.UnitPrice) as 'revenue',
                //YEAR(OrderDate) as'Year'
                //from Categories c,Products p,[Order Details] od,Orders o
                //where c.CategoryID = p.CategoryID
                //and p.ProductID = od.ProductID
                //and od.OrderID = o.OrderID
                //group by CategoryName,YEAR(OrderDate)




//                --18.Which Product is the most popular? (number of items)
//WITH SUM_QUANTITY
//AS(
//    SELECT

//        ProductID
//        , SUM(Quantity) AS 'SumOfQuantity'

//    FROM
//        [Order Details]

//    GROUP BY

//        ProductID
//)
//SELECT
//    ProductName
//    ,SUM(Quantity) AS 'Sold Quantity'
//FROM
//    [Order Details]

//        JOIN Products

//            ON Products.ProductID = [Order Details].ProductID
//GROUP BY
//    [Order Details].ProductID
//	,ProductName
//HAVING

//    SUM(Quantity) = (
//        SELECT MAX(SumOfQuantity)

//        FROM SUM_QUANTITY
//	)
//go





            }
        }
    }
}
