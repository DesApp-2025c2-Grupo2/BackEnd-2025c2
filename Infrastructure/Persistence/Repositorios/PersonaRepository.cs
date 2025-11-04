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
                .Include(p => p.SituacionesTerapeuticas.Where(st => st.FechaFin == null || st.FechaFin > DateTime.Now.Date))
                .FirstOrDefaultAsync(p => p.Id == id);
            return persona;
        }

        public async Task<bool> ToggleStatusAsync(int id, DateTime? fecha)
        {
            var persona = await _context.Personas.FirstOrDefaultAsync(p => p.Id == id);
            if (persona == null) throw new KeyNotFoundException("Persona no encontrada.");

            if (persona.Baja == null)
            {
                persona.Baja = fecha ?? DateTime.Now.Date;
            }
            else
            {
                persona.Baja = null;
                persona.Alta = fecha ?? DateTime.Now.Date;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Persona persona, Dictionary<int, DateTime?> situacionesTerapeuticas)
        {
            var existingPersona = await _context.Personas
                .Include(p => p.Telefonos)
                .Include(p => p.Emails)
                .Include(p => p.Direcciones)
                .Include(p => p.Documentacion)
                .Include(p => p.SituacionesTerapeuticas.Where(st => st.FechaFin == null || st.FechaFin > DateTime.Now.Date)).ThenInclude(st => st.SituacionTerapeutica)
                .FirstOrDefaultAsync(p => p.Id == persona.Id);
            if (existingPersona == null) throw new KeyNotFoundException("Persona no encontrada.");
            _context.Entry(existingPersona).CurrentValues.SetValues(persona);
            existingPersona.Telefonos.Clear();
            foreach (var telefono in persona.Telefonos)
            {
                existingPersona.Telefonos.Add(telefono);
            }
            existingPersona.Emails.Clear();
            foreach (var email in persona.Emails)
            {
                existingPersona.Emails.Add(email);
            }
            existingPersona.Direcciones.Clear();
            foreach (var direccion in persona.Direcciones)
            {
                existingPersona.Direcciones.Add(direccion);
            }
            existingPersona.SituacionesTerapeuticas.Clear();
            List<SituacionTerapeutica> situaciones = await _context.SituacionesTerapeuticas
                .Where(st => situacionesTerapeuticas.Keys.Contains(st.Id))
                .ToListAsync();

            existingPersona.SituacionesTerapeuticas = situaciones.Select(st => new RegistroTerapeutico
            {
                SituacionTerapeuticaId = st.Id,
                FechaInicio = DateTime.Now.Date,
                FechaFin = situacionesTerapeuticas[st.Id]
            }).ToList();
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
