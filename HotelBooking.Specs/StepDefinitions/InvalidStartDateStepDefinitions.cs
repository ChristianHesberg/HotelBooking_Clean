using System;
using HotelBooking.Core;
using Moq;
using Reqnroll;
using Xunit;

namespace HotelBooking.Specs.StepDefinitions;

[Binding]
public class InvalidStartDateStepDefinitions
{
    private DateTime _startDate;
    private DateTime _endDate;
    private Booking _booking;
    private bool _bookingSuccess;

    private Mock<IRepository<Room>> roomRepository = new();
    private Mock<IRepository<Booking>> bookingRepository = new();
    private BookingManager bookingManager;

    public InvalidStartDateStepDefinitions()
    {
        bookingManager = new BookingManager(bookingRepository.Object, roomRepository.Object);
    }

    [Given(@"the start date is yesterday")]
    public void GivenStartDateIsYesterday()
    {
        _startDate = DateTime.Now.AddDays(-1);
    }

    [Given(@"the end date is valid tomorrow")]
    public void GivenTheEndDateIsTomorrow()
    {
        _endDate = DateTime.Now.AddDays(1);
    }

    private Action act;

    [When(@"I create a booking with these wrong dates")]
    public void WhenICreateABookingWithTheseDates()
    {
        _booking = new Booking()
        {
            Id = 1,
            StartDate = _startDate,
            EndDate = _endDate
        };
    }

    [Then(@"the booking should reject and inform user to fix dates")]
    public void ThenTheBookingShouldBeCreatedSuccessfully()
    {
        //this should work, but does not. 
        Assert.Throws<ArgumentException>(() => bookingManager.CreateBooking(_booking));
    }
}