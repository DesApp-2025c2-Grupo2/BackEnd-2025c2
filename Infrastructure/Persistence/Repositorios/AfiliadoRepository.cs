using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;


namespace Infrastructure.Persistence.Repositorios
{
    public class AfiliadoRepository : IAfiliadoRepository
    {
        private readonly ProjectContext _context;
        public AfiliadoRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task<Afiliado> GetByIdAsync(int id)
        {
            return await _context.Afiliados
                .Include(a => a.Integrantes)
                    .ThenInclude(p => p.Telefonos)
                .Include(a => a.Integrantes)
                    .ThenInclude(p => p.Emails)
                .Include(a => a.Integrantes)
                    .ThenInclude(p => p.Documentacion)
                .Include(a => a.Integrantes)
                    .ThenInclude(p => p.Direcciones)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Afiliado> GetByNumeroAsync(int numeroAfiliado)
        {
            return await _context.Afiliados
                .Include(a => a.Integrantes)
                    .ThenInclude(p => p.Telefonos)
                .Include(a => a.Integrantes)
                    .ThenInclude(p => p.Emails)
                .Include(a => a.Integrantes)
                    .ThenInclude(p => p.Documentacion)
                .Include(a => a.Integrantes)
                    .ThenInclude(p => p.Direcciones)
                .FirstOrDefaultAsync(a => a.NumeroAfiliado == numeroAfiliado);
        }

        public async Task AddAsync(Afiliado afiliado)
        {
            await _context.Afiliados.AddAsync(afiliado);
        }

        public async Task<IEnumerable<Afiliado>> GetAllAsync()
        {
            return await _context.Afiliados
                .Include(a => a.Integrantes)
                    .ThenInclude(p => p.Telefonos)
                .Include(a => a.Integrantes)
                    .ThenInclude(p => p.Emails)
                .Include(a => a.Integrantes)
                    .ThenInclude(p => p.Documentacion)
                .Include(a => a.Integrantes)
                    .ThenInclude(p => p.Direcciones)
                .ToListAsync();
        }
        public async Task UpdateAsync(Afiliado afiliado)
        {
            _context.Afiliados.Update(afiliado);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
