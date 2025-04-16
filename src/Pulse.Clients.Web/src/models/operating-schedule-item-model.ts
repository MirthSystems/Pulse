import { DayOfWeek } from '../enums';

export interface OperatingScheduleItem {
  id: number;
  venueId: number;
  dayOfWeek: DayOfWeek;
  timeOfOpen: string; // HH:mm:ss format
  timeOfClose: string; // HH:mm:ss format
  isClosed: boolean;
}
