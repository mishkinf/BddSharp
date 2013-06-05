Feature: Restaurants

Scenario: Get Open Restaurants 
	Given I have loaded test data fixtures
    When I visit the restaurants page
    Then I click on #open_restaurants
    Then I should see "mishkinsRestaurant" restaurant listed