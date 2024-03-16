using System.Security.Claims;

namespace task_api.Models
{
    public class Jwt
    {
        public static dynamic getTokenBody(ClaimsIdentity identity)
        {
            try
            {
                if (identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "Verificar si se envía un token válido",
                        result = ""
                    };
                }

                var id = identity.Claims.FirstOrDefault(x => x.Type == "uid").Value;

                return new
                {
                    success = true,
                    message = "exito",
                    result = id
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Catch: " + ex.Message,
                    result = ""
                };
            }
        }
    }
}
