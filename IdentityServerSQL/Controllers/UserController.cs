using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServerSQL.Models;
using IdentityServerSQL.Repository;
using IdentityServerSQL.Services;
using IdentityServerSQL.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PasswordHasher = ServiceStack.Auth.PasswordHasher;

namespace IdentityServerSQL.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("identity_server")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        // private readonly IUserRepository _userRepository;
        // private readonly IRoleRepository _roleRepository; 
        // private readonly IUserRoleRepository _userRoleRepository; 
        private PasswordHasher hash = new PasswordHasher();
        private readonly IUnitOfWork _uow;
        
        public UserController(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            //_userRepository = userRepository;
            _uow = unitOfWork;
            //_roleRepository = roleRepository;
            //_userRoleRepository = userRoleRepository;
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Login([FromBody]LoginEntity user)
        {
            bool needRehash;
            var userDbSearch = await _uow.UserRepository.Query("UserName = @Email", new {Email = user.Email});

            var userDb = userDbSearch.FirstOrDefault();
            bool passwordIsCorrect = hash.VerifyPassword(userDb.Password, user.Password, out needRehash);
            
            if (!userDbSearch.Any() || !passwordIsCorrect)
                return NotFound(new { message = "Usuário ou senha inválidos" });
            
            var rolesDb = await _uow.UserRoleRepository.Query("ApplicationUserId = @ApplicationUserId", new {ApplicationUserId = userDb.ApplicationUserId});
            var roles = rolesDb.Select(u =>  _uow.RoleRepository.GetById(u.ApplicationRoleId.ToString()).Result).ToList();
            
            var token = TokenService.GenerateToken(userDb, roles);
            var expirationTime = DateTime.Now.AddHours(HashingOptions.ExpirationInHours);
            user.Password = null;

            return Ok(new LoginResponse(userDb.ApplicationUserId, userDb.Email, userDb.UserName, userDb.Name, userDb.LastName,
                roles, token, expirationTime.ToString()));
        }
        
        [HttpGet]
        [Authorize]
        [Microsoft.AspNetCore.Mvc.Route("user_data")]
        public async Task<ActionResult<dynamic>> UserData()
        {
            var userId = User.Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid").FirstOrDefault();
            var expirationTime = User.Claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration").FirstOrDefault();
            //var expirationDate = new DateTime(Convert.ToInt64(exp)*1000); //unix timestamp is in seconds, javascript in milliseconds
            
            if (userId == null || expirationTime == null) return NotFound(new { message = "Token informado inválido" });
            
            var userDb = (await _uow.UserRepository.Query("id = @UserId", new {UsrId = userId.Value})).FirstOrDefault();
            
            var rolesDb = await _uow.UserRoleRepository.Query("ApplicationUserId = @ApplicationUserId", 
                new {ApplicationUserId = userDb.ApplicationUserId});
            var roles = rolesDb.Select(u =>  _uow.RoleRepository.GetById(u.ApplicationRoleId.ToString()).Result).ToList();
            
            return new LoginResponse(userDb.ApplicationUserId, userDb.Email, userDb.UserName, userDb.Name, userDb.LastName,
                roles, "", expirationTime.Value);
        }
        
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.Route("role")]
        public async  Task<IActionResult> AddRoles([FromBody] RoleEntity role)
        {
            var newRole = new ApplicationRole(role.Name);

            var roleExists =
                await _uow.RoleRepository.Query("name = @Name", new {Name = role.Name});

            if (roleExists.Any())
                return BadRequest("Role " + role.Name + " já registrado.");
            
            _uow.RoleRepository.Add(newRole);
            
            bool result = _uow.Commit();
            if (result)
                return Ok(newRole);
            
            return BadRequest("Ocorreu um erro ao gravar no banco de dados.");
        }

        [HttpPut]
        [Authorize(Roles="Admin")]
        [Microsoft.AspNetCore.Mvc.Route("role_to_user")]
        public async  Task<IActionResult> AddRolesToUser([FromBody] RoleToUserEntity roleToUser)
        {
            var role = await _uow.RoleRepository.GetById(roleToUser.RoleId.ToString());
            var user = await _uow.UserRepository.GetById(roleToUser.UserId.ToString());
            
            if (role == null || user == null)
                return BadRequest("Ids informados incorretos.");

            var verifyExists = (await _uow.UserRoleRepository.Query(
                    "ApplicationUserId = @ApplicationUserId and ApplicationRoleId = @ApplicationRoleId", 
                    new {ApplicationUserId = user.ApplicationUserId, ApplicationRoleId = role.ApplicationRoleId}))
                .Any();
            
            if (verifyExists)
                return BadRequest("Usuário já tem essa Role");
            
            _uow.UserRoleRepository.Add(new ApplicationUserRole(user, role));
            
            _uow.UserRepository.Update(user);

            bool result = _uow.Commit();
            if (result)
                return Ok("Role "+ role.Name + " adicionada para o usuário " + user.UserName);
            
            return BadRequest("Ocorreu um erro ao gravar no banco de dados.");
        }
        
        
        
        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("is_authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);
        
        // POST api/user/register
        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterEntity model)
        {
            if (ModelState.IsValid)
            {
                var userDb = new ApplicationUser(
                    model.Email, model.Email, model.Name, model.LastName,
                    hash.HashPassword(model.Password),null
                );

                var userExists =
                    //await _uow.UserRepository.Query($"UserName = '{model.Email}'");
                    await _uow.UserRepository.Query("UserName = @Email", new {Email = model.Email});

                if (userExists.Any())
                    return BadRequest("E-mail " + model.Email + " já registrado.");

                var defaultRole = _configuration.GetValue<string>("DefaultRole");
                _uow.RoleRepository.Query("Name = @Name", new {Name = defaultRole});
                _uow.UserRepository.Add(userDb);
                _uow.UserRoleRepository.Add(new ApplicationUserRole());
                
                
                if (! _uow.Commit()) return BadRequest("Ocorreu um erro ao gravar no banco de dados.");
                var token = TokenService.GenerateToken(userDb, null);
                var expirationTime = DateTime.Now.AddHours(HashingOptions.ExpirationInHours);

                return Ok(new LoginResponse(userDb.ApplicationUserId, userDb.Email, userDb.UserName, userDb.Name, userDb.LastName,
                   null, token, expirationTime.ToString()));
            }

            string errorMessage =
                string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");
        }
    }
}