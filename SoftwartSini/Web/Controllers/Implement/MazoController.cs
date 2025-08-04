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
    public class MazoController : BaseController<MazoDTO, Mazo>
    {
        public MazoController(IBaseBusiness<Mazo, MazoDTO> business, ILogger<MazoController> logger)
            : base(business, logger)
        {
        }

        protected override int GetEntityId(MazoDTO dto)
        {
            return dto.Id; // Asegúrate que tu DTO tenga esta propiedad
        }
    }
}
