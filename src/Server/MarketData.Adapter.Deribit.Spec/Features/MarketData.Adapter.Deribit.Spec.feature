Feature: Start the server and load the configuration
	
# Background:
#     Given Put your Background here
@version1
Scenario: The daemon market data adapter is start the service is available
    Given a configuration file
    And instrument configuration section
	When the service is started
    Then the service is available

# @version1 @ignore
# Scenario: The daemon market data adapter is stopped the service shutdown
#     Given a configuration file
#     And instrument configuration section
#     And the service started
# 	When the service is stopped
#     Then the service close all subcsriptions

Scenario: The daemon fetch the BTC Future when it is starting 
    Given a configuration file
    And instrument configuration section with:
    | currency | kind   | expired |
    | BTC      | future | false   |
    When the service is starting
    Then the service fetchs all instruments

Scenario: The daemon fetch all instruments when it is starting 
    Given a configuration file
    And instrument configuration section with:
    | currency | kind   | expired |
    | BTC      | future | false   |
    | BTC      | option | false   |
    | ETH      | future | false   |
    | ETH      | option | false   |
    When the service is starting
    Then the service fetchs all instruments    