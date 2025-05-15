import { ApiService } from "./base";
import {
  type PagedResult,
  VenueItem,
  type VenueItemModel,
  VenueItemExtended,
  type VenueItemExtendedModel,
  OperatingScheduleItem,
  type OperatingScheduleItemModel,
  SpecialItem,
  type SpecialItemModel,
  type GetVenuesRequest,
  type CreateVenueRequest,
  type UpdateVenueRequest,
} from "../../models";

export interface VenuesService {
  getVenues(request: GetVenuesRequest): Promise<PagedResult<VenueItem>>;
  getVenueById(id: string): Promise<VenueItemExtended>;
  getVenueBusinessHours(id: string): Promise<OperatingScheduleItem[]>;
  getVenueSpecials(id: string): Promise<SpecialItem[]>;
  createVenue(request: CreateVenueRequest): Promise<VenueItemExtended>;
  updateVenue(
    id: string,
    request: UpdateVenueRequest
  ): Promise<VenueItemExtended>;
  deleteVenue(id: string): Promise<boolean>;
}

export class VenuesApiService extends ApiService implements VenuesService {
  private readonly ENDPOINT = "/api/venues";

  async getVenues(request: GetVenuesRequest): Promise<PagedResult<VenueItem>> {
    const queryParams = this.objectToQueryParams(request);
    const url = `${this.ENDPOINT}?${queryParams}`;
    const data = await this.fetch<PagedResult<VenueItemModel>>(
      url,
      "GET",
      undefined,
      false
    );
    return {
      items: data.items.map((item) => new VenueItem(item)),
      pagingInfo: data.pagingInfo,
    };
  }

  async getVenueById(id: string): Promise<VenueItemExtended> {
    const data = await this.fetch<VenueItemExtendedModel>(
      `${this.ENDPOINT}/${id}`,
      "GET",
      undefined,
      false
    );
    return new VenueItemExtended(data);
  }

  async getVenueBusinessHours(id: string): Promise<OperatingScheduleItem[]> {
    const data = await this.fetch<OperatingScheduleItemModel[]>(
      `${this.ENDPOINT}/${id}/business-hours`,
      "GET",
      undefined,
      false
    );
    return data.map((item) => new OperatingScheduleItem(item));
  }

  async getVenueSpecials(id: string): Promise<SpecialItem[]> {
    const data = await this.fetch<SpecialItemModel[]>(
      `${this.ENDPOINT}/${id}/specials`,
      "GET",
      undefined,
      false
    );
    return data.map((item) => new SpecialItem(item));
  }

  async createVenue(request: CreateVenueRequest): Promise<VenueItemExtended> {
    const data = await this.fetch<VenueItemExtendedModel>(
      `${this.ENDPOINT}`,
      "POST",
      request,
      true
    );
    return new VenueItemExtended(data);
  }

  async updateVenue(
    id: string,
    request: UpdateVenueRequest
  ): Promise<VenueItemExtended> {
    const data = await this.fetch<VenueItemExtendedModel>(
      `${this.ENDPOINT}/${id}`,
      "PUT",
      request,
      true
    );
    return new VenueItemExtended(data);
  }

  async deleteVenue(id: string): Promise<boolean> {
    return await this.fetch<boolean>(
      `${this.ENDPOINT}/${id}`,
      "DELETE",
      undefined,
      true
    );
  }
}
