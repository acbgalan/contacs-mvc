using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Contacts.Models;
using Microsoft.EntityFrameworkCore;

namespace Contacts.DataAccess.Data
{
    internal static class ApplicationSeeding
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasData(
                new Group { Id = 1, Name = "Contactos de emergencia" },
                new Group { Id = 2, Name = "Amigos" },
                new Group { Id = 3, Name = "Empresa" },
                new Group { Id = 4, Name = "Familia" },
                new Group { Id = 5, Name = "Otros" }
            );

        }
    }
}
