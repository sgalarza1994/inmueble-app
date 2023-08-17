using Microsoft.AspNetCore.Identity;
using NetKubernetes.Models;

namespace NetKubernetes.Data;

public class LoadData 
{
    public static async  Task InsertData(AppDbContext context, UserManager<Usuario> userManager)
    {
        if(!userManager.Users.Any())
        {
             var usuario = new Usuario 
             {
                 Nombre = "Steven",
                 Apellido = "Galarza",
                 Email = "stegalarza@hotmail.com",
                 UserName = "sgalarza",
                 Telefono = "979951482"
             };

             await userManager.CreateAsync(usuario,"$Steven1994");
        }
        if(!context.Inmuebles!.Any())
        {   
             context.Inmuebles!.AddRange(

                 new Inmueble 
                 {
                    Nombre = "Casa Playera",
                    Direccion = "Villamil",
                    Precio = 35000M,
                    FechaCreacion = DateTime.Now
                 },
                    new Inmueble 
                 {
                    Nombre = "Casa Sierra",
                    Direccion = "Banios",
                    Precio = 65000M,
                    FechaCreacion = DateTime.Now
                 }
             );
             context.SaveChanges();
        }
    }
}