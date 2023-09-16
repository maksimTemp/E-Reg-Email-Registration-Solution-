namespace SharedLibrary.Messages
{

    /// <summary>
    /// Message class for sending email confirmation codes.
    /// </summary>
    public class EmailCodeMessage
    {
        public string Email { get; set; }
        
        public string GeneratedCode { get; set; }

        public DateTime Expiration { get; set; }
    }
}
