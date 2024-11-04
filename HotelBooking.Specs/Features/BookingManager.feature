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
	And the end date is tomorrow
	When I create a booking with these dates
	Then the booking should reject and inform user to fix dates
	
Scenario: User inputs invalid end date 
	Given the start date is today
	And the end date is yesterday
	When I create a booking with these dates
	Then the booking should reject and inform user to fix dates
	
Scenario: User inputs end date before start date 
	Given the start date is tomorrow
	And the end date is today
	When I create a booking with these dates
	Then the booking should reject and inform user to fix dates
	
Scenario: User tries to book but no rooms are available 
	Given the start date is today
	And the end date is tomorrow
	But no rooms are available
	When I create a booking with these dates
	Then the booking should reject and inform user there are no available rooms