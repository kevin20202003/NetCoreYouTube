using System.Collections.Generic;

namespace NetCoreYouTube.Models
{
    public class Usuario
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }

        // Método estático para obtener una lista de usuarios de ejemplo
        public static List<Usuario> Db()
        {
            var list = new List<Usuario>
            {
                new Usuario
                {
                    Id = "1",
                    Nombre = "Kevin",
                    Password = "123",
                    Rol = "empleado"
                },
                new Usuario
                {
                    Id = "2",
                    Nombre = "Juan",
                    Password = "123",
                    Rol = "empleado"
                },
                new Usuario
                {
                    Id = "3",
                    Nombre = "Marcos",
                    Password = "123",
                    Rol = "asesor"
                },
                new Usuario
                {
                    Id = "4",
                    Nombre = "Lucas",
                    Password = "123",
                    Rol = "administrador"
                }
            };

            return list;
        }
    }
}
