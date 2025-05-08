/* eslint-disable */

import { DateTime } from 'luxon';
import { apiClient, Special, Venue, Address, OperatingSchedule, DayOfWeek, SpecialTypes } from '../index';

/**
 * This file contains examples of how to use the Pulse API client
 * It is not meant to be imported or executed directly, but rather to serve as a reference
 */

// Example: Basic authentication
// @ts-ignore: Unused function for documentation purposes
async function authenticationExample() {
  // Set auth token (typically obtained from authentication service)
  apiClient.setAuthToken('your-auth-token-here');
  
  // Later when logging out
  apiClient.clearAuthToken();
}

// Example: Working with venues
// @ts-ignore: Unused function for documentation purposes
async function venuesExample() {
  try {
    // Get all venues with pagination
    const venues = await apiClient.venues.getVenues('', 1, 10);
    console.log(`Found ${venues.length} venues`);
    
    // Get a specific venue
    const venueId = '123';
    const venueResponse = await apiClient.venues.getVenue(venueId);
    
    // Convert to Venue model
    const venue = Venue.fromResponse(venueResponse);
    console.log(`Venue name: ${venue.name}`);
    console.log(`Address: ${venue.address.getFormattedAddress()}`);
    
    // Create a new venue
    const newVenue = new Venue({
      name: 'My New Restaurant',
      description: 'A fantastic dining experience',
      phoneNumber: '555-123-4567',
      website: 'https://example.com',
      email: 'contact@example.com',
      address: new Address({
        streetAddress: '123 Main St',
        locality: 'Metropolis',
        region: 'NY',
        postcode: '10001',
        country: 'USA'
      })
    });
    
    // Add business hours for each day of the week
    newVenue.addBusinessHours([
      new OperatingSchedule({
        dayOfWeek: DayOfWeek.Monday,
        timeOfOpen: '09:00',
        timeOfClose: '21:00',
        isClosed: false,
        venueId: '' // Will be set after creation
      }),
      new OperatingSchedule({
        dayOfWeek: DayOfWeek.Tuesday,
        timeOfOpen: '09:00',
        timeOfClose: '21:00',
        isClosed: false,
        venueId: '' // Will be set after creation
      }),
      // ...add other days
    ]);
    
    // Create venue in the API
    const createdVenueId = await apiClient.venues.createVenue(newVenue);
    console.log(`Created venue with ID: ${createdVenueId}`);
    
    // Update venue
    newVenue.id = createdVenueId;
    newVenue.description = 'Updated description';
    await apiClient.venues.updateVenue(createdVenueId, newVenue);
    
    // Get venue business hours
    const businessHours = await apiClient.venues.getBusinessHours(createdVenueId);
    console.log('Business hours:', businessHours);
    
    // Get venue specials
    const venueSpecials = await apiClient.venues.getVenueSpecials(createdVenueId);
    console.log('Venue specials:', venueSpecials);
  } catch (error) {
    console.error('Error working with venues:', error);
  }
}

// Example: Working with operating schedules
// @ts-ignore: Unused function for documentation purposes
async function operatingSchedulesExample() {
  try {
    const venueId = '123';
    
    // Get operating schedules for a venue
    const schedules = await apiClient.operatingSchedules.getVenueOperatingSchedules(venueId);
    const operatingSchedules = schedules.map(schedule => OperatingSchedule.fromResponse(schedule));
    
    // Display formatted schedules
    operatingSchedules.forEach(schedule => {
      console.log(schedule.getFormattedSchedule());
    });
    
    // Check if venue is open at a specific time
    const dateToCheck = DateTime.now();
    const isOpen = operatingSchedules.some(schedule => schedule.isOpenAt(dateToCheck));
    console.log(`The venue is ${isOpen ? 'open' : 'closed'} now.`);
    
    // Update an operating schedule using Luxon
    if (operatingSchedules.length > 0) {
      const schedule = operatingSchedules[0];
      const newOpenTime = DateTime.fromFormat('08:00', 'HH:mm');
      const newCloseTime = DateTime.fromFormat('22:00', 'HH:mm');
      
      schedule.setOpenTime(newOpenTime);
      schedule.setCloseTime(newCloseTime);
      
      await apiClient.operatingSchedules.updateOperatingSchedule(
        schedule.id as string, 
        schedule
      );
      console.log('Updated operating schedule:', schedule.getFormattedSchedule());
    }
  } catch (error) {
    console.error('Error working with operating schedules:', error);
  }
}

