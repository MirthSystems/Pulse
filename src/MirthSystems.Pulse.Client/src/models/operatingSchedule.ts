export interface OperatingScheduleItem {
  id: string;
  dayOfWeek: number;
  dayName: string;
  openTime: string;
  closeTime: string;
  isClosed: boolean;
}

export interface OperatingScheduleItemExtended extends OperatingScheduleItem {
  venueId: string;
  venueName: string;
}

export interface CreateOperatingScheduleRequest {
  venueId: string;
  dayOfWeek: number;
  timeOfOpen: string;
  timeOfClose: string;
  isClosed: boolean;
}

export interface UpdateOperatingScheduleRequest {
  timeOfOpen: string;
  timeOfClose: string;
  isClosed: boolean;
}
