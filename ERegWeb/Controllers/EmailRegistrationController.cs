using ERegServer.DataContext;
using ERegWeb.Domain;
using ERegWeb.Models.Requests;
using ERegWeb.Tools;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Messages;

namespace ERegWeb.Controllers
{
    [Route("[controller]/[action]")]
    public class EmailRegistrationController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ERegDataContext _dataContext;

        public EmailRegistrationController(IPublishEndpoint publishEndpoint, ERegDataContext dataContext)
        {

            _publishEndpoint = publishEndpoint;
            _dataContext = dataContext;
        }

        /// <summary>
        /// Endpoint for registering a user by email.
        /// </summary>
        [HttpPost]
        public IActionResult RegisterEmail([FromBody] EmailRegistrationRequest request)
        {
            if (!EmailValidator.ValidateEmail(request.Email))
            {
                return BadRequest("Invalid email format");
            }

            string generatedCode = EmailCodeGenerator.GenerateCode();
            DateTime expiration = DateTime.UtcNow.AddMinutes(30);

            var emailCode = new EmailCodeGenerated
            {
                Email = request.Email,
                Code = generatedCode,
                Expiration = expiration
            };
            _dataContext.EmailCodesGenerated.Add(emailCode);
            _dataContext.SaveChanges();

            var message = new EmailCodeMessage
            {
                Email = request.Email,
                GeneratedCode = generatedCode,
                Expiration = expiration
            };
            _publishEndpoint.Publish(message);

            return Ok("Code sent successfully");
        }

        /// <summary>
        /// Endpoint for validating the email confirmation code.
        /// </summary>
        [HttpPost]
        public IActionResult ValidateEmailCode([FromBody] ValidateEmailCodeRequest request)
        {
            var emailCode = _dataContext.EmailCodesGenerated
                .FirstOrDefault(x => x.Email == request.Email);

            if (emailCode == null)
            {
                return BadRequest("Email not found");
            }

            if (emailCode.Expiration < DateTime.UtcNow)
            {
                return BadRequest("Code has expired");
            }

            if (emailCode.Code != request.Code)
            {
                return BadRequest("Invalid code");
            }

            _dataContext.EmailCodesGenerated.Remove(emailCode);

            var validationMessage = new ValidatedEmailCodeMessage
            {
                Email = request.Email,
                InputCode = request.Code,
                IsCodeСonfirmed = true
            };
            _publishEndpoint.Publish(validationMessage);

            return Ok("Email validated successfully");
        }
    }
}
