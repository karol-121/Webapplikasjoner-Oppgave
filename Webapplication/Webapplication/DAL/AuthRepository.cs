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
        private readonly ApplicationDbContext _DB; //database objekt

        private ILogger<AuthRepository> _Local_Log; //log objekt
        public AuthRepository(ApplicationDbContext DB, ILogger<AuthRepository> logger)
        {
            _DB = DB;
            _Local_Log = logger;
        }


        //summary: hjelpe funksjon som genererer hash verdi ut av et passord og salt
        //parameters: string passord - passord streng, byte[] salt - array med bytes som representerer salt 
        //returns: byte[] array som inneholder hash verdi 
        private static byte[] GenerateHash(string password, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(password: password, salt: salt, prf: KeyDerivationPrf.HMACSHA512, iterationCount: 1000, numBytesRequested: 32);
        }

        //summary: hjelpe funksjon som genererer salt verdi, dette engentlig brukes kun ved oppretting av nye brukerene
        //returns: byte[] array som inneholder salt verdi 
        private static byte[] GenerateSalt()
        {
            var rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            rNGCryptoServiceProvider.GetBytes(salt);
            return salt;
        }

        //summary: funksjon som indetifiserer administrator ved å sammenlike bruker info med registrerte brukerene 
        //parameters: UserInfo userInfo - objekt som inneholder bruker information som brukes for verifikasjon
        //returns: true - hvis identifikasjon er vellykket, false - hvis identifikasjon er ikke vellykket
        public async Task<bool> AuthenticateAdministrator(UserInfo userInfo)
        {
            try
            {
                Admin matchingAdmin = await _DB.Admins.FirstOrDefaultAsync(a => a.Username == userInfo.Username); //hentes admin med passende bruker navn
                byte[] hash = GenerateHash(userInfo.Password, matchingAdmin.Salt); //generere hash verdi ut av oppgitt passord og lagret salt verdi
                bool result = hash.SequenceEqual(matchingAdmin.Password); //sjekkes genererte hash verdi mot den orginale, som verifiserer som oppgitt passord er riktig eller ikke

                return result; //true - hash verdier er like, false - hash verdier er ikke like

            } catch (Exception e) //hvis noe skjer, så logges det og returneres false 
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
