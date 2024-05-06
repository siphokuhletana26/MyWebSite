using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;
using System.Data.SqlClient;

namespace MyWebSite.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString = "Data Source = R135-03; Initial Catalog = WebAppDb; Integrated Security = false";

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(string username, string password, string confirmPassword, string emailAddress, string contactNumber)
        {
            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match");
                return View();
            }

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@Username", username);
                parameters.Add("@Password", password);
                parameters.Add("@EmailAddress", emailAddress);
                parameters.Add("@ContactNo", contactNumber);

                var result = connection.QueryFirstOrDefault<string>("RegisterUser", parameters, commandType: CommandType.StoredProcedure);

                if (result == "Registration successful")
                {
                    return RedirectToAction("Login", "User ");
                }
                else if (result == "Username already exists")
                {
                    ModelState.AddModelError(string.Empty, "Username already exists");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Registration failed");
                }
            }

            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        { 
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var user = connection.QueryFirstOrDefault(
                    "UserLogin",
                    new { Username = username, Password = password },
                    commandType: CommandType.StoredProcedure);

                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserNo", (int)user.UserNo);
                    HttpContext.Session.SetString("Username", username);

                    if (user.UserType == "P")
                    {

                        return RedirectToAction("PatientDashboard", "Patient");
                    }
                    else if (user.UserType == "N")
                    {

                        return RedirectToAction("NurseDashboard", "Nurse");
                    }
                    else if (user.UserType == "O")
                    {
                        return RedirectToAction("OfficeMangerDasboard", "OfficeManager");
                    }
                    else if (user.UserType == "A")
                    {
                        return RedirectToAction("AdminDashboard", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Login", "User");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid username or password.";
                    return View();
                }

            }
        }
    }
}
