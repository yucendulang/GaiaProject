using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GaiaProject.Models.AccountViewModels
{
    public class UserFriend
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }

        public string UserNameTo { get; set; }
        public int UserIdTo { get; set; }


    }
}
