using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostService.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ExternalUserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
