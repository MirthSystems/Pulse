import { DateTime } from 'luxon';
import { VenueItem, type VenueItemModel } from './venue';

const SpecialTypes = {
    Food: 0,
    Drink: 1,
    Entertainment: 2,
} as const;

export type SpecialTypes = typeof SpecialTypes[keyof typeof SpecialTypes];

export interface SpecialItemModel {
    id: string;
    venueId: string;
    content: string;
    type: number;
    typeName: string;
    startDate: string; // "yyyy-MM-dd"
    startTime: string; // "HH:mm"
    endTime?: string; // "HH:mm"
    isCurrentlyRunning: boolean;
    isRecurring: boolean;
}

export interface SpecialItemExtendedModel extends SpecialItemModel {
    expirationDate?: string; // "yyyy-MM-dd"
    cronSchedule?: string;
    createdAt: string; // ISO 8601
    updatedAt?: string; // ISO 8601
    venue: VenueItemModel;
}

export class SpecialItem {
    id: string;
    venueId: string;
    content: string;
    type: SpecialTypes;
    typeName: string;
    startDate: DateTime;
    startTime: DateTime;
    endTime?: DateTime;
    isCurrentlyRunning: boolean;
    isRecurring: boolean;

    constructor(model: SpecialItemModel) {
        this.id = model.id;
        this.venueId = model.venueId;
        this.content = model.content;
        this.type = model.type as SpecialTypes;
        this.typeName = model.typeName;
        this.startDate = DateTime.fromFormat(model.startDate, 'yyyy-MM-dd');
        this.startTime = DateTime.fromFormat(model.startTime, 'HH:mm');
        this.endTime = model.endTime ? DateTime.fromFormat(model.endTime, 'HH:mm') : undefined;
        this.isCurrentlyRunning = model.isCurrentlyRunning;
        this.isRecurring = model.isRecurring;
    }
}

export class SpecialItemExtended extends SpecialItem {
    expirationDate?: DateTime;
    cronSchedule?: string;
    createdAt: DateTime;
    updatedAt?: DateTime;
    venue: VenueItem;

    constructor(model: SpecialItemExtendedModel) {
        super(model);
        this.expirationDate = model.expirationDate ? DateTime.fromFormat(model.expirationDate, 'yyyy-MM-dd') : undefined;
        this.cronSchedule = model.cronSchedule;
        this.createdAt = DateTime.fromISO(model.createdAt);
        this.updatedAt = model.updatedAt ? DateTime.fromISO(model.updatedAt) : undefined;
        this.venue = new VenueItem(model.venue);
    }
}