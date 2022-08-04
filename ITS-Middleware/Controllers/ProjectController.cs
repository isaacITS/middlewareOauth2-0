using ITS_Middleware.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using ITS_Middleware.ExceptionsHandler;
using ITS_Middleware.Helpers;
using Firebase.Auth;
using Firebase.Storage;

namespace ITS_Middleware.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        RequestHelper requestHelper = new();
        IConfiguration config;

        public ProjectController(ILogger<ProjectController> logger, IConfiguration _config)
        {
            _logger = logger;
            config = _config;
        }

        //Registrar proyecto
        public async Task<IActionResult> Register()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var methods = await requestHelper.GetAllAuthMethods();
                ViewData["MetodosAuth"] = methods;
                return PartialView();
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Register(ProjectImgFile projectImgFile)
        {
            try
            {
                Proyecto project = new(){
                    Nombre = projectImgFile.Nombre,
                    Enlace = projectImgFile.Enlace,
                    Descripcion = projectImgFile.Descripcion,
                    FechaAlta = DateTime.Now,
                    IdUsuarioRegsitra = int.Parse(HttpContext.Session.GetString("idUser")),
                    Activo = true,
                    ImageUrl = "",
                    MetodosAutenticacion = projectImgFile.MetodosAutenticacion
                };

                if (projectImgFile.ImageFile != null)
                {
                    string[] imageName = projectImgFile.ImageFile.FileName.Split(".");
                    string imageUrl = await UploadImage(projectImgFile.ImageFile, $"{projectImgFile.Nombre}.{imageName[imageName.Length - 1]}");
                    project.ImageUrl = string.IsNullOrEmpty(imageUrl) ? "" : imageUrl;
                }
                if (ModelState.IsValid)
                {
                    var response = await requestHelper.RegisterProject(project);
                    return Json(response);
                }
                return Json(new { Ok = false, Status = 400, Msg = "Información incompleta, intenta nuevamente" });
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }

        //Editar proyecto
        public async Task<IActionResult> EditProject(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    if (id == null)
                    {
                        return Json(new { Ok = false, Status = 400, Msg = "No se ingresó un ID válido" });
                    }
                    var project = await requestHelper.GetProjectById(id);
                    if (project == null)
                    {
                        return Json(new { Ok = false, Status = 400, Msg = "El ID no coincide con un proyecto registrado" });
                    }
                    ProjectImgFile projectImg = new ProjectImgFile()
                    {
                        Activo = project.Activo,
                        FechaAlta = project.FechaAlta,
                        Descripcion = project.Descripcion,
                        Enlace = project.Enlace,
                        Id = project.Id,
                        IdUsuarioRegsitra = project.IdUsuarioRegsitra,
                        ImageUrl = project.ImageUrl,
                        MetodosAutenticacion = project.MetodosAutenticacion,
                        Nombre = project.Nombre
                    };
                    var metodos = await requestHelper.GetAllAuthMethods();
                    ViewData["MetodosAuth"] = metodos;
                    return PartialView(projectImg);
                }
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProject(ProjectImgFile projectImgFile)
        {
            try
            {
                Proyecto project = new()
                {
                    Id = projectImgFile.Id,
                    Nombre = projectImgFile.Nombre,
                    Enlace = projectImgFile.Enlace,
                    Descripcion = projectImgFile.Descripcion,
                    FechaAlta = projectImgFile.FechaAlta,
                    IdUsuarioRegsitra = projectImgFile.IdUsuarioRegsitra,
                    Activo = projectImgFile.Activo,
                    ImageUrl = projectImgFile.ImageUrl,
                    MetodosAutenticacion = projectImgFile.MetodosAutenticacion
                };
                if (projectImgFile.ImageFile != null)
                {
                    string[] imageName = projectImgFile.ImageFile.FileName.Split(".");
                    string imageUrl = await UploadImage(projectImgFile.ImageFile, $"{projectImgFile.Nombre}.{imageName[imageName.Length - 1]}");
                    project.ImageUrl = string.IsNullOrEmpty(imageUrl) ? "" : imageUrl;
                }
                var response = await requestHelper.UpdateProject(project);
                return Json(response);
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }


        //Eliminar proyecto
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    if (id == null)
                    {
                        return Json(new { Ok = false, Status = 400, Msg = "No se ingresó un ID válido" });
                    }
                    var project = await requestHelper.GetProjectById(id);
                    if (project == null)
                    {
                        return Json(new { Ok = false, Status = 400, Msg = "No se encontró un proyecto registrado" });
                    }
                    return PartialView(project);
                }
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }



        [HttpPost]
        public async Task<IActionResult> DeleteProjectPost([FromBody] int id)
        {
            try
            {
                var response = await requestHelper.DeleteProject(id);
                return Json(response);
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }

        //Estatus activo/desactivo proyectos
        public async Task<IActionResult> UpdateStatus(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    if (id == null)
                    {
                        return Json(new { Ok = false, Msg = "No se ingresó un ID válido" });
                    }
                    var project = await requestHelper.GetProjectById(id);
                    if (project == null)
                    {
                        return Json(new { Ok = false, Msg = "El ID no coincide con un usuario registrado" });
                    }
                    return PartialView("ChangeStatus", project);
                }
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatusPost(int id)
        {
            try
            {
                var response = await requestHelper.UpdateProjectStatus(id);
                return Json(response);
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }


        public async Task<string> UploadImage(IFormFile imageUrl, string name)
        {
            Stream file = imageUrl.OpenReadStream();

            string email = config.GetSection("ApplicationSettings:FirebaseStorage:email").Value.ToString();
            string key = config.GetSection("ApplicationSettings:FirebaseStorage:key").Value.ToString();
            string path = config.GetSection("ApplicationSettings:FirebaseStorage:path").Value.ToString();
            string apiKey = config.GetSection("ApplicationSettings:FirebaseStorage:apiKey").Value.ToString();

            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, key);
            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
                path,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                }
                )
                .Child("assets")
                .Child(name)
                .PutAsync(file, cancellation.Token);
            var downloadUrl = await task;
            return downloadUrl;
        }
    }
}
