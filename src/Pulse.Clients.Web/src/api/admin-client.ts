import axios from 'axios';
import { useAuthStore } from '@/stores/auth';
import type {
  NewOperatingScheduleRequest,
  NewSpecialRequest,
  OperatingScheduleItem,
  SpecialItem,
  SpecialWithDetails,
  TagItem,
  TagRequest,
  UpdateOperatingScheduleRequest,
  UpdateSpecialRequest,
  VenueItem,
  VenueRequest,
  VenueTypeItem,
  VenueTypeRequest,
  VenueWithDetails,
} from '@/models';

const apiDomain = import.meta.env.VITE_API_DOMAIN || 'https://localhost:7253';
const baseUrl = `${apiDomain}/api/admin`;

/**
 * Client for interacting with the admin API endpoints
 */
class AdminClient {
  /**
   * Get all venues
   */
  async getVenues (): Promise<VenueItem[]> {
    const token = await this.getAuthToken();
    const response = await axios.get<VenueItem[]>(`${baseUrl}/venues`, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Get a venue by ID with detailed information
   */
  async getVenue (id: number): Promise<VenueWithDetails> {
    const token = await this.getAuthToken();
    const response = await axios.get<VenueWithDetails>(`${baseUrl}/venues/${id}`, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Create a new venue
   */
  async createVenue (request: VenueRequest): Promise<VenueItem> {
    const token = await this.getAuthToken();
    const response = await axios.post<VenueItem>(`${baseUrl}/venues`, request, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Update an existing venue
   */
  async updateVenue (id: number, request: VenueRequest): Promise<void> {
    const token = await this.getAuthToken();
    await axios.put(`${baseUrl}/venues/${id}`, request, {
      headers: this.getAuthHeader(token),
    });
  }

  /**
   * Delete a venue
   */
  async deleteVenue (id: number): Promise<void> {
    const token = await this.getAuthToken();
    await axios.delete(`${baseUrl}/venues/${id}`, {
      headers: this.getAuthHeader(token),
    });
  }

  /**
   * Get all venue types
   */
  async getVenueTypes (): Promise<VenueTypeItem[]> {
    const token = await this.getAuthToken();
    const response = await axios.get<VenueTypeItem[]>(`${baseUrl}/venue-types`, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Get a venue type by ID
   */
  async getVenueType (id: number): Promise<VenueTypeItem> {
    const token = await this.getAuthToken();
    const response = await axios.get<VenueTypeItem>(`${baseUrl}/venue-types/${id}`, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Create a new venue type
   */
  async createVenueType (request: VenueTypeRequest): Promise<VenueTypeItem> {
    const token = await this.getAuthToken();
    const response = await axios.post<VenueTypeItem>(`${baseUrl}/venue-types`, request, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Update an existing venue type
   */
  async updateVenueType (id: number, request: VenueTypeRequest): Promise<void> {
    const token = await this.getAuthToken();
    await axios.put(`${baseUrl}/venue-types/${id}`, request, {
      headers: this.getAuthHeader(token),
    });
  }

  /**
   * Delete a venue type
   */
  async deleteVenueType (id: number): Promise<void> {
    const token = await this.getAuthToken();
    await axios.delete(`${baseUrl}/venue-types/${id}`, {
      headers: this.getAuthHeader(token),
    });
  }

  /**
   * Get all specials
   */
  async getSpecials (): Promise<SpecialItem[]> {
    const token = await this.getAuthToken();
    const response = await axios.get<SpecialItem[]>(`${baseUrl}/specials`, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Get a special by ID with detailed information
   */
  async getSpecial (id: number): Promise<SpecialWithDetails> {
    const token = await this.getAuthToken();
    const response = await axios.get<SpecialWithDetails>(`${baseUrl}/specials/${id}`, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Create a new special
   */
  async createSpecial (request: NewSpecialRequest): Promise<SpecialItem> {
    const token = await this.getAuthToken();
    const response = await axios.post<SpecialItem>(`${baseUrl}/specials`, request, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Update an existing special
   */
  async updateSpecial (id: number, request: UpdateSpecialRequest): Promise<void> {
    const token = await this.getAuthToken();
    await axios.put(`${baseUrl}/specials/${id}`, request, {
      headers: this.getAuthHeader(token),
    });
  }

  /**
   * Delete a special
   */
  async deleteSpecial (id: number): Promise<void> {
    const token = await this.getAuthToken();
    await axios.delete(`${baseUrl}/specials/${id}`, {
      headers: this.getAuthHeader(token),
    });
  }

  /**
   * Get schedules for a venue
   */
  async getVenueSchedules (venueId: number): Promise<OperatingScheduleItem[]> {
    const token = await this.getAuthToken();
    const response = await axios.get<OperatingScheduleItem[]>(`${baseUrl}/venues/${venueId}/schedules`, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Create a new schedule for a venue
   */
  async createSchedule (venueId: number, request: NewOperatingScheduleRequest): Promise<OperatingScheduleItem> {
    const token = await this.getAuthToken();
    const response = await axios.post<OperatingScheduleItem>(`${baseUrl}/venues/${venueId}/schedules`, request, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Update schedules for a venue
   */
  async updateVenueSchedules (venueId: number, requests: UpdateOperatingScheduleRequest[]): Promise<void> {
    const token = await this.getAuthToken();
    await axios.put(`${baseUrl}/venues/${venueId}/schedules`, requests, {
      headers: this.getAuthHeader(token),
    });
  }

  /**
   * Get a schedule by ID
   */
  async getSchedule (id: number): Promise<OperatingScheduleItem> {
    const token = await this.getAuthToken();
    const response = await axios.get<OperatingScheduleItem>(`${baseUrl}/schedules/${id}`, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Update a schedule
   */
  async updateSchedule (id: number, request: UpdateOperatingScheduleRequest): Promise<void> {
    const token = await this.getAuthToken();
    await axios.put(`${baseUrl}/schedules/${id}`, request, {
      headers: this.getAuthHeader(token),
    });
  }

  /**
   * Delete a schedule
   */
  async deleteSchedule (id: number): Promise<void> {
    const token = await this.getAuthToken();
    await axios.delete(`${baseUrl}/schedules/${id}`, {
      headers: this.getAuthHeader(token),
    });
  }

  /**
   * Get all tags
   */
  async getTags (): Promise<TagItem[]> {
    const token = await this.getAuthToken();
    const response = await axios.get<TagItem[]>(`${baseUrl}/tags`, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Get a tag by ID
   */
  async getTag (id: number): Promise<TagItem> {
    const token = await this.getAuthToken();
    const response = await axios.get<TagItem>(`${baseUrl}/tags/${id}`, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Create a new tag
   */
  async createTag (request: TagRequest): Promise<TagItem> {
    const token = await this.getAuthToken();
    const response = await axios.post<TagItem>(`${baseUrl}/tags`, request, {
      headers: this.getAuthHeader(token),
    });
    return response.data;
  }

  /**
   * Update an existing tag
   */
  async updateTag (id: number, request: TagRequest): Promise<void> {
    const token = await this.getAuthToken();
    await axios.put(`${baseUrl}/tags/${id}`, request, {
      headers: this.getAuthHeader(token),
    });
  }

  /**
   * Delete a tag
   */
  async deleteTag (id: number): Promise<void> {
    const token = await this.getAuthToken();
    await axios.delete(`${baseUrl}/tags/${id}`, {
      headers: this.getAuthHeader(token),
    });
  }

  // Helper methods
  private async getAuthToken (): Promise<string | null> {
    const authStore = useAuthStore();
    return await authStore.getToken();
  }

  private getAuthHeader (token: string | null): Record<string, string> {
    return token ? { Authorization: `Bearer ${token}` } : {};
  }
}

export default new AdminClient();
