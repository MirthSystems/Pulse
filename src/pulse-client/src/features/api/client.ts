import {
    OperatingScheduleItem,
    type OperatingScheduleItemModel,
    OperatingScheduleItemExtended,
    type OperatingScheduleItemExtendedModel,
    SpecialItem,
    type SpecialItemModel,
    SpecialItemExtended,
    type SpecialItemExtendedModel,
    VenueItem,
    type VenueItemModel,
    VenueItemExtended,
    type VenueItemExtendedModel,
    SearchSpecialsResult,
    type SearchSpecialsResultModel,
    type PagedResult,
    type CreateOperatingScheduleRequest,
    type UpdateOperatingScheduleRequest,
    type CreateSpecialRequest,
    type UpdateSpecialRequest,
    type GetSpecialsRequest,
    type GetVenuesRequest,
    type CreateVenueRequest,
    type UpdateVenueRequest,
} from '../../models';

interface VenuesApi {
    getVenues(request: GetVenuesRequest): Promise<PagedResult<VenueItem>>;
    getVenueById(id: string): Promise<VenueItemExtended>;
    getVenueBusinessHours(id: string): Promise<OperatingScheduleItem[]>;
    getVenueSpecials(id: string): Promise<SpecialItem[]>;
    createVenue(request: CreateVenueRequest): Promise<VenueItemExtended>;
    updateVenue(id: string, request: UpdateVenueRequest): Promise<VenueItemExtended>;
    deleteVenue(id: string): Promise<boolean>;
}

interface SpecialsApi {
    searchSpecials(request: GetSpecialsRequest): Promise<PagedResult<SearchSpecialsResult>>;
    getSpecialById(id: string): Promise<SpecialItemExtended>;
    createSpecial(request: CreateSpecialRequest): Promise<SpecialItemExtended>;
    updateSpecial(id: string, request: UpdateSpecialRequest): Promise<SpecialItemExtended>;
    deleteSpecial(id: string): Promise<boolean>;
}

interface OperatingSchedulesApi {
    getOperatingScheduleById(id: string): Promise<OperatingScheduleItemExtended>;
    createOperatingSchedule(request: CreateOperatingScheduleRequest): Promise<OperatingScheduleItemExtended>;
    updateOperatingSchedule(id: string, request: UpdateOperatingScheduleRequest): Promise<OperatingScheduleItemExtended>;
    deleteOperatingSchedule(id: string): Promise<boolean>;
}

export class ApiClient {
    private baseUrl: string;
    private authToken?: string;

    constructor(baseUrl: string, authToken?: string) {
        this.baseUrl = baseUrl;
        this.authToken = authToken;
    }

