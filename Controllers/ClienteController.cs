using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using bazar_prg.Models;
using bazar_prg.Data;
using bazar_prg.Integration.Sengrid;

using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Rotativa.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace bazar_prg.Controllers
{


    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly ApplicationDbContext _context;
      
        private readonly SendMailIntegration _sendgrid;

         private readonly UserManager<IdentityUser> _userManager;


        public ClienteController(ILogger<ClienteController> logger, ApplicationDbContext context, SendMailIntegration sendgrid, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context= context;
            _sendgrid= sendgrid;
            _userManager = userManager;

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

       
         public IActionResult Listado()
        {
            return View(_context.DataCliente.ToList());
        }


      public IActionResult ExportarExcel() 
        {
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var clientes = _context.DataCliente.AsNoTracking().ToList();
            using (var libro = new ExcelPackage())
            {
                var worksheet = libro.Workbook.Worksheets.Add("clientes");
                worksheet.Cells["A1"].LoadFromCollection(clientes, PrintHeaders: true);
                for (var col = 1; col < clientes.Count + 1; col++)
                {
                    worksheet.Column(col).AutoFit();
                }
                // Agregar formato de tabla
                var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: clientes.Count + 1, toColumn: 2), "Pedidos");
                tabla.ShowHeader = true;
                tabla.TableStyle = TableStyles.Light6;
                tabla.ShowTotal = true;

                return File(libro.GetAsByteArray(), excelContentType, "ListadoContactanos.xlsx");
            }
        }


// GET: Pedido/Edit/5
        public async Task<IActionResult> Edit(int? ID)
        {
            if (ID == null || _context.DataCliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.DataCliente.FindAsync(ID);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Name,Surname,Estado")] Cliente cliente)
        {
            if (Id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Listado));
            }
            return View(cliente);
        }








       private bool ClienteExists(int id)
        {
          return (_context.DataCliente?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}