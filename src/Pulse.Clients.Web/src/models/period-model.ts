/**
 * Represents a period of time expressed in human chronological terms.
 */
export interface Period {
  nanoseconds: number;
  ticks: number;
  milliseconds: number;
  seconds: number;
  minutes: number;
  hours: number;
  days: number;
  weeks: number;
  months: number;
  years: number;
  hasTimeComponent: boolean;
  hasDateComponent: boolean;
}
