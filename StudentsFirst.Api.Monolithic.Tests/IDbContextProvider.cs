using System;
using Microsoft.EntityFrameworkCore;

namespace StudentsFirst.Api.Monolithic.Tests
{
    public interface IDbContextProvider<TContext>: IDisposable
        where TContext : DbContext
    {
        public TContext Context { get; }
    }
}
