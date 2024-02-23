using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Models.ViewModels
{
    public class ContactVM
    {
        public Contact Contact { get; set; }
        public IEnumerable<CheckboxVM> GroupCheckList { get; set; }
        public IEnumerable<int> CheckedGroups { get; set; }
    }
}
