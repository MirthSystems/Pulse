import { type SpecialTypes } from './special';
import { type DayOfWeek } from './day-of-week';
import { type PageQueryParams } from './paging';

export interface OperatingHours {
    dayOfWeek: DayOfWeek; // 0-6, Sunday to Saturday
    timeOfOpen: string; // "HH:mm"
    timeOfClose: string; // "HH:mm"
    isClosed: boolean;
}

export interface CreateOperatingScheduleRequest extends OperatingHours {
    venueId: string;
}

export interface UpdateOperatingScheduleRequest {
    timeOfOpen: string; // "HH:mm"
    timeOfClose: string; // "HH:mm"
    isClosed: boolean;
}

export interface CreateSpecialRequest {
    venueId: string;
    content: string;
    type: SpecialTypes;
    startDate: string; // "yyyy-MM-dd"
    startTime: string; // "HH:mm"
    endTime?: string; // "HH:mm"
    expirationDate?: string; // "yyyy-MM-dd"
    isRecurring: boolean;
    cronSchedule?: string;
}

export interface UpdateSpecialRequest {
    content: string;
    type: SpecialTypes;
    startDate: string; // "yyyy-MM-dd"
    startTime: string; // "HH:mm"
    endTime?: string; // "HH:mm"
    expirationDate?: string; // "yyyy-MM-dd"
    isRecurring: boolean;
    cronSchedule?: string;
}

export interface GetSpecialsRequest extends PageQueryParams {
    address: string;
    radius?: number;
    searchDateTime?: string; // ISO 8601
    searchTerm?: string;
    venueId?: string;
    specialTypeId?: number;
    isCurrentlyRunning?: boolean;
}

export interface GetVenuesRequest extends PageQueryParams {
    searchText?: string;
    address?: string;
    radiusInMiles?: number;
    openOnDayOfWeek?: number;
    timeOfDay?: string; // "HH:mm"
    hasActiveSpecials?: boolean;
    specialTypeId?: number;
    includeAddressDetails?: boolean;
    includeBusinessHours?: boolean;
    sortOrder?: number;
}

export interface CreateVenueRequest {
    name: string;
    description?: string;
    phoneNumber?: string;
    website?: string;
    email?: string;
    profileImage?: string;
    address: AddressRequest;
    hoursOfOperation: OperatingHours[];
}

export interface UpdateVenueRequest {
    name: string;
    description?: string;
    phoneNumber?: string;
    website?: string;
    email?: string;
    profileImage?: string;
    address: AddressRequest;
}

export interface AddressRequest {
    streetAddress: string;
    secondaryAddress?: string;
    locality: string;
    region: string;
    postcode: string;
    country: string;
}