using System.ComponentModel.DataAnnotations;

namespace Lemon.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }

        [Display(Name = "Tipo de documento")]
        [Required(ErrorMessage = "El campo tipo de documento no puede ser nulo")]
        public string TipoDocumento { get; set; } = null!;

        [Display(Name = "Documento")]
        [Required(ErrorMessage = "El campo documento es requerido")]
        [RegularExpression(@"^[1-9][0-9]{6,10}$", ErrorMessage = "El campo documento debe tener entre 6 y 10 dígitos(el primer digito no puede ser 0)")]
        public int? Documento { get; set; }

        [Display(Name = "Nombre o Razón Social")]
        [Required(ErrorMessage = "El campo nombre o razón social no puede ser nulo")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El nombre o razón social debe tener entre 5 y 100 caracteres")]
        [RegularExpression(@"^[A-Za-zñÑ]+(?:\s[A-Za-zñÑ]+)*$", ErrorMessage = "El nombre o razón social solo puede contener letras y sin espacios consecutivos")]
        public string NombreRazonSocial { get; set; } = null!;


        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El campo correo no puede ser nulo")]
        [StringLength(255, ErrorMessage = "El campo correo no puede tener más de 255 caracteres")]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "El formato del correo electrónico no es válido(no debe contener espacios)")]
        public string Correo { get; set; } = null!;

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo teléfono no puede ser nulo")]
        [RegularExpression(@"^(?!(?:[^0]*0){4,})[1-9][0-9]{6,9}$", ErrorMessage = "El teléfono no puede comenzar con 0 y debe tener entre 7 y 10 dígitos(Solo numeros, tampoco 0 consecutivos)")]

        public string Telefono { get; set; } = null!;

        public int Estado { get; set; }
    }
}
