using BenchmarkDotNet.Attributes;

using System;
using System.Linq;
using Dapper.Tests.Performance.Linq2Db;
using LinqToDB;
using LinqToDB.Data;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.Tests.Performance
{
    [Description("LINQ to DB")]
    public class Linq2DBBenchmarks : BenchmarkBase
    {
        private Linq2DBContext _dbContext;

        private static readonly Func<Linq2DBContext, int, Post> compiledQuery = CompiledQuery.Compile((Linq2DBContext db, int id) => db.Posts.First(c => c.Id == id));
        private static readonly Func<Linq2DBContext, int, CancellationToken, Task<Post>> compiledQueryAsync = CompiledQuery.Compile((Linq2DBContext db, int id, CancellationToken token) => db.Posts.FirstOrDefaultAsync(c => c.Id == id, token));

        [GlobalSetup]
        public void Setup()
        {
            BaseSetup();
            //DataConnection.DefaultSettings = new Linq2DBSettings(_connection.ConnectionString);
            _dbContext = new Linq2DBContext(_connection);
        }

        [Benchmark(Description = "First")]
        public Post First()
        {
            Step();
            return _dbContext.Posts.First(p => p.Id == i);
        }

        [Benchmark(Description = "First (Compiled)")]
        public Post Compiled()
        {
            Step();
            return compiledQuery(_dbContext, i);
        }
        
        [Benchmark(Description = "First (Compiled) async")]
        public async Task<Post> CompiledASync()
        {
            Step();
            return await compiledQueryAsync(_dbContext, i, CancellationToken.None);
        }

        [Benchmark(Description = "Query<T>")]
        public Post Query()
        {
            Step();
            return _dbContext.Query<Post>("select * from Posts where Id = @id", new { id = i }).First();
        }

        [Benchmark(Description = "Query<T> linq")]
        public Post QueryLinq()
        {
            Step();
            return _dbContext.Posts.First( p => p.Id == i);
        }
        
        [Benchmark(Description = "Query<T> linq async")]
        public async Task<Post> QueryLinqASync()
        {
            Step();
            return await _dbContext.Posts.FirstAsync( p => p.Id == i);
        }
    }
}
