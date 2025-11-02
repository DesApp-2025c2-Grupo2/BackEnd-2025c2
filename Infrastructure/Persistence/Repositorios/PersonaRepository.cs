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


        public async Task AddAsync(Persona persona)
        {
            await _context.Personas.AddAsync(persona);
        }

        public async Task<List<Persona>> GetByAfiliadoIdAsync(int afiliadoId)
        {
            return await _context.Personas
            .Where(p => p.AfiliadoId == afiliadoId)
            .ToListAsync();
        }


        public async Task<Persona> GetByIdAsync(int id)
        {
            return await _context.Personas
                .Include(p => p.Telefonos)
                .Include(p => p.Emails)
                .Include(p => p.Documentacion)
                .Include(p => p.Direcciones)
                // incluimos las situaciones terapeuticas asociadas siempre y cuando la FechaFin sea null o mayor a la sysdate, obviamente truncamos las fechas para que ignoren el horario
                .Include(p => p.SituacionesTerapeuticas.Where(st => st.FechaFin == null || st.FechaFin > DateTime.Now.Date))
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Persona>> GetAllAsync()
        {
            return await _context.Personas
                .Include(p => p.Telefonos)
                .Include(p => p.Emails)
                .Include(p => p.Documentacion)
                .Include(p => p.Direcciones)
                .ToListAsync();
        }



        public async Task UpdateAsync(Persona persona)
        {
            _context.Personas.Update(persona);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
