using SysPet.Models;

namespace SysPet.Data
{
    public class DepartmentsData : DataAccessBase<DepartamentosViewModel>
    {
        public override int Create(DepartamentosViewModel item)
        {
            var sql = $@"INSERT INTO Departamentos VALUES (
                        '{item.Nombre}', 
                        '{item.Descripcion}', 
                         1, 
                        '{FormatDate(item.Fecha)}')";

            return Execute(sql);
        }

        public override int Delete(int id)
        {
            var sql = $"DELETE FROM Departamentos WHERE IdDepartamento = @id;";

            return Execute(sql, new { id });
        }

        public async override Task<IEnumerable<DepartamentosViewModel>> GetAll()
        {
            try
            {
                var sql = @$"SELECT [IdDepartamento]
                              ,[Nombre]
                              ,[Descripcion]
                              ,[Estado]
                              ,[Fecha]
                          FROM [dbo].[Departamentos] WITH (NOLOCK)";

                return await GetItems(sql);
            }
            catch
            {
                return new List<DepartamentosViewModel>();
            }
        }

        public async override Task<DepartamentosViewModel> GetItem(int id)
        {
            var sql = @$"SELECT [IdDepartamento]
                              ,[Nombre]
                              ,[Descripcion]
                              ,[Estado]
                              ,[Fecha]
                          FROM [Pets].[dbo].[Departamentos] WITH (NOLOCK)
                          WHERE [IdDepartamento] = @id";

            return await Get(sql, new { id });
        }

        public override int Update(DepartamentosViewModel item, int id)
        {
            var sql = $@"UPDATE Departamentos SET
                        Nombre='{item.Nombre}', 
                        Descripcion='{item.Descripcion}', 
                        Estado={GetEstado(item.Estado)}
                        WHERE [IdDepartamento] = @id";

            return Execute(sql, new { id });
        }
    }
}
