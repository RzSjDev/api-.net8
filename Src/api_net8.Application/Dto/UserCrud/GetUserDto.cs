using System;
using System.Security.Cryptography.X509Certificates;
using api.net.Models;

namespace api_net9.Application.Dto.UserCrud
{
    public class GetUserDto
    {

        
        public string name { get; set; }
        public string family { get; set; }
        public int age { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string email { get; set; }
    }




}