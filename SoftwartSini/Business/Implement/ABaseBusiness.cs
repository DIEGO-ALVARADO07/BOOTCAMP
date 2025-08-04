using Business.Interfaz;
using Entity.DTO.BaseDTO;
using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implement
{
    public abstract class ABaseBusiness<T,D> : IBaseBusiness<T,D> where T : BaseModel where D : BaseDTO
    {
        public abstract Task<List<D>> GetAllAsync();
        public abstract Task<D> GetByIdAsync(int id);
        public abstract Task<D> CreateAsync(D dto);
        public abstract Task<D> UpdateAsync(D dto);
        public abstract Task<bool> DeleteAsync(int id);
    }
}
