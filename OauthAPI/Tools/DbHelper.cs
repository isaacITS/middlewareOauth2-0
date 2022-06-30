using Microsoft.AspNetCore.Mvc;
using OauthAPI.Models.Context;
using OauthAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace OauthAPI.Tools
{
    internal static class DbHelper
    {
        private static readonly OauthContextDb _context = new();


        /* ==> DATABASE USERS HELPERS */
        /*=> GET USER BY EMAIL*/
        internal static dynamic GetUserByEmail(string email)
        {
            var getUser = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (getUser != null)
            {
                return getUser;
            }
            return 0;
        }

        /*=> GET USER BY ID*/
        public static dynamic GetUserById(int id)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return user;
            }
            return 0;
        }

        /*=> CHECK IF THE EMAIL IS ALREADY REGISTERED*/
        public static bool isEmailAvailable(string email)
        {
            var emialResult = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (emialResult != null)
            {
                return false;
            }
            return true;
        }

        /*=> REGISTER NEW USER*/
        public static bool RegisterUser(Usuario userModel)
        {
            if (isEmailAvailable(userModel.Email))
            {
                _context.Add(userModel);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        /*=> UPDATE USER*/
        public static bool UpdateUser(Usuario userModel)
        {
            userModel.Pass = setPassword(userModel);
            var local = _context.Set<Usuario>().Local.FirstOrDefault(entry => entry.Id.Equals(userModel.Id));
            if (local != null) _context.Entry(local).State = EntityState.Detached;
            _context.Entry(userModel).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        // => UPDATE USER STATUS
        public static dynamic UpdateUserStatus(int id)
        {
            if (id > 1)
            {
                var user = GetUserById(id);
                if (user != 0)
                {
                    user.Activo = !user.Activo;
                    var local = _context.Set<Usuario>().Local.FirstOrDefault(entry => entry.Id.Equals(user.Id));
                    if (local != null) _context.Entry(local).State = EntityState.Detached;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                    return user;
                }
            }
            return 0;
        }

        //=> DELETE USER
        public static bool DeleteUser(int id)
        {
            if (id > 1)
            {
                var user = GetUserById(id);
                if (user != 0)
                {
                    _context.Usuarios.Remove(user);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        //SET THE PASSWORD FOR THE USER UPDATED
        public static string setPassword(Usuario userModel)
        {
            if (userModel.Pass == "0000000000" || userModel.Pass.Length < 8 || string.IsNullOrEmpty(userModel.Pass))
            {
                var getUsrData = _context.Usuarios.FirstOrDefault(u => u.Id == userModel.Id);
                if (getUsrData != null) return getUsrData.Pass;
            }
            return Encrypt.sha256(userModel.Pass);
        }


        /* ==> DATABASE PROJECTS HELPERS */

    }
}
