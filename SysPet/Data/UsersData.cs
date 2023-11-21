using SysPet.Models;

namespace SysPet.Data
{
    public class UsersData : DataAccessBase<UsuariosViewModel>
    {
        public override int Create(UsuariosViewModel item)
        {
            var sql = $@"INSERT INTO Usuarios VALUES (
                        '{item.Nombre}', 
                        '{item.Email}', 
                        '{item.Contrasenia}', 
                        '{item.IdRol}',  
                        1)";

            return Execute(sql);
        }

        public override int Delete(int id)
        {
            var sql = $"DELETE FROM Usuarios WHERE Id = @id;";

            return Execute(sql, new { id });
        }

        public async override Task<IEnumerable<UsuariosViewModel>> GetAll()
        {
            try
            {
                var sql = @$"SELECT u.Id,u.Nombre, u.Email,u.Contrasenia,u.Estado, r.Nombre AS Rol
                            FROM Usuarios u WITH (NOLOCK)
                            INNER JOIN Roles r WITH (NOLOCK) on r.IdRol = u.IdRol";

                return await GetItems(sql);
            }
            catch
            {
                return new List<UsuariosViewModel>();
            }
        }

        public async Task<IEnumerable<UsuariosViewModel>> GetRoles()
        {
            try
            {
                var sql = @$"SELECT IdRol,Nombre
                        FROM [dbo].[Roles] WITH (NOLOCK)";

                return await GetItems(sql);
            }
            catch
            {
                return new List<UsuariosViewModel>();
            }
        }

        public async override Task<UsuariosViewModel> GetItem(int id)
        {
            var sql = @$"SELECT u.Id,u.Nombre, u.Email,u.Contrasenia,u.Estado, r.Nombre AS Rol
                         FROM Usuarios u WITH (NOLOCK)
                         INNER JOIN Roles r WITH (NOLOCK) on r.IdRol = u.IdRol 
                         WHERE u.Id = @id";
            return await Get(sql, new { id });
        }

        public async Task<UsuariosViewModel> GetUserManager(string email, string password)
        {
            try
            {
                var sql = @$"SELECT Email,Contrasenia,Nombre,Id,IdRol
                        FROM Usuarios WITH (NOLOCK)
                        WHERE Email = @email AND Contrasenia = @password";
                var user = await Get(sql, new { email, password });
                return user;
            }
            catch
            {

                throw;
            }
        }

        public override int Update(UsuariosViewModel item, int id)
        {
            var sql = $@"UPDATE Usuarios SET 
                        Nombre='{item.Nombre}', 
                        Apellidos='{item.Contrasenia}', 
                        Estado={GetEstado(item.Estado)}
                        WHERE Id = @id";

            return Execute(sql, new { id });
        }
    }
}
