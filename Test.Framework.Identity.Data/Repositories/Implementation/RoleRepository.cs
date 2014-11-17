using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Data;
using Test.Framework.Extensions;
using Test.Framework.Identity.Entity;
using Test.Framework.Identity.Model;
using Test.Framework.Identity.Enum;

namespace Test.Framework.Identity.Data
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        #region Private Members

        #endregion

        #region Constructors
        public RoleRepository(IDatabase database)
            : base(database)
        {
        }

        public RoleRepository(string connectionName)
            : base(connectionName)
        {
        }
        #endregion

        public bool Delete(Guid roleId)
        {
            string commandText = "Delete from roles where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", roleId);

            return this.Database.Execute(commandText, parameters);
        }

        public async Task<bool> DeleteAsync(Guid roleId)
        {
            string commandText = "Delete from roles where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", roleId);

            var result = await this.Database.ExecuteAsync(commandText, parameters);
            return result;
        }

        public bool Delete(Role role)
        {
            if (role == null ||
                role.Id.IsEmpty())
                return false;

            return Delete(role.Id);
        }

        public async Task<bool> DeleteAsync(Role role)
        {
            if (role == null ||
                role.Id.IsEmpty())
                return false;

            var result = await DeleteAsync(role.Id);
            return result;
        }

        public bool Insert(Role role)
        {
            return this.Database.Insert<Role>(role);
        }

        public async Task<bool> InsertAsync(Role role)
        {
            var result = await this.Database.InsertAsync<Role>(role);
            return result;
        }

        public string GetRoleName(Guid roleId)
        {
            string commandText = "Select Name from roles where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", roleId);

            var role = this.Database.Select<IdentityRole>(commandText, parameters).FirstOrDefault();

            if (role == null)
                return null;

            return role.Name;
        }

        public async Task<string> GetRoleNameAsync(Guid roleId)
        {
            string commandText = "Select Name from roles where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", roleId);

            var tempRole = await this.Database.SelectAsync<IdentityRole>(commandText, parameters);
            var role = tempRole.FirstOrDefault();

            if (role == null)
                return null;

            return role.Name;
        }

        public Guid GetRoleId(string roleName)
        {
            string commandText = "Select Id from roles where Name = @name";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("name", roleName);

            var role = this.Database.Select<IdentityRole>(commandText, parameters).FirstOrDefault();

            if (role == null)
                return Guid.Empty;

            return role.Id;
        }

        public async Task<Guid> GetRoleIdAsync(string roleName)
        {
            string commandText = "Select Id from roles where Name = @name";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("name", roleName);

            var tempRole = await this.Database.SelectAsync<IdentityRole>(commandText, parameters);
            var role = tempRole.FirstOrDefault();

            if (role == null)
                return Guid.Empty;

            return role.Id;
        }

        public Role GetRoleById(Guid roleId)
        {
            string commandText = "Select * from roles where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", roleId);

            return this.Database.Select<Role>(commandText, parameters).FirstOrDefault();
        }

        public async Task<Role> GetRoleByIdAsync(Guid roleId)
        {
            string commandText = "Select * from roles where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", roleId);

            var result = await this.Database.SelectAsync<Role>(commandText, parameters);
            return result.FirstOrDefault();
        }

        public Role GetRoleByName(string roleName)
        {
            string commandText = "Select * from roles where Name = @name";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("name", roleName);

            return this.Database.Select<Role>(commandText, parameters).FirstOrDefault();
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            string commandText = "Select * from roles where Name = @name";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("name", roleName);

            var result = await this.Database.SelectAsync<Role>(commandText, parameters);
            return result.FirstOrDefault();
        }

        public bool Update(Role role)
        {
            return this.Database.Update<Role>(role);
        }

        public async Task<bool> UpdateAsync(Role role)
        {
            var result = await this.Database.UpdateAsync<Role>(role);
            return result;
        }
    }
}
