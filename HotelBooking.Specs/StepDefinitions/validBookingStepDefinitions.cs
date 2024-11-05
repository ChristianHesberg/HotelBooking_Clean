using Xunit;
using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.Infrastructure.Repositories;
using Moq;
using Reqnroll;


[Binding]
public class validBookingStepDefinitions
{
    private DateTime _startDate;
    private DateTime _endDate;
    private Booking _booking;
    private bool _bookingSuccess;
    private Mock<IRepository<Room>> roomRepository = new();
    private Mock<IRepository<Booking>> bookingRepository = new();
    private BookingManager bookingManager;


    public validBookingStepDefinitions()
    {
        var availableRooms = new List<Room> { new Room()
        {
            Id = 1,
            Description = "Cool room"
        } };
        roomRepository.Setup(x => x.GetAll()).Returns(availableRooms);
        bookingManager = new BookingManager(bookingRepository.Object, roomRepository.Object);
    }

    [Given(@"the start (.*)")]
    public void GivenTheStartDateIsToday(string date)
    {
        _startDate = DateTime.Parse(date); }

    [Given(@"the end date is one day after")]
    public void GivenTheEndDateIsTomorrow()
    {
        _endDate = _startDate.AddDays(1);
    }

    [When(@"I create a booking with these dates")]
    public void WhenICreateABookingWithTheseDates()
    {
        _booking = new Booking()
        {
            Id = 1,
            StartDate = _startDate,
            EndDate = _endDate
        };
        _bookingSuccess = bookingManager.CreateBooking(_booking);
    }

    [Then(@"the booking should be created successfully")]
    public void ThenTheBookingShouldBeCreatedSuccessfully()
    {
        Assert.True(_bookingSuccess, "Expected the booking to be created successfully.");
    }
}