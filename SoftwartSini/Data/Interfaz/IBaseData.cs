using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model.Base;


namespace Data.Interfaz
{
    public interface IBaseData<T> where T : BaseModel
    {
        Task<T> CreateAsync(T entity);
        Task<List<T>> GetAll();
        Task<bool> DeleteAsync(int id);
        Task<T> UpdateAsync(T entity);
        Task<T> GetByIdAsync(int id);  
    }
}
