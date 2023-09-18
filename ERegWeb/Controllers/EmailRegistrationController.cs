using ERegServer.DataContext;
using ERegWeb.Domain;
using ERegWeb.Models.Requests;
using ERegWeb.Tools;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLibrary.Messages;

namespace ERegWeb.Controllers
{
    [Route("[controller]/[action]")]
    public class EmailRegistrationController : ControllerBase
    {
        private readonly ILogger<EmailRegistrationController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ERegDataContext _dataContext;

        public EmailRegistrationController(ILogger<EmailRegistrationController> logger, IPublishEndpoint publishEndpoint, ERegDataContext dataContext)
        {
            _logger = logger;
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

            var emailCode = _dataContext.EmailCodesGenerated
                .FirstOrDefault(x => x.Email == request.Email);

            if (emailCode == null)
            {
                string generatedCode = EmailCodeGenerator.GenerateCode();
                DateTime expiration = DateTime.UtcNow.AddMinutes(30);

                emailCode = new EmailCodeGenerated
                {
                    Email = request.Email,
                    Code = generatedCode,
                    Expiration = expiration
                };
                try
                {
                    _dataContext.EmailCodesGenerated.Add(emailCode);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Не удалось добавить новую запись в базу данных");
                    return StatusCode(500, "An internal problem has occurred, try again later");
                }
            }
            else
            {
                if (emailCode.Expiration < DateTime.UtcNow)
                {
                    emailCode.Code = EmailCodeGenerator.GenerateCode();
                    emailCode.Expiration = DateTime.UtcNow.AddMinutes(30);
                }
            }
            try
            {
                _dataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось сохранить новый значения в БД");
                return StatusCode(500, "An internal problem has occurred, try again later");
            };

            var message = new EmailCodeMessage
            {
                Email = request.Email,
                GeneratedCode = emailCode.Code,
                Expiration = (DateTime)emailCode.Expiration
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

            try
            {
                _dataContext.EmailCodesGenerated.Remove(emailCode);
                _dataContext.SaveChanges();
            }
            catch (Exception )
            {
                _logger.LogError($"Не удалось удалить запись из базы данных");
            }
            

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
