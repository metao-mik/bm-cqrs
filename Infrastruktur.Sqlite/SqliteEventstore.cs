using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Billmorro.Infrastruktur.Eventsourcing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastruktur.Sqlite
{
    public class SqliteEventstore : EventStore
    {
        private readonly EventsDBContext _dbcontext;

        public SqliteEventstore(string databaseFile)
        {
            _dbcontext = new EventsDBContext(databaseFile);
        }

        public void UpdateSchema()
        {
            _dbcontext.Database.Migrate();
        }

        public IEnumerable<Eventset> History()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventEnvelope> History(Guid stream)
        {
            throw new NotImplementedException();
        }

        public void Publish(List<UncommittedEvent> uncommittedEvents)
        {
            throw new NotImplementedException();
        }

        public IObservable<int> Eventsets
        {
            get { throw new NotImplementedException(); }
        }

        public int LastEventSetId
        {
            get { throw new NotImplementedException(); }
        }
    }

    [Table("CQRS_Events")]
    public class SerializedEvent
    {
        [Key]
        public Guid EventID { get; set; }
        public Guid StreamID { get; set; }
        //public int StreamLfd { get; set; }
        public Guid CommandId { get; set; }
        public Guid UserID { get; set; }
        public Guid DeviceID { get; set; }
        public Guid SessionID { get; set; }
        public Guid CorrelationID { get; set; }
        public string Modul { get; set; }
        public Guid ModulInstanceID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int EventSet { get; set; }
        public Guid EventType { get; set; }
        public string Payload { get; set; }
    }

    public class EventsDBContext: DbContext
    {
        public EventsDBContext(string databaseFile) : base(new DbContextOptions<EventsDBContext>())
        {
            _databaseFile = databaseFile;
        }

        private readonly string _databaseFile;
        public EventsDBContext(DbContextOptions options) : base(options) { }

        public DbSet<SerializedEvent> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source="+_databaseFile);
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class EventsDbContextFactory : IDesignTimeDbContextFactory<EventsDBContext>
    {
        public EventsDBContext CreateDbContext(string[] args)
        {
            return new EventsDBContext(new DbContextOptions<EventsDBContext>());
        }
    }
}
