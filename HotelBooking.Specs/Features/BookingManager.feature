Feature: BookingManager

Service for managing bookings

@mytag
Scenario: Create valid booking 
	Given the start date is today
	And the end date is tomorrow
	When I create a booking with these dates
	Then the booking should be created successfully
	
	
Scenario: User inputs invalid start date 
	Given the start date is yesterday
	And the end date is valid tomorrow
	When I create a booking with these dates
	Then the booking should reject and inform user to fix dates
	
