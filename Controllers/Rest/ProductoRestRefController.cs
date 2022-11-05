using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using bazar_prg.Data;
using bazar_prg.Models;
using bazar_prg.Services;


namespace bazar_prg.Controllers.Rest
{
    [ApiController]
    [Route("api/productoref")]
    public class ProductoRestRefController : ControllerBase
    {
        private readonly ProductoService _service;


        public ProductoRestRefController(ProductoService service){
             _service = service;
        }

  
        [HttpPost]
        public Task<Productos> CreateProducto(Productos producto){
            return _service.crearProducto(producto);
        }

    }
}