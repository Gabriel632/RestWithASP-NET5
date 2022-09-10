using RestWithASPNET5.Data.VO;

namespace RestWithASPNET5.Business
{
    public interface ILoginBussines
    {
        TokenVO ValidateCredentials(UserVO user);
        TokenVO ValidateCredentials(TokenVO token);
        bool RevokeToken(string userName);
    }
}
