using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using bazar_prg.Models;
using bazar_prg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Dynamic;

namespace bazar_prg.Controllers
{
    public class VistaClienteController : Controller
    {
        private readonly ILogger<VistaClienteController> _logger;
         
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; 

        public VistaClienteController(ApplicationDbContext context,
            ILogger<VistaClienteController> logger,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;

        }




         public async Task<IActionResult> VistaCliente(string? searchString)
        { var userID = _userManager.GetUserName(User);
            var items = from o in _context.DataPedido select o;
            items = items.
                Where(w => w.UserID.Equals(userID));
            var datos = await items.OrderByDescending(w => w.ID).ToListAsync();

            dynamic model = new ExpandoObject();
            model.elementosDatos = datos;

            return View(model);
        }
           
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}