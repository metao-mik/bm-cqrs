using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using Billmorro.Infrastruktur.Eventsourcing;
using Billmorro.Infrastruktur.Reactive;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace Infrastruktur.Sqlite
{
    public interface Serializer
    {
        (Guid typeid, string blob) Serialize(Event @event);
        Event Deserialize(Guid typeid, string blob);
    }

    public class SqliteEventstore : EventStore
    {
        private readonly Serializer _serializer;
        private readonly Func<DateTime> _clock;
        private readonly EventsDBContext _dbcontext;

        private readonly Subject<int> _eventsets = new Subject<int>();
        public IObservable<int> Eventsets => _eventsets;

        public SqliteEventstore(string databaseFile, Serializer serializer, Func<DateTime> clock)
        {
            _serializer = serializer;
            _clock = clock;
            _dbcontext = new EventsDBContext(databaseFile);
        }

        public void UpdateSchema()
        {
            _dbcontext.Database.Migrate();
        }

        public IEnumerable<Eventset> History()
        {
            return
                _dbcontext.Events
                    .ToList()
                    .GroupBy(_ => _.EventSet)
                    .Select(es => new Eventset(es.Key,
                        new ReadOnlyCollection<EventEnvelope>(es.Select(Deserialize_Event).ToList())));
        }

        private EventEnvelope Deserialize_Event(SerializedEvent src)
        {
            return new EventEnvelope(
                src.StreamID,
                _serializer.Deserialize(src.EventType, src.Payload),
                src.CreatedOn
            );
        }

        public IEnumerable<EventEnvelope> History(Guid stream)
        {
            return
                _dbcontext.Events
                    .Where(_ => _.StreamID == stream)
                    .ToList()
                    .Select(Deserialize_Event);
        }

        public void Publish(List<UncommittedEvent> uncommittedEvents)
        {
            int eventsetid;

            using (var tx = _dbcontext.Database.BeginTransaction())
            {
                eventsetid = LastEventSetId + 1;
                foreach (var uncommittedEvent in uncommittedEvents)
                {
                    var serialized = _serializer.Serialize(uncommittedEvent.Event);
                    _dbcontext.Events.Add(new SerializedEvent
                    {
                        EventSet = eventsetid,
                        CreatedOn = _clock(),
                        EventID = Guid.NewGuid(),
                        EventType = serialized.typeid,
                        Payload = serialized.blob,
                        StreamID = uncommittedEvent.Stream
                    });
                }
                _dbcontext.SaveChanges();
                tx.Commit();
            }

            // Synchron: eigentlich neuer Thread aus Threadpool.
            _eventsets.Next(eventsetid);
        }

        public int LastEventSetId
        {
            get
            {
                var eventsetids = _dbcontext.Events.Select(_ => _.EventSet).ToList();
                return eventsetids.Any() ? eventsetids.Max() : 0;
            }
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
