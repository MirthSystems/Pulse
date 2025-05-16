import { DateTime } from 'luxon';

export const SpecialTypes = {
    Food: 0,
    Drink: 1,
    Entertainment: 2,
} as const;

export type SpecialTypes = typeof SpecialTypes[keyof typeof SpecialTypes];

export interface SpecialItemModel {
    id: string;
    venueId: string;
    content: string;
    type: SpecialTypes;
    startDate: string; // ISO 8601
    endDate?: string; // ISO 8601
    createdAt: string; // ISO 8601
    updatedAt?: string; // ISO 8601
}

export class SpecialItem {
    id: string;
    venueId: string;
    content: string;
    type: SpecialTypes;
    startDate: DateTime;
    endDate?: DateTime;
    createdAt: DateTime;
    updatedAt?: DateTime;

    constructor(model: SpecialItemModel) {
        this.id = model.id;
        this.venueId = model.venueId;
        this.content = model.content;
        this.type = model.type;
        this.startDate = DateTime.fromISO(model.startDate);
        this.endDate = model.endDate ? DateTime.fromISO(model.endDate) : undefined;
        this.createdAt = DateTime.fromISO(model.createdAt);
        this.updatedAt = model.updatedAt ? DateTime.fromISO(model.updatedAt) : undefined;
    }

    public toModel(): SpecialItemModel {
        return {
            id: this.id,
            venueId: this.venueId,
            content: this.content,
            type: this.type,
            startDate: this.startDate.toISO() as string,
            endDate: this.endDate?.toISO() as string | undefined,
            createdAt: this.createdAt.toISO() as string,
            updatedAt: this.updatedAt?.toISO() as string | undefined,
        };
    }
}

export interface SpecialItemExtendedModel extends SpecialItemModel {
    venueName: string;
}

export class SpecialItemExtended extends SpecialItem {
    venueName: string;

    constructor(model: SpecialItemExtendedModel) {
        super(model);
        this.venueName = model.venueName;
    }

    public override toModel(): SpecialItemExtendedModel {
        const baseModel = super.toModel();
        return {
            ...baseModel,
            venueName: this.venueName,
        };
    }
}