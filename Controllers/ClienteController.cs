using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using bazar_prg.Models;
using bazar_prg.Data;
using bazar_prg.Integration.Sengrid;

namespace bazar_prg.Controllers
{


    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly ApplicationDbContext _context;
      
        private readonly SendMailIntegration _sendgrid;

        public ClienteController(ILogger<ClienteController> logger, ApplicationDbContext context, SendMailIntegration sendgrid)
        {
            _logger = logger;
            _context= context;
            _sendgrid= sendgrid;
        }

        public IActionResult Index()
        {
            return View();
        }

       [HttpPost]
      public async Task<IActionResult> Create(Cliente? objCliente)
    {
        _context.Add(objCliente);
        _context.SaveChanges();
        await _sendgrid.SendMail(objCliente.Email,objCliente.Name,
                "Bienvenido al e-comerce",
                "Revisaremos su consulta en breves momentos y le responderemos",
                SendMailIntegration.SEND_SENDGRID);            
                    
        ViewData["Message"] = "Se registro el contacto";
        
        return View("Index");
    }


          public async Task<IActionResult> Details(int? id){
            Productos objProduct = await _context.DataProducto.FindAsync(id);
            if(objProduct == null){
                return NotFound();
            }
            return View(objProduct);
        }

       
    
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}