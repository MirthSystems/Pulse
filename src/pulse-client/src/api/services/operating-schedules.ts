import { ApiService } from "./base";
import {
  OperatingScheduleItemExtended,
  type OperatingScheduleItemExtendedModel,
  type CreateOperatingScheduleRequest,
  type UpdateOperatingScheduleRequest,
} from "../../models";

export interface OperatingSchedulesService {
  getOperatingScheduleById(id: string): Promise<OperatingScheduleItemExtended>;
  createOperatingSchedule(
    request: CreateOperatingScheduleRequest
  ): Promise<OperatingScheduleItemExtended>;
  updateOperatingSchedule(
    id: string,
    request: UpdateOperatingScheduleRequest
  ): Promise<OperatingScheduleItemExtended>;
  deleteOperatingSchedule(id: string): Promise<boolean>;
}

export class OperatingSchedulesApiService
  extends ApiService
  implements OperatingSchedulesService
{
  private readonly ENDPOINT = "/api/operating-schedules";

  async getOperatingScheduleById(
    id: string
  ): Promise<OperatingScheduleItemExtended> {
    const data = await this.fetch<OperatingScheduleItemExtendedModel>(
      `${this.ENDPOINT}/${id}`,
      "GET",
      undefined,
      false
    );
    return new OperatingScheduleItemExtended(data);
  }

  async createOperatingSchedule(
    request: CreateOperatingScheduleRequest
  ): Promise<OperatingScheduleItemExtended> {
    const data = await this.fetch<OperatingScheduleItemExtendedModel>(
      `${this.ENDPOINT}`,
      "POST",
      request,
      true
    );
    return new OperatingScheduleItemExtended(data);
  }

  async updateOperatingSchedule(
    id: string,
    request: UpdateOperatingScheduleRequest
  ): Promise<OperatingScheduleItemExtended> {
    const data = await this.fetch<OperatingScheduleItemExtendedModel>(
      `${this.ENDPOINT}/${id}`,
      "PUT",
      request,
      true
    );
    return new OperatingScheduleItemExtended(data);
  }

  async deleteOperatingSchedule(id: string): Promise<boolean> {
    return await this.fetch<boolean>(
      `${this.ENDPOINT}/${id}`,
      "DELETE",
      undefined,
      true
    );
  }
}
