using System.ComponentModel.DataAnnotations;

namespace ERegWeb.Domain
{
    public class EmailCodeGenerated
    {
        [Key]
        public string Email { get; set; }

        public string Code { get; set; }
        public DateTime ?Expiration { get; set; }
    }
}
