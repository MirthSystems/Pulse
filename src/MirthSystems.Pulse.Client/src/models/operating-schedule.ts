import { DateTime } from 'luxon';
import { type DayOfWeek } from './day-of-week';

export interface OperatingScheduleItemModel {
    id: string;
    dayOfWeek: DayOfWeek;
    dayName: string;
    openTime: string; // "HH:mm"
    closeTime: string; // "HH:mm"
    isClosed: boolean;
}

export interface OperatingScheduleItemExtendedModel extends OperatingScheduleItemModel {
    venueId: string;
    venueName: string;
}

export class OperatingScheduleItem {
    id: string;
    dayOfWeek: DayOfWeek;
    dayName: string;
    openTime: DateTime;
    closeTime: DateTime;
    isClosed: boolean;

    constructor(model: OperatingScheduleItemModel) {
        this.id = model.id;
        this.dayOfWeek = model.dayOfWeek;
        this.dayName = model.dayName;
        this.openTime = DateTime.fromISO(model.openTime);
        this.closeTime = DateTime.fromISO(model.closeTime);
        this.isClosed = model.isClosed;
    }
}

export class OperatingScheduleItemExtended extends OperatingScheduleItem {
    venueId: string;
    venueName: string;

    constructor(model: OperatingScheduleItemExtendedModel) {
        super(model);
        this.venueId = model.venueId;
        this.venueName = model.venueName;
    }
}