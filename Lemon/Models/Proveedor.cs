using System.ComponentModel.DataAnnotations;

namespace Lemon.Models
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }

        [Display(Name = "Tipo de Documento")]
        [Required(ErrorMessage = "El campo tipo de documento no puede ser nulo")]
        public string TipoDocumento { get; set; } = null!;

        [Display(Name = "Documento")]
        [Required(ErrorMessage = "El campo documento es requerido")]
        [RegularExpression(@"^[0-9]{6,10}$", ErrorMessage = "El campo documento debe tener entre 6 y 10 digitos")]
        public int? Documento { get; set; }  

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo nombre no puede ser nulo")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "El debe tener entre 5 y 30 caracteres" )]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "El campo nombre no puede tener números ni caracteres especiales")]
        public string NombreRazonSocial { get; set; } = null!;

        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El campo correo no puede ser nulo")]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "El formato del correo eléctronico no es válido")]
        public string Correo { get; set; } = null!;

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo teléfono no puede ser nulo")]
        [RegularExpression(@"^[0-9]{7,10}$", ErrorMessage = "El telefono debe contener solo números y estar entre 7 y 10 caracteres")]
        public string Telefono { get; set; } = null!;

        public int Estado { get; set; }
    }
}
