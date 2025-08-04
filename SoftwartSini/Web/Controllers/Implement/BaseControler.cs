using Business.Interfaz;
using Entity.DTO.BaseDTO;
using Entity.Model.Base;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implements
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TDto, TEntity> : ControllerBase where TEntity : BaseModel where TDto : BaseDTO
    {
        protected readonly IBaseBusiness<TEntity, TDto> _business;
        protected readonly ILogger<BaseController<TDto, TEntity>> _logger;

        protected BaseController(IBaseBusiness<TEntity, TDto> business, ILogger<BaseController<TDto, TEntity>> logger)
        {
            _business = business;
            _logger = logger;
        }


        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            try
            {
                var entities = await _business.GetAllAsync();
                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener todos los registros: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            try
            {
                var entity = await _business.GetByIdAsync(id);
                if (entity == null)
                    return NotFound($"Registro con ID {id} no encontrado");

                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener registro con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdEntity = await _business.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = GetEntityId(createdEntity) }, createdEntity);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al crear registro: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear registro: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update([FromBody] TDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedEntity = await _business.UpdateAsync(dto);
                return Ok(updatedEntity);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar registro: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar registro: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _business.DeleteAsync(id);
            if (!result)
                return NotFound($"Registro con ID {id} no encontrado");

            return Ok(new { Success = true, Message = "Eliminado permanentemente" });
        }


        // Método abstracto para obtener el ID de la entidad creada para el CreatedAtAction
        protected abstract int GetEntityId(TDto dto);
    }
}