import { ApiService } from "./base";
import {
  type PagedResult,
  SearchSpecialsResult,
  type SearchSpecialsResultModel,
  SpecialItemExtended,
  type SpecialItemExtendedModel,
  type GetSpecialsRequest,
  type CreateSpecialRequest,
  type UpdateSpecialRequest,
} from "../../../models";

export interface SpecialsService {
  searchSpecials(
    request: GetSpecialsRequest
  ): Promise<PagedResult<SearchSpecialsResult>>;
  getSpecialById(id: string): Promise<SpecialItemExtended>;
  createSpecial(request: CreateSpecialRequest): Promise<SpecialItemExtended>;
  updateSpecial(
    id: string,
    request: UpdateSpecialRequest
  ): Promise<SpecialItemExtended>;
  deleteSpecial(id: string): Promise<boolean>;
}

export class SpecialsApiService
  extends ApiService
  implements SpecialsService
{
  private readonly ENDPOINT = "/api/specials";

  async searchSpecials(
    request: GetSpecialsRequest
  ): Promise<PagedResult<SearchSpecialsResult>> {
    const queryParams = this.objectToQueryParams(request);
    const url = `${this.ENDPOINT}?${queryParams}`;
    const data = await this.fetch<PagedResult<SearchSpecialsResultModel>>(
      url,
      "GET",
      undefined,
      false
    );
    return {
      items: data.items.map((item) => new SearchSpecialsResult(item)),
      pagingInfo: data.pagingInfo,
    };
  }

  async getSpecialById(id: string): Promise<SpecialItemExtended> {
    const data = await this.fetch<SpecialItemExtendedModel>(
      `${this.ENDPOINT}/${id}`,
      "GET",
      undefined,
      false
    );
    return new SpecialItemExtended(data);
  }

  async createSpecial(
    request: CreateSpecialRequest
  ): Promise<SpecialItemExtended> {
    const data = await this.fetch<SpecialItemExtendedModel>(
      `${this.ENDPOINT}`,
      "POST",
      request,
      true
    );
    return new SpecialItemExtended(data);
  }

  async updateSpecial(
    id: string,
    request: UpdateSpecialRequest
  ): Promise<SpecialItemExtended> {
    const data = await this.fetch<SpecialItemExtendedModel>(
      `${this.ENDPOINT}/${id}`,
      "PUT",
      request,
      true
    );
    return new SpecialItemExtended(data);
  }

  async deleteSpecial(id: string): Promise<boolean> {
    return await this.fetch<boolean>(
      `${this.ENDPOINT}/${id}`,
      "DELETE",
      undefined,
      true
    );
  }
}