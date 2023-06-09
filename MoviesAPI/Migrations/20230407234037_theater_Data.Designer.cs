﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoviesAPI;
using NetTopologySuite.Geometries;

#nullable disable

namespace MoviesAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230407234037_theater_Data")]
    partial class theater_Data
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MoviesAPI.Entities.Actor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Actors");

                    b.HasData(
                        new
                        {
                            Id = 5,
                            DOB = new DateTime(1962, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Jim Carrey"
                        },
                        new
                        {
                            Id = 6,
                            DOB = new DateTime(1965, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Robert Downey Jr."
                        },
                        new
                        {
                            Id = 7,
                            DOB = new DateTime(1981, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Chris Evans"
                        });
                });

            modelBuilder.Entity("MoviesAPI.Entities.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 4,
                            Name = "Aventura"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Animación"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Suspenso"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Romance"
                        });
                });

            modelBuilder.Entity("MoviesAPI.Entities.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("InTheaters")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LaunchDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Poster")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.ToTable("Movies");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            InTheaters = true,
                            LaunchDate = new DateTime(2019, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Avengers: Endgame"
                        },
                        new
                        {
                            Id = 3,
                            InTheaters = false,
                            LaunchDate = new DateTime(2024, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Avengers: Infinity Wars"
                        },
                        new
                        {
                            Id = 4,
                            InTheaters = false,
                            LaunchDate = new DateTime(2023, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Sonic the Hedgehog"
                        },
                        new
                        {
                            Id = 5,
                            InTheaters = false,
                            LaunchDate = new DateTime(2024, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Emma"
                        },
                        new
                        {
                            Id = 6,
                            InTheaters = false,
                            LaunchDate = new DateTime(2024, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Wonder Woman 1984"
                        });
                });

            modelBuilder.Entity("MoviesAPI.Entities.MoviesActors", b =>
                {
                    b.Property<int>("ActorId")
                        .HasColumnType("int");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<string>("Character")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("ActorId", "MovieId");

                    b.HasIndex("MovieId");

                    b.ToTable("MoviesActors");

                    b.HasData(
                        new
                        {
                            ActorId = 6,
                            MovieId = 2,
                            Character = "Tony Stark",
                            Order = 1
                        },
                        new
                        {
                            ActorId = 7,
                            MovieId = 2,
                            Character = "Steve Rogers",
                            Order = 2
                        },
                        new
                        {
                            ActorId = 6,
                            MovieId = 3,
                            Character = "Tony Stark",
                            Order = 1
                        },
                        new
                        {
                            ActorId = 7,
                            MovieId = 3,
                            Character = "Steve Rogers",
                            Order = 2
                        },
                        new
                        {
                            ActorId = 5,
                            MovieId = 4,
                            Character = "Dr. Ivo Robotnik",
                            Order = 1
                        });
                });

            modelBuilder.Entity("MoviesAPI.Entities.MoviesGenres", b =>
                {
                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.HasKey("GenreId", "MovieId");

                    b.HasIndex("MovieId");

                    b.ToTable("MoviesGenres");

                    b.HasData(
                        new
                        {
                            GenreId = 6,
                            MovieId = 2
                        },
                        new
                        {
                            GenreId = 4,
                            MovieId = 2
                        },
                        new
                        {
                            GenreId = 6,
                            MovieId = 3
                        },
                        new
                        {
                            GenreId = 4,
                            MovieId = 3
                        },
                        new
                        {
                            GenreId = 4,
                            MovieId = 4
                        },
                        new
                        {
                            GenreId = 6,
                            MovieId = 5
                        },
                        new
                        {
                            GenreId = 7,
                            MovieId = 5
                        },
                        new
                        {
                            GenreId = 6,
                            MovieId = 6
                        },
                        new
                        {
                            GenreId = 4,
                            MovieId = 6
                        });
                });

            modelBuilder.Entity("MoviesAPI.Entities.MoviesTheaters", b =>
                {
                    b.Property<int>("TheatherId")
                        .HasColumnType("int");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int?>("TheaterId")
                        .HasColumnType("int");

                    b.HasKey("TheatherId", "MovieId");

                    b.HasIndex("MovieId");

                    b.HasIndex("TheaterId");

                    b.ToTable("MoviesTheaters");
                });

            modelBuilder.Entity("MoviesAPI.Entities.Theater", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Point>("Location")
                        .HasColumnType("geography");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.HasKey("Id");

                    b.ToTable("Theaters");

                    b.HasData(
                        new
                        {
                            Id = 4,
                            Location = (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-69.9118804 18.4826214)"),
                            Name = "Sambil"
                        },
                        new
                        {
                            Id = 5,
                            Location = (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-69.856427 18.506934)"),
                            Name = "Megacentro"
                        },
                        new
                        {
                            Id = 6,
                            Location = (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-73.986227 40.730898)"),
                            Name = "Village East Cinema"
                        });
                });

            modelBuilder.Entity("MoviesAPI.Entities.MoviesActors", b =>
                {
                    b.HasOne("MoviesAPI.Entities.Actor", "Actor")
                        .WithMany("MoviesActors")
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoviesAPI.Entities.Movie", "Movie")
                        .WithMany("MoviesActors")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actor");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MoviesAPI.Entities.MoviesGenres", b =>
                {
                    b.HasOne("MoviesAPI.Entities.Genre", "Genre")
                        .WithMany("MoviesGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoviesAPI.Entities.Movie", "Movie")
                        .WithMany("MoviesGenres")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MoviesAPI.Entities.MoviesTheaters", b =>
                {
                    b.HasOne("MoviesAPI.Entities.Movie", "Movie")
                        .WithMany("MoviesTheaters")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoviesAPI.Entities.Theater", "Theater")
                        .WithMany("MoviesTheaters")
                        .HasForeignKey("TheaterId");

                    b.Navigation("Movie");

                    b.Navigation("Theater");
                });

            modelBuilder.Entity("MoviesAPI.Entities.Actor", b =>
                {
                    b.Navigation("MoviesActors");
                });

            modelBuilder.Entity("MoviesAPI.Entities.Genre", b =>
                {
                    b.Navigation("MoviesGenres");
                });

            modelBuilder.Entity("MoviesAPI.Entities.Movie", b =>
                {
                    b.Navigation("MoviesActors");

                    b.Navigation("MoviesGenres");

                    b.Navigation("MoviesTheaters");
                });

            modelBuilder.Entity("MoviesAPI.Entities.Theater", b =>
                {
                    b.Navigation("MoviesTheaters");
                });
#pragma warning restore 612, 618
        }
    }
}
