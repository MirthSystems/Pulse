// API models based on swagger.json
export interface VenueItem {
  id: number;
  name: string;
  description?: string;
  addressLine1: string;
  addressLine2?: string;
  addressLine3?: string;
  addressLine4?: string;
  locality: string;
  region: string;
  postcode: string;
  country: string;
  phoneNumber?: string;
  email?: string;
  website?: string;
  imageLink?: string;
  venueTypeId: number;
  venueTypeName?: string;
}

export interface SpecialItem {
  id: number;
  content: string;
  type: number; // 0: Food, 1: Drink, 2: Entertainment
  startDate: string;
  startTime: string;
  endTime?: string;
  expirationDate?: string;
  isRecurring: boolean;
  recurringPeriod?: Period;
  activeDaysOfWeek?: number;
  venueId: number;
}

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

export interface VenueTypeItem {
  id: number;
  name: string;
  description?: string;
}

export interface TagItem {
  id: number;
  name: string;
  usageCount: number;
}

// Add more as needed from swagger.json
