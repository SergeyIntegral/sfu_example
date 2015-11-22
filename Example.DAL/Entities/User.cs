using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Example.Core.Consts;
using Microsoft.AspNet.Identity;
using Example.DAL.Entities.Abstract;

namespace Example.DAL.Entities
{
    public class User : StringEntity, IUser, IDatesEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Index { get; set; }

        public string UserName { get; set; }

        [MaxLength(450)]
        public string FullName { get; set; }

        [Required]
        public UserRoles Role { get; set; }

        public int AccessFailedCount { get; set; }

        [Required]
        public string Email { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTimeOffset? LockoutEndDateUtc { get; set; }

        public string PasswordHash { get; set; }

        public string Phone { get; set; }

        /// <summary>
        /// Дата последней аутентификации
        /// </summary>
        public DateTimeOffset? DateOfLastVisit { get; set; }

        /// <summary>
        /// Флаг завершения сессии пользователя
        /// </summary>
        public bool? IsEndSession { get; set; }

        public virtual BinaryData Avatar { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
