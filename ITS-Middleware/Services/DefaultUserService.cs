﻿using ITS_Middleware.Models;
using ITS_Middleware.Models.Entities;

namespace ITS_Middleware.Services
{
    public class DefaultUserService
    {
        public MiddlewareDbContext? _context;

        public DefaultUserService(MiddlewareDbContext context)
        {
            _context = context;
            try
            {
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());
            }
        }
    }

        
}