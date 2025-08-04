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
    public class CardController : BaseController<CardDTO, Card>
    {
        public CardController(IBaseBusiness<Card, CardDTO> business, ILogger<CardController> logger)
            : base(business, logger)
        {
        }

        protected override int GetEntityId(CardDTO dto)
        {
            return dto.Id; // Asegúrate que tu DTO tenga esta propiedad
        }
    }
}
