using SysPet.Models;

namespace SysPet.Data
{
    public class HistoriesData : DataAccessBase<HistorialesViewModel>
    {
        public override int Create(HistorialesViewModel item)
        {
            var sql = $@"INSERT INTO Historiales VALUES (
                        '{FormatDate(item.FechaVisita)}', 
                        '{item.Motivo}', 
                        '{item.Diagnostico}', 
                         {item.IdPaciente},
                         {item.UserId})";

            return Execute(sql);
        }

        public override int Delete(int id)
        {
            var sql = $"DELETE FROM Historiales WHERE Id = @id;";

            return Execute(sql, new { id });
        }

        public async override Task<IEnumerable<HistorialesViewModel>> GetAll()
        {
            try
            {
                var sql = @$"SELECT h.[Id]
                              ,h.[FechaVisita]
                              ,h.[Motivo]
                              ,h.[Diagnostico]
                              ,p.[Nombre] Paciente
                              ,p.[Imagen]
                              ,p.[TipoContenido]
	                          ,ps.Nombre + ' ' + ps.ApellidoPaterno + ' ' + ApellidoMaterno AS FullName
                          FROM [dbo].[Historiales] h
                          INNER JOIN Pacientes p on p.IdPaciente = h.IdPaciente
                          INNER JOIN Personas ps on ps.IdPersona = p.IdPersona
                          WHERE p.Estado = 1 AND ps.Estado = 1";

                return await GetItems(sql);
            }
            catch
            {
                return new List<HistorialesViewModel>();
            }
        }

        public async Task<IEnumerable<HistorialesViewModel>> GetAll(int? userId)
        {
            try
            {
                var sql = @$"SELECT h.[Id]
                              ,h.[FechaVisita]
                              ,h.[Motivo]
                              ,h.[Diagnostico]
                              ,p.[Nombre] Paciente
                              ,p.[Imagen]
                              ,p.[TipoContenido]
	                          ,ps.Nombre + ' ' + ps.ApellidoPaterno + ' ' + ApellidoMaterno AS FullName
                          FROM [dbo].[Historiales] h
                          INNER JOIN Pacientes p on p.IdPaciente = h.IdPaciente
                          INNER JOIN Personas ps on ps.IdPersona = p.IdPersona
                          INNER JOIN Usuarios u on u.Id = h.IdUser
                          WHERE p.Estado = 1 AND ps.Estado = 1 AND u.Id = @userId";

                return await GetItems(sql, new { userId });
            }
            catch
            {
                return new List<HistorialesViewModel>();
            }
        }

        public async override Task<HistorialesViewModel> GetItem(int id)
        {
            var sql = @$"SELECT h.[Id]
                              ,h.[FechaVisita]
                              ,h.[Motivo]
                              ,h.[Diagnostico]
                              ,p.[Nombre] Paciente
	                          ,p.[Imagen]
                              ,p.[TipoContenido]
	                          ,ps.Nombre + ' ' + ps.ApellidoPaterno + ' ' + ApellidoMaterno AS FullName
                          FROM [dbo].[Historiales] h
                          INNER JOIN Pacientes p on p.IdPaciente = h.IdPaciente
                          INNER JOIN Personas ps on ps.IdPersona = p.IdPersona
                          WHERE h.[Id] = @id AND p.Estado = 1 AND ps.Estado = 1";

            return await Get(sql, new { id });
        }

        public override int Update(HistorialesViewModel item, int id)
        {
            var date = item.FechaVisita.Date.Year < DateTime.Now.Year ? DateTime.Now.Date : item.FechaVisita.Date;
            var sql = $@"UPDATE Historiales SET
                        FechaVisita='{FormatDate(date)}', 
                        Motivo='{item.Motivo}', 
                        Diagnostico='{item.Diagnostico}'
                        WHERE [Id] = @id";

            return Execute(sql, new { id });
        }
    }
}
