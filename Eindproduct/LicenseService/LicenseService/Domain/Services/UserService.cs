using LicenseService.Domain.Interfaces;

namespace LicenseService.Domain.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;

    public UserService(IUserRepository userRepository, ITokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _userRepository.Login(email, password);
        return _tokenGenerator.GenerateAccessToken(user);
    }
}