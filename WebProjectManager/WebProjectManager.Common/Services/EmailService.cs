using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

namespace WebProjectManager.Common.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            
            try
            {
                // Lấy config từ appsettings
                _smtpServer = _configuration["EmailSettings:SmtpServer"];
                _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                _smtpUsername = _configuration["EmailSettings:SmtpUsername"];
                _smtpPassword = _configuration["EmailSettings:SmtpPassword"];
                _senderEmail = _configuration["EmailSettings:SenderEmail"];
                _senderName = _configuration["EmailSettings:SenderName"];
                
                // Debug.WriteLine($"Email Service Configuration:");
                // Debug.WriteLine($"SMTP Server: {_smtpServer}");
                // Debug.WriteLine($"SMTP Port: {_smtpPort}");
                // Debug.WriteLine($"SMTP Username: {_smtpUsername}");
                // Debug.WriteLine($"Sender Email: {_senderEmail}");
                // Debug.WriteLine($"Sender Name: {_senderName}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing email service: {ex.Message}");
                throw;
            }
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            await SendEmailAsync(new[] { to }, subject, htmlBody);
        }

        public async Task SendEmailAsync(string[] to, string subject, string htmlBody)
        {
            try
            {
                Debug.WriteLine($"Subject: {subject}");
                
                string sanitizedSubject = SanitizeSubject(subject);
                Debug.WriteLine($"Sanitized subject: {sanitizedSubject}");
                
                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    
                    client.Timeout = 10000; // 10s
                    
                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(_senderEmail, _senderName);
                        
                        foreach (var recipient in to)
                        {
                            mailMessage.To.Add(recipient);
                        }
                        
                        mailMessage.Subject = sanitizedSubject;
                        mailMessage.Body = htmlBody;
                        mailMessage.IsBodyHtml = true;
                        
                        Debug.WriteLine("About to send email...");
                        await client.SendMailAsync(mailMessage);
                        Debug.WriteLine("Email sent successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to send email: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
        }
        
        private string SanitizeSubject(string subject)
        {
            if (string.IsNullOrEmpty(subject))
                return "No Subject";
                
            string sanitized = subject
                .Replace('\r', ' ')
                .Replace('\n', ' ')
                .Replace('\t', ' ');
                
            sanitized = new string(sanitized.Where(c => !char.IsControl(c)).ToArray());
            
            sanitized = sanitized.Trim();
            
            if (string.IsNullOrWhiteSpace(sanitized))
                return "No Subject";
                
            return sanitized;
        }
    }
} 