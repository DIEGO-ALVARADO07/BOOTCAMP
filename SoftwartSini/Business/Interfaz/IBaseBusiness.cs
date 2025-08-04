using Entity.DTO.BaseDTO;
using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaz
{
    public interface IBaseBusiness<T,D> where T : BaseModel where D : BaseDTO
    {
        Task <D> GetByIdAsync(int id);
        Task<List<D>> GetAllAsync();
        Task<D> CreateAsync(D dto);
        Task<D> UpdateAsync(D dto);
        Task<bool> DeleteAsync(int id);
    }
}
