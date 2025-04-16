import { SpecialTypes } from '../enums';
import type { Period } from './period-model';

/**
 * Represents a special item offered by a venue.
 */
export interface SpecialItem {
  id: number;
  content: string;
  type: SpecialTypes;
  startDate: string; // YYYY-MM-DD format
  startTime: string; // HH:mm:ss format
  endTime?: string; // HH:mm:ss format
  expirationDate?: string; // YYYY-MM-DD format
  isRecurring: boolean;
  recurringPeriod?: Period;
  activeDaysOfWeek?: number;
  venueId: number;
}
