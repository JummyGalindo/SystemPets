using SysPet.Models;

namespace SysPet.Data
{
    public class CostumerDetailData : DataAccessBase<CostumerDetailViewModel>
    {
        public override int Create(CostumerDetailViewModel item)
        {
            throw new NotImplementedException();
        }

        public override int Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<CostumerDetailViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<CostumerDetailViewModel> GetCostumerDetail(int id)
        {
            try
            {
                var costumer = new CostumerDetailViewModel();
                var appointmentList = new List<CitasViewModel>();
                var petList = new List<MascotasViewModel>();

                var sql = @$"SELECT p.[IdPersona]
                              ,p.[Nombre]
                              ,p.[Direccion]
                              ,p.[Cuidad] Ciudad
                              ,p.[CodigoPostal]
                              ,p.[Telefono]
                              ,p.[Estado] EstadoPersona
                              ,p.[ApellidoPaterno]
                              ,p.[ApellidoMaterno]
	                          ,pa.[Nombre] Paciente
	                          ,pa.[Color]
	                          ,pa.[Raza]
	                          ,pa.[Peso]
	                          ,pa.[Sexo]
	                          ,pa.Imagen
                              ,pa.TipoContenido
                          FROM [dbo].[Personas] p WITH (NOLOCK)
                          INNER JOIN [dbo].[Pacientes] pa WITH (NOLOCK) on pa.IdPersona = p.IdPersona
                          WHERE p.IdTipoPersona = 2 AND p.IdPersona = @id AND p.Estado = 1 AND pa.Estado = 1";

                var personsWithPatiens =  await GetItems(sql, new { id });
                if (personsWithPatiens != null && personsWithPatiens.Any())
                {
                    foreach (var item in personsWithPatiens)
                    {
                        costumer.IdPersona = item.IdPersona;
                        costumer.Nombre = item.Nombre;
                        costumer.Direccion = item.Direccion;
                        costumer.Ciudad = item.Ciudad;
                        costumer.CodigoPostal = item.CodigoPostal;
                        costumer.Telefono = item.Telefono;
                        costumer.EstadoPersona = item.EstadoPersona;
                        costumer.Estado = item.EstadoPersona ? "Activo" : "Inactivo";
                        costumer.ApellidoMaterno = item.ApellidoMaterno;
                        costumer.ApellidoPaterno = item.ApellidoPaterno;
                        costumer.FullName = $"{item.Nombre} {item.ApellidoPaterno} {item.ApellidoMaterno}";
                        petList.Add(new MascotasViewModel
                        {
                            IdPersona = item.IdPersona,
                            Nombre = item.Paciente,
                            Color = item.Color,
                            Raza = item.Raza,
                            Peso = item.Peso,
                            Sexo = item.Sexo,
                            Imagen = item.Imagen,
                            TipoContenido = item.TipoContenido,
                        });

                    }
                    costumer.Pacientes = petList;
                }

                var query = @$"SELECT p.[IdPersona]
                              ,p.[Nombre]
                              ,p.[Direccion]
                              ,p.[Cuidad] Ciudad
                              ,p.[CodigoPostal]
                              ,p.[Telefono]
                              ,p.[Estado] EstadoPersona
                              ,p.[ApellidoPaterno]
                              ,p.[ApellidoMaterno]
	                          ,c.[Id] IdCita
	                          ,c.[FechaCita]
	                          ,c.[Motivo]
	                          ,ec.[Nombre] EstadoCita
                          FROM [dbo].[Personas] p WITH (NOLOCK)
                          INNER JOIN [dbo].[Citas] c WITH (NOLOCK) on c.IdPersona = p.IdPersona
                          INNER JOIN [dbo].[EstadoCitas] ec WITH (NOLOCK) on ec.Id = c.IdEstado
                          WHERE p.IdTipoPersona = 2 AND p.IdPersona = @id AND p.Estado = 1";

                var personsWithAppointments = await GetItems(query, new { id });
                if (personsWithAppointments != null && personsWithAppointments.Any())
                {
                    foreach (var item in personsWithAppointments)
                    {
                        costumer.IdPersona = item.IdPersona;
                        costumer.Nombre = item.Nombre;
                        costumer.Direccion = item.Direccion;
                        costumer.Ciudad = item.Ciudad;
                        costumer.CodigoPostal = item.CodigoPostal;
                        costumer.Telefono = item.Telefono;
                        costumer.EstadoPersona = item.EstadoPersona;
                        costumer.Estado = item.EstadoPersona ? "Activo" : "Inactivo";
                        costumer.ApellidoMaterno = item.ApellidoMaterno;
                        costumer.ApellidoPaterno = item.ApellidoPaterno;
                        costumer.FullName = $"{item.Nombre} {item.ApellidoPaterno} {item.ApellidoMaterno}";
                        appointmentList.Add(new CitasViewModel
                        {
                            IdPersona = item.IdPersona,
                            Id = item.IdCita,
                            FechaCita = item.FechaCita,
                            Motivo = item.Motivo,
                            Estado = item.EstadoCita
                        });

                    }
                    costumer.Citas = appointmentList;
                }

                costumer.Citas = costumer.Citas ?? new List<CitasViewModel>();
                costumer.Pacientes = costumer.Pacientes ?? new List<MascotasViewModel>();

                return costumer;
            }
            catch
            {
                return new CostumerDetailViewModel();
            }
        }

        public override Task<CostumerDetailViewModel> GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public override int Update(CostumerDetailViewModel item, int id)
        {
            throw new NotImplementedException();
        }
    }
}
