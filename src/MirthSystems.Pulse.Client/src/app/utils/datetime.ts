import { DateTime, WeekdayNumbers } from 'luxon';

/**
 * Utility functions for working with dates and times consistently
 * across the application using Luxon DateTime library
 */

/**
 * Format options for displaying dates and times
 */
export const DateTimeFormats = {
  DATE_ONLY: 'MM/dd/yyyy',
  SHORT_DATE: 'M/d/yy',
  DATE_WITH_WEEKDAY: 'ccc, MMM d, yyyy',
  TIME_ONLY: 'h:mm a',
  TIME_WITH_SECONDS: 'h:mm:ss a',
  DATE_TIME: 'MM/dd/yyyy h:mm a',
  ISO_DATE: 'yyyy-MM-dd',
  ISO_TIME: 'HH:mm:ss',
  ISO_DATE_TIME: "yyyy-MM-dd'T'HH:mm:ss",
  RELATIVE: 'relative',
};

/**
 * Get the current date and time
 * @param timezone Optional timezone (defaults to local timezone)
 */
export const now = (timezone?: string): DateTime => {
  return timezone ? DateTime.now().setZone(timezone) : DateTime.now();
};

/**
 * Parse a string to a DateTime object
 * @param dateString String representation of date
 * @param format Optional format string for parsing
 */
export const parseDate = (dateString: string, format?: string): DateTime => {
  if (format) {
    return DateTime.fromFormat(dateString, format);
  }
  
  // Try common formats
  const dateTime = DateTime.fromISO(dateString);
  if (dateTime.isValid) return dateTime;

  return DateTime.fromSQL(dateString);
};

/**
 * Format a DateTime object to a string
 * @param date The DateTime object to format
 * @param format The format to use (from DateTimeFormats)
 * @param timezone Optional timezone for display
 */
export const formatDateTime = (
  date: DateTime,
  format: string = DateTimeFormats.DATE_TIME,
  timezone?: string
): string => {
  const dateInZone = timezone ? date.setZone(timezone) : date;
  
  if (format === DateTimeFormats.RELATIVE) {
    return dateInZone.toRelative() || '';
  }
  
  return dateInZone.toFormat(format);
};

/**
 * Convert a time string (e.g., "19:30:00") to a formatted time string (e.g., "7:30 PM")
 * @param timeString Time string in 24-hour format (HH:mm:ss or HH:mm)
 */
export const formatTimeString = (
  timeString: string, 
  format: string = DateTimeFormats.TIME_ONLY
): string => {
  // Create a DateTime object for today with the specified time
  const today = DateTime.now().startOf('day');
  
  // Split the time string into hours, minutes, and optional seconds
  const parts = timeString.split(':');
  const hours = parseInt(parts[0], 10);
  const minutes = parseInt(parts[1], 10);
  const seconds = parts.length > 2 ? parseInt(parts[2], 10) : 0;
  
  const dateTime = today.set({ hour: hours, minute: minutes, second: seconds });
  
  return dateTime.toFormat(format);
};

/**
 * Check if a DateTime is in the past
 */
export const isPast = (date: DateTime): boolean => {
  return date < DateTime.now();
};

/**
 * Check if a DateTime is in the future
 */
export const isFuture = (date: DateTime): boolean => {
  return date > DateTime.now();
};

/**
 * Check if two DateTimes are on the same day
 */
export const isSameDay = (date1: DateTime, date2: DateTime): boolean => {
  return date1.hasSame(date2, 'day');
};

/**
 * Get the days difference between two dates
 */
export const daysBetween = (date1: DateTime, date2: DateTime): number => {
  return Math.abs(
    Math.floor(date2.diff(date1, 'days').days)
  );
};

/**
 * Creates a range of dates
 * @param startDate Start of the range
 * @param endDate End of the range
 * @param step Step in days (defaults to 1)
 */
export const createDateRange = (
  startDate: DateTime,
  endDate: DateTime,
  step: number = 1
): DateTime[] => {
  const range: DateTime[] = [];
  let currentDate = startDate;
  
  while (currentDate <= endDate) {
    range.push(currentDate);
    currentDate = currentDate.plus({ days: step });
  }
  
  return range;
};

/**
 * Find the next occurrence of a day of the week
 * @param dayOfWeek 0 for Sunday, 1 for Monday, etc.
 * @param startFrom Optional starting date (defaults to today)
 */
export const nextDayOfWeek = (
  dayOfWeek: number,
  startFrom: DateTime = DateTime.now()
): DateTime => {
  // Convert JS day of week (0-6) to Luxon weekday (1-7)
  const luxonWeekday = dayOfWeek === 0 ? 7 : dayOfWeek;
  const result = startFrom.set({ weekday: luxonWeekday as WeekdayNumbers });
  return result <= startFrom ? result.plus({ weeks: 1 }) : result;
};

/**
 * Check if a time is between two other times on the same day
 * @param time The time to check
 * @param startTime The start time
 * @param endTime The end time
 */
export const isTimeBetween = (
  time: DateTime,
  startTime: DateTime,
  endTime: DateTime
): boolean => {
  // Normalize all times to the same day for comparison
  const baseDay = DateTime.now().startOf('day');
  
  const normalizedTime = baseDay.set({
    hour: time.hour,
    minute: time.minute,
    second: time.second
  });
  
  const normalizedStartTime = baseDay.set({
    hour: startTime.hour,
    minute: startTime.minute,
    second: startTime.second
  });
  
  const normalizedEndTime = baseDay.set({
    hour: endTime.hour,
    minute: endTime.minute,
    second: endTime.second
  });
  
  // Handle cases where end time is on the next day (e.g., 1:00 AM)
  if (normalizedEndTime < normalizedStartTime) {
    return normalizedTime >= normalizedStartTime || 
           normalizedTime <= normalizedEndTime;
  }
  
  return normalizedTime >= normalizedStartTime && 
         normalizedTime <= normalizedEndTime;
};