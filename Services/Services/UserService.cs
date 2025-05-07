using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Interfaces.Services;
using Core.Entities;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Services.Validators;
using System.Threading.Tasks;
using Core.Interfaces;
using System.Security.Cryptography;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private List <User> _users = new List<User>
        { 
            new User{ UserName = "Admin", Password = "Password", Id = 1}
        };

        //private string _token { get; set; }

        //public UserService(string token)
        //{
        //    _token = token;
        //}
        // Para Hashear la Contraseña
        // SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string HashPassword(string Password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Password);
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-","");
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _unitOfWork.UserRepository.GetAllAsync();
        }

        public async Task<string> Login(User user)
        {
            var LoginUser = await _unitOfWork.UserRepository.GetUser(user.UserName, HashPassword(user.Password));

            if(LoginUser == null)
            {
                return string.Empty;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("6f4d75aab32aef76b24c058d1bf7b979");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, LoginUser.UserName),
                    new Claim("id", LoginUser.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string userToken = tokenHandler.WriteToken(token);
            return userToken;
        }

        public async Task<User> Register(User newUser)
        {
            UserValidator validator = new UserValidator();
            var result = await validator.ValidateAsync(newUser);
            if (!result.IsValid)
            {
                throw new ArgumentException(result.Errors[0].ErrorMessage);
            }
            newUser.Password = HashPassword(newUser.Password);
            await _unitOfWork.UserRepository.AddAsync(newUser);
            await _unitOfWork.CommitAsync();

            return newUser;
        }

        public async Task<User> GetById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("El Usuario no existe");
            }

            return user;
        }



    }
}