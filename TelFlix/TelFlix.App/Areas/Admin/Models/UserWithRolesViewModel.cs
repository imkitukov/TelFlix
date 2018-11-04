using System.Collections.Generic;

namespace TelFlix.App.Areas.Admin.Models
{
    public class UserWithRolesViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
