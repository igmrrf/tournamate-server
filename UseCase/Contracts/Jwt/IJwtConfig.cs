
using Domain.Entities;

namespace Infrastructure.InternalServices.Jwt
{
    public interface IJwtConfig
    {
          Task<string> GenerateJwtAsync(User user);
    }
}
