
using System.Threading.Tasks;

namespace TaskMgntAPI.Services
{
    public interface IEmailService
    {
        Task<string> GetUserEmailByIdAsync(string userId);
        Task SendEmailAsync(string toEmail, string subject, string body, string fromEmail = null);
    }

}