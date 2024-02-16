﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contacts.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "{0} tiene un tamaño máximo de {1} caracteres")]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        public List<Contact> Contacts { get; set; }
    }
}
