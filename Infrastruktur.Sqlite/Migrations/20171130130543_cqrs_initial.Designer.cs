﻿// <auto-generated />
using Infrastruktur.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Billmorro.Tests.Migrations
{
    [DbContext(typeof(EventsDBContext))]
    [Migration("20171130130543_cqrs_initial")]
    partial class cqrs_initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Infrastruktur.Sqlite.SerializedEvent", b =>
                {
                    b.Property<Guid>("EventID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CommandId");

                    b.Property<Guid>("CorrelationID");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid>("DeviceID");

                    b.Property<int>("EventSet");

                    b.Property<Guid>("EventType");

                    b.Property<string>("Modul");

                    b.Property<Guid>("ModulInstanceID");

                    b.Property<string>("Payload");

                    b.Property<Guid>("SessionID");

                    b.Property<Guid>("StreamID");

                    b.Property<Guid>("UserID");

                    b.HasKey("EventID");

                    b.ToTable("CQRS_Events");
                });
#pragma warning restore 612, 618
        }
    }
}
