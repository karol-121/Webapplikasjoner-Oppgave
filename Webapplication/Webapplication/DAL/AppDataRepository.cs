using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.DAL
{
    public class AppDataRepository : IAppDataRepository
    {
        private readonly ApplicationDbContext _DB;

        public AppDataRepository(ApplicationDbContext DB)
        {
            _DB = DB;
        }

        //summary: funksjon som henter alle ruter som finnes inn i databasen
        //returns: liste med route objekter
        public async Task<List<Route>> GetRoutes()
        {
            return await _DB.Routes.ToListAsync();
        }

        //summary: funksjon som henter route med bestemt id
        //parameters: int id - id til objektet som skal vises
        //returns: Route objekt
        public async Task<Route> GetRoute(int id)
        {
            return await _DB.Routes.FindAsync(id);
        }

        //summary: funksjon som legger inn gitt route objekt.
        //her blir det også generet et relatert return objekt som også blir lagt inn
        //parameters: Route route - route objekt som skal legges inn
        public async Task AddRoute(Route route)
        {
            //lage et relatert return route
            Route route_return = new Route
            {
                Origin = route.Destination,
                Destination = route.Origin,
                Return_id = route.Id
            };

            //koble relatert objekt til den opprinelige
            route.Return_id = route_return.Id; //dette er ikke sikkert at fungerer altså her kan det hende at objektet har ikke id enda

            //legge begge inn i databasen
            _DB.Routes.Add(route);
            _DB.Routes.Add(route_return);

            await _DB.SaveChangesAsync();
        }

        //summary: funksjon som endrer allerede eksisterende objekt med data oppgitt i inn objekt
        //her påføres det også relevante endringer på relatert objekt 
        //parameters: Route route - objekt med nye verdier 
        public async Task EditRoute(Route route)
        {
            var a = await _DB.Routes.FindAsync(route.Id); //hente opprinelig objekt
            var b = await _DB.Routes.FindAsync(route.Return_id); //hente relatert objekt

            //påføre endringer på oppringlig objekt
            a.Origin = route.Origin;
            a.Destination = route.Destination;

            //påføre endringer på relatert objekt
            b.Origin = route.Destination;
            b.Destination = route.Origin;

            //sette inn de nye objekter (vet ikke om det er nødvendig)
            _DB.Routes.Add(a);
            _DB.Routes.Add(b);

            //lagre endringer
            await _DB.SaveChangesAsync();
        }

        //summary: funksjon som sletter objekt med bestemt id
        //her blir det også relatert objektet slettet
        //parameters: int id - id til objektet som skal fjernes
        public async Task DeleteRoute(int id)
        {
            var a = await _DB.Routes.FindAsync(id); //find the requested route to delete
            var b = await _DB.Routes.FindAsync(a.Return_id); //find the related route that also should be deleted

            _DB.Routes.Remove(a); //remove the requested route
            _DB.Routes.Remove(b); //remover related route

            await _DB.SaveChangesAsync(); //lagrer endringer 
        }
    }
}
