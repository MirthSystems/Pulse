import { DateTime as LuxonDateTime, DurationUnit } from 'luxon';

/**
 * DateTime utility class that provides consistent date and time operations
 * throughout the application using the luxon library.
 */
export class DateTime {
  /**
   * Gets the current date and time.
   */
  static now(): LuxonDateTime {
    return LuxonDateTime.now();
  }

  /**
   * Creates a DateTime from an ISO string.
   */
  static fromISO(isoString: string): LuxonDateTime {
    return LuxonDateTime.fromISO(isoString);
  }

  /**
   * Creates a DateTime from a JavaScript Date object.
   */
  static fromJSDate(date: Date): LuxonDateTime {
    return LuxonDateTime.fromJSDate(date);
  }

  /**
   * Formats a date using the specified format string.
   * @see https://moment.github.io/luxon/#/formatting for format options
   */
  static format(date: LuxonDateTime, format: string): string {
    return date.toFormat(format);
  }

  /**
   * Returns a formatted date string in localized format.
   */
  static formatDate(date: LuxonDateTime): string {
    return date.toLocaleString(LuxonDateTime.DATE_FULL);
  }

  /**
   * Returns a formatted time string in localized format.
   */
  static formatTime(date: LuxonDateTime): string {
    return date.toLocaleString(LuxonDateTime.TIME_SIMPLE);
  }

  /**
   * Returns a formatted date and time string in localized format.
   */
  static formatDateTime(date: LuxonDateTime): string {
    return date.toLocaleString(LuxonDateTime.DATETIME_FULL);
  }

  /**
   * Adds the specified amount of time to a date.
   */
  static add(date: LuxonDateTime, amount: number, unit: DurationUnit): LuxonDateTime {
    return date.plus({ [unit]: amount });
  }

  /**
   * Subtracts the specified amount of time from a date.
   */
  static subtract(date: LuxonDateTime, amount: number, unit: DurationUnit): LuxonDateTime {
    return date.minus({ [unit]: amount });
  }

  /**
   * Calculates the difference between two dates.
   */
  static diff(date1: LuxonDateTime, date2: LuxonDateTime, unit: DurationUnit = 'milliseconds'): number {
    return date2.diff(date1).as(unit);
  }

  /**
   * Checks if a date is before another date.
   */
  static isBefore(date1: LuxonDateTime, date2: LuxonDateTime): boolean {
    return date1 < date2;
  }

  /**
   * Checks if a date is after another date.
   */
  static isAfter(date1: LuxonDateTime, date2: LuxonDateTime): boolean {
    return date1 > date2;
  }

  /**
   * Checks if a date is between two other dates.
   */
  static isBetween(date: LuxonDateTime, start: LuxonDateTime, end: LuxonDateTime): boolean {
    return date >= start && date <= end;
  }

  /**
   * Returns a relative time string (e.g., "2 hours ago", "in 3 days").
   */
  static toRelative(date: LuxonDateTime): string | null {
    return date.toRelative();
  }
}