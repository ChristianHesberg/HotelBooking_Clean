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

    [Given(@"an invalid start (.*)")]
    public void GivenStartDateIsYesterday(string date)
    {
        _startDate = DateTime.Parse(date);
    }

    [Given(@"the end date is after the start date")]
    public void GivenTheEndDateIsTomorrow()
    {
        _endDate = _startDate.AddDays(1);
    }

    private Action act;

    [When(@"I create a booking with these invalid dates")]
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
        Assert.Throws<ArgumentException>(() => bookingManager.CreateBooking(_booking));
    }
}