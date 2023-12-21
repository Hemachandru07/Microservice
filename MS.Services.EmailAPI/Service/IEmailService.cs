using MS.Services.EmailAPI.Models.Dto;

namespace MS.Services.EmailAPI.Service
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task RegisterEmailAndLog(string email);
    }
}
