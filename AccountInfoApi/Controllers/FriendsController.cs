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
    [Route("api/friends/[controller]")]
    public class FriendsController : Controller
    {
        private MySqlConnection connection = null;
        private hutchymmoContext _context;

        public FriendsController(hutchymmoContext context){
            _context = context;
        }

        [HttpGet("username/{Username}")]
        public async Task<ActionResult<IEnumerable<FriendsList>>> GetUserInfos(){
            return await _context.UserInformation.ToListAsync();
        }

        public void DBConnect(){
            if(connection == null){
                connection = new MySqlConnection("Server=HUTCHYSERVER;Port=3306;Database=hutchymmo;Uid=callumhutchy;Pwd=gwynedd070995;");
                connection.Open();
            }
        }

    }
}