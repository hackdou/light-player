using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace LightPlayer.Test.Support
{
    public class TestFixture : IDisposable
    {
        private readonly WebApplicationFactory<TestStartup> _factory;

        private readonly IServiceScope _scope;

        public TestFixture()
        {
            _factory = new TestWebApplicationFactory();
            _scope = _factory.Services.CreateScope();
        }

        public void Dispose()
        {
            _scope.Dispose();
            _factory.Dispose();
        }

        public T GetRequiredService<T>()
        {
            return _scope.ServiceProvider.GetRequiredService<T>();
        }

        public IEnumerable<T> GetServices<T>()
        {
            return _scope.ServiceProvider.GetServices<T>();
        }
    }
}