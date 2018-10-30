using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TelFlix.Data.Models.Contracts;

namespace TelFlix.Data.Models
{
    public class User : IdentityUser, IDeletable
    {
        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<MoviesUsers> Favorites { get; set; }
    }
}
