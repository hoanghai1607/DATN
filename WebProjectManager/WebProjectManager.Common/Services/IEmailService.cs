using System;
using System.Threading.Tasks;

namespace WebProjectManager.Common.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string htmlBody);
        Task SendEmailAsync(string[] to, string subject, string htmlBody);
    }
}