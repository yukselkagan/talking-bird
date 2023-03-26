using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostService.Model
{
    public class User
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
