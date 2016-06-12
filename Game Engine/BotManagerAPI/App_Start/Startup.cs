﻿using BotManagerAPI;
using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace BotManagerAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("PlayerDashboardDB");

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}