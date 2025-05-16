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
        this.dayOfWeek = model.dayOfWeek as DayOfWeek;
        this.dayName = model.dayName;
        // Handle cases where openTime or closeTime might be invalid or not set, especially if isClosed is true
        this.openTime = model.isClosed || !model.openTime ? DateTime.now() : DateTime.fromFormat(model.openTime, 'HH:mm');
        this.closeTime = model.isClosed || !model.closeTime ? DateTime.now() : DateTime.fromFormat(model.closeTime, 'HH:mm');
        this.isClosed = model.isClosed;
    }

    public toModel(): OperatingScheduleItemModel {
        return {
            id: this.id,
            dayOfWeek: this.dayOfWeek,
            dayName: this.dayName,
            openTime: this.isClosed ? '00:00' : this.openTime.toFormat('HH:mm'),
            closeTime: this.isClosed ? '00:00' : this.closeTime.toFormat('HH:mm'),
            isClosed: this.isClosed,
        };
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