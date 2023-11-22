using Dapper;
using SysPet.Models;

namespace SysPet.Data
{
    public class ProductsData : DataAccessBase<ProductosViewModel>
    {

        public ProductsData()
        {
        }

        public override async Task<IEnumerable<ProductosViewModel>> GetAll()
        {
            try
            {
                var sql = @$"SELECT [IdProducto],[Nombre],[FechaIngreso],[Proveedor],[Cantidad],[Stock],[PrecioUnitario],
                                    [PrecioSugerido],[Descripcion],[FechaVencimiento],[Estado],[Imagen], [TipoContenido]
                            FROM [dbo].[Productos] WITH (NOLOCK) WHERE Estado = 1";

                return await GetItems(sql);
            }
            catch
            {
                return new List<ProductosViewModel>();
            }
            
        }

        public async Task<IEnumerable<ProductosViewModel>> GetAllProductsToShow()
        {
            try
            {
                var sql = @$"SELECT [IdProducto],[Nombre],[FechaIngreso],[Proveedor],[Cantidad],[Stock],[PrecioUnitario],
                                    [PrecioSugerido],[Descripcion],[FechaVencimiento],[Estado],[Imagen], [TipoContenido]
                            FROM [dbo].[Productos] WITH (NOLOCK) WHERE Estado = 1 AND Stock > 0";

                return await GetItems(sql);
            }
            catch
            {
                return new List<ProductosViewModel>();
            }

        }

        public async Task<IEnumerable<ProductosViewModel>> GetExpiredProducts()
        {
            try
            {
                var sql = @$"SELECT [Stock]
                                  ,[FechaVencimiento]
                                  ,[Nombre]
                            FROM [dbo].[Productos] WITH (NOLOCK)
                            WHERE [FechaVencimiento] BETWEEN GETDATE() AND DATEADD(day, 30, GETDATE()) AND Stock > 0;";

                return await GetItems(sql);
            }
            catch
            {
                return new List<ProductosViewModel>();
            }

        }

        public async Task<IEnumerable<ProductosViewModel>> GetExpirdeStok()
        {
            try
            {
                var sql = @$"SELECT [Stock]
                                  ,[FechaVencimiento]
                                  ,[Nombre]
                             FROM [dbo].[Productos] WITH (NOLOCK)
                             WHERE Stock > 0 AND Stock < 5;";

                return await GetItems(sql);
            }
            catch
            {
                return new List<ProductosViewModel>();
            }

        }

        public override async Task<ProductosViewModel> GetItem(int idProducto)
        {
            var sql = @$"SELECT [IdProducto],[Nombre],[FechaIngreso],[Proveedor],[Cantidad],[Stock],[PrecioUnitario],[PrecioSugerido],[Descripcion],[FechaVencimiento],
                                [Estado],[Imagen], [TipoContenido]  FROM [dbo].[Productos] WITH (NOLOCK) WHERE idProducto = @idProducto AND Estado = 1";

            return await Get(sql, new { idProducto });
        }

        public override int Create(ProductosViewModel producto)
        {
            var sql = @$"INSERT INTO Productos (Nombre, FechaIngreso, Proveedor, Cantidad, Stock, PrecioUnitario, PrecioSugerido, Descripcion, FechaVencimiento, Estado,Imagen,NombreArchivo,TipoContenido)
            VALUES (@Nombre, @FechaIngreso, @Proveedor, @Cantidad, @Stock, @PrecioUnitario, @PrecioSugerido, @Descripcion, @FechaVencimiento, @Estado,@Imagen,@NombreArchivo,@TipoContenido);";

            var estado = 1;
            var fecha = FormatDate(producto.FechaIngreso);
            var fechaVencimiento = FormatDate(producto.FechaVencimiento);
            var parameters = new { producto.Nombre, FechaIngreso = fecha, producto.Proveedor, producto.Cantidad, producto.Stock, 
                producto.PrecioUnitario, producto.PrecioSugerido, producto.Descripcion, FechaVencimiento = fechaVencimiento, Estado = estado,
                producto.Imagen,
                producto.NombreArchivo,
                producto.TipoContenido
            };
                    
            return Execute(sql, parameters);
        }

        public override int Update(ProductosViewModel producto, int idProducto)
        {
            var estado = GetEstado(producto.Estado);
            var fechaVencimiento = FormatDate(producto.FechaVencimiento);

            var sql = @$"UPDATE Productos SET Nombre=@Nombre,
                                Cantidad=@Cantidad, 
                                Stock=@Stock,
                                PrecioUnitario=@PrecioUnitario,
                                PrecioSugerido=@PrecioSugerido,
                                Descripcion=@Descripcion,
                                FechaVencimiento=@FechaVencimiento,
                                Estado=@Estado,
                                Imagen=@Imagen,
                                NombreArchivo=@NombreArchivo,
                                TipoContenido=@TipoContenido
                        WHERE idProducto = @idProducto";

            var parameters = new
            {
                producto.Nombre,
                producto.Cantidad,
                producto.Stock,
                producto.PrecioUnitario,
                producto.PrecioSugerido,
                producto.Descripcion,
                FechaVencimiento = fechaVencimiento,
                Estado = estado,
                producto.Imagen,
                producto.NombreArchivo,
                producto.TipoContenido,
                idProducto
            };

            return Execute(sql, parameters);
        }

        public int UpdateStock(int stock, int idProducto)
        {
            var sql = @$"UPDATE Productos SET  
                                Stock={stock}
                        WHERE idProducto = @idProducto";

            return Execute(sql, new { idProducto });
        }

        public int UpdateStock(List<ProductosViewModel> products)
        {
            foreach (ProductosViewModel producto in products)
            {
                var sql = @$"UPDATE Productos SET  
                                Stock={producto.Stock}
                        WHERE idProducto = @idProducto";
                var idProducto = producto.IdProducto;

                Execute(sql, new { idProducto });
            }

            return 0;
        }

        public override int Delete(int idProducto)
        {
            var sql = $"UPDATE Productos SET Estado = 0 WHERE IdProducto = @idProducto;";

            return Execute(sql, new { idProducto});
        }

    }
}
