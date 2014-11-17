using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Identity.Model
{
    public class IdentityRole : IRole<Guid>
    {
        /// <summary>
        /// Default constructor for Role 
        /// </summary>
        public IdentityRole()
        {
            Id = Guid.NewGuid();
        }
        /// <summary>
        /// Constructor that takes names as argument 
        /// </summary>
        /// <param name="name"></param>
        public IdentityRole(string name)
            : this()
        {
            Name = name;
        }

        public IdentityRole(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// Role ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        public string Name { get; set; }
    }
}
