using Microsoft.AspNetCore.Mvc;
using OauthAPI.Models.Context;
using OauthAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using OauthAPI.Helpers;

namespace OauthAPI.Tools
{
    internal static class DbHelper
    {
        private static readonly OauthContextDb _context = new();
        private static ResponseApi apiResponse = new();
        private static FirebaseHelper firebaseHelper = new();

        /* ====================> DATABASE USERS HELPERS <==========================*/
        //=> GET ALL USERS
        internal static List<Usuario> GetAllUsers()
        {
            return _context.Usuarios.Where(u => u.Id > 0).ToList();
        }

        /*=> GET USER BY EMAIL*/
        internal static Usuario GetUserByEmail(string email)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email);
        }

        /*=> GET USER BY ID*/
        public static Usuario GetUserById(int id)
        {
            return _context.Usuarios.Where(u => u.Id == id).FirstOrDefault();
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
            if (userModel.Id > 1)
            {
                var userSaved = GetUserById(userModel.Id);
                userModel.Activo = userSaved.Activo;
                if (string.IsNullOrEmpty(userModel.Pass))
                {
                    userModel.Pass = userSaved.Pass;
                }
                var local = _context.Set<Usuario>().Local.FirstOrDefault(entry => entry.Id.Equals(userModel.Id));
                if (local != null) _context.Entry(local).State = EntityState.Detached;
                _context.Entry(userModel).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        // => UPDATE USER STATUS
        public static Usuario UpdateUserStatus(int id)
        {
            if (id > 1)
            {
                var user = GetUserById(id);
                if (user != null)
                {
                    user.Activo = !user.Activo;
                    var local = _context.Set<Usuario>().Local.FirstOrDefault(entry => entry.Id.Equals(user.Id));
                    if (local != null) _context.Entry(local).State = EntityState.Detached;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                    return user;
                }
            }
            return null;
        }

        //==> UPDATE USER TOKEN RECOVERY
        public static bool UpdateTokenUser(string email, string token)
        {
            var getUser = GetUserByEmail(email);
            if (getUser == null)
            {
                return false;
            }
            getUser.TokenRecovery = token;
            var local = _context.Set<Usuario>().Local.FirstOrDefault(entry => entry.Id.Equals(getUser.Id));
            if (local != null) _context.Entry(local).State = EntityState.Detached;
            _context.Entry(getUser).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        //==> UPDATE USER PASSWORD
        public static bool UpdateUserPassword(Usuario userModel)
        {
            if (userModel == null)
            {
                return false;
            }
            var local = _context.Set<Usuario>().Local.FirstOrDefault(entry => entry.Id.Equals(userModel.Id));
            if (local != null) _context.Entry(local).State = EntityState.Detached;
            _context.Entry(userModel).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        //=> DELETE USER
        public static bool DeleteUser(int id)
        {
            if (id > 1)
            {
                var referencedProjects = _context.Proyectos.Where(p => p.IdUsuarioRegsitra == id).ToList();
                if (referencedProjects.Count() > 0)
                {
                    foreach (var project in referencedProjects)
                    {
                        project.IdUsuarioRegsitra = 1;
                        var local = _context.Set<Proyecto>().Local.FirstOrDefault(entry => entry.Id.Equals(project.Id));
                        if (local != null) _context.Entry(local).State = EntityState.Detached;
                        _context.Entry(project).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
                var user = GetUserById(id);
                if (user != null)
                {
                    _context.Usuarios.Remove(user);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        /*===========================> DATABASE PROJECT HELPERS <================================*/
        //=> GET ALL PROJECTS
        internal static List<Proyecto> GetAllProjects()
        {
            return _context.Proyectos.Where(p => p.Id > 0).ToList();
        }

        /*=> GET PROJECT BY NAME*/
        internal static Proyecto GetProjectByName(string name)
        {
            return _context.Proyectos.FirstOrDefault(p => p.Nombre == name);
        }

        /*=> GET PROJECT BY ID*/
        public static Proyecto GetProjectById(int id)
        {
            return _context.Proyectos.Where(p => p.Id == id).FirstOrDefault();
        }

        /*=> CHECK IF THE PROJECT NAME IS ALREADY REGISTERED*/
        public static bool isNameAvailable(string name)
        {
            var nameResult = _context.Proyectos.FirstOrDefault(p => p.Nombre == name);
            if (nameResult != null)
            {
                return false;
            }
            return true;
        }

        /*=> REGISTER NEW PROJECT*/
        public static bool RegisterProject(Proyecto projectModel)
        {
            if (isNameAvailable(projectModel.Nombre))
            {
                _context.Add(projectModel);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        /*=> UPDATE PROJECT*/
        public static bool UpdateProject(Proyecto projectModel)
        {
            var projectSaved = GetProjectById(projectModel.Id);
            if (projectSaved != null)
            {
                projectModel.FechaAlta = projectSaved.FechaAlta;
                var local = _context.Set<Proyecto>().Local.FirstOrDefault(entry => entry.Id.Equals(projectModel.Id));
                if (local != null) _context.Entry(local).State = EntityState.Detached;
                _context.Entry(projectModel).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        // => UPDATE PROJECT STATUS
        public static dynamic UpdateProjectStatus(int id)
        {
            var project = GetProjectById(id);
            if (project != null)
            {
                project.Activo = !project.Activo;
                var local = _context.Set<Proyecto>().Local.FirstOrDefault(entry => entry.Id.Equals(project.Id));
                if (local != null) _context.Entry(local).State = EntityState.Detached;
                _context.Entry(project).State = EntityState.Modified;
                _context.SaveChanges();
                return project;
            }
            return null;
        }

        //=> DELETE PROJECT
        public static bool DeleteProject(int id)
        {
            var project = GetProjectById(id);
            if (project != null)
            {
                _context.Proyectos.Remove(project);
                _context.SaveChanges();
                return true;
            }
            return false;
        }




        /* =====================> DATABASE USERS BY PROJECT HELPERS <========================*/
        //=> GET ALL USERS BY PROJECTS
        internal static List<UsuariosProyecto> GetAllUsersByProject()
        {
            return _context.UsuariosProyectos.Where(uP => uP.Id > 0).ToList();
        }

        /*=> GET  USER BY PROJECT BY EMAIL*/
        internal static UsuariosProyecto GetUserByProjectByEmail(string email)
        {
            var getUserByProject = _context.UsuariosProyectos.FirstOrDefault(uP => uP.Email == email);
            return getUserByProject;
        }

        internal static UsuariosProyecto GetUserByProjectByPhone(string phoneNumber)
        {
            var getUserByProject = _context.UsuariosProyectos.FirstOrDefault(uP => uP.Telefono == phoneNumber);
            return getUserByProject;
        }

        /*=> GET USER BY PROJECT BY ID*/
        public static UsuariosProyecto GetUserByProjectById(int id)
        {
            var UserByproject = _context.UsuariosProyectos.Where(uP => uP.Id == id).FirstOrDefault();
            return UserByproject;
        }

        /*=> CHECK IF THE USER BY PROJECT EMAIL IS ALREADY REGISTERED*/
        public static bool isUserProjectEmailAvailable(string email)
        {
            var emailResult = _context.UsuariosProyectos.FirstOrDefault(uP => uP.Email == email);
            if (emailResult != null)
            {
                return false;
            }
            return true;
        }

        /*=> REGISTER NEW USER BY PROJECT*/
        public static bool RegisterUSerByProject(UsuariosProyecto userModel)
        {
            if (isUserProjectEmailAvailable(userModel.Email))
            {
                _context.Add(userModel);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        /*=> UPDATE USER BY PROJECT*/
        public static bool UpdateUserByProject(UsuariosProyecto userModel)
        {
            var userSaved = GetUserByProjectById(userModel.Id);
            userModel.FechaCreacion = userSaved.FechaCreacion;
            userModel.FechaAcceso = userSaved.FechaAcceso;
            userModel.Activo = userSaved.Activo;
            if (string.IsNullOrEmpty(userModel.Pass))
            {
                userModel.Pass = userSaved.Pass;
            }

            var local = _context.Set<UsuariosProyecto>().Local.FirstOrDefault(entry => entry.Id.Equals(userModel.Id));
            if (local != null) _context.Entry(local).State = EntityState.Detached;
            _context.Entry(userModel).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        // => UPDATE USER BY PROJECT STATUS
        public static dynamic UpdateUserByProjectStatus(int id)
        {
            var userSaved = GetUserByProjectById(id);
            if (userSaved != null)
            {
                userSaved.Activo = !userSaved.Activo;
                var local = _context.Set<UsuariosProyecto>().Local.FirstOrDefault(entry => entry.Id.Equals(userSaved.Id));
                if (local != null) _context.Entry(local).State = EntityState.Detached;
                _context.Entry(userSaved).State = EntityState.Modified;
                _context.SaveChanges();
                return userSaved;
            }
            return null;
        }

        //=> DELETE USER BY PROJECT
        public static bool DeleteUserByProject(int id)
        {
            var userSaved = GetUserByProjectById(id);
            if (userSaved != null)
            {
                _context.UsuariosProyectos.Remove(userSaved);
                _context.SaveChanges();
                return true;
            }
            return false;
        }


        //==> GET ALL AUTH METHODS
        public static dynamic GetAllAuthMethods()
        {
            var AuthMethods = _context.MetodosAuths.Where(m => m.Id > 0).ToList();
            return AuthMethods;
        }


        //======> SIGN IN METHODS
        public static ResponseApi SignIn(string email, string pass)
        {
            var user = GetUserByEmail(email);
            if (user == null)
            {
                apiResponse.Ok = false; apiResponse.Status = 400; apiResponse.MsgHeader = "Usuario no registrado"; apiResponse.Msg = "No se ha encontrado el usuario";
                return apiResponse;
            }
            if (!user.Activo)
            {
                apiResponse.Ok = false; apiResponse.Status = 400; apiResponse.MsgHeader = "Usuario deshabilitado"; apiResponse.Msg = "El usuario se encuentra deshabilitado";
                return apiResponse;
            }
            if (user.Pass != pass)
            {
                apiResponse.Ok = false; apiResponse.Status = 400; apiResponse.MsgHeader = "Contraseña incorrecta"; apiResponse.Msg = "La contraseña ingresada no es correcta";
                return apiResponse;
            }
            apiResponse.Ok = true; apiResponse.Status = 200; apiResponse.Msg = user.Nombre; apiResponse.MsgHeader = Convert.ToString(user.Id);
            return apiResponse;
        }


        public static ResponseApi SignInUserProject(SigninData signinData)
        {
            var userProject = new UsuariosProyecto();
            apiResponse.Ok = false; apiResponse.Status = 400; apiResponse.MsgHeader = "Credenciales incorrectas"; apiResponse.Msg = "Las credenciales ingresadas no son correctas, verifica tus datos";
            if (!string.IsNullOrEmpty(signinData.Email)) userProject = GetUserByProjectByEmail(signinData.Email);
            
            if (userProject != null)
            {
                if (userProject.Pass != signinData.Pass || signinData.ProjectId != userProject.IdProyecto) return apiResponse;
                if (!userProject.Activo) 
                { 
                    apiResponse.MsgHeader = "Usuario deshabilitado"; apiResponse.Msg = "El usuario se actualmente se encuentra inactivo";
                    return apiResponse;
                }
                apiResponse.Ok = true; apiResponse.Status = 200; apiResponse.MsgHeader = "Ok"; apiResponse.Msg = "Ok";
                return apiResponse;
            } 
            return apiResponse;
        }

        public static async Task<ResponseApi> SignInService(SigninData signinData)
        {
            apiResponse.Ok = false; apiResponse.Status = 400; apiResponse.MsgHeader = "No se ha encontrado el usuario"; apiResponse.Msg = "Al parecer el usuario no se encuentra registrado";
            var userProject = new UsuariosProyecto();

            if (!string.IsNullOrEmpty(signinData.Email)) userProject = GetUserByProjectByEmail(signinData.Email);
            if (!string.IsNullOrEmpty(signinData.PhoneNumber)) userProject = GetUserByProjectByPhone(signinData.PhoneNumber);

            if (string.IsNullOrEmpty(signinData.UserUid)) return apiResponse;
            
            var userFromFirebase = await firebaseHelper.GetUserByUid(signinData.UserUid);
            if (userFromFirebase == null) return apiResponse;

            if (userProject == null || userProject.IdProyecto != signinData.ProjectId || userFromFirebase.Uid != signinData.UserUid) return apiResponse;

            apiResponse.Ok = true; apiResponse.Status = 200; apiResponse.MsgHeader = "Ok"; apiResponse.Msg = "Ok";
            return apiResponse;
        }
    }
}
