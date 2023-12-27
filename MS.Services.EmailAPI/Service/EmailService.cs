using Azure.Messaging.ServiceBus.Administration;
using Microsoft.EntityFrameworkCore;
using MS.Services.EmailAPI.Data;
using MS.Services.EmailAPI.Message;
using MS.Services.EmailAPI.Models;
using MS.Services.EmailAPI.Models.Dto;
using System.Text;

namespace MS.Services.EmailAPI.Service
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total " +cartDto.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");
            foreach(var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task LogOrderPlaced(RewardsMessage rewardsMessage)
        {
            string message = "New Order placed. <br/> Order Id : " + rewardsMessage.OrderId;
            await LogAndEmail(message, "microserviceapp@gmail.com");
        }

        public async Task RegisterEmailAndLog(string email)
        {
            string message = "User Registration successful. <br/> Email : " + email;
            await LogAndEmail(message, "microserviceapp@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };
                await using var _db = new AppDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
