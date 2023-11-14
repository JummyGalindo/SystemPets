using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace SysPet.Models
{
    public class SalesViewModel
    {
        [DisplayName("No.")]
        public int Id { get; set; }
        [DisplayName("Total de artículos")]
        public int CantidadArticulos { get { return DetalleVenta != null ? DetalleVenta.Count : 0; } }
        [DisplayName("Fecha de venta")]
        public DateTime FechaVenta { get; set; } = DateTime.Now.Date;
        [DisplayName("Venta Total")]
        public decimal Total { get { return DetalleVenta != null || DetalleVenta.Any() ? DetalleVenta.Sum(x => x.Total) : 0; } }

		[DisplayName("Total de artículos")]
		public decimal TotalArticulos { get; set; }
        public List<SalesDetailViewModel> DetalleVenta { get; set; }
        public List<SelectListItem> Productos { get; set; }
        public List<ProductosViewModel> ListaProductos { get; set; }

        public string Descripcion { get; set; }

        public decimal Cantidad { get; set; }
        [DisplayName("Precio Unitario")]
        public decimal Precio { get; set; }
        [DisplayName("Total de la Venta")]
        public decimal TotalSale { get; set; }
        public decimal TotalItem { get; set; }

        [DisplayName("Artículo")]
        public string Articulo { get; set; }
        public byte[] Imagen { get; set; }
        public string TipoContenido { get; set; }

        public string Mes { get; set; }
        public int Anio { get; set; }
        public int MesNumero { get; set; }
        public int TotalVentas { get; set; }

        public SalesViewModel()
        {
            DetalleVenta = new List<SalesDetailViewModel>();
        }
        public int? UserId { get; set; }
    }
}
