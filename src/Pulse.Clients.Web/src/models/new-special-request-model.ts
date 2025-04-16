import { SpecialTypes } from '../enums';
import type { Period } from './period-model';

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
  recurringPeriod?: Period;
  activeDaysOfWeek?: number;
  venueId: number;
  tagIds?: number[];
}
