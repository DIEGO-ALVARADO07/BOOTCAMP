using AutoMapper;
using Data.Interfaz;
using Entity.DTO.BaseDTO;
using Entity.Model.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implement
{
    public class BaseBusiness<T, D> : ABaseBusiness<T, D> where T : BaseModel where D : BaseDTO
    {
        protected readonly IMapper _mapper;
        protected readonly IBaseData<T> _data;
        protected readonly ILogger<BaseBusiness<T, D>> _logger;


        public BaseBusiness(IBaseData<T> data, IMapper mapper, ILogger<BaseBusiness<T, D>> logger) : base()
        {
            _data = data;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task<List<D>> GetAllAsync()
        {
            try
            {
                var entities = await _data.GetAll();
                _logger.LogInformation($"Obteniendo todos los registros de {typeof(T).Name}");
                return _mapper.Map<IList<D>>(entities).ToList();
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener registros de {typeof(T).Name}: {ex.Message}");
                throw;
            }
        }

        public override async Task<D> GetByIdAsync(int id)
        {
            try
            {
                var entities = await _data.GetByIdAsync(id);
                if (entities == null)
                {
                    _logger.LogWarning($"No se encontró {typeof(T).Name} con ID {id}");
                    return null;
                }
                _logger.LogInformation($"Obteniendo {typeof(T).Name} con ID {id}");
                return _mapper.Map<D>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener {typeof(T).Name} con ID {id}: {ex.Message}");
                throw;
            }
        }

        public override async Task<D> CreateAsync(D dto)
        {
            try
            {
                var entity = _mapper.Map<T>(dto);
                entity = await _data.CreateAsync(entity);
                _logger.LogInformation($"Creando nuevo {typeof(T).Name}");
                return _mapper.Map<D>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear {typeof(T).Name} desde DTO: {ex.Message}");
                throw;
            }
        }

        public override async Task<D> UpdateAsync(D dto)
        {
            try
            {
                var entity = _mapper.Map<T>(dto);
                entity = await _data.UpdateAsync(entity);
                _logger.LogInformation($"Actualizando {typeof(T).Name} desde DTO");
                return _mapper.Map<D>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear {typeof(D).Name} desde DTO: {ex.Message}");
                throw;
            }

        }

        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Eliminando {typeof(T).Name} con ID: {id}");
                return await _data.DeleteAsync(id);
            }
            catch (Exception ex){
                _logger.LogError($"Error al eliminar a {typeof(T).Name}: {ex.Message}");
                throw;  
            }
        }

    }
}