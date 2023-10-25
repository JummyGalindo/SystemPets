﻿using SysPet.Models;

namespace SysPet.Data
{
    public class UsersData : DataAccessBase<PersonasViewModel>
    {
        public override int Create(PersonasViewModel item)
        {
            var sql = $@"INSERT INTO Personas VALUES (
                        '{item.Nombre}', 
                        '{item.Apellidos}', 
                        '{item.Ciudad}', 
                        '{item.CodigoPostal}', 
                        '{item.Telefono}', 
                        1)";

            return Execute(sql);
        }

        public override int Delete(int id)
        {
            var sql = $"DELETE FROM Personas WHERE IdPersona = @id;";

            return Execute(sql, new { id });
        }

        public async override Task<IEnumerable<PersonasViewModel>> GetAll()
        {
            try
            {
                var sql = @$"SELECT [IdPersona],[Nombre],[Apellidos],[Ciudad],[CodigoPostal],[Telefono],[Estado]
                        FROM [dbo].[Personas]";

                return await GetItems(sql);
            }
            catch (Exception)
            {
                return new List<PersonasViewModel>();
            }
        }

        public async override Task<PersonasViewModel> GetItem(int id)
        {
            var sql = @$"SELECT [IdPersona],[Nombre],[Apellidos],[Ciudad],[CodigoPostal],[Telefono],[Estado]
                        FROM [dbo].[Personas] WHERE IdPersona = @id";
            return await Get(sql, new { id });
        }

        public override int Update(PersonasViewModel item, int id)
        {
            var sql = $@"UPDATE Personas SET 
                        Nombre='{item.Nombre}', 
                        Apellidos='{item.Apellidos}', 
                        Ciudad='{item.Ciudad}', 
                        CodigoPostal='{item.CodigoPostal}', 
                        Telefono='{item.Telefono}', 
                        Estado={GetEstado(item.Estado)}
                        WHERE IdPersona = @id";

            return Execute(sql, new { id });
        }
    }
}