import axios from 'axios';
import type {
  SpecialItem,
  SpecialWithDetails,
  SpecialWithVenue,
  VenueWithActiveSpecials,
} from '@/models';
import { SpecialTypes } from '@/enums';

const apiDomain = import.meta.env.VITE_API_DOMAIN || 'https://localhost:7253';
const baseUrl = `${apiDomain}/api/specials`;

/**
 * Client for interacting with the specials API endpoints
 */
class SpecialsClient {
  /**
   * Find venues with active specials near a geographic point
   */
  async findVenuesWithSpecialsNearPoint (
    latitude: number,
    longitude: number,
    radiusMiles: number = 5.0
  ): Promise<VenueWithActiveSpecials[]> {
    const response = await axios.get<VenueWithActiveSpecials[]>(`${baseUrl}/nearby`, {
      params: { latitude, longitude, radiusMiles },
    });
    return response.data;
  }

  /**
   * Find venues with active specials near an address
   */
  async findVenuesWithSpecialsNearAddress (
    address: string,
    radiusMiles: number = 5.0
  ): Promise<VenueWithActiveSpecials[]> {
    const response = await axios.get<VenueWithActiveSpecials[]>(`${baseUrl}/nearby/address`, {
      params: { address, radiusMiles },
    });
    return response.data;
  }

  /**
   * Find venues with specials for a specific time near a geographic point
   */
  async findVenuesWithSpecialsForTimeNearPoint (
    latitude: number,
    longitude: number,
    dateTime: string,
    radiusMiles: number = 5.0
  ): Promise<VenueWithActiveSpecials[]> {
    const response = await axios.get<VenueWithActiveSpecials[]>(`${baseUrl}/nearby/future`, {
      params: { latitude, longitude, dateTime, radiusMiles },
    });
    return response.data;
  }

  /**
   * Find venues with specials for a specific time near an address
   */
  async findVenuesWithSpecialsForTimeNearAddress (
    address: string,
    dateTime: string,
    radiusMiles: number = 5.0
  ): Promise<VenueWithActiveSpecials[]> {
    const response = await axios.get<VenueWithActiveSpecials[]>(`${baseUrl}/nearby/address/future`, {
      params: { address, dateTime, radiusMiles },
    });
    return response.data;
  }

  /**
   * Get specials for a specific venue
   */
  async getVenueSpecials (venueId: number): Promise<SpecialItem[]> {
    const response = await axios.get<SpecialItem[]>(`${baseUrl}/venues/${venueId}`);
    return response.data;
  }

  /**
   * Get specials by type (food, drink, entertainment)
   */
  async getSpecialsByType (type: SpecialTypes): Promise<SpecialWithVenue[]> {
    const response = await axios.get<SpecialWithVenue[]>(`${baseUrl}/by-type/${type}`);
    return response.data;
  }

  /**
   * Get specials by tag name
   */
  async getSpecialsByTag (tagName: string): Promise<SpecialWithVenue[]> {
    const response = await axios.get<SpecialWithVenue[]>(`${baseUrl}/by-tag/${tagName}`);
    return response.data;
  }

  /**
   * Get detailed information about a specific special
   */
  async getSpecialDetails (id: number): Promise<SpecialWithDetails> {
    const response = await axios.get<SpecialWithDetails>(`${baseUrl}/${id}`);
    return response.data;
  }
}

export default new SpecialsClient();
