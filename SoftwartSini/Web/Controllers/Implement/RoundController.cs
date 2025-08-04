using Business.Interfaz;
using Entity.DTO;
using Entity.DTO.BaseDTO;
using Entity.Model;
using Entity.Model.Game;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Controllers.Implements;
using Web.Controllers.Interface;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoundController : BaseController<RoundDTO, Round>
    {
        public RoundController(IBaseBusiness<Round, RoundDTO> business, ILogger<RoundController> logger)
            : base(business, logger)
        {
        }

        protected override int GetEntityId(RoundDTO dto)
        {
            return dto.Id; // Asegúrate que tu DTO tenga esta propiedad
        }
    }
}
