using ApplicationCore.Utils;
using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceUsuario
    {
        public USUARIO GetUsuario(string email, string password)
        {
            RepositoryUsuario repository = new RepositoryUsuario();

            // Encriptar el password para poder compararlo
             string crytpPasswd = Cryptography.EncrypthAES(password);

             return repository.GetUsuario(email, crytpPasswd); //ESTA ES LA QUE SE USA

            //return repository.GetUsuario(email, password);
        }

        public USUARIO GetUsuarioByID(int id)
        {
            RepositoryUsuario repository = new RepositoryUsuario();
            USUARIO oUsuario = repository.GetUsuarioByID(id);
            oUsuario.contrasenha = Cryptography.DecrypthAES(oUsuario.contrasenha); //ESTA ES LA QUE SE USA
            oUsuario.contrasenha = oUsuario.contrasenha;

            return oUsuario;
        }

        public USUARIO Save(USUARIO usuario)
        {
            RepositoryUsuario repository = new RepositoryUsuario();
            usuario.contrasenha = Cryptography.EncrypthAES(usuario.contrasenha);
            return repository.Save(usuario);
        }

        public ICollection<USUARIO> GetUsuariosEncargados(){
            return new RepositoryUsuario().GetUsuariosEncargados();
        }
        
    }
}

