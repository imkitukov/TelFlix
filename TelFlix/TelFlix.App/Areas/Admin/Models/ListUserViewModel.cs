using System.Collections.Generic;

namespace TelFlix.App.Areas.Admin.Models
{
    public class ListUserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
