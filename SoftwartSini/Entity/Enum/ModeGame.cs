using System.ComponentModel.DataAnnotations;

namespace Entity.Enum
{
    public enum ModeGame
    {
        [Display(Name = "Muerte Súbita")]
        Entrada = 1,

        [Display(Name = "Partida Normal")]
        Salida = 2,
    }
}
