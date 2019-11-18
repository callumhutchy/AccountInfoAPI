using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using AccountInfoApi.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Net.Http;
using MySql.Data.MySqlClient;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;

namespace AccountInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : Controller
    {
        private MySqlConnection connection = null;
        private hutchymmoContext _context;

        public UserInfoController(hutchymmoContext context){
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInformation>>> GetUserInfos(){
            return await _context.UserInformation.ToListAsync();
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<UserInformation>> GetUserInfo(int id){
            var userInfo = await _context.UserInformation.FindAsync(id);
            if(userInfo == null){
                return NotFound();
            }

            return userInfo;
        }

        [HttpGet("username/{Username}")]
        public async Task<bool> GetUserInfo(string username){
            var userInfo = await _context.UserInformation.FirstOrDefaultAsync( x => x.Username.Equals(username));
            if(userInfo == null){
                return false;
            }
            return true;
        }

        [Route("create")]
        [HttpPost]
        public int CreateUserInfo([FromBody] UserInfoInputJson json){
            
            int result = 0;
          
            DBConnect();
            
            string password = json.password;

            byte[] salt = hashPassword(ref password);

            MySqlCommand cmd = new MySqlCommand("register_user", connection);
            
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("userIN",json.username));
            cmd.Parameters.Add(new MySqlParameter("emailIN",json.email));
            cmd.Parameters.Add(new MySqlParameter("passwordIN",password));
            cmd.Parameters.Add(new MySqlParameter("valcodeIN", Generate(7)));
            cmd.Parameters.Add(new MySqlParameter("adminIN", json.admin));       
            cmd.Parameters.Add(new MySqlParameter("saltIN", salt));
            cmd.Parameters.Add(new MySqlParameter("resultOUT", MySqlDbType.Int32));
            cmd.Parameters["resultOUT"].Direction = System.Data.ParameterDirection.Output;
            
            cmd.ExecuteNonQuery();

            result = (int)cmd.Parameters["resultOUT"].Value;
            
            return result;

        }

    
        public byte[] hashPassword(ref string password){
            var salt =  getSalt();
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            password = Convert.ToBase64String(hashBytes);
            return salt;
        }

        public byte[] getSalt(){
            byte[] salt;
           new RNGCryptoServiceProvider().GetBytes(salt= new byte[16]);
           return salt;
        }

        [HttpGet("salt/{Username}")]
        public async Task<byte[]> GetSalt(string username){
            return (byte[]) _context.UserInformation.Where(x=>x.Username.Equals(username)).FirstOrDefault().Salt;
        }

        [HttpGet("auth_token/{Username}")]
        public async Task<string> GetAuth(string username){
            return _context.UserInformation.Where(x=>x.Username.Equals(username)).FirstOrDefault().AuthenticationToken;
        }

        [HttpGet("logged_in/{Username}")]
        public async Task<bool> UserHasLoggedIn(string username){
            if( _context.UserInformation.Where(x=>x.Username.Equals(username)).FirstOrDefault().LoggedIn == 1){
                return true;
            }else{
                return false;
            }
        }

        [HttpGet("log_in/{Username}")]
        public async Task<bool> UserLogIn(string username){

            if(_context.UserInformation.Where(x=>x.Username.Equals(username)).FirstOrDefault().LoggedIn == 1){
                return false;
            }

            var result = _context.UserInformation.Where(x=>x.Username.Equals(username)).FirstOrDefault();
            result.LoggedIn = 1;
            _context.SaveChanges();

            return true;
        }

        [HttpGet("log_out/{Username}")]
        public async Task<bool> UserLogOut(string username){
            if(_context.UserInformation.Where(x=>x.Username.Equals(username)).FirstOrDefault().LoggedIn == 0){
                return false;
            }

            var result = _context.UserInformation.Where(x=>x.Username.Equals(username)).FirstOrDefault();
            result.LoggedIn = 0;
            _context.SaveChanges();

            return true;
        }

        [HttpPost("login")]
        public async Task<string> Login([FromBody] UserAndPass content){ 
            Console.WriteLine(content.username + " | " + content.password);
            if(_context.UserInformation.Where(x=>x.Username.Equals(content.username)).FirstOrDefault().LoggedIn == 1){
                Console.WriteLine("User account already logged in, please log out elsewhere.");
                return "User account already logged in, please log out elsewhere.";
            }

            if(_context.UserInformation.Where(x=>x.Username.Equals(content.username) && x.Password.Equals(content.password)).Count() > 0){
                string code = GenerateAuthToken();
                Console.WriteLine(code);
                _context.UserInformation.Where(x=>x.Username.Equals(content.username) && x.Password.Equals(content.password)).FirstOrDefault().AuthenticationToken = code;
                _context.SaveChanges();
                Console.WriteLine("Information correct:"+code);
                return "Information correct:"+code;
            }else{
                Console.WriteLine("Account information is incorrect, please check and try again.");
                return "Account information is incorrect, please check and try again.";
            }

        }

        public void DBConnect(){
            if(connection == null){
                connection = new MySqlConnection("Server=HUTCHYSERVER;Port=3306;Database=hutchymmo;Uid=callumhutchy;Pwd=gwynedd070995;");
                connection.Open();
            }
        }

         private static char[] chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static Random _random = new Random();

        public static string Generate(int length = 6)
        {
            string id = "";
            for (int i = 0; i < length; i++)
            {
                id += chars[_random.Next(chars.Length)];
            }
            return id;
        }

        private static string GenerateAuthToken(int length = 10){
            string code = "";
            for(int i = 0; i < length; i++){
                code += chars[_random.Next(36)];
            }
            return code;
        }

    }
}

[DataContract]
public class UserInfoInputJson{
    [DataMember(Name = "username")]
    public string username {get;set;}
    [DataMember(Name = "email")]
    public string email {get;set;}
    [DataMember(Name = "password")]
    public string password {get;set;}
    [DataMember(Name = "admin")]
    public int admin {get; set;}
}

[DataContract]
public class UserAndPass{
    [DataMember(Name = "username")]
    public string username;
    [DataMember(Name = "password")]
    public string password;
}