using SysPet.Models;

namespace SysPet.Data
{
    public class DatingData : DataAccessBase<CitasViewModel>
    {
        public override int Create(CitasViewModel item)
        {
            var getEstado = item.FechaCita.Day < DateTime.Now.Day ? 4 : 1;

            var sql = $@"INSERT INTO Citas values (
                        '{FormatDateTime(item.FechaCita)}', 
                        '{item.Motivo}', 
                          {item.IdPersona},
                           {getEstado}, {item.UserId})";

            return Execute(sql);
        }

        public override int Delete(int id)
        {
            var sql = $"DELETE FROM Citas WHERE Id = @id;";

            return Execute(sql, new { id });
        }

        public async override Task<IEnumerable<CitasViewModel>> GetAll()
        {
            try
            {
                var sql = @$"SELECT c.[Id],c.[FechaCita],c.[Motivo],p.[Nombre],p.[ApellidoPaterno],p.[ApellidoMaterno], e.[Nombre] Estado
                              FROM [dbo].[Citas] c
                              INNER JOIN [dbo].[Personas] p on p.IdPersona = c.IdPersona
                              INNER JOIN [dbo].[EstadoCitas] e on e.Id = c.IdEstado AND p.Estado = 1";

                return await GetItems(sql);
            }
            catch
            {
                return new List<CitasViewModel>();
            }
        }

        public async Task<IEnumerable<CitasViewModel>> GetAll(int? userId)
        {
            try
            {
                var sql = @$"SELECT c.[Id],c.[FechaCita],c.[Motivo],p.[Nombre],p.[ApellidoPaterno],p.[ApellidoMaterno], e.[Nombre] Estado
                              FROM [dbo].[Citas] c
                              INNER JOIN [dbo].[Personas] p on p.IdPersona = c.IdPersona
                              INNER JOIN [dbo].[EstadoCitas] e on e.Id = c.IdEstado AND p.Estado = 1
                              INNER JOIN [dbo].[Usuarios] a on a.Id = c.IdUser AND a.Estado = 1
                              WHERE a.Id = @userId";

                return await GetItems(sql, new { userId });
            }
            catch
            {
                return new List<CitasViewModel>();
            }
        }

        public async Task<IEnumerable<CitasViewModel>> GetOnlyAppointments()
        {
            try
            {
                var sql = @$"SELECT CONVERT(date, c.[FechaCita]) AS Fecha
                                  ,e.[Nombre]
	                              ,COUNT(*) AS CantidadRegistros
                              FROM [dbo].[Citas] c
                              INNER JOIN [dbo].[EstadoCitas] e on e.Id = c.IdEstado
                              GROUP BY  CONVERT(date, c.[FechaCita]), e.Nombre
                            ORDER BY Fecha;";

                return await GetItems(sql);
            }
            catch
            {
                return new List<CitasViewModel>();
            }
        }

        public async Task<IEnumerable<CitasViewModel>> GetStates()
        {
            try
            {
                var sql = @$"SELECT [Id],[Nombre] FROM [dbo].[EstadoCitas]";

                return await GetItems(sql);
            }
            catch
            {
                return new List<CitasViewModel>();
            }
        }

        public async override Task<CitasViewModel> GetItem(int id)
        {
            var sql = @$"SELECT c.[Id],c.[FechaCita],c.[Motivo],p.[Nombre],p.[ApellidoPaterno],p.[ApellidoMaterno], e.[Nombre] Estado
                          FROM [dbo].[Citas] c
                          INNER JOIN [dbo].[Personas] p on p.IdPersona = c.IdPersona
                          INNER JOIN [dbo].[EstadoCitas] e on e.Id = c.IdEstado
                          INNER JOIN [dbo].[Usuarios] a on a.Id = c.IdUser 
                          WHERE c.Id = @id AND p.Estado = 1 AND a.Estado = 1";

            return await Get(sql, new { id });
        }

        public override int Update(CitasViewModel item, int id)
        {
            var sql = $@"UPDATE Citas SET
                        FechaCita='{FormatDateTime(item.FechaCita)}', 
                        Motivo='{item.Motivo}', 
                        IdEstado={item.IdEstado}
                        WHERE Id = @id";

            return Execute(sql, new { id });
        }
    }
}
