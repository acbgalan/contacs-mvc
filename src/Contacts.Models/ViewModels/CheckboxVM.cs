using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Models.ViewModels
{
    public class CheckboxVM
    {
        public int Id { get; set; }
        public string LabelName { get; set; }
        public bool IsChecked { get; set; }
    }
}
