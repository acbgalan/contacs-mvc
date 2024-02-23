using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contacts.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "{0} tiene un tamaño máximo de {1} caracteres")]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "{0} tiene un tamaño máximo de {1} caracteres")]
        public string Alias { get; set; }

        [Phone]
        [DisplayName("Teléfono")]
        public string Phone { get; set; }

        [EmailAddress]
        [DisplayName("Correo electrónico")]
        public string Email { get; set; }

        [StringLength(500, ErrorMessage = "{0} tiene un tamaño máximo de {1} caracteres")]
        [DisplayName("Notas")]
        public string Notes { get; set; }

        public List<Group> Groups { get; set; }
        public List<Website> Websites { get; set; }
    }
}
