using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Moq;


namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;
        private Mock<IRepository<Booking>> mockBookingRepository;
        private Mock<IRepository<Room>> mockRoomRepository;

        public BookingManagerTests(){
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            mockBookingRepository = new Mock<IRepository<Booking>>();
            mockRoomRepository = new Mock<IRepository<Room>>();
            bookingManager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);
        }

        [Theory]
        [MemberData(nameof(Get_FindAvailableRoom_ThrowsArgumentException_Data))]
        public void FindAvailableRoom_ThrowsArgumentException(DateTime start, DateTime end)
        {
            // Act
            Action act = () => bookingManager.FindAvailableRoom(start, end);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        public static IEnumerable<object[]> Get_FindAvailableRoom_ThrowsArgumentException_Data()
        {
            var data = new List<object[]>
            {
                new object[] { DateTime.Today, DateTime.Today },
                new object[] { DateTime.Today.AddDays(-1), DateTime.Today },
                new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(-1) },
            };
            return data;
        }

        [Theory]
        [MemberData(nameof(Get_FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne_Data))]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne(DateTime start, DateTime end)
        {
            // Arrange
            var mockBookings = new List<Booking>
            {
                new Booking()
                {
                    Id = 1,
                    StartDate = DateTime.Today.AddDays(5),
                    EndDate = DateTime.Today.AddDays(6),
                    IsActive = true,
                    CustomerId = 1,
                    RoomId = 1
                }
            };
            var mockRooms = new List<Room>
            {
                new Room()
                {
                    Id = 1,
                    Description = "A cozy and nice room for the price"
                }
            };

            mockBookingRepository
                .Setup(m => m.GetAll())
                .Returns(mockBookings);

            mockRoomRepository
                .Setup(m => m.GetAll())
                .Returns(mockRooms);
            
            // Act
            int roomId = bookingManager.FindAvailableRoom(start, end);
            // Assert
            Assert.NotEqual(-1, roomId);
        }
        
        public static IEnumerable<object[]> Get_FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne_Data()
        {
            var data = new List<object[]>
            {
                new object[] { DateTime.Today.AddDays(7), DateTime.Today.AddDays(8) },
                new object[] { DateTime.Today.AddDays(3), DateTime.Today.AddDays(4) },
            };
            return data;
        }

        [Theory]
        [MemberData(nameof(Get_FindAvailableRoom_RoomAvailable_ReturnsAvailableRoom_Data))]
        public void FindAvailableRoom_RoomAvailable_ReturnsAvailableRoom(DateTime start, DateTime end)
        {
            // This test was added to satisfy the following test design
            // principle: "Tests should have strong assertions".

            // Arrange
            var mockBookings = new List<Booking>
            {
                new Booking()
                {
                    Id = 1,
                    StartDate = DateTime.Today.AddDays(5),
                    EndDate = DateTime.Today.AddDays(6),
                    IsActive = true,
                    CustomerId = 1,
                    RoomId = 1
                }
            };
            var mockRooms = new List<Room>
            {
                new Room()
                {
                    Id = 1,
                    Description = "A cozy and nice room for the price"
                }
            };
            
            mockBookingRepository
                .Setup(m => m.GetAll())
                .Returns(mockBookings);

            mockRoomRepository
                .Setup(m => m.GetAll())
                .Returns(mockRooms);
            
            // Act
            int roomId = bookingManager.FindAvailableRoom(start, end);

            var bookingForReturnedRoomId = mockBookingRepository.Object.GetAll().Where(
                b => b.RoomId == roomId
                && b.StartDate <= start
                && b.EndDate >= end
                && b.IsActive);
            
            // Assert
            Assert.Empty(bookingForReturnedRoomId);
        }
        public static IEnumerable<object[]> Get_FindAvailableRoom_RoomAvailable_ReturnsAvailableRoom_Data()
        {
            var data = new List<object[]>
            {
                new object[] { DateTime.Today.AddDays(7), DateTime.Today.AddDays(8) },
                new object[] { DateTime.Today.AddDays(3), DateTime.Today.AddDays(4) },
            };
            return data;
        }

        [Theory]
        [MemberData(nameof(Get_CreateBooking_RoomIsAvailable_ReturnsTrue_Data))]
        public void CreateBooking_RoomIsAvailable_ReturnsTrue(Booking booking)
        {
            //Arrange
            var mockBookings = new List<Booking>
            {
                new Booking()
                {
                    Id = 1,
                    StartDate = DateTime.Today.AddDays(5),
                    EndDate = DateTime.Today.AddDays(6),
                    IsActive = true,
                    CustomerId = 1,
                    RoomId = 1
                }
            };
            var mockRooms = new List<Room>
            {
                new Room()
                {
                    Id = 1,
                    Description = "A cozy and nice room for the price"
                },
                new Room()
                {
                    Id = 2,
                    Description = "A less cozy room. Might have bed bugs."
                },
            };
            
            mockBookingRepository
                .Setup(m => m.GetAll())
                .Returns(mockBookings);

            mockRoomRepository
                .Setup(m => m.GetAll())
                .Returns(mockRooms);
            
            //Act
            var res = bookingManager.CreateBooking(booking);
            
            //Assert
            Assert.True(res);
        }
        
        public static IEnumerable<object[]> Get_CreateBooking_RoomIsAvailable_ReturnsTrue_Data()
        {
            var data = new List<object[]>
            {
                new object[] { new Booking()
                {
                    Id = 2,
                    StartDate = DateTime.Today.AddDays(3),
                    EndDate = DateTime.Today.AddDays(4),
                    IsActive = true,
                    CustomerId = 2,
                }},
                new object[] { new Booking()
                {
                    Id = 2,
                    StartDate = DateTime.Today.AddDays(7),
                    EndDate = DateTime.Today.AddDays(8),
                    IsActive = true,
                    CustomerId = 2,
                }},
                new object[] { new Booking()
                {
                    Id = 2,
                    StartDate = DateTime.Today.AddDays(5),
                    EndDate = DateTime.Today.AddDays(6),
                    IsActive = true,
                    CustomerId = 2,
                }},
            };
            return data;
        }
        
        [Theory]
        [MemberData(nameof(Get_CreateBooking_RoomIsNotAvailable_ReturnsFalse_Data))]
        public void CreateBooking_RoomIsNotAvailable_ReturnsFalse(Booking booking)
        {
            var mockBookings = new List<Booking>
            {
                new Booking()
                {
                    Id = 1,
                    StartDate = DateTime.Today.AddDays(5),
                    EndDate = DateTime.Today.AddDays(6),
                    IsActive = true,
                    CustomerId = 1,
                }
            };
            var mockRooms = new List<Room>
            {
                new Room()
                {
                    Id = 1,
                    Description = "A cozy and nice room for the price"
                },
            };
            //Arrange
            mockBookingRepository
                .Setup(m => m.GetAll())
                .Returns(mockBookings);

            mockRoomRepository
                .Setup(m => m.GetAll())
                .Returns(mockRooms);
            
            //Act
            var res = bookingManager.CreateBooking(booking);
            
            //Assert
            Assert.False(res);
        }
        public static IEnumerable<object[]> Get_CreateBooking_RoomIsNotAvailable_ReturnsFalse_Data()
        {
            var data = new List<object[]>
            {
                new object[] { new Booking()
                {
                    Id = 2,
                    StartDate = DateTime.Today.AddDays(4),
                    EndDate = DateTime.Today.AddDays(5),
                    IsActive = true,
                    CustomerId = 2,
                    RoomId = 1
                }},
                new object[] { new Booking()
                {
                    Id = 2,
                    StartDate = DateTime.Today.AddDays(5),
                    EndDate = DateTime.Today.AddDays(6),
                    IsActive = true,
                    CustomerId = 2,
                    RoomId = 1
                }},
                new object[] { new Booking()
                {
                    Id = 2,
                    StartDate = DateTime.Today.AddDays(6),
                    EndDate = DateTime.Today.AddDays(7),
                    IsActive = true,
                    CustomerId = 2,
                    RoomId = 1
                }},
            };
            return data;
        }

        [Fact]
        public void GetFullyOccupiedDates_StartDateIsGreaterThanEndDate_ThrowException()
        {
            //Arrange
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(-3);
            
            // Act
            Action act = () => bookingManager.GetFullyOccupiedDates(start, end);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void GetFullyOccupiedDates_DateIsFullyBooked_ReturnsDate()
        {
            var data = GetBookingData();
            //Arrange
            mockBookingRepository
                .Setup(m => m.GetAll())
                .Returns(data);

            mockRoomRepository
                .Setup(m => m.GetAll())
                .Returns(GetRoomData());
            
            //Act
            var res = bookingManager.GetFullyOccupiedDates(data.First().StartDate, data.First().EndDate);
            
            //Assert
            Assert.NotEmpty(res);
            Assert.True(res.Count == 2);
            Assert.Contains(data.First().StartDate, res);
            Assert.Contains(data.First().EndDate, res);
        }
        
        [Fact]
        public void GetFullyOccupiedDates_OneDateIsBookedNextDateIsAvailable_ReturnsOneDate()
        {
            var start = DateTime.Today.AddDays(1);
            var endFirst = DateTime.Today.AddDays(2);
            var endSecond = DateTime.Today.AddDays(3);
            
            var bookings = new List<Booking>
            {
                new Booking()
                {
                    Id = 1,
                    StartDate = DateTime.Today.AddDays(1),
                    EndDate = endFirst,
                    IsActive = true,
                    CustomerId = 1,
                    RoomId = 1
                },
                new Booking()
                {
                    Id = 2,
                    StartDate = start,
                    EndDate = endSecond,
                    IsActive = true,
                    CustomerId = 2,
                    RoomId = 2
                },
            };
            
            var rooms = new List<Room>
            {
                new Room()
                {
                    Id = 1,
                    Description = "A cozy and nice room for the price."
                },
                new Room()
                {
                    Id = 2,
                    Description = "A less cozy room. May contain bed bugs."
                }
            };
            //Arrange
            mockBookingRepository
                .Setup(m => m.GetAll())
                .Returns(bookings);

            mockRoomRepository
                .Setup(m => m.GetAll())
                .Returns(rooms);
            
            //Act
            var res = bookingManager.GetFullyOccupiedDates(start, endSecond);
            
            //Assert
            Assert.NotEmpty(res);
            Assert.True(res.Count == 2);
            Assert.DoesNotContain(endSecond, res);
            Assert.Contains(start, res);
            Assert.Contains(endFirst, res);
        }

        [Fact]
        public void GetFullyOccupiedDates_ThereAreNoBookings_ReturnEmptyList()
        {
            var start = DateTime.Today.AddDays(1);
            var end = DateTime.Today.AddDays(2);
            
            var bookings = new List<Booking>
            {
            };
            
            //Arrange
            mockBookingRepository
                .Setup(m => m.GetAll())
                .Returns(bookings);

            mockRoomRepository
                .Setup(m => m.GetAll())
                .Returns(GetRoomData());
            
            //Act
            var res = bookingManager.GetFullyOccupiedDates(start, end);
            
            //Assert
            Assert.Empty(res);
        }
        public static Booking GetBooking()
        {
            return
                new Booking()
                {
                    Id = 2,
                    StartDate = DateTime.Today.AddDays(20),
                    EndDate = DateTime.Today.AddDays(30),
                    IsActive = true,
                    CustomerId = 2,
                    RoomId = 1
                };
        }

        public static IEnumerable<Booking> GetBookingData()
        {
            var bookings = new List<Booking>
            {
                new Booking()
                {
                    Id = 1,
                    StartDate = DateTime.Today.AddDays(1),
                    EndDate = DateTime.Today.AddDays(2),
                    IsActive = true,
                    CustomerId = 1,
                    RoomId = 1
                }
            };
            return bookings;
        }
        
        public static IEnumerable<Room> GetRoomData()
        {
            var rooms = new List<Room>
            {
                new Room()
                {
                    Id = 1,
                    Description = "A cozy and nice room for the price"
                }
            };
            return rooms;
        }
    }
}
