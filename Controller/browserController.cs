﻿using AzoreMessanger.Data;
using AzoreMessanger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            int minUnusedBrowserNumber = 1;


            Browser newBrowser = new Browser()
            {
                browsername = browserInfo.browsername,
                browsernumber = minUnusedBrowserNumber,
                userId = browserInfo.userId,
            };
            _context.Browsers.Add(newBrowser);
            _context.SaveChanges();
            return Ok(minUnusedBrowserNumber);
            
        }


    }



}