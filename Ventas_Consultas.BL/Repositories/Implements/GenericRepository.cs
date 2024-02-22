using Microsoft.EntityFrameworkCore;
using Ventas_consultas.BL.Repositories;
using Ventas_Consultas.BL.Data;

namespace Ventas_Consultas.BL.Repositories.Implements
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly Ventas_ConsultasContext ventas_consultasContext;


        public GenericRepository(Ventas_ConsultasContext ventas_consultasContext)
        {
            this.ventas_consultasContext = ventas_consultasContext;
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);

            if (entity == null)
                throw new Exception("The entity is null");

            ventas_consultasContext.Set<TEntity>().Remove(entity);
            await ventas_consultasContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await ventas_consultasContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await ventas_consultasContext.Set<TEntity>().FindAsync(id) ?? throw new Exception($"Entity with id {id} not found.");
        }

        public async Task<TEntity> Insert(TEntity entity)
        {
            ventas_consultasContext.Set<TEntity>().Add(entity);
            await ventas_consultasContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            ventas_consultasContext.Entry(entity).State = EntityState.Modified;
            await ventas_consultasContext.SaveChangesAsync();
            return entity;
        }

    }
}
