using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using bazar_prg.Models;
using bazar_prg.Data;

namespace bazar_prg.Controllers.Rest
{
    [ApiController]
    [Route("api/producto")]
    public class ProductoRestController : ControllerBase
    {
         private readonly ApplicationDbContext _context;

        public ProductoRestController(ApplicationDbContext context){
             _context = context;
        }

        [HttpGet]
        public IEnumerable<Productos> ListProductos()
        {
             var listProductos=_context.DataProducto.OrderBy(s => s.Id).ToList();   
             return listProductos.ToArray();
        }

       [HttpGet("{id}")]
        public Productos GetProduct(int? id)
        {
            var producto =  _context.DataProducto.Find(id);
            return producto;
        }

        [HttpPost]
        public Productos CreateProduct(Productos producto){
            _context.Add(producto);
            _context.SaveChanges();
            return producto;
        }

    }
}