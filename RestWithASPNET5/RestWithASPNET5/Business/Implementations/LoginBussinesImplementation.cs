using Microsoft.IdentityModel.JsonWebTokens;
using RestWithASPNET5.Configurations;
using RestWithASPNET5.Data.VO;
using RestWithASPNET5.Repository;
using RestWithASPNET5.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace RestWithASPNET5.Business.Implementations
{
    public class LoginBussinesImplementation : ILoginBussines
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfiguration _configuration;

        private readonly IUserRepository _repository;
        private readonly ITokenService _tokenService;

        public LoginBussinesImplementation(TokenConfiguration configuration, IUserRepository repository, ITokenService tokenService)
        {
            _configuration = configuration;
            _repository = repository;
            _tokenService = tokenService;
        }

        public TokenVO ValidateCredentials(UserVO userCredential)
        {
            var user = _repository.ValidateCredentials(userCredential);
            if (user == null) return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpiry);

            _repository.RefreshUserInfo(user);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            return new TokenVO(
                authenticated: true,
                created: createDate.ToString(DATE_FORMAT),
                expiration: expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
            );
        }

        public TokenVO ValidateCredentials(TokenVO token)
        {
            var accessToken = token.AccessToken;
            var refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            var userName = principal.Identity.Name;

            var user = _repository.ValidateCredentials(userName);
            if (
                user == null ||
                user.RefreshToken != refreshToken || 
                user.RefreshTokenExpiryTime <= DateTime.Now
            ) return null;

            accessToken = _tokenService.GenerateAccessToken(principal.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            _repository.RefreshUserInfo(user);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            return new TokenVO(
                authenticated: true,
                created: createDate.ToString(DATE_FORMAT),
                expiration: expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
            );
        }

        public bool RevokeToken(string userName)
        {
            return _repository.RevokeToken(userName);
        }
    }
}
