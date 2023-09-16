namespace ERegWeb.Tools
{
    /// <summary>
    /// Utility class for generating email confirmation codes.
    /// </summary>
    public static class EmailCodeGenerator
    {
        private const string AllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// Generates a random email confirmation code.
        /// </summary>
        /// <param name="length">Length of the code (default is 6).</param>
        /// <returns>The generated code.</returns>
        public static string GenerateCode(int length = 6)
        {
            var random = new Random();
            char[] code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = AllowedCharacters[random.Next(AllowedCharacters.Length)];
            }

            return new string(code);
        }
    }
}
