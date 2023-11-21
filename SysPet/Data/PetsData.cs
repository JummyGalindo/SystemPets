using SysPet.Models;

namespace SysPet.Data
{
    public class PetsData : DataAccessBase<MascotasViewModel>
    {
        public override int Create(MascotasViewModel item)
        {
            var query = $@"INSERT INTO Pacientes (Nombre,Raza,Especie,Sexo,Edad,Color,Peso,Estado,Fecha,IdPersona,Imagen,NombreArchivo,TipoContenido, IdUser)
                        VALUES(@Nombre,@Raza,@Especie,@Sexo,@Edad,@Color,@Peso,@Estado,@Fecha,@IdPersona,@Imagen,@NombreArchivo,@TipoContenido,@UserId)";
            var estado = 1;
            var fecha = FormatDate(DateTime.Now.Date);
            var parameters = new
            {
                item.Nombre, item.Raza, item.Especie, item.Sexo,item.Edad,item.Color,item.Peso,Estado = estado,Fecha = fecha, item.IdPersona,item.Imagen,item.NombreArchivo,item.TipoContenido, item.UserId
            };

            return Execute(query, parameters);
        }

        public override int Delete(int id)
        {
            var sql = $"UPDATE Pacientes SET Estado = 0 WHERE IdPaciente = @id;";

            return Execute(sql, new { id });
        }

        public async override Task<IEnumerable<MascotasViewModel>> GetAll()
        {
            try
            {
                var sql = @$"SELECT p.[IdPaciente]
                              ,p.[Nombre]
                              ,p.[Raza]
                              ,p.[Especie]
                              ,p.[Sexo]
                              ,p.[Edad]
                              ,p.[Color]
                              ,p.[Peso]
                              ,p.[Estado]
                              ,p.[Fecha]
                              ,a.Nombre Propietario
	                          ,a.ApellidoPaterno
                              ,a.ApellidoMaterno
                              ,p.Imagen
                          FROM [dbo].[Pacientes] p WITH (NOLOCK)
                          INNER JOIN [dbo].[Personas] a WITH (NOLOCK) on a.IdPersona = p.IdPersona
                          WHERE p.Estado = 1 AND a.Estado = 1";

                return await GetItems(sql);
            }
            catch
            {
                return new List<MascotasViewModel>();
            }
        }

        public async Task<IEnumerable<MascotasViewModel>> GetAll(int? userId)
        {
            try
            {
                var sql = @$"SELECT p.[IdPaciente]
                              ,p.[Nombre]
                              ,p.[Raza]
                              ,p.[Especie]
                              ,p.[Sexo]
                              ,p.[Edad]
                              ,p.[Color]
                              ,p.[Peso]
                              ,p.[Estado]
                              ,p.[Fecha]
                              ,a.Nombre Propietario
	                          ,a.ApellidoPaterno
                              ,a.ApellidoMaterno
                              ,p.Imagen
                          FROM [dbo].[Pacientes] p WITH (NOLOCK)
                          INNER JOIN [dbo].[Personas] a WITH (NOLOCK) on a.IdPersona = p.IdPersona
                          INNER JOIN [dbo].[Usuarios] u WITH (NOLOCK) on u.Id = p.IdUser
                          WHERE p.Estado = 1 AND a.Estado = 1 AND u.Id = @userId AND u.Estado = 1";

                return await GetItems(sql, new { userId });
            }
            catch
            {
                return new List<MascotasViewModel>();
            }
        }

        public async override Task<MascotasViewModel> GetItem(int id)
        {
            var sql = @$"SELECT p.[IdPaciente]
                              ,p.[Nombre]
                              ,p.[Raza]
                              ,p.[Especie]
                              ,p.[Sexo]
                              ,p.[Edad]
                              ,p.[Color]
                              ,p.[Peso]
                              ,p.[Estado]
                              ,p.[Fecha]
                              ,a.Nombre Propietario
	                          ,a.ApellidoPaterno
                              ,a.ApellidoMaterno
                              ,p.Imagen
                              ,p.NombreArchivo,p.TipoContenido
                          FROM [dbo].[Pacientes] p WITH (NOLOCK)
                          INNER JOIN [dbo].[Personas] a WITH (NOLOCK) on a.IdPersona = p.IdPersona
                          WHERE p.IdPaciente = @id AND p.Estado = 1 AND a.Estado = 1";

            return await Get(sql, new { id });
        }

        public override int Update(MascotasViewModel item, int id)
        {
            var estado = GetEstado(item.Estado);
            var sql = $@"UPDATE Pacientes SET
                        Nombre=@Nombre,
                        Raza=@Raza,
                        Especie=@Especie,
                        Sexo=@Sexo,
                        Edad=@Edad,
                        Color=@Color,
                        Peso=@Peso, 
                        Estado=@Estado,
                        Imagen=@Imagen,
                        NombreArchivo=@NombreArchivo,
                        TipoContenido=@TipoContenido
                        WHERE IdPaciente = @id";
            var parameters = new
            {
                item.Nombre,
                item.Raza,
                item.Especie,
                item.Sexo,
                item.Edad,
                item.Color,
                item.Peso,
                Estado = estado,
                item.Imagen,
                item.TipoContenido,
                item.NombreArchivo,
                id
            };

            return Execute(sql, parameters);
        }
    }
}
