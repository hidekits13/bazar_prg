using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace bazar_prg.Models
{
   
    [Table("t_order_detail")]
    public class DetallePedido
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID {get; set;}

        public Productos? Producto {get; set;}

        public int Cantidad{get; set;}
        public Decimal Precio { get; set; }
        public Pedido? pedido {get; set;}
    }
}