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
    public class Website
    {
        public int Id { get; set; }

        [Url]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido")]
        [StringLength(100, ErrorMessage = "{0} tiene un tamaño máximo de {1} caracteres")]
        [DisplayName("URL")]
        public string Url { get; set; }

        [StringLength(250, ErrorMessage = "{0} tiene un tamaño máximo de {1} caracteres")]
        [DisplayName("Descripción")]
        public string Description { get; set; }

        [ForeignKey("Contact")]
        public int ContactId { get; set; }

        public Contact Contact { get; set; }
    }
}
