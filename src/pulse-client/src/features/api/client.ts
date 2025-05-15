import {
  OperatingSchedulesApiService,
  type OperatingSchedulesService,
  SpecialsApiService,
  type SpecialsService,
  VenuesApiService,
  type VenuesService,
} from "./services";

export class ApiClient {
  private _venues: VenuesService;
  private _specials: SpecialsService;
  private _operatingSchedules: OperatingSchedulesService;
  private baseUrl: string;
  private authToken?: string;

  constructor(baseUrl: string, authToken?: string) {
    this.baseUrl = baseUrl;
    this.authToken = authToken;
    this._venues = new VenuesApiService(this.baseUrl, this.authToken);
    this._specials = new SpecialsApiService(this.baseUrl, this.authToken);
    this._operatingSchedules = new OperatingSchedulesApiService(this.baseUrl, this.authToken);
  }

  get venues(): VenuesService {
    return this._venues;
  }

  get specials(): SpecialsService {
    return this._specials;
  }

  get operatingSchedules(): OperatingSchedulesService {
    return this._operatingSchedules;
  }

  setAuthToken(token: string): void {
    this.authToken = token;
    this._venues = new VenuesApiService(this.baseUrl, this.authToken);
    this._specials = new SpecialsApiService(this.baseUrl, this.authToken);
    this._operatingSchedules = new OperatingSchedulesApiService(this.baseUrl, this.authToken);
  }
}
