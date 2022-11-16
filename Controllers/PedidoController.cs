using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using bazar_prg.Data;
using bazar_prg.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Rotativa.AspNetCore;

namespace bazar_prg.Controllers
{
    public class PedidoController : Controller
    {
        private readonly ILogger<PagoController> _logger;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly ApplicationDbContext _context;

        public PedidoController(ILogger<PagoController> logger,
                              UserManager<IdentityUser> userManager,
                              ApplicationDbContext context)
                              {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            }

        public IActionResult Index()
        {
            return View(_context.DataPedido.ToList());
        }


     public IActionResult ExportarExcel() 
        {
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var pedidos = _context.DataPedido.AsNoTracking().ToList();
            using (var libro = new ExcelPackage())
            {
                var worksheet = libro.Workbook.Worksheets.Add("Pedidos");
                worksheet.Cells["A1"].LoadFromCollection(pedidos, PrintHeaders: true);
                for (var col = 1; col < pedidos.Count + 1; col++)
                {
                    worksheet.Column(col).AutoFit();
                }
                // Agregar formato de tabla
                var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: pedidos.Count + 1, toColumn: 2), "Pedidos");
                tabla.ShowHeader = true;
                tabla.TableStyle = TableStyles.Light6;
                tabla.ShowTotal = true;

                return File(libro.GetAsByteArray(), excelContentType, "Pedidos.xlsx");
            }
        }


// GET: Pedido/Edit/5
        public async Task<IActionResult> Edit(int? ID)
        {
            if (ID == null || _context.DataPedido == null)
            {
                return NotFound();
            }

            var pedidos = await _context.DataPedido.FindAsync(ID);
            if (pedidos == null)
            {
                return NotFound();
            }
            return View(pedidos);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int ID, [Bind("ID,UserID,Total,Status")] Pedido pedidos)
        {
            if (ID != pedidos.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedidos.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pedidos);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        private bool PedidoExists(int id)
        {
          return (_context.DataPedido?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}