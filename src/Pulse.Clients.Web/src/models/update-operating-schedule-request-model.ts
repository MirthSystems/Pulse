import { DayOfWeek } from '../enums';

export interface UpdateOperatingScheduleRequest {
  id: number;
  dayOfWeek: DayOfWeek;
  timeOfOpen: string; // HH:mm:ss
  timeOfClose: string; // HH:mm:ss
  isClosed: boolean;
}