// Example: Working with specials
// @ts-ignore: Unused function for documentation purposes
async function specialsExample() {
  try {
    // Search for nearby specials
    const searchParams = {
      address: '123 Main St, Metropolis, NY',
      radius: 5,
      isCurrentlyRunning: true,
    };
    
    const specials = await apiClient.specials.getSpecials(searchParams);
    console.log(`Found ${specials.length} current specials`);
    
    // Convert API responses to Special model
    // NOTE: Used for demonstration of model conversion
    const specialsModels = specials.map(special => Special.fromResponse(special));
    
    // Create a new special using Luxon for date/time handling
    const newSpecial = new Special({
      venueId: '123',
      content: 'Half-price appetizers',
      type: SpecialTypes.Food,
      isRecurring: true,
      // Set the start date and time using Luxon
      startDate: DateTime.now().toFormat('yyyy-MM-dd'),
      startTime: '16:00'
    });
    
    // Set expiration date using Luxon
    const expirationDate = DateTime.now().plus({ months: 1 });
    newSpecial.setExpirationDate(expirationDate);
    
    // Create the special in the API
    const createdSpecialId = await apiClient.specials.createSpecial(newSpecial);
    console.log(`Created special with ID: ${createdSpecialId}`);
    
    // Get the created special
    const createdSpecialResponse = await apiClient.specials.getSpecial(createdSpecialId);
    const createdSpecial = Special.fromResponse(createdSpecialResponse);
    
    // Use Luxon DateTime objects for date calculations
    if (createdSpecial.startDateTime) {
      const daysDifference = createdSpecial.startDateTime.diff(DateTime.now(), 'days').days;
      console.log(`This special starts in ${Math.round(daysDifference)} days`);
    }
    
    if (createdSpecial.expirationDateTime) {
      const daysDifference = createdSpecial.expirationDateTime.diff(DateTime.now(), 'days').days;
      console.log(`This special expires in ${Math.round(daysDifference)} days`);
    }
    
    // Check if the special is active
    const isActive = createdSpecial.isActive(DateTime.now());
    console.log(`The special is currently ${isActive ? 'active' : 'inactive'}`);
    
    // Update the special
    createdSpecial.content = 'Updated special: 75% off drinks';
    createdSpecial.type = SpecialTypes.Drink;
    await apiClient.specials.updateSpecial(createdSpecialId, createdSpecial);
    
    // Search for specials with advanced filtering
    const searchDateTime = DateTime.now().plus({ days: 2 });
    const advancedResults = await apiClient.specials.searchSpecials(
      '123 Main St, Metropolis, NY',
      {
        radius: 10,
        searchDateTime: searchDateTime,
        specialTypeId: SpecialTypes.Drink,
        page: 1,
        pageSize: 20
      }
    );
    
    console.log(`Found ${advancedResults.length} specials for the search criteria`);
  } catch (error) {
    console.error('Error working with specials:', error);
  }
}

// Example: Error handling
// @ts-ignore: Unused function for documentation purposes
async function errorHandlingExample() {
  try {
    // Try to get a non-existent venue
    const nonExistentVenueId = '999999';
    await apiClient.venues.getVenue(nonExistentVenueId);
  } catch (error: any) {
    if (error.status === 404) {
      console.error('The venue was not found');
    } else if (error.status === 401) {
      console.error('Authentication error. Please log in again.');
      // Handle authentication error, maybe redirect to login
    } else {
      console.error('An unexpected error occurred:', error.message);
    }
    
    // If we have problem details, we can get more information
    if (error.problemDetails) {
      console.error('Error details:', error.problemDetails);
    }
  }
}