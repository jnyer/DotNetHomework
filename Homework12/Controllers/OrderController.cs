﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly OrderContext DBOrder;

        //构造函数把OrerContext 作为参数，Asp.net core 框架可以自动注入OrderContext对象
        public OrderController(OrderContext context)
        {
            this.DBOrder = context;
        }

        // GET: api/order/{id}  id为路径参数
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(string id)
        {
            var order = DBOrder.Orders.FirstOrDefault(o =>o.Id  == id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        // GET: api/order
        // GET: api/order/ customerName=张三
        [HttpGet]
        public ActionResult<List<Order>> GetOrders(string customerName)
        {
            var query = DBOrder.Orders.
                Include("Customer").
                Where(o => true);
            if (customerName != null)
                query = query.Where(o => o.CustomerName == customerName);
            return query.ToList();
        }


        // POST: api/order
        [HttpPost]
        public ActionResult<Order> PostOrder(Order order)
        {
            try
            {
                DBOrder.Orders.Add(order);
                DBOrder.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return order;
        }

        // PUT: api/order/{id}
        [HttpPut("{id}")]
        public ActionResult<Order> PutTodoItem(string id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest("Id cannot be modified!");
            }
            try
            {
                DBOrder.Entry(order).State = EntityState.Modified;
                DBOrder.SaveChanges();
            }
            catch (Exception e)
            {
                string error = e.Message;
                if (e.InnerException != null) error = e.InnerException.Message;
                return BadRequest(error);
            }
            return NoContent();
        }

        // DELETE: api/order/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteTodoItem(string id)
        {
            try
            {
                var order = DBOrder.Orders.FirstOrDefault(t => t.Id == id);
                if (order != null)
                {
                    DBOrder.Remove(order);
                    DBOrder.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return NoContent();
        }

    }
}