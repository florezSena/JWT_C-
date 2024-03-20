using System.ComponentModel.DataAnnotations;

namespace Lemon.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El campo correo no puede ser nulo")]
        [StringLength(255, ErrorMessage = "El campo correo no puede tener más de 255 caracteres")]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "El formato del correo electrónico no es válido(no debe contener espacios)")]
        public string Correo { get; set; } = null!;
        [Display(Name = "Nombre o usuario")]
        [Required(ErrorMessage = "El campo nombre o usuario no puede ser nulo")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "El nombre o usuario debe tener entre 5 y 30 caracteres")]
        [RegularExpression(@"^[A-Za-zñÑ]+(?:\s[A-Za-zñÑ]+)*$", ErrorMessage = "El nombre o usuario solo puede contener letras y sin espacios consecutivos")]
        public string NombreUsuario { get; set; } = null!;
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo contraseña no puede ser nulo")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 15 caracteres")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9\s])(?!.*\s).{8,15}$", ErrorMessage = "La contraseña no debe contener espacios, debe tener al menos 1 mayúscula, 1 número, 1 carácter especial, tener entre 8 y 15 caracteres")]
        public string Contraseña { get; set; } = null!;
        public int IdRol { get; set; }
        public int Estado { get; set; }
        public virtual Rol? IdRolNavigation { get; set; } = null!;

        public void SetPassword(string password)
        {
            Contraseña = PasswordEncoder.EncodePassword(password);
        }

        public bool VerifyPassword(string password)
        {
            return PasswordEncoder.VerifyPassword(password, Contraseña);
        }
    }
}