    private async fetch<T>(url: string, method: string, body?: unknown, requiresAuth: boolean = false): Promise<T> {
        const headers: HeadersInit = {
            'Content-Type': 'application/json',
        };

        if (requiresAuth) {
            if (!this.authToken) {
                throw new Error('Authentication token is required for this operation');
            }
            headers['Authorization'] = `Bearer ${this.authToken}`;
        }

        const response = await fetch(`${this.baseUrl}${url}`, {
            method,
            headers,
            body: body ? JSON.stringify(body) : undefined,
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        return data as T;
    }

    private toQueryParams(obj: object): URLSearchParams {
        const params = new URLSearchParams();
        for (const [key, value] of Object.entries(obj)) {
            if (value !== undefined) {
                params.append(key, String(value));
            }
        }
        return params;
    }

    venuesApi: VenuesApi = {
        getVenues: async (request: GetVenuesRequest): Promise<PagedResult<VenueItem>> => {
            const queryParams = this.toQueryParams(request).toString();
            const url = `/api/venues?${queryParams}`;
            const raw = await this.fetch<PagedResult<VenueItemModel>>(url, 'GET', undefined, false);
            const items = raw.items.map(item => new VenueItem(item));
            return { items, pagingInfo: raw.pagingInfo };
        },
        getVenueById: async (id: string): Promise<VenueItemExtended> => {
            const raw = await this.fetch<VenueItemExtendedModel>(`/api/venues/${id}`, 'GET', undefined, false);
            return new VenueItemExtended(raw);
        },
        getVenueBusinessHours: async (id: string): Promise<OperatingScheduleItem[]> => {
            const raw = await this.fetch<OperatingScheduleItemModel[]>(`/api/venues/${id}/business-hours`, 'GET', undefined, false);
            return raw.map(item => new OperatingScheduleItem(item));
        },
        getVenueSpecials: async (id: string): Promise<SpecialItem[]> => {
            const raw = await this.fetch<SpecialItemModel[]>(`/api/venues/${id}/specials`, 'GET', undefined, false);
            return raw.map(item => new SpecialItem(item));
        },
        createVenue: async (request: CreateVenueRequest): Promise<VenueItemExtended> => {
            const raw = await this.fetch<VenueItemExtendedModel>('/api/venues', 'POST', request, true);
            return new VenueItemExtended(raw);
        },
        updateVenue: async (id: string, request: UpdateVenueRequest): Promise<VenueItemExtended> => {
            const raw = await this.fetch<VenueItemExtendedModel>(`/api/venues/${id}`, 'PUT', request, true);
            return new VenueItemExtended(raw);
        },
        deleteVenue: async (id: string): Promise<boolean> => {
            return await this.fetch<boolean>(`/api/venues/${id}`, 'DELETE', undefined, true);
        },
    };

    specialsApi: SpecialsApi = {
        searchSpecials: async (request: GetSpecialsRequest): Promise<PagedResult<SearchSpecialsResult>> => {
            const queryParams = this.toQueryParams(request).toString();
            const url = `/api/specials?${queryParams}`;
            const raw = await this.fetch<PagedResult<SearchSpecialsResultModel>>(url, 'GET', undefined, false);
            const items = raw.items.map(item => new SearchSpecialsResult(item));
            return { items, pagingInfo: raw.pagingInfo };
        },
        getSpecialById: async (id: string): Promise<SpecialItemExtended> => {
            const raw = await this.fetch<SpecialItemExtendedModel>(`/api/specials/${id}`, 'GET', undefined, false);
            return new SpecialItemExtended(raw);
        },
        createSpecial: async (request: CreateSpecialRequest): Promise<SpecialItemExtended> => {
            const raw = await this.fetch<SpecialItemExtendedModel>('/api/specials', 'POST', request, true);
            return new SpecialItemExtended(raw);
        },
        updateSpecial: async (id: string, request: UpdateSpecialRequest): Promise<SpecialItemExtended> => {
            const raw = await this.fetch<SpecialItemExtendedModel>(`/api/specials/${id}`, 'PUT', request, true);
            return new SpecialItemExtended(raw);
        },
        deleteSpecial: async (id: string): Promise<boolean> => {
            return await this.fetch<boolean>(`/api/specials/${id}`, 'DELETE', undefined, true);
        },
    };

    operatingSchedulesApi: OperatingSchedulesApi = {
        getOperatingScheduleById: async (id: string): Promise<OperatingScheduleItemExtended> => {
            const raw = await this.fetch<OperatingScheduleItemExtendedModel>(`/api/operating-schedules/${id}`, 'GET', undefined, false);
            return new OperatingScheduleItemExtended(raw);
        },
        createOperatingSchedule: async (request: CreateOperatingScheduleRequest): Promise<OperatingScheduleItemExtended> => {
            const raw = await this.fetch<OperatingScheduleItemExtendedModel>('/api/operating-schedules', 'POST', request, true);
            return new OperatingScheduleItemExtended(raw);
        },
        updateOperatingSchedule: async (id: string, request: UpdateOperatingScheduleRequest): Promise<OperatingScheduleItemExtended> => {
            const raw = await this.fetch<OperatingScheduleItemExtendedModel>(`/api/operating-schedules/${id}`, 'PUT', request, true);
            return new OperatingScheduleItemExtended(raw);
        },
        deleteOperatingSchedule: async (id: string): Promise<boolean> => {
            return await this.fetch<boolean>(`/api/operating-schedules/${id}`, 'DELETE', undefined, true);
        },
    };
}