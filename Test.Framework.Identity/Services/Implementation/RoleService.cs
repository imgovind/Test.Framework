using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Identity.Data;
using Test.Framework.Identity.Entity;
using Test.Framework.Identity.Model;
using Test.Framework.Extensions;
using AutoMapper;

namespace Test.Framework.Identity.Services
{
    /// <summary>
    /// Class that represents the Role table in the MySQL Database
    /// </summary>
    public class RoleService : IRoleService
    {
        private IIdentityDataProvider dataProvider;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public RoleService(IIdentityDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        #region Private Methods

        private IdentityRole CreateIdentityInstance(Role role)
        {
            Mapper.CreateMap<Role, IdentityRole>();
            IdentityRole result = Mapper.Map<IdentityRole>(role);
            return result;
        }

        private Role CreateRoleInstance(IdentityRole identityRole)
        {
            Mapper.CreateMap<IdentityRole, Role>();
            Role result = Mapper.Map<Role>(identityRole);
            return result;
        }

        #endregion

        /// <summary>
        /// Deltes a role from the Roles table
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns></returns>
        public bool Delete(Guid roleId)
        {
            //string commandText = "Delete from Roles where Id = @id";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@id", roleId);

            //return _database.Execute(commandText, parameters);
            return dataProvider.RoleRepository().Delete(roleId);
        }

        public bool Delete(int clusterId, Guid roleId)
        {
            //string commandText = "Delete from Roles where Id = @id";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@id", roleId);

            //return _database.Execute(commandText, parameters);
            return dataProvider.RoleRepository(clusterId).Delete(roleId);
        }

        public async Task<bool> DeleteAsync(Guid roleId)
        {
            //string commandText = "Delete from Roles where Id = @id";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@id", roleId);

            //return _database.Execute(commandText, parameters);
            var result = await dataProvider.RoleRepository().DeleteAsync(roleId);
            return result;
        }

        public async Task<bool> DeleteAsync(int clusterId, Guid roleId)
        {
            //string commandText = "Delete from Roles where Id = @id";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@id", roleId);

            //return _database.Execute(commandText, parameters);
            var result = await dataProvider.RoleRepository(clusterId).DeleteAsync(roleId);
            return result;
        }

        /// <summary>
        /// Inserts a new Role in the Roles table
        /// </summary>
        /// <param name="roleName">The role's name</param>
        /// <returns></returns>
        public bool Insert(IdentityRole role)
        {
            //string commandText = "Insert into Roles (Id, Name) values (@id, @name)";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@name", role.Name);
            //parameters.Add("@id", role.Id);

            //return _database.Execute(commandText, parameters);
            if (role == null)
                return false;

            return dataProvider.RoleRepository().Insert(CreateRoleInstance(role));
        }

        public bool Insert(int clusterId, IdentityRole role)
        {
            //string commandText = "Insert into Roles (Id, Name) values (@id, @name)";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@name", role.Name);
            //parameters.Add("@id", role.Id);

            //return _database.Execute(commandText, parameters);
            if (role == null)
                return false;

            return dataProvider.RoleRepository(clusterId).Insert(CreateRoleInstance(role));
        }

        public async Task<bool> InsertAsync(IdentityRole role)
        {
            //string commandText = "Insert into Roles (Id, Name) values (@id, @name)";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@name", role.Name);
            //parameters.Add("@id", role.Id);

            //return _database.Execute(commandText, parameters);
            if (role == null)
                return false;

            var result = await dataProvider.RoleRepository().InsertAsync(CreateRoleInstance(role));
            return result;
        }

        public async Task<bool> InsertAsync(int clusterId, IdentityRole role)
        {
            //string commandText = "Insert into Roles (Id, Name) values (@id, @name)";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@name", role.Name);
            //parameters.Add("@id", role.Id);

            //return _database.Execute(commandText, parameters);
            if (role == null)
                return false;

            var result = await dataProvider.RoleRepository(clusterId).InsertAsync(CreateRoleInstance(role));
            return result;
        }

        /// <summary>
        /// Returns a role name given the roleId
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns>Role name</returns>
        public string GetRoleName(Guid roleId)
        {
            //string commandText = "Select Name from Roles where Id = @id";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@id", roleId);

            //return _database.GetStrValue(commandText, parameters);
            return dataProvider.RoleRepository().GetRoleName(roleId);
        }

        public string GetRoleName(int clusterId, Guid roleId)
        {
            //string commandText = "Select Name from Roles where Id = @id";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@id", roleId);

            //return _database.GetStrValue(commandText, parameters);
            return dataProvider.RoleRepository(clusterId).GetRoleName(roleId);
        }

        public async Task<string> GetRoleNameAsync(Guid roleId)
        {
            //string commandText = "Select Name from Roles where Id = @id";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@id", roleId);

            //return _database.GetStrValue(commandText, parameters);
            var result = await dataProvider.RoleRepository().GetRoleNameAsync(roleId);
            return result;
        }

        public async Task<string> GetRoleNameAsync(int clusterId, Guid roleId)
        {
            //string commandText = "Select Name from Roles where Id = @id";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@id", roleId);

            //return _database.GetStrValue(commandText, parameters);
            var result = await dataProvider.RoleRepository(clusterId).GetRoleNameAsync(roleId);
            return result;
        }

        /// <summary>
        /// Returns the role Id given a role name
        /// </summary>
        /// <param name="roleName">Role's name</param>
        /// <returns>Role's Id</returns>
        public Guid GetRoleId(string roleName)
        {
            //Guid roleId = null;
            //string commandText = "Select Id from Roles where Name = @name";
            //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", roleName } };

            //var result = _database.QueryValue(commandText, parameters);
            //if (result != null)
            //{
            //    return Convert.ToString(result);
            //}

            //return roleId;
            return dataProvider.RoleRepository().GetRoleId(roleName);
        }

        public Guid GetRoleId(int clusterId, string roleName)
        {
            //Guid roleId = null;
            //string commandText = "Select Id from Roles where Name = @name";
            //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", roleName } };

            //var result = _database.QueryValue(commandText, parameters);
            //if (result != null)
            //{
            //    return Convert.ToString(result);
            //}

            //return roleId;
            return dataProvider.RoleRepository(clusterId).GetRoleId(roleName);
        }

        public async Task<Guid> GetRoleIdAsync(string roleName)
        {
            //Guid roleId = null;
            //string commandText = "Select Id from Roles where Name = @name";
            //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", roleName } };

            //var result = _database.QueryValue(commandText, parameters);
            //if (result != null)
            //{
            //    return Convert.ToString(result);
            //}

            //return roleId;
            var result = await dataProvider.RoleRepository().GetRoleIdAsync(roleName);
            return result;
        }

        public async Task<Guid> GetRoleIdAsync(int clusterId, string roleName)
        {
            //Guid roleId = null;
            //string commandText = "Select Id from Roles where Name = @name";
            //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", roleName } };

            //var result = _database.QueryValue(commandText, parameters);
            //if (result != null)
            //{
            //    return Convert.ToString(result);
            //}

            //return roleId;
            var result = await dataProvider.RoleRepository(clusterId).GetRoleIdAsync(roleName);
            return result;
        }

        /// <summary>
        /// Gets the IdentityRole given the role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IdentityRole GetRoleById(Guid roleId)
        {
            //var roleName = GetRoleName(roleId);
            //IdentityRole role = null;

            //if (roleName != null)
            //{
            //    role = new IdentityRole(roleName, roleId);
            //}

            //return role;

            var result = dataProvider.RoleRepository().GetRoleById(roleId);

            if (result == null)
                return null;

            return CreateIdentityInstance(result);
        }

        public IdentityRole GetRoleById(int clusterId, Guid roleId)
        {
            //var roleName = GetRoleName(roleId);
            //IdentityRole role = null;

            //if (roleName != null)
            //{
            //    role = new IdentityRole(roleName, roleId);
            //}

            //return role;

            var result = dataProvider.RoleRepository(clusterId).GetRoleById(roleId);


            if (result == null)
                return null;

            return CreateIdentityInstance(result);
        }

        public async Task<IdentityRole> GetRoleByIdAsync(Guid roleId)
        {
            //var roleName = GetRoleName(roleId);
            //IdentityRole role = null;

            //if (roleName != null)
            //{
            //    role = new IdentityRole(roleName, roleId);
            //}

            //return role;

            var result = await dataProvider.RoleRepository().GetRoleByIdAsync(roleId);

            if (result == null)
                return null;

            return CreateIdentityInstance(result);
        }

        public async Task<IdentityRole> GetRoleByIdAsync(int clusterId, Guid roleId)
        {
            //var roleName = GetRoleName(roleId);
            //IdentityRole role = null;

            //if (roleName != null)
            //{
            //    role = new IdentityRole(roleName, roleId);
            //}

            //return role;

            var result = await dataProvider.RoleRepository(clusterId).GetRoleByIdAsync(roleId);

            if (result == null)
                return null;

            return CreateIdentityInstance(result);
        }

        /// <summary>
        /// Gets the IdentityRole given the role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IdentityRole GetRoleByName(string roleName)
        {
            //var roleId = GetRoleId(roleName);
            //IdentityRole role = null;

            //if (roleId != null)
            //{
            //    role = new IdentityRole(roleName, roleId);
            //}

            //return role;

            var result = dataProvider.RoleRepository().GetRoleByName(roleName);

            if (result == null)
                return null;

            return CreateIdentityInstance(result);
        }

        public IdentityRole GetRoleByName(int clusterId, string roleName)
        {
            //var roleId = GetRoleId(roleName);
            //IdentityRole role = null;

            //if (roleId != null)
            //{
            //    role = new IdentityRole(roleName, roleId);
            //}

            //return role;

            var result = dataProvider.RoleRepository(clusterId).GetRoleByName(roleName);


            if (result == null)
                return null;

            return CreateIdentityInstance(result);
        }

        public async Task<IdentityRole> GetRoleByNameAsync(string roleName)
        {
            //var roleId = GetRoleId(roleName);
            //IdentityRole role = null;

            //if (roleId != null)
            //{
            //    role = new IdentityRole(roleName, roleId);
            //}

            //return role;

            var result = await dataProvider.RoleRepository().GetRoleByNameAsync(roleName);

            if (result == null)
                return null;

            return CreateIdentityInstance(result);
        }

        public async Task<IdentityRole> GetRoleByNameAsync(int clusterId, string roleName)
        {
            //var roleId = GetRoleId(roleName);
            //IdentityRole role = null;

            //if (roleId != null)
            //{
            //    role = new IdentityRole(roleName, roleId);
            //}

            //return role;

            var result = await dataProvider.RoleRepository(clusterId).GetRoleByNameAsync(roleName);

            if (result == null)
                return null;

            return CreateIdentityInstance(result);
        }

        public bool Update(IdentityRole role)
        {
            //string commandText = "Update Roles set Name = @name where Id = @id";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@id", role.Id);

            //return _database.Execute(commandText, parameters);
            if (role == null)
                return false;

            return dataProvider.RoleRepository().Update(CreateRoleInstance(role));
        }

        public bool Update(int clusterId, IdentityRole role)
        {
            //string commandText = "Update Roles set Name = @name where Id = @id";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@id", role.Id);

            //return _database.Execute(commandText, parameters);
            if (role == null)
                return false;

            return dataProvider.RoleRepository(clusterId).Update(CreateRoleInstance(role));
        }

        public async Task<bool> UpdateAsync(IdentityRole role)
        {
            //string commandText = "Update Roles set Name = @name where Id = @id";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@id", role.Id);

            //return _database.Execute(commandText, parameters);
            if (role == null)
                return false;

            var result = await dataProvider.RoleRepository().UpdateAsync(CreateRoleInstance(role));
            return result;
        }

        public async Task<bool> UpdateAsync(int clusterId, IdentityRole role)
        {
            //string commandText = "Update Roles set Name = @name where Id = @id";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@id", role.Id);

            //return _database.Execute(commandText, parameters);
            if (role == null)
                return false;

            var result = await dataProvider.RoleRepository(clusterId).UpdateAsync(CreateRoleInstance(role));
            return result;
        }
    }
}
