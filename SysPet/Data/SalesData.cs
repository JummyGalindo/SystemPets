using SysPet.Models;

namespace SysPet.Data
{
    public class SalesData : DataAccessBase<SalesViewModel>
    {
        public override int Create(SalesViewModel item)
        {
            var sql = $@"INSERT INTO Ventas VALUES('{FormatDate(item.FechaVenta)}',{item.CantidadArticulos}, {item.Total}, {item.UserId}) SELECT CAST(SCOPE_IDENTITY() as int)";
            var result = ExecuteWithId(sql);

            foreach (var detalle in item.DetalleVenta)
            {
                detalle.IdVenta = result;
                var query = $@"INSERT INTO DetalleVenta VALUES({detalle.IdVenta},'{detalle.Descripcion}', {detalle.Precio}, {detalle.Cantidad}, {detalle.Total}, {detalle.IdProducto})";
                Execute(query);
            }

            return result;
        }

        public override int Delete(int id)
        {
            var query = $"DELETE FROM DetalleVenta WHERE IdVenta = @id;";
            Execute(query, new { id });

            var sql = $"DELETE FROM Ventas WHERE Id = @id;";

            return Execute(sql, new { id });
        }

        public async override Task<IEnumerable<SalesViewModel>> GetAll()
        {
            try
            {
                var sql = @$"SELECT v.[Id]
                              ,v.[FechaVenta]
                              ,v.[CantidadArticulos] TotalArticulos
                              ,v.[Total] As TotalSale
	                          ,d.[Descripcion]
                              ,d.[Precio]
                              ,d.[Cantidad]
                              ,d.[Total] TotalItem
	                          ,p.Nombre Articulo
                              ,p.[Imagen], p.[TipoContenido]
                          FROM [dbo].[Ventas] v WITH (NOLOCK)
                          INNER JOIN [dbo].[DetalleVenta] d WITH (NOLOCK) on d.IdVenta = v.Id
                          INNER JOIN [dbo].[Productos] p WITH (NOLOCK) on p.IdProducto = d.IdProducto";

                return await GetItems(sql);
            }
            catch
            {
                return new List<SalesViewModel>();
            }
        }

        public async Task<IEnumerable<SalesViewModel>> GetAll(int? userId)
        {
            try
            {
                var sql = @$"SELECT v.[Id]
                              ,v.[FechaVenta]
                              ,v.[CantidadArticulos] TotalArticulos
                              ,v.[Total] As TotalSale
	                          ,d.[Descripcion]
                              ,d.[Precio]
                              ,d.[Cantidad]
                              ,d.[Total] TotalItem
	                          ,p.Nombre Articulo
                              ,p.[Imagen], p.[TipoContenido]
                          FROM [dbo].[Ventas] v WITH (NOLOCK)
                          INNER JOIN [dbo].[DetalleVenta] d WITH (NOLOCK) on d.IdVenta = v.Id
                          INNER JOIN [dbo].[Productos] p WITH (NOLOCK) on p.IdProducto = d.IdProducto
                          INNER JOIN [dbo].[Usuarios] u WITH (NOLOCK) on u.Id = v.IdUser AND u.Estado = 1";

                return await GetItems(sql);
            }
            catch
            {
                return new List<SalesViewModel>();
            }
        }

        public async Task<IEnumerable<SalesViewModel>> GetOnlySales()
        {
            try
            {
                var sql = @$"SELECT [FechaVenta]
	                              ,Sum(CantidadArticulos) AS Cantidad
                            FROM [dbo].[Ventas] WITH (NOLOCK)
                            GROUP BY FechaVenta
                            ORDER BY FechaVenta";

                return await GetItems(sql);
            }
            catch
            {
                return new List<SalesViewModel>();
            }
        }

        public async Task<IEnumerable<SalesViewModel>> GetOnlySalesDetail()
        {
            try
            {
                var sql = @$"SELECT TOP(4) d.[Cantidad]
	                              ,p.Nombre AS Articulo
	                              ,SUM(d.Cantidad) AS ToTal
                              FROM [dbo].[DetalleVenta] d WITH (NOLOCK)
                              INNER JOIN [dbo].[Productos] p WITH (NOLOCK) ON d.IdProducto = p.IdProducto
                              GROUP BY d.Cantidad, p.Nombre
                              ORDER BY Total DESC";

                return await GetItems(sql);
            }
            catch
            {
                return new List<SalesViewModel>();
            }
        }

        public async Task<IEnumerable<SalesViewModel>> GetOnlyTotalSales()
        {
            try
            {
                var sql = @$"SELECT 
                                YEAR([FechaVenta]) AS Anio,
                                MONTH([FechaVenta]) AS MesNumero,
                                DATENAME(MONTH, [FechaVenta]) AS Mes,
                                SUM([Total]) AS TotalVentas
                            FROM 
                                [dbo].[Ventas] WITH (NOLOCK)
                            WHERE 
                                [FechaVenta] >= DATEADD(MONTH, -7, GETDATE())
                            GROUP BY 
                                YEAR([FechaVenta]), MONTH([FechaVenta]), DATENAME(MONTH, [FechaVenta])
                            ORDER BY 
                                Anio DESC, MesNumero DESC;
                            ";

                return await GetItems(sql);
            }
            catch
            {
                return new List<SalesViewModel>();
            }
        }

        public async override Task<SalesViewModel> GetItem(int id)
        {
            var sql = @$"SELECT v.[Id]
                              ,v.[FechaVenta]
                              ,v.[CantidadArticulos]
                              ,v.[Total] As TotalSale
	                          ,d.[Descripcion]
                              ,d.[Precio]
                              ,d.[Cantidad]
                              ,d.[Total]
	                          ,p.Nombre Articulo
                              ,p.[Imagen], p.[TipoContenido]
                          FROM [dbo].[Ventas] v WITH (NOLOCK)
                          INNER JOIN [dbo].[DetalleVenta] d WITH (NOLOCK) on d.IdVenta = v.Id
                          INNER JOIN [dbo].[Productos] p WITH (NOLOCK) on p.IdProducto = d.IdProducto
                          WHERE v.[Id] = @id";

            return await Get(sql, new { id });
        }

        public override int Update(SalesViewModel item, int id)
        {
            //var sql = $@"UPDATE Ventas SET 
            //            Descripcion={item.Descripcion},
            //            FechaVenta={item.FechaVenta},
            //            Cantidad={item.Cantidad},
            //            Precio={item.Precio},
            //            Total={item.Total}
            //            WHERE [Id] = @id";

            return Execute("", new { id });
        }
    }
}
