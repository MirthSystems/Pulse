import { DayOfWeek } from '../enums';

export interface NewOperatingScheduleRequest {
  dayOfWeek: DayOfWeek;
  timeOfOpen: string; // HH:mm:ss
  timeOfClose: string; // HH:mm:ss
  isClosed: boolean;
}
