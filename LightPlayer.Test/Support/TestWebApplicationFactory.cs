using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LightPlayer.Test.Support
{
    public class TestWebApplicationFactory : WebApplicationFactory<TestStartup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost
                .CreateDefaultBuilder<TestStartup>(Array.Empty<string>())
                .UseStartup<TestStartup>();
        }
    }
}