Feature: BookingManager

Service for managing bookings

    @mytag
    Scenario: Create valid booking
        Given the start <date>
        And the end date is one day after
        When I create a booking with these dates
        Then the booking should be created successfully

    Examples:
      | date |
    #min
    | 11-06-2024 |
    #nominal
    | 05-01-2025 |
    #max
    | 12-30-9999 |

    Scenario: User inputs invalid start date
        Given an invalid start <date>
        And the end date is after the start date
        When I create a booking with these invalid dates
        Then the booking should reject and inform user to fix dates

    Examples:
      | date       |
      | 11-04-2024 |
      | 01-01-0001 |