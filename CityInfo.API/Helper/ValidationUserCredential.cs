using CityInfo.API.Models;

namespace CityInfo.API.Helper;

public static class ValidationUserCredentials
{
    public static CityInfoUser ValidationUserCredential(string? username, string? password)
    {
        return new CityInfoUser(1, username ?? "", "jaleh", "hosseini", "tehran");
    }
}
