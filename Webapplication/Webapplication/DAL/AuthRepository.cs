using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.DAL
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _DB;

        private ILogger<AuthRepository> _Local_Log; //log objekt
        public AuthRepository(ApplicationDbContext DB, ILogger<AuthRepository> logger)
        {
            _DB = DB;
            _Local_Log = logger;
        }

        private static byte[] GenerateHash(string password, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(password: password, salt: salt, prf: KeyDerivationPrf.HMACSHA512, iterationCount: 1000, numBytesRequested: 32);
        }

        private static byte[] GenerateSalt()
        {
            var rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            rNGCryptoServiceProvider.GetBytes(salt);
            return salt;
        }

        public async Task<bool> SetAdministratorSessionToken(UserInfo userInfo)
        {
            try
            {
                Admin matchingAdmin = await _DB.Admins.FirstOrDefaultAsync(a => a.Username == userInfo.Username);
                byte[] hash = GenerateHash(userInfo.Password, matchingAdmin.Salt);
                bool result = hash.SequenceEqual(matchingAdmin.Password);

                return result;

            } catch (Exception e)
            {
                _Local_Log.LogError(e.Message);
                return false;
            }
        }


        //OOPS: this is for now in order to be able to create user that will be used to logg inn afterwards, this propably will be deleted in final version
        //thus it is implemented "bare minimum"
        public async Task<bool> RegisterAdministrator(UserInfo userInfo)
        {
            var salt = GenerateSalt();

            Admin newAdmin = new Admin
            {
                Username = userInfo.Username,
                Salt = salt,
                Password = GenerateHash(userInfo.Password, salt)
            };

            await _DB.AddAsync(newAdmin);
            _DB.SaveChanges();
            return true;
        }





    }
}
