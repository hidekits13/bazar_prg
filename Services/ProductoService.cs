using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using bazar_prg.Models;
using bazar_prg.Data;


namespace bazar_prg.Services
{
    public class ProductoService 
    {
        private readonly ILogger<ProductoService> _logger;
        private readonly ApplicationDbContext _context;

        public ProductoService(ILogger<ProductoService> logger,ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Productos> crearProducto(Productos p){
            if(p.Precio < 1){
                throw new SystemException("No se puede ingresar datos con precio menor 1 sol");
            }
            _context.Add(p);
            await _context.SaveChangesAsync();
            return p;
        }
       
    }
}