Feature: Start the server and load the configuration
	
# Background:
#     Given Put your Background here

Scenario: The daemon market data adapter is start the service is available
    Given a configuration file
    And an instrument list
	When the service is started
    Then the service is available

Scenario: The daemon market data adapter is stopped the service shutdown
    Given a configuration file
    And an instrument list
    And the service started
	When the service is stopped
    Then the service close all subcsription
    And close the process

