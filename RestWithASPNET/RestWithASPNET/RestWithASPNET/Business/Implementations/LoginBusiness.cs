using RestWithASPNET.Configurations;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Repository;
using RestWithASPNET.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestWithASPNET.Business.Implementations
{
    public class LoginBusiness : ILoginBusiness
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfiguration _configuration;
        private IUserRepository _repository;
        private readonly ITokenService _tokenService;

        public LoginBusiness(TokenConfiguration configuration, IUserRepository repository, ITokenService tokenService)
        {
            _configuration = configuration;
            _repository = repository;
            _tokenService = tokenService;
        }

        public TokenVO ValidateCredentials(UserVO userVO)
        {
            var user = _repository.ValidateCredentials(userVO);
            if (user == null) return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpiry);

            _repository.RefreshUserInfo(user);

            DateTime createdAt = DateTime.Now;
            DateTime expirationDate = createdAt.AddMinutes(_configuration.Minutes);

            return new TokenVO(
                true,
                createdAt.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
                );
        }

        public TokenVO ValidateCredentials(TokenVO tokenVO)
        {
            var accessToken = tokenVO.AccessToken;
            var refreshToken = tokenVO.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            var userName = principal.Identity.Name;

            var user = _repository.ValidateCredentials(userName);

            if (user == null ||
                user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now) return null;

            accessToken = _tokenService.GenerateAccessToken(principal.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            _repository.RefreshUserInfo(user);

            DateTime createdAt = DateTime.Now;
            DateTime expirationDate = createdAt.AddMinutes(_configuration.Minutes);

            return new TokenVO(
                true,
                createdAt.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
                );
        }

        public bool RevokeToken(string UserName)
        {
            return _repository.RevokeToken(UserName);
        }
    }
}
