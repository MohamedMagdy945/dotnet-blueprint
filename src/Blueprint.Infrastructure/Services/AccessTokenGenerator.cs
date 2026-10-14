using Blueprint.Application.Common.Models;
using Microsoft.Extensions.Options;

namespace Blueprint.Infrastructure.Services;

public sealed class AccessTokenGenerator(IOptions<AccessTokenSettings> options) : IJwtGenerator
{

    public AccessTokenResult Generate(UserClaims userClaims)
    {

    }

}