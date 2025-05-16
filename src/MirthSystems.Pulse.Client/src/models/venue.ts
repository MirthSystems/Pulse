import { DateTime } from 'luxon';
import { OperatingScheduleItem, type OperatingScheduleItemModel } from './operating-schedule';

export interface VenueItemModel {
    id: string;
    name: string;
    description?: string;
    locality: string;
    region: string;
    profileImage?: string;
    latitude?: number;
    longitude?: number;
}

export interface VenueItemExtendedModel extends VenueItemModel {
    phoneNumber: string;
    website: string;
    email: string;
    streetAddress: string;
    secondaryAddress: string;
    postcode: string;
    country: string;
    businessHours: OperatingScheduleItemModel[];
    createdAt: string; // ISO 8601
    updatedAt?: string; // ISO 8601
}

export class VenueItem {
    id: string;
    name: string;
    description?: string;
    locality: string;
    region: string;
    profileImage?: string;
    latitude?: number;
    longitude?: number;

    constructor(model: VenueItemModel) {
        this.id = model.id;
        this.name = model.name;
        this.description = model.description;
        this.locality = model.locality;
        this.region = model.region;
        this.profileImage = model.profileImage;
        this.latitude = model.latitude;
        this.longitude = model.longitude;
    }
}

export class VenueItemExtended extends VenueItem {
    phoneNumber: string;
    website: string;
    email: string;
    streetAddress: string;
    secondaryAddress: string;
    postcode: string;
    country: string;
    businessHours: OperatingScheduleItem[];
    createdAt: DateTime;
    updatedAt?: DateTime;

    constructor(model: VenueItemExtendedModel) {
        super(model);
        this.phoneNumber = model.phoneNumber;
        this.website = model.website;
        this.email = model.email;
        this.streetAddress = model.streetAddress;
        this.secondaryAddress = model.secondaryAddress;
        this.postcode = model.postcode;
        this.country = model.country;
        this.businessHours = model.businessHours.map(bh => new OperatingScheduleItem(bh));
        this.createdAt = DateTime.fromISO(model.createdAt);
        this.updatedAt = model.updatedAt ? DateTime.fromISO(model.updatedAt) : undefined;
    }

    public toModel(): VenueItemExtendedModel {
        return {
            // VenueItemModel properties
            id: this.id,
            name: this.name,
            description: this.description,
            locality: this.locality,
            region: this.region,
            profileImage: this.profileImage,
            latitude: this.latitude,
            longitude: this.longitude,
            // VenueItemExtendedModel specific properties
            phoneNumber: this.phoneNumber,
            website: this.website,
            email: this.email,
            streetAddress: this.streetAddress,
            secondaryAddress: this.secondaryAddress,
            postcode: this.postcode,
            country: this.country,
            businessHours: this.businessHours.map(bh => bh.toModel()), // Assuming OperatingScheduleItem has toModel()
            createdAt: this.createdAt.toISO() as string,
            updatedAt: this.updatedAt?.toISO() as string | undefined,
        };
    }
}