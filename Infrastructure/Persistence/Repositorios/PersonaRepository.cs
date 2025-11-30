using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;

namespace Infrastructure.Persistence.Repositorios
{
    public class PersonaRepository: IPersonaRepository
    {
        private readonly ProjectContext _context;
        public PersonaRepository(ProjectContext context)
        {
            _context = context;
        }


        public async Task<Persona> AddAsync(Persona persona,Dictionary<int,DateTime?> situacionesTerapeuticas)
        {
            var maxNumeroIntegrante = await _context.Personas
                .Where(p => p.AfiliadoId == persona.AfiliadoId)
                .MaxAsync(p => (int?)p.NumeroIntegrante) ?? throw new InvalidOperationException("Inconsistencia de datos del afiliado y grupo familiar.");
            persona.NumeroIntegrante = maxNumeroIntegrante + 1;
            List<SituacionTerapeutica> situaciones = await _context.SituacionesTerapeuticas
                .Where(st => situacionesTerapeuticas.Keys.Contains(st.Id))
                .ToListAsync();
            persona.SituacionesTerapeuticas = situaciones.Select(st => new RegistroTerapeutico
            {
                SituacionTerapeuticaId = st.Id,
                FechaInicio = DateTime.Now,
                FechaFin = situacionesTerapeuticas[st.Id]
            }).ToList();
            await _context.Personas.AddAsync(persona);
            await _context.SaveChangesAsync();
            return persona;
        }


        public async Task<Persona> GetByIdAsync(int id)
        {
            var persona = await _context.Personas
                .Include(p => p.Telefonos)
                .Include(p => p.Emails)
                .Include(p => p.Documentacion)
                .Include(p => p.Direcciones)
                .Include(p => p.SituacionesTerapeuticas.Where(st => st.FechaFin == null || st.FechaFin > DateTime.Now.Date)).ThenInclude(rt => rt.SituacionTerapeutica)
                .FirstOrDefaultAsync(p => p.Id == id);
            return persona;
        }

        public async Task<bool> ToggleStatusAsync(int personaId, bool activo, DateTime? fecha)
        {
            var persona = await _context.Personas
                .Include(p => p.Afiliado)
                .FirstOrDefaultAsync(p => p.Id == personaId);

            if (persona == null)
                throw new KeyNotFoundException("Familiar no encontrado.");

            if (activo)
            {
                // ACTIVAR
                persona.Alta = fecha ?? DateTime.Now;
            }
            else
            {
                // DAR DE BAJA
                persona.Baja = fecha;
                // Si es baja programada, mantenemos el alta actual
                // Si es baja inmediata, no tocamos Alta
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Persona persona, Dictionary<int, DateTime?> situacionesTerapeuticas)
        {
            var existing = await _context.Personas
                .Include(p => p.Telefonos)
                .Include(p => p.Emails)
                .Include(p => p.Direcciones)
                .Include(p => p.Documentacion)
                .Include(p => p.SituacionesTerapeuticas)
                .ThenInclude(st => st.SituacionTerapeutica)
                .FirstOrDefaultAsync(p => p.Id == persona.Id);

            if (existing == null)
                throw new KeyNotFoundException("Persona no encontrada.");

            _context.Entry(existing).CurrentValues.SetValues(persona);

            existing.Telefonos.RemoveAll(t => !persona.Telefonos.Any(nt => nt.Numero == t.Numero));

            foreach (var nt in persona.Telefonos)
            {
                if (!existing.Telefonos.Any(et => et.Numero == nt.Numero))
                    existing.Telefonos.Add(new Telefono { Numero = nt.Numero });
            }

            existing.Emails.RemoveAll(e => !persona.Emails.Any(ne => ne.Correo == e.Correo));

            foreach (var ne in persona.Emails)
            {
                if (!existing.Emails.Any(ee => ee.Correo == ne.Correo))
                    existing.Emails.Add(new Email { Correo = ne.Correo });
            }

            existing.Direcciones.RemoveAll(d =>
                !persona.Direcciones.Any(nd =>
                    nd.Calle == d.Calle &&
                    nd.Altura == d.Altura &&
                    nd.Piso == d.Piso &&
                    nd.Departamento == d.Departamento));

            foreach (var nd in persona.Direcciones)
            {
                if (!existing.Direcciones.Any(ed =>
                    ed.Calle == nd.Calle &&
                    ed.Altura == nd.Altura &&
                    ed.Piso == nd.Piso &&
                    ed.Departamento == nd.Departamento))
                {
                    existing.Direcciones.Add(new Direccion
                    {
                        Calle = nd.Calle,
                        Altura = nd.Altura,
                        Piso = nd.Piso,
                        Departamento = nd.Departamento,
                        ProvinciaCiudad = nd.ProvinciaCiudad
                    });
                }
            }

            var situacionesIds = situacionesTerapeuticas.Keys.ToList();

            existing.SituacionesTerapeuticas.RemoveAll(st =>
                !situacionesIds.Contains(st.SituacionTerapeuticaId));

            foreach (var stId in situacionesIds)
            {
                var existente = existing.SituacionesTerapeuticas
                    .FirstOrDefault(x => x.SituacionTerapeuticaId == stId);

                if (existente == null)
                {
                    existing.SituacionesTerapeuticas.Add(new RegistroTerapeutico
                    {
                        SituacionTerapeuticaId = stId,
                        FechaInicio = DateTime.Now,
                        FechaFin = situacionesTerapeuticas[stId]
                    });
                }
                else
                {
                    existente.FechaFin = situacionesTerapeuticas[stId];
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
