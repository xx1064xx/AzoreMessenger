using AzoreMessanger.Data;
using AzoreMessanger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AzoreMessanger.Controller
{
    public class BrowserController
    {
    }

    public class BrowserInfo
    {
        public long userId { get; set; }
        public string browsername { get; set; }
    }

    // Api configurations

    [Route("api/[controller]")]
    [ApiController]

    public class BrowsersController : ControllerBase
    {
        private readonly MessengerAppContext _context;
        public BrowsersController(MessengerAppContext context)
        {
            _context = context;
        }


        [HttpPost("setBrowser")]
        public IActionResult setBrowser(BrowserInfo browserInfo)
        {

            // rechnet die tiefstverfügbare Browsernummer aus (Für bessere Darstellung)
            int minBrowserNummer = 0;

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Jusa\\Documents\\asp.net\\WebApplication1\\Data\\azoreDb.mdf;Integrated Security=True;Connect Timeout=30";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                long x = browserInfo.userId;

                // SQL-Abfrage, um die niedrigste unbenutzte Browsernummer für UserID 2 zu erhalten
                string sqlQuery = $@"
        SELECT TOP 1 n
        FROM (
            SELECT 1 AS n
            UNION ALL
            SELECT MAX(browsernumber) + 1
            FROM Browsers
            WHERE userID = {x}
        ) AS Numbers
        WHERE n NOT IN (SELECT browsernumber FROM Browsers WHERE userID = {x})
        ORDER BY n";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // Führe die Abfrage aus und lese das Ergebnis
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Das Ergebnis abrufen und anzeigen
                            minBrowserNummer = Convert.ToInt32(reader[0]);
                            Console.WriteLine($"Die niedrigste unbenutzte Browsernummer für UserID {x} ist: {minBrowserNummer}");
                        }
                        else
                        {
                            Console.WriteLine("Keine unbenutzten Browsernummern gefunden.");
                        }
                    }
                }
            }


            Browser newBrowser = new Browser()
            {
                browsername = browserInfo.browsername,
                browsernumber = minBrowserNummer,
                userId = browserInfo.userId,
            };
            _context.Browsers.Add(newBrowser);
            _context.SaveChanges();
            return Ok(minBrowserNummer);
            
        }


    }



}