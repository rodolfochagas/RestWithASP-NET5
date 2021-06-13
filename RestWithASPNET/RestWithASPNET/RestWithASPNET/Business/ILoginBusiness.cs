using RestWithASPNET.Data.VO;

namespace RestWithASPNET.Business
{
    public interface ILoginBusiness
    {
        TokenVO ValidateCredentials(UserVO userVO);
        TokenVO ValidateCredentials(TokenVO tokenVO);
        bool RevokeToken(string UserName);
    }
}
