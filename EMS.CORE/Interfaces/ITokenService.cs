using EMS.CORE.Entities;

namespace EMS.CORE.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUserEntity user, IList<string> roles);
    }
}