import { IAddress, Address } from './Address';
import { IOperatingSchedule, OperatingSchedule } from './OperatingSchedule';

export interface IVenue {
  id?: string;
  name: string;
  description?: string | null;
  phoneNumber?: string | null;
  website?: string | null;
  email?: string | null;
  profileImage?: string | null;
  address?: IAddress;
  businessHours?: IOperatingSchedule[];
}

export interface IVenueResponse {
  id: string;
  name: string;
  description?: string | null;
  phoneNumber?: string | null;
  website?: string | null;
  email?: string | null;
  profileImage?: string | null;
  address: {
    id: string;
    streetAddress: string;
    secondaryAddress?: string | null;
    locality: string;
    region: string;
    postcode: string;
    country: string;
  };
}

export class Venue implements IVenue {
  id?: string;
  name: string;
  description?: string | null;
  phoneNumber?: string | null;
  website?: string | null;
  email?: string | null;
  profileImage?: string | null;
  address: Address;
  businessHours?: OperatingSchedule[];

  constructor(data: Partial<Venue> = {}) {
    this.id = data.id;
    this.name = data.name || '';
    this.description = data.description;
    this.phoneNumber = data.phoneNumber;
    this.website = data.website;
    this.email = data.email;
    this.profileImage = data.profileImage;
    this.address = data.address || new Address();
    this.businessHours = data.businessHours || [];
  }

  static fromResponse(response: IVenueResponse): Venue {
    return new Venue({
      id: response.id,
      name: response.name,
      description: response.description,
      phoneNumber: response.phoneNumber,
      website: response.website,
      email: response.email,
      profileImage: response.profileImage,
      address: Address.fromResponse(response.address)
    });
  }

  addBusinessHours(schedules: OperatingSchedule[]): void {
    if (!this.businessHours) {
      this.businessHours = [];
    }
    this.businessHours.push(...schedules);
  }

  isOpenAt(dateTime: Date | string): boolean {
    if (!this.businessHours || this.businessHours.length === 0) {
      return false;
    }

    const date = typeof dateTime === 'string' ? new Date(dateTime) : dateTime;

    const dayOfWeek = date.getDay();

    const schedule = this.businessHours.find(s => s.dayOfWeek === dayOfWeek);
    if (!schedule || schedule.isClosed) {
      return false;
    }

    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const timeString = `${hours}:${minutes}`;

    return timeString >= schedule.timeOfOpen && timeString <= schedule.timeOfClose;
  }
}