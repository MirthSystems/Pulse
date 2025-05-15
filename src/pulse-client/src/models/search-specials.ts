import { VenueItemExtended, type VenueItemExtendedModel } from './venue';
import { SpecialItem, type SpecialItemModel } from './special';

export interface SearchSpecialsResultModel {
    venue: VenueItemExtendedModel;
    specials: { items: SpecialItemModel[] };
}

export class SearchSpecialsResult {
    venue: VenueItemExtended;
    specials: { items: SpecialItem[] };

    constructor(model: SearchSpecialsResultModel) {
        this.venue = new VenueItemExtended(model.venue);
        this.specials = { items: model.specials.items.map(s => new SpecialItem(s)) };
    }
}