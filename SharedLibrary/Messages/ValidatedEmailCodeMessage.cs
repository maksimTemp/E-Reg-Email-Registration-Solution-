namespace SharedLibrary.Messages
{

    /// <summary>
    /// Message class for indicating the validation of an email confirmation code.
    /// </summary>
    public class ValidatedEmailCodeMessage
    {
        public string Email { get; set; }
        public string InputCode { get; set; }
        public bool IsCodeСonfirmed { get; set; }
    }
}
