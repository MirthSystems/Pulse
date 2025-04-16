import { SpecialTypes } from '../enums';
type Duration = import('luxon').Duration;

/**
 * Request payload for creating a new special.
 */
export interface NewSpecialRequest {
  content: string;
  type: SpecialTypes;
  startDate: string; // YYYY-MM-DD
  startTime: string; // HH:mm:ss
  endTime?: string; // HH:mm:ss
  expirationDate?: string; // YYYY-MM-DD
  isRecurring: boolean;
  recurringPeriod?: Duration;
  activeDaysOfWeek?: number;
  venueId: number;
  tagIds?: number[];
}
