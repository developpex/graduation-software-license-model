using LicenseManager.Domain.Interfaces;
using LicenseManager.Domain.Models;

namespace LicenseManager.Domain.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICertificateService _certificateService;

    public UserService(IUserRepository userRepository, ICertificateService certificateService)
    {
        _userRepository = userRepository;
        _certificateService = certificateService;
    }

    public async Task<User> Login(string email, string password)
    {
        var license = await _certificateService.GetCertificate();
        return await _userRepository.Login(email, password, license);
    }
}