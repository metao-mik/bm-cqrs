using System;
using System.Collections.Generic;
using System.Linq;
using Billmorro.Infrastruktur.Eventsourcing;
using Billmorro.Infrastruktur.Implementierung;
using Billmorro.Infrastruktur.Reactive;
using Infrastruktur.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Billmorro.Tests.Infrastruktur
{
    [TestClass]
    public sealed class EventStoreTests_InMemory : EventStoreTests
    {
        protected override EventStore Create_SUT()
        {
            return new InMemoryEventStore(() => _timestamp);
        }

    }

    [TestClass]
    public sealed class EventStoreTests_Sqlite : EventStoreTests
    {
        protected override EventStore Create_SUT()
        {
            return new SqliteEventstore();
        }

    }

    public abstract class EventStoreTests
    {
        protected abstract EventStore Create_SUT();

        [TestInitialize]
        public void Setup()
        {
            _timestamp = DateTime.UtcNow;
            SUT = Create_SUT();
        }

        private IEnumerable<Eventset> Historie()
        {
            return SUT.History();
        }

        private IEnumerable<EventEnvelope> Historie_per_Stream(Guid stream)
        {
            return SUT.History(stream);
        }

        private void Publish(Guid stream, params Event[] @event)
        {
            SUT.Publish(@event.Select(e => new UncommittedEvent(stream, e)).ToList());
        }

        private void Publish2(Guid stream1, Event @event1, Guid stream2, Event @event2)
        {
            SUT.Publish(new List<UncommittedEvent>
            {
                new UncommittedEvent(stream1, @event1),
                new UncommittedEvent(stream2, @event2)
            });
        }

        private void WaitForConsistency()
        {
        }

        private Action Subscribe(Action<int> observer)
        {
            return SUT.Eventsets.Subscribe(observer).Dispose;
        }

        private int CurrentEventSetId()
        {
            return SUT.LastEventSetId;
        }

        private DateTime MarkTime()
        {
            _timestamp += TimeSpan.FromSeconds(10);
            return _timestamp;
        }

        private Event CreateEvent() => new TestEvent_Struct(Guid.NewGuid().ToString());
        private Guid CreateStream() => Guid.NewGuid();

        
        private EventStore SUT;
        protected DateTime _timestamp;


        [TestMethod]
        public void Initial_ist_die_Historie_leer()
        {
            Assert.AreEqual(0, Historie().Count());                
        }

        [TestMethod]
        public void Nach_Publish_eines_Events_enthält_die_Historie_ein_EventSet_mit_einem_Event()
        {
            var @event = CreateEvent();
            var stream = CreateStream();

            var time = MarkTime();
            Publish(stream, @event);

            var history = Historie().ToList();
            Assert.AreEqual(1, history.Count);

            var eventset = history.Single();
            Assert.AreEqual(1, eventset.Id);
            Assert.AreEqual(1, eventset.Events.Count);
            Assert.AreEqual(stream, eventset.Events.Single().Stream);
            Assert.AreEqual(time, eventset.Events.Single().Timestamp);
            Assert.AreEqual(@event, eventset.Events.Single().Event);
        }

        [TestMethod]
        public void Nach_einem_zweiten_Publish_enthält_die_Historie_ein_zweites_EventSet()
        {
            var stream1 = CreateStream();
            var @event1 = CreateEvent();
            var time1 = MarkTime();
            Publish(stream1, @event1);

            var stream2 = CreateStream();
            var @event2 = CreateEvent();
            var time2 = MarkTime();
            Publish(stream2, @event2);

            var history = Historie().ToList();
            Assert.AreEqual(2, history.Count);
            {
                var eventset = history.Take(1).Single();
                Assert.AreEqual(1, eventset.Id);
                Assert.AreEqual(1, eventset.Events.Count);
                Assert.AreEqual(stream1, eventset.Events.Single().Stream);
                Assert.AreEqual(time1, eventset.Events.Single().Timestamp);
                Assert.AreEqual(@event1, eventset.Events.Single().Event);
            }
            {
                var eventset = history.Skip(1).Single();
                Assert.AreEqual(2, eventset.Id);
                Assert.AreEqual(1, eventset.Events.Count);
                Assert.AreEqual(stream2, eventset.Events.Single().Stream);
                Assert.AreEqual(time2, eventset.Events.Single().Timestamp);
                Assert.AreEqual(@event2, eventset.Events.Single().Event);
            }
        }

        [TestMethod]
        public void Publish_von_mehreren_Events_in_einem_EventSet__ein_Stream()
        {
            var @event1 = CreateEvent();
            var @event2 = CreateEvent();
            var stream = CreateStream();

            var time = MarkTime();
            Publish(stream, @event1, @event2);

            var history = Historie().ToList();
            Assert.AreEqual(1, history.Count);

            var eventset = history.Single();
            Assert.AreEqual(1, eventset.Id);
            Assert.AreEqual(2, eventset.Events.Count);
            Assert.IsTrue(eventset.Events.Any(e => @event1==e.Event));
            Assert.IsTrue(eventset.Events.Any(e => @event2==e.Event));
        }

        [TestMethod]
        public void Publish_von_mehreren_Events_in_einem_EventSet__zwei_Streams()
        {
            var @event1 = CreateEvent();
            var @event2 = CreateEvent();
            var stream1 = CreateStream(); // z.B. Bon1 => Die ID des Aggregats 
            var stream2 = CreateStream(); // z.B. Bon2

            Publish2(stream1, @event1, stream2, @event2);

            var history = Historie().ToList();
            Assert.AreEqual(1, history.Count);

            var eventset = history.Single();
            Assert.AreEqual(1, eventset.Id);
            Assert.AreEqual(2, eventset.Events.Count);
            Assert.IsTrue(eventset.Events.Any(e => @event1 == e.Event && e.Stream == stream1));
            Assert.IsTrue(eventset.Events.Any(e => @event2 == e.Event && e.Stream == stream2));
        }



        [TestMethod]
        public void Events_koennen_in_Streams_abgerufen_werden()
        {
            var @event1 = CreateEvent();
            var @event2 = CreateEvent();
            var @event3 = CreateEvent();
            var stream1 = CreateStream();
            var stream2 = CreateStream();

            Publish(stream1, @event1);
            Publish2(stream2, @event2, stream1, @event3);

            var historie = Historie_per_Stream(stream1).ToList();
            Assert.AreEqual(2, historie.Count());
            Assert.IsTrue(historie.Any(e => @event1 == e.Event && e.Stream == stream1));
            Assert.IsTrue(historie.Any(e => @event3 == e.Event && e.Stream == stream1));
        }

        [TestMethod]
        public void Subscriber_werden_über_neue_Eventset_benachrichtigt()
        {
            var eventsetid0 = CurrentEventSetId();
            var eventsetid = eventsetid0;
            var unsubscribe = Subscribe(eid => eventsetid = eid);

            var @event1 = CreateEvent();
            var stream1 = CreateStream();

            Publish(stream1, @event1);

            WaitForConsistency();

            Assert.AreNotEqual(eventsetid0,eventsetid);
        }


        [TestMethod]
        public void Nach_dem_Unsubscribe_werden_keine_Benachrichtigungen_mehr_Empfangen()
        {
            var eventsetid0 = CurrentEventSetId();
            var eventsetid = eventsetid0;
            var unsubscribe = Subscribe(eid => eventsetid = eid);

            unsubscribe();
            var @event1 = CreateEvent();
            var stream1 = CreateStream();

            Publish(stream1, @event1);

            WaitForConsistency();

            Assert.AreEqual(eventsetid0, eventsetid);
        }
    }

    public struct TestEvent_Struct : Event
    {
        public TestEvent_Struct(string payload)
        {
            Payload = payload;
        }

        public readonly string Payload;
    }

    /*public class TestEvent_Class_Field : Event
    {
        public TestEvent_Class_Field()
        {
        }

        public TestEvent_Class_Field(string payload)
        {
            Payload = payload;
        }

        public readonly string Payload;
    }

    public class TestEvent_Class_Property : Event
    {
        public TestEvent_Class_Property()
        {
        }

        public TestEvent_Class_Property(string payload)
        {
            Payload = payload;
        }

        public string Payload { get; set; }
    }*/
}