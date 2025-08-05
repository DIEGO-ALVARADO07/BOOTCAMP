using Data.Interfaz;
using Entity.Context;
using Entity.Model.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations
{
    public abstract class ABaseData<T> : IBaseData<T> where T : BaseModel
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected ABaseData(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public abstract Task<T> CreateAsync(T entity);
        public abstract Task<List<T>> GetAll();
        public abstract Task<bool> DeleteAsync(int id);
        public abstract Task<T> UpdateAsync(T entity);
        public abstract Task<T> GetByIdAsync(int id);
    }
}
