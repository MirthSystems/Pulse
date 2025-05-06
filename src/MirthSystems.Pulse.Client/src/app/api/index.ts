/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

import { DateTime, Duration } from "luxon";

export class ApiClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        this.http = http ? http : window as any;
        this.baseUrl = baseUrl ?? import.meta.env.VITE_API_URL ?? "https://localhost:5001";
    }

    /**
     * @return OK
     */
    operatingSchedulesGET(id: string, signal?: AbortSignal): Promise<ApiResponse<OperatingScheduleDetailApiResponse>> {
        let url_ = this.baseUrl + "/api/operating-schedules/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            signal,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processOperatingSchedulesGET(_response);
        });
    }

    protected processOperatingSchedulesGET(response: Response): Promise<ApiResponse<OperatingScheduleDetailApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = OperatingScheduleDetailApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = ObjectApiResponse.fromJS(resultData404);
            return throwException("Not Found", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<OperatingScheduleDetailApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @param body (optional) 
     * @return OK
     */
    operatingSchedulesPUT(id: string, body?: UpdateOperatingScheduleRequest | undefined, signal?: AbortSignal): Promise<ApiResponse<OperatingScheduleDetailApiResponse>> {
        let url_ = this.baseUrl + "/api/operating-schedules/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "PUT",
            signal,
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processOperatingSchedulesPUT(_response);
        });
    }

    protected processOperatingSchedulesPUT(response: Response): Promise<ApiResponse<OperatingScheduleDetailApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = OperatingScheduleDetailApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result401 = ObjectApiResponse.fromJS(resultData401);
            return throwException("Unauthorized", status, _responseText, _headers, result401);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = ObjectApiResponse.fromJS(resultData404);
            return throwException("Not Found", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<OperatingScheduleDetailApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @return OK
     */
    operatingSchedulesDELETE(id: string, signal?: AbortSignal): Promise<ApiResponse<BooleanApiResponse>> {
        let url_ = this.baseUrl + "/api/operating-schedules/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "DELETE",
            signal,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processOperatingSchedulesDELETE(_response);
        });
    }

    protected processOperatingSchedulesDELETE(response: Response): Promise<ApiResponse<BooleanApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = BooleanApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result401 = ObjectApiResponse.fromJS(resultData401);
            return throwException("Unauthorized", status, _responseText, _headers, result401);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = ObjectApiResponse.fromJS(resultData404);
            return throwException("Not Found", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<BooleanApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @param body (optional) 
     * @return Created
     */
    operatingSchedulesPOST(body?: CreateOperatingScheduleRequest | undefined, signal?: AbortSignal): Promise<ApiResponse<OperatingScheduleDetailApiResponse>> {
        let url_ = this.baseUrl + "/api/operating-schedules";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            signal,
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processOperatingSchedulesPOST(_response);
        });
    }

    protected processOperatingSchedulesPOST(response: Response): Promise<ApiResponse<OperatingScheduleDetailApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 201) {
            return response.text().then((_responseText) => {
            let result201: any = null;
            let resultData201 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result201 = OperatingScheduleDetailApiResponse.fromJS(resultData201);
            return new ApiResponse(status, _headers, result201);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result401 = ObjectApiResponse.fromJS(resultData401);
            return throwException("Unauthorized", status, _responseText, _headers, result401);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<OperatingScheduleDetailApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @return OK
     */
    venueGET(venueId: string, signal?: AbortSignal): Promise<ApiResponse<OperatingScheduleDetailListApiResponse>> {
        let url_ = this.baseUrl + "/api/operating-schedules/venue/{venueId}";
        if (venueId === undefined || venueId === null)
            throw new Error("The parameter 'venueId' must be defined.");
        url_ = url_.replace("{venueId}", encodeURIComponent("" + venueId));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            signal,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processVenueGET(_response);
        });
    }

    protected processVenueGET(response: Response): Promise<ApiResponse<OperatingScheduleDetailListApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = OperatingScheduleDetailListApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = ObjectApiResponse.fromJS(resultData404);
            return throwException("Not Found", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<OperatingScheduleDetailListApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @param body (optional) 
     * @return Created
     */
    venuePOST(venueId: string, body?: CreateOperatingScheduleRequest[] | undefined, signal?: AbortSignal): Promise<ApiResponse<BooleanApiResponse>> {
        let url_ = this.baseUrl + "/api/operating-schedules/venue/{venueId}";
        if (venueId === undefined || venueId === null)
            throw new Error("The parameter 'venueId' must be defined.");
        url_ = url_.replace("{venueId}", encodeURIComponent("" + venueId));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            signal,
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processVenuePOST(_response);
        });
    }

    protected processVenuePOST(response: Response): Promise<ApiResponse<BooleanApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 201) {
            return response.text().then((_responseText) => {
            let result201: any = null;
            let resultData201 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result201 = BooleanApiResponse.fromJS(resultData201);
            return new ApiResponse(status, _headers, result201);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result401 = ObjectApiResponse.fromJS(resultData401);
            return throwException("Unauthorized", status, _responseText, _headers, result401);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<BooleanApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @param radius (optional) 
     * @param searchDateTime (optional) 
     * @param searchTerm (optional) 
     * @param venueId (optional) 
     * @param specialTypeId (optional) 
     * @param isCurrentlyRunning (optional) 
     * @param page (optional) 
     * @param pageSize (optional) 
     * @return OK
     */
    specialsGET(address: string, radius?: number | undefined, searchDateTime?: string | undefined, searchTerm?: string | undefined, venueId?: string | undefined, specialTypeId?: number | undefined, isCurrentlyRunning?: boolean | undefined, page?: number | undefined, pageSize?: number | undefined, signal?: AbortSignal): Promise<ApiResponse<SpecialListItemPagedApiResponse>> {
        let url_ = this.baseUrl + "/api/specials?";
        if (address === undefined || address === null)
            throw new Error("The parameter 'address' must be defined and cannot be null.");
        else
            url_ += "Address=" + encodeURIComponent("" + address) + "&";
        if (radius === null)
            throw new Error("The parameter 'radius' cannot be null.");
        else if (radius !== undefined)
            url_ += "Radius=" + encodeURIComponent("" + radius) + "&";
        if (searchDateTime === null)
            throw new Error("The parameter 'searchDateTime' cannot be null.");
        else if (searchDateTime !== undefined)
            url_ += "SearchDateTime=" + encodeURIComponent("" + searchDateTime) + "&";
        if (searchTerm === null)
            throw new Error("The parameter 'searchTerm' cannot be null.");
        else if (searchTerm !== undefined)
            url_ += "SearchTerm=" + encodeURIComponent("" + searchTerm) + "&";
        if (venueId === null)
            throw new Error("The parameter 'venueId' cannot be null.");
        else if (venueId !== undefined)
            url_ += "VenueId=" + encodeURIComponent("" + venueId) + "&";
        if (specialTypeId === null)
            throw new Error("The parameter 'specialTypeId' cannot be null.");
        else if (specialTypeId !== undefined)
            url_ += "SpecialTypeId=" + encodeURIComponent("" + specialTypeId) + "&";
        if (isCurrentlyRunning === null)
            throw new Error("The parameter 'isCurrentlyRunning' cannot be null.");
        else if (isCurrentlyRunning !== undefined)
            url_ += "IsCurrentlyRunning=" + encodeURIComponent("" + isCurrentlyRunning) + "&";
        if (page === null)
            throw new Error("The parameter 'page' cannot be null.");
        else if (page !== undefined)
            url_ += "Page=" + encodeURIComponent("" + page) + "&";
        if (pageSize === null)
            throw new Error("The parameter 'pageSize' cannot be null.");
        else if (pageSize !== undefined)
            url_ += "PageSize=" + encodeURIComponent("" + pageSize) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            signal,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processSpecialsGET(_response);
        });
    }

    protected processSpecialsGET(response: Response): Promise<ApiResponse<SpecialListItemPagedApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = SpecialListItemPagedApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<SpecialListItemPagedApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @param body (optional) 
     * @return Created
     */
    specialsPOST(body?: CreateSpecialRequest | undefined, signal?: AbortSignal): Promise<ApiResponse<SpecialDetailApiResponse>> {
        let url_ = this.baseUrl + "/api/specials";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            signal,
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processSpecialsPOST(_response);
        });
    }

    protected processSpecialsPOST(response: Response): Promise<ApiResponse<SpecialDetailApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 201) {
            return response.text().then((_responseText) => {
            let result201: any = null;
            let resultData201 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result201 = SpecialDetailApiResponse.fromJS(resultData201);
            return new ApiResponse(status, _headers, result201);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result401 = ObjectApiResponse.fromJS(resultData401);
            return throwException("Unauthorized", status, _responseText, _headers, result401);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<SpecialDetailApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @return OK
     */
    specialsGET2(id: string, signal?: AbortSignal): Promise<ApiResponse<SpecialDetailApiResponse>> {
        let url_ = this.baseUrl + "/api/specials/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            signal,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processSpecialsGET2(_response);
        });
    }

    protected processSpecialsGET2(response: Response): Promise<ApiResponse<SpecialDetailApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = SpecialDetailApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = ObjectApiResponse.fromJS(resultData404);
            return throwException("Not Found", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<SpecialDetailApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @param body (optional) 
     * @return OK
     */
    specialsPUT(id: string, body?: UpdateSpecialRequest | undefined, signal?: AbortSignal): Promise<ApiResponse<SpecialDetailApiResponse>> {
        let url_ = this.baseUrl + "/api/specials/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "PUT",
            signal,
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processSpecialsPUT(_response);
        });
    }

    protected processSpecialsPUT(response: Response): Promise<ApiResponse<SpecialDetailApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = SpecialDetailApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result401 = ObjectApiResponse.fromJS(resultData401);
            return throwException("Unauthorized", status, _responseText, _headers, result401);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = ObjectApiResponse.fromJS(resultData404);
            return throwException("Not Found", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<SpecialDetailApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @return OK
     */
    specialsDELETE(id: string, signal?: AbortSignal): Promise<ApiResponse<BooleanApiResponse>> {
        let url_ = this.baseUrl + "/api/specials/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "DELETE",
            signal,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processSpecialsDELETE(_response);
        });
    }

    protected processSpecialsDELETE(response: Response): Promise<ApiResponse<BooleanApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = BooleanApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result401 = ObjectApiResponse.fromJS(resultData401);
            return throwException("Unauthorized", status, _responseText, _headers, result401);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = ObjectApiResponse.fromJS(resultData404);
            return throwException("Not Found", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<BooleanApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @param page (optional) 
     * @param pageSize (optional) 
     * @return OK
     */
    venuesGET(page?: number | undefined, pageSize?: number | undefined, signal?: AbortSignal): Promise<ApiResponse<VenueListItemPagedApiResponse>> {
        let url_ = this.baseUrl + "/api/venues?";
        if (page === null)
            throw new Error("The parameter 'page' cannot be null.");
        else if (page !== undefined)
            url_ += "page=" + encodeURIComponent("" + page) + "&";
        if (pageSize === null)
            throw new Error("The parameter 'pageSize' cannot be null.");
        else if (pageSize !== undefined)
            url_ += "pageSize=" + encodeURIComponent("" + pageSize) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            signal,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processVenuesGET(_response);
        });
    }

    protected processVenuesGET(response: Response): Promise<ApiResponse<VenueListItemPagedApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = VenueListItemPagedApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<VenueListItemPagedApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @param body (optional) 
     * @return Created
     */
    venuesPOST(body?: CreateVenueRequest | undefined, signal?: AbortSignal): Promise<ApiResponse<VenueDetailApiResponse>> {
        let url_ = this.baseUrl + "/api/venues";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            signal,
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processVenuesPOST(_response);
        });
    }

    protected processVenuesPOST(response: Response): Promise<ApiResponse<VenueDetailApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 201) {
            return response.text().then((_responseText) => {
            let result201: any = null;
            let resultData201 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result201 = VenueDetailApiResponse.fromJS(resultData201);
            return new ApiResponse(status, _headers, result201);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result401 = ObjectApiResponse.fromJS(resultData401);
            return throwException("Unauthorized", status, _responseText, _headers, result401);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<VenueDetailApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @return OK
     */
    venuesGET2(id: string, signal?: AbortSignal): Promise<ApiResponse<VenueDetailApiResponse>> {
        let url_ = this.baseUrl + "/api/venues/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            signal,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processVenuesGET2(_response);
        });
    }

    protected processVenuesGET2(response: Response): Promise<ApiResponse<VenueDetailApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = VenueDetailApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = ObjectApiResponse.fromJS(resultData404);
            return throwException("Not Found", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<VenueDetailApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @param body (optional) 
     * @return OK
     */
    venuesPUT(id: string, body?: UpdateVenueRequest | undefined, signal?: AbortSignal): Promise<ApiResponse<VenueDetailApiResponse>> {
        let url_ = this.baseUrl + "/api/venues/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "PUT",
            signal,
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processVenuesPUT(_response);
        });
    }

    protected processVenuesPUT(response: Response): Promise<ApiResponse<VenueDetailApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = VenueDetailApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result401 = ObjectApiResponse.fromJS(resultData401);
            return throwException("Unauthorized", status, _responseText, _headers, result401);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = ObjectApiResponse.fromJS(resultData404);
            return throwException("Not Found", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<VenueDetailApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @return OK
     */
    venuesDELETE(id: string, signal?: AbortSignal): Promise<ApiResponse<BooleanApiResponse>> {
        let url_ = this.baseUrl + "/api/venues/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "DELETE",
            signal,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processVenuesDELETE(_response);
        });
    }

    protected processVenuesDELETE(response: Response): Promise<ApiResponse<BooleanApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = BooleanApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result401 = ObjectApiResponse.fromJS(resultData401);
            return throwException("Unauthorized", status, _responseText, _headers, result401);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = ObjectApiResponse.fromJS(resultData404);
            return throwException("Not Found", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<BooleanApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @return OK
     */
    businessHours(id: string, signal?: AbortSignal): Promise<ApiResponse<BusinessHoursApiResponse>> {
        let url_ = this.baseUrl + "/api/venues/{id}/business-hours";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            signal,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processBusinessHours(_response);
        });
    }

    protected processBusinessHours(response: Response): Promise<ApiResponse<BusinessHoursApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = BusinessHoursApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = ObjectApiResponse.fromJS(resultData404);
            return throwException("Not Found", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<BusinessHoursApiResponse>>(new ApiResponse(status, _headers, null as any));
    }

    /**
     * @return OK
     */
    specialsGET3(id: string, signal?: AbortSignal): Promise<ApiResponse<VenueSpecialsApiResponse>> {
        let url_ = this.baseUrl + "/api/venues/{id}/specials";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            signal,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processSpecialsGET3(_response);
        });
    }

    protected processSpecialsGET3(response: Response): Promise<ApiResponse<VenueSpecialsApiResponse>> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = VenueSpecialsApiResponse.fromJS(resultData200);
            return new ApiResponse(status, _headers, result200);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ObjectApiResponse.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result404 = ObjectApiResponse.fromJS(resultData404);
            return throwException("Not Found", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ApiResponse<VenueSpecialsApiResponse>>(new ApiResponse(status, _headers, null as any));
    }
}

export class BooleanApiResponse implements IBooleanApiResponse {
    success?: boolean;
    message?: string | null;
    data?: boolean;

    constructor(data?: IBooleanApiResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.success = _data["success"] !== undefined ? _data["success"] : <any>null;
            this.message = _data["message"] !== undefined ? _data["message"] : <any>null;
            this.data = _data["data"] !== undefined ? _data["data"] : <any>null;
        }
    }

    static fromJS(data: any): BooleanApiResponse {
        data = typeof data === 'object' ? data : {};
        let result = new BooleanApiResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["success"] = this.success !== undefined ? this.success : <any>null;
        data["message"] = this.message !== undefined ? this.message : <any>null;
        data["data"] = this.data !== undefined ? this.data : <any>null;
        return data;
    }
}

export interface IBooleanApiResponse {
    success?: boolean;
    message?: string | null;
    data?: boolean;
}

export class BusinessHours implements IBusinessHours {
    venueId?: string | null;
    venueName!: string;
    scheduleItems!: OperatingScheduleListItem[];

    constructor(data?: IBusinessHours) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            if (data.scheduleItems) {
                this.scheduleItems = [];
                for (let i = 0; i < data.scheduleItems.length; i++) {
                    let item = data.scheduleItems[i];
                    this.scheduleItems[i] = item && !(<any>item).toJSON ? new OperatingScheduleListItem(item) : <OperatingScheduleListItem>item;
                }
            }
        }
        if (!data) {
            this.scheduleItems = [];
        }
    }

    init(_data?: any) {
        if (_data) {
            this.venueId = _data["venueId"] !== undefined ? _data["venueId"] : <any>null;
            this.venueName = _data["venueName"] !== undefined ? _data["venueName"] : <any>null;
            if (Array.isArray(_data["scheduleItems"])) {
                this.scheduleItems = [] as any;
                for (let item of _data["scheduleItems"])
                    this.scheduleItems!.push(OperatingScheduleListItem.fromJS(item));
            }
            else {
                this.scheduleItems = <any>null;
            }
        }
    }

    static fromJS(data: any): BusinessHours {
        data = typeof data === 'object' ? data : {};
        let result = new BusinessHours();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["venueId"] = this.venueId !== undefined ? this.venueId : <any>null;
        data["venueName"] = this.venueName !== undefined ? this.venueName : <any>null;
        if (Array.isArray(this.scheduleItems)) {
            data["scheduleItems"] = [];
            for (let item of this.scheduleItems)
                data["scheduleItems"].push(item ? item.toJSON() : <any>null);
        }
        return data;
    }
}

export interface IBusinessHours {
    venueId?: string | null;
    venueName: string;
    scheduleItems: IOperatingScheduleListItem[];
}

export class BusinessHoursApiResponse implements IBusinessHoursApiResponse {
    success?: boolean;
    message?: string | null;
    data?: BusinessHours;

    constructor(data?: IBusinessHoursApiResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            this.data = data.data && !(<any>data.data).toJSON ? new BusinessHours(data.data) : <BusinessHours>this.data;
        }
    }

    init(_data?: any) {
        if (_data) {
            this.success = _data["success"] !== undefined ? _data["success"] : <any>null;
            this.message = _data["message"] !== undefined ? _data["message"] : <any>null;
            this.data = _data["data"] ? BusinessHours.fromJS(_data["data"]) : <any>null;
        }
    }

    static fromJS(data: any): BusinessHoursApiResponse {
        data = typeof data === 'object' ? data : {};
        let result = new BusinessHoursApiResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["success"] = this.success !== undefined ? this.success : <any>null;
        data["message"] = this.message !== undefined ? this.message : <any>null;
        data["data"] = this.data ? this.data.toJSON() : <any>null;
        return data;
    }
}

export interface IBusinessHoursApiResponse {
    success?: boolean;
    message?: string | null;
    data?: IBusinessHours;
}

export class CreateAddressRequest implements ICreateAddressRequest {
    streetAddress!: string;
    secondaryAddress?: string | null;
    locality!: string;
    region!: string;
    postcode!: string;
    country!: string;

    constructor(data?: ICreateAddressRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.streetAddress = _data["streetAddress"] !== undefined ? _data["streetAddress"] : <any>null;
            this.secondaryAddress = _data["secondaryAddress"] !== undefined ? _data["secondaryAddress"] : <any>null;
            this.locality = _data["locality"] !== undefined ? _data["locality"] : <any>null;
            this.region = _data["region"] !== undefined ? _data["region"] : <any>null;
            this.postcode = _data["postcode"] !== undefined ? _data["postcode"] : <any>null;
            this.country = _data["country"] !== undefined ? _data["country"] : <any>null;
        }
    }

    static fromJS(data: any): CreateAddressRequest {
        data = typeof data === 'object' ? data : {};
        let result = new CreateAddressRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["streetAddress"] = this.streetAddress !== undefined ? this.streetAddress : <any>null;
        data["secondaryAddress"] = this.secondaryAddress !== undefined ? this.secondaryAddress : <any>null;
        data["locality"] = this.locality !== undefined ? this.locality : <any>null;
        data["region"] = this.region !== undefined ? this.region : <any>null;
        data["postcode"] = this.postcode !== undefined ? this.postcode : <any>null;
        data["country"] = this.country !== undefined ? this.country : <any>null;
        return data;
    }
}

export interface ICreateAddressRequest {
    streetAddress: string;
    secondaryAddress?: string | null;
    locality: string;
    region: string;
    postcode: string;
    country: string;
}

export class CreateOperatingScheduleRequest implements ICreateOperatingScheduleRequest {
    venueId!: string;
    dayOfWeek!: DayOfWeek;
    timeOfOpen!: string;
    timeOfClose!: string;
    isClosed!: boolean;

    constructor(data?: ICreateOperatingScheduleRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.venueId = _data["venueId"] !== undefined ? _data["venueId"] : <any>null;
            this.dayOfWeek = _data["dayOfWeek"] !== undefined ? _data["dayOfWeek"] : <any>null;
            this.timeOfOpen = _data["timeOfOpen"] !== undefined ? _data["timeOfOpen"] : <any>null;
            this.timeOfClose = _data["timeOfClose"] !== undefined ? _data["timeOfClose"] : <any>null;
            this.isClosed = _data["isClosed"] !== undefined ? _data["isClosed"] : <any>null;
        }
    }

    static fromJS(data: any): CreateOperatingScheduleRequest {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOperatingScheduleRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["venueId"] = this.venueId !== undefined ? this.venueId : <any>null;
        data["dayOfWeek"] = this.dayOfWeek !== undefined ? this.dayOfWeek : <any>null;
        data["timeOfOpen"] = this.timeOfOpen !== undefined ? this.timeOfOpen : <any>null;
        data["timeOfClose"] = this.timeOfClose !== undefined ? this.timeOfClose : <any>null;
        data["isClosed"] = this.isClosed !== undefined ? this.isClosed : <any>null;
        return data;
    }
}

export interface ICreateOperatingScheduleRequest {
    venueId: string;
    dayOfWeek: DayOfWeek;
    timeOfOpen: string;
    timeOfClose: string;
    isClosed: boolean;
}

export class CreateSpecialRequest implements ICreateSpecialRequest {
    venueId!: string;
    content!: string;
    type!: SpecialTypes;
    startDate!: string;
    startTime!: string;
    endTime?: string | null;
    expirationDate?: string | null;
    isRecurring!: boolean;
    cronSchedule?: string | null;

    constructor(data?: ICreateSpecialRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.venueId = _data["venueId"] !== undefined ? _data["venueId"] : <any>null;
            this.content = _data["content"] !== undefined ? _data["content"] : <any>null;
            this.type = _data["type"] !== undefined ? _data["type"] : <any>null;
            this.startDate = _data["startDate"] !== undefined ? _data["startDate"] : <any>null;
            this.startTime = _data["startTime"] !== undefined ? _data["startTime"] : <any>null;
            this.endTime = _data["endTime"] !== undefined ? _data["endTime"] : <any>null;
            this.expirationDate = _data["expirationDate"] !== undefined ? _data["expirationDate"] : <any>null;
            this.isRecurring = _data["isRecurring"] !== undefined ? _data["isRecurring"] : <any>null;
            this.cronSchedule = _data["cronSchedule"] !== undefined ? _data["cronSchedule"] : <any>null;
        }
    }

    static fromJS(data: any): CreateSpecialRequest {
        data = typeof data === 'object' ? data : {};
        let result = new CreateSpecialRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["venueId"] = this.venueId !== undefined ? this.venueId : <any>null;
        data["content"] = this.content !== undefined ? this.content : <any>null;
        data["type"] = this.type !== undefined ? this.type : <any>null;
        data["startDate"] = this.startDate !== undefined ? this.startDate : <any>null;
        data["startTime"] = this.startTime !== undefined ? this.startTime : <any>null;
        data["endTime"] = this.endTime !== undefined ? this.endTime : <any>null;
        data["expirationDate"] = this.expirationDate !== undefined ? this.expirationDate : <any>null;
        data["isRecurring"] = this.isRecurring !== undefined ? this.isRecurring : <any>null;
        data["cronSchedule"] = this.cronSchedule !== undefined ? this.cronSchedule : <any>null;
        return data;
    }
}

export interface ICreateSpecialRequest {
    venueId: string;
    content: string;
    type: SpecialTypes;
    startDate: string;
    startTime: string;
    endTime?: string | null;
    expirationDate?: string | null;
    isRecurring: boolean;
    cronSchedule?: string | null;
}

export class CreateVenueRequest implements ICreateVenueRequest {
    name!: string;
    description?: string | null;
    phoneNumber?: string | null;
    website?: string | null;
    email?: string | null;
    profileImage?: string | null;
    address!: CreateAddressRequest;
    businessHours!: CreateOperatingScheduleRequest[];

    constructor(data?: ICreateVenueRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            this.address = data.address && !(<any>data.address).toJSON ? new CreateAddressRequest(data.address) : <CreateAddressRequest>this.address;
            if (data.businessHours) {
                this.businessHours = [];
                for (let i = 0; i < data.businessHours.length; i++) {
                    let item = data.businessHours[i];
                    this.businessHours[i] = item && !(<any>item).toJSON ? new CreateOperatingScheduleRequest(item) : <CreateOperatingScheduleRequest>item;
                }
            }
        }
        if (!data) {
            this.address = new CreateAddressRequest();
            this.businessHours = [];
        }
    }

    init(_data?: any) {
        if (_data) {
            this.name = _data["name"] !== undefined ? _data["name"] : <any>null;
            this.description = _data["description"] !== undefined ? _data["description"] : <any>null;
            this.phoneNumber = _data["phoneNumber"] !== undefined ? _data["phoneNumber"] : <any>null;
            this.website = _data["website"] !== undefined ? _data["website"] : <any>null;
            this.email = _data["email"] !== undefined ? _data["email"] : <any>null;
            this.profileImage = _data["profileImage"] !== undefined ? _data["profileImage"] : <any>null;
            this.address = _data["address"] ? CreateAddressRequest.fromJS(_data["address"]) : new CreateAddressRequest();
            if (Array.isArray(_data["businessHours"])) {
                this.businessHours = [] as any;
                for (let item of _data["businessHours"])
                    this.businessHours!.push(CreateOperatingScheduleRequest.fromJS(item));
            }
            else {
                this.businessHours = <any>null;
            }
        }
    }

    static fromJS(data: any): CreateVenueRequest {
        data = typeof data === 'object' ? data : {};
        let result = new CreateVenueRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name !== undefined ? this.name : <any>null;
        data["description"] = this.description !== undefined ? this.description : <any>null;
        data["phoneNumber"] = this.phoneNumber !== undefined ? this.phoneNumber : <any>null;
        data["website"] = this.website !== undefined ? this.website : <any>null;
        data["email"] = this.email !== undefined ? this.email : <any>null;
        data["profileImage"] = this.profileImage !== undefined ? this.profileImage : <any>null;
        data["address"] = this.address ? this.address.toJSON() : <any>null;
        if (Array.isArray(this.businessHours)) {
            data["businessHours"] = [];
            for (let item of this.businessHours)
                data["businessHours"].push(item ? item.toJSON() : <any>null);
        }
        return data;
    }
}

export interface ICreateVenueRequest {
    name: string;
    description?: string | null;
    phoneNumber?: string | null;
    website?: string | null;
    email?: string | null;
    profileImage?: string | null;
    address: ICreateAddressRequest;
    businessHours: ICreateOperatingScheduleRequest[];
}

export enum DayOfWeek {
    _0 = 0,
    _1 = 1,
    _2 = 2,
    _3 = 3,
    _4 = 4,
    _5 = 5,
    _6 = 6,
}

export class ObjectApiResponse implements IObjectApiResponse {
    success?: boolean;
    message?: string | null;
    data?: any | null;

    constructor(data?: IObjectApiResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.success = _data["success"] !== undefined ? _data["success"] : <any>null;
            this.message = _data["message"] !== undefined ? _data["message"] : <any>null;
            this.data = _data["data"] !== undefined ? _data["data"] : <any>null;
        }
    }

    static fromJS(data: any): ObjectApiResponse {
        data = typeof data === 'object' ? data : {};
        let result = new ObjectApiResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["success"] = this.success !== undefined ? this.success : <any>null;
        data["message"] = this.message !== undefined ? this.message : <any>null;
        data["data"] = this.data !== undefined ? this.data : <any>null;
        return data;
    }
}

export interface IObjectApiResponse {
    success?: boolean;
    message?: string | null;
    data?: any | null;
}

export class OperatingScheduleDetail implements IOperatingScheduleDetail {
    id?: string | null;
    venueId?: string | null;
    venueName!: string;
    dayOfWeek?: DayOfWeek;
    dayName!: string;
    openTime!: string;
    closeTime!: string;
    isClosed?: boolean;

    constructor(data?: IOperatingScheduleDetail) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"] !== undefined ? _data["id"] : <any>null;
            this.venueId = _data["venueId"] !== undefined ? _data["venueId"] : <any>null;
            this.venueName = _data["venueName"] !== undefined ? _data["venueName"] : <any>null;
            this.dayOfWeek = _data["dayOfWeek"] !== undefined ? _data["dayOfWeek"] : <any>null;
            this.dayName = _data["dayName"] !== undefined ? _data["dayName"] : <any>null;
            this.openTime = _data["openTime"] !== undefined ? _data["openTime"] : <any>null;
            this.closeTime = _data["closeTime"] !== undefined ? _data["closeTime"] : <any>null;
            this.isClosed = _data["isClosed"] !== undefined ? _data["isClosed"] : <any>null;
        }
    }

    static fromJS(data: any): OperatingScheduleDetail {
        data = typeof data === 'object' ? data : {};
        let result = new OperatingScheduleDetail();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id !== undefined ? this.id : <any>null;
        data["venueId"] = this.venueId !== undefined ? this.venueId : <any>null;
        data["venueName"] = this.venueName !== undefined ? this.venueName : <any>null;
        data["dayOfWeek"] = this.dayOfWeek !== undefined ? this.dayOfWeek : <any>null;
        data["dayName"] = this.dayName !== undefined ? this.dayName : <any>null;
        data["openTime"] = this.openTime !== undefined ? this.openTime : <any>null;
        data["closeTime"] = this.closeTime !== undefined ? this.closeTime : <any>null;
        data["isClosed"] = this.isClosed !== undefined ? this.isClosed : <any>null;
        return data;
    }
}

export interface IOperatingScheduleDetail {
    id?: string | null;
    venueId?: string | null;
    venueName: string;
    dayOfWeek?: DayOfWeek;
    dayName: string;
    openTime: string;
    closeTime: string;
    isClosed?: boolean;
}

export class OperatingScheduleDetailApiResponse implements IOperatingScheduleDetailApiResponse {
    success?: boolean;
    message?: string | null;
    data?: OperatingScheduleDetail;

    constructor(data?: IOperatingScheduleDetailApiResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            this.data = data.data && !(<any>data.data).toJSON ? new OperatingScheduleDetail(data.data) : <OperatingScheduleDetail>this.data;
        }
    }

    init(_data?: any) {
        if (_data) {
            this.success = _data["success"] !== undefined ? _data["success"] : <any>null;
            this.message = _data["message"] !== undefined ? _data["message"] : <any>null;
            this.data = _data["data"] ? OperatingScheduleDetail.fromJS(_data["data"]) : <any>null;
        }
    }

    static fromJS(data: any): OperatingScheduleDetailApiResponse {
        data = typeof data === 'object' ? data : {};
        let result = new OperatingScheduleDetailApiResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["success"] = this.success !== undefined ? this.success : <any>null;
        data["message"] = this.message !== undefined ? this.message : <any>null;
        data["data"] = this.data ? this.data.toJSON() : <any>null;
        return data;
    }
}

export interface IOperatingScheduleDetailApiResponse {
    success?: boolean;
    message?: string | null;
    data?: IOperatingScheduleDetail;
}

export class OperatingScheduleDetailListApiResponse implements IOperatingScheduleDetailListApiResponse {
    success?: boolean;
    message?: string | null;
    data?: OperatingScheduleDetail[] | null;

    constructor(data?: IOperatingScheduleDetailListApiResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            if (data.data) {
                this.data = [];
                for (let i = 0; i < data.data.length; i++) {
                    let item = data.data[i];
                    this.data[i] = item && !(<any>item).toJSON ? new OperatingScheduleDetail(item) : <OperatingScheduleDetail>item;
                }
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.success = _data["success"] !== undefined ? _data["success"] : <any>null;
            this.message = _data["message"] !== undefined ? _data["message"] : <any>null;
            if (Array.isArray(_data["data"])) {
                this.data = [] as any;
                for (let item of _data["data"])
                    this.data!.push(OperatingScheduleDetail.fromJS(item));
            }
            else {
                this.data = <any>null;
            }
        }
    }

    static fromJS(data: any): OperatingScheduleDetailListApiResponse {
        data = typeof data === 'object' ? data : {};
        let result = new OperatingScheduleDetailListApiResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["success"] = this.success !== undefined ? this.success : <any>null;
        data["message"] = this.message !== undefined ? this.message : <any>null;
        if (Array.isArray(this.data)) {
            data["data"] = [];
            for (let item of this.data)
                data["data"].push(item ? item.toJSON() : <any>null);
        }
        return data;
    }
}

export interface IOperatingScheduleDetailListApiResponse {
    success?: boolean;
    message?: string | null;
    data?: IOperatingScheduleDetail[] | null;
}

export class OperatingScheduleListItem implements IOperatingScheduleListItem {
    id?: string | null;
    dayOfWeek?: DayOfWeek;
    dayName!: string;
    openTime!: string;
    closeTime!: string;
    isClosed?: boolean;

    constructor(data?: IOperatingScheduleListItem) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"] !== undefined ? _data["id"] : <any>null;
            this.dayOfWeek = _data["dayOfWeek"] !== undefined ? _data["dayOfWeek"] : <any>null;
            this.dayName = _data["dayName"] !== undefined ? _data["dayName"] : <any>null;
            this.openTime = _data["openTime"] !== undefined ? _data["openTime"] : <any>null;
            this.closeTime = _data["closeTime"] !== undefined ? _data["closeTime"] : <any>null;
            this.isClosed = _data["isClosed"] !== undefined ? _data["isClosed"] : <any>null;
        }
    }

    static fromJS(data: any): OperatingScheduleListItem {
        data = typeof data === 'object' ? data : {};
        let result = new OperatingScheduleListItem();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id !== undefined ? this.id : <any>null;
        data["dayOfWeek"] = this.dayOfWeek !== undefined ? this.dayOfWeek : <any>null;
        data["dayName"] = this.dayName !== undefined ? this.dayName : <any>null;
        data["openTime"] = this.openTime !== undefined ? this.openTime : <any>null;
        data["closeTime"] = this.closeTime !== undefined ? this.closeTime : <any>null;
        data["isClosed"] = this.isClosed !== undefined ? this.isClosed : <any>null;
        return data;
    }
}

export interface IOperatingScheduleListItem {
    id?: string | null;
    dayOfWeek?: DayOfWeek;
    dayName: string;
    openTime: string;
    closeTime: string;
    isClosed?: boolean;
}

export class PaginationData implements IPaginationData {
    page?: number;
    pageSize?: number;
    totalCount?: number;
    totalPages?: number;
    readonly hasPreviousPage?: boolean;
    readonly hasNextPage?: boolean;

    constructor(data?: IPaginationData) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.page = _data["page"] !== undefined ? _data["page"] : <any>null;
            this.pageSize = _data["pageSize"] !== undefined ? _data["pageSize"] : <any>null;
            this.totalCount = _data["totalCount"] !== undefined ? _data["totalCount"] : <any>null;
            this.totalPages = _data["totalPages"] !== undefined ? _data["totalPages"] : <any>null;
            (<any>this).hasPreviousPage = _data["hasPreviousPage"] !== undefined ? _data["hasPreviousPage"] : <any>null;
            (<any>this).hasNextPage = _data["hasNextPage"] !== undefined ? _data["hasNextPage"] : <any>null;
        }
    }

    static fromJS(data: any): PaginationData {
        data = typeof data === 'object' ? data : {};
        let result = new PaginationData();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["page"] = this.page !== undefined ? this.page : <any>null;
        data["pageSize"] = this.pageSize !== undefined ? this.pageSize : <any>null;
        data["totalCount"] = this.totalCount !== undefined ? this.totalCount : <any>null;
        data["totalPages"] = this.totalPages !== undefined ? this.totalPages : <any>null;
        data["hasPreviousPage"] = this.hasPreviousPage !== undefined ? this.hasPreviousPage : <any>null;
        data["hasNextPage"] = this.hasNextPage !== undefined ? this.hasNextPage : <any>null;
        return data;
    }
}

export interface IPaginationData {
    page?: number;
    pageSize?: number;
    totalCount?: number;
    totalPages?: number;
    hasPreviousPage?: boolean;
    hasNextPage?: boolean;
}

export class SpecialDetail implements ISpecialDetail {
    id?: string | null;
    venueId?: string | null;
    venueName!: string;
    content!: string;
    type?: SpecialTypes;
    typeName!: string;
    startDate!: string;
    startTime!: string;
    endTime?: string | null;
    expirationDate?: string | null;
    isRecurring?: boolean;
    cronSchedule?: string | null;
    isCurrentlyRunning?: boolean;
    createdAt?: DateTime;
    updatedAt?: DateTime | null;

    constructor(data?: ISpecialDetail) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"] !== undefined ? _data["id"] : <any>null;
            this.venueId = _data["venueId"] !== undefined ? _data["venueId"] : <any>null;
            this.venueName = _data["venueName"] !== undefined ? _data["venueName"] : <any>null;
            this.content = _data["content"] !== undefined ? _data["content"] : <any>null;
            this.type = _data["type"] !== undefined ? _data["type"] : <any>null;
            this.typeName = _data["typeName"] !== undefined ? _data["typeName"] : <any>null;
            this.startDate = _data["startDate"] !== undefined ? _data["startDate"] : <any>null;
            this.startTime = _data["startTime"] !== undefined ? _data["startTime"] : <any>null;
            this.endTime = _data["endTime"] !== undefined ? _data["endTime"] : <any>null;
            this.expirationDate = _data["expirationDate"] !== undefined ? _data["expirationDate"] : <any>null;
            this.isRecurring = _data["isRecurring"] !== undefined ? _data["isRecurring"] : <any>null;
            this.cronSchedule = _data["cronSchedule"] !== undefined ? _data["cronSchedule"] : <any>null;
            this.isCurrentlyRunning = _data["isCurrentlyRunning"] !== undefined ? _data["isCurrentlyRunning"] : <any>null;
            this.createdAt = _data["createdAt"] ? DateTime.fromISO(_data["createdAt"].toString()) : <any>null;
            this.updatedAt = _data["updatedAt"] ? DateTime.fromISO(_data["updatedAt"].toString()) : <any>null;
        }
    }

    static fromJS(data: any): SpecialDetail {
        data = typeof data === 'object' ? data : {};
        let result = new SpecialDetail();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id !== undefined ? this.id : <any>null;
        data["venueId"] = this.venueId !== undefined ? this.venueId : <any>null;
        data["venueName"] = this.venueName !== undefined ? this.venueName : <any>null;
        data["content"] = this.content !== undefined ? this.content : <any>null;
        data["type"] = this.type !== undefined ? this.type : <any>null;
        data["typeName"] = this.typeName !== undefined ? this.typeName : <any>null;
        data["startDate"] = this.startDate !== undefined ? this.startDate : <any>null;
        data["startTime"] = this.startTime !== undefined ? this.startTime : <any>null;
        data["endTime"] = this.endTime !== undefined ? this.endTime : <any>null;
        data["expirationDate"] = this.expirationDate !== undefined ? this.expirationDate : <any>null;
        data["isRecurring"] = this.isRecurring !== undefined ? this.isRecurring : <any>null;
        data["cronSchedule"] = this.cronSchedule !== undefined ? this.cronSchedule : <any>null;
        data["isCurrentlyRunning"] = this.isCurrentlyRunning !== undefined ? this.isCurrentlyRunning : <any>null;
        data["createdAt"] = this.createdAt ? this.createdAt.toString() : <any>null;
        data["updatedAt"] = this.updatedAt ? this.updatedAt.toString() : <any>null;
        return data;
    }
}

export interface ISpecialDetail {
    id?: string | null;
    venueId?: string | null;
    venueName: string;
    content: string;
    type?: SpecialTypes;
    typeName: string;
    startDate: string;
    startTime: string;
    endTime?: string | null;
    expirationDate?: string | null;
    isRecurring?: boolean;
    cronSchedule?: string | null;
    isCurrentlyRunning?: boolean;
    createdAt?: DateTime;
    updatedAt?: DateTime | null;
}

export class SpecialDetailApiResponse implements ISpecialDetailApiResponse {
    success?: boolean;
    message?: string | null;
    data?: SpecialDetail;

    constructor(data?: ISpecialDetailApiResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            this.data = data.data && !(<any>data.data).toJSON ? new SpecialDetail(data.data) : <SpecialDetail>this.data;
        }
    }

    init(_data?: any) {
        if (_data) {
            this.success = _data["success"] !== undefined ? _data["success"] : <any>null;
            this.message = _data["message"] !== undefined ? _data["message"] : <any>null;
            this.data = _data["data"] ? SpecialDetail.fromJS(_data["data"]) : <any>null;
        }
    }

    static fromJS(data: any): SpecialDetailApiResponse {
        data = typeof data === 'object' ? data : {};
        let result = new SpecialDetailApiResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["success"] = this.success !== undefined ? this.success : <any>null;
        data["message"] = this.message !== undefined ? this.message : <any>null;
        data["data"] = this.data ? this.data.toJSON() : <any>null;
        return data;
    }
}

export interface ISpecialDetailApiResponse {
    success?: boolean;
    message?: string | null;
    data?: ISpecialDetail;
}

export class SpecialListItem implements ISpecialListItem {
    id?: string | null;
    venueId?: string | null;
    venueName!: string;
    content!: string;
    type?: SpecialTypes;
    typeName!: string;
    startDate!: string;
    startTime!: string;
    endTime?: string | null;
    isCurrentlyRunning?: boolean;
    isRecurring?: boolean;

    constructor(data?: ISpecialListItem) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"] !== undefined ? _data["id"] : <any>null;
            this.venueId = _data["venueId"] !== undefined ? _data["venueId"] : <any>null;
            this.venueName = _data["venueName"] !== undefined ? _data["venueName"] : <any>null;
            this.content = _data["content"] !== undefined ? _data["content"] : <any>null;
            this.type = _data["type"] !== undefined ? _data["type"] : <any>null;
            this.typeName = _data["typeName"] !== undefined ? _data["typeName"] : <any>null;
            this.startDate = _data["startDate"] !== undefined ? _data["startDate"] : <any>null;
            this.startTime = _data["startTime"] !== undefined ? _data["startTime"] : <any>null;
            this.endTime = _data["endTime"] !== undefined ? _data["endTime"] : <any>null;
            this.isCurrentlyRunning = _data["isCurrentlyRunning"] !== undefined ? _data["isCurrentlyRunning"] : <any>null;
            this.isRecurring = _data["isRecurring"] !== undefined ? _data["isRecurring"] : <any>null;
        }
    }

    static fromJS(data: any): SpecialListItem {
        data = typeof data === 'object' ? data : {};
        let result = new SpecialListItem();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id !== undefined ? this.id : <any>null;
        data["venueId"] = this.venueId !== undefined ? this.venueId : <any>null;
        data["venueName"] = this.venueName !== undefined ? this.venueName : <any>null;
        data["content"] = this.content !== undefined ? this.content : <any>null;
        data["type"] = this.type !== undefined ? this.type : <any>null;
        data["typeName"] = this.typeName !== undefined ? this.typeName : <any>null;
        data["startDate"] = this.startDate !== undefined ? this.startDate : <any>null;
        data["startTime"] = this.startTime !== undefined ? this.startTime : <any>null;
        data["endTime"] = this.endTime !== undefined ? this.endTime : <any>null;
        data["isCurrentlyRunning"] = this.isCurrentlyRunning !== undefined ? this.isCurrentlyRunning : <any>null;
        data["isRecurring"] = this.isRecurring !== undefined ? this.isRecurring : <any>null;
        return data;
    }
}

export interface ISpecialListItem {
    id?: string | null;
    venueId?: string | null;
    venueName: string;
    content: string;
    type?: SpecialTypes;
    typeName: string;
    startDate: string;
    startTime: string;
    endTime?: string | null;
    isCurrentlyRunning?: boolean;
    isRecurring?: boolean;
}

export class SpecialListItemPagedApiResponse implements ISpecialListItemPagedApiResponse {
    success?: boolean;
    message?: string | null;
    data?: SpecialListItem[] | null;
    pagination!: PaginationData;

    constructor(data?: ISpecialListItemPagedApiResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            if (data.data) {
                this.data = [];
                for (let i = 0; i < data.data.length; i++) {
                    let item = data.data[i];
                    this.data[i] = item && !(<any>item).toJSON ? new SpecialListItem(item) : <SpecialListItem>item;
                }
            }
            this.pagination = data.pagination && !(<any>data.pagination).toJSON ? new PaginationData(data.pagination) : <PaginationData>this.pagination;
        }
        if (!data) {
            this.pagination = new PaginationData();
        }
    }

    init(_data?: any) {
        if (_data) {
            this.success = _data["success"] !== undefined ? _data["success"] : <any>null;
            this.message = _data["message"] !== undefined ? _data["message"] : <any>null;
            if (Array.isArray(_data["data"])) {
                this.data = [] as any;
                for (let item of _data["data"])
                    this.data!.push(SpecialListItem.fromJS(item));
            }
            else {
                this.data = <any>null;
            }
            this.pagination = _data["pagination"] ? PaginationData.fromJS(_data["pagination"]) : new PaginationData();
        }
    }

    static fromJS(data: any): SpecialListItemPagedApiResponse {
        data = typeof data === 'object' ? data : {};
        let result = new SpecialListItemPagedApiResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["success"] = this.success !== undefined ? this.success : <any>null;
        data["message"] = this.message !== undefined ? this.message : <any>null;
        if (Array.isArray(this.data)) {
            data["data"] = [];
            for (let item of this.data)
                data["data"].push(item ? item.toJSON() : <any>null);
        }
        data["pagination"] = this.pagination ? this.pagination.toJSON() : <any>null;
        return data;
    }
}

export interface ISpecialListItemPagedApiResponse {
    success?: boolean;
    message?: string | null;
    data?: ISpecialListItem[] | null;
    pagination: IPaginationData;
}

export enum SpecialTypes {
    _0 = 0,
    _1 = 1,
    _2 = 2,
}

export class UpdateAddressRequest implements IUpdateAddressRequest {
    streetAddress!: string;
    secondaryAddress?: string | null;
    locality!: string;
    region!: string;
    postcode!: string;
    country!: string;

    constructor(data?: IUpdateAddressRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.streetAddress = _data["streetAddress"] !== undefined ? _data["streetAddress"] : <any>null;
            this.secondaryAddress = _data["secondaryAddress"] !== undefined ? _data["secondaryAddress"] : <any>null;
            this.locality = _data["locality"] !== undefined ? _data["locality"] : <any>null;
            this.region = _data["region"] !== undefined ? _data["region"] : <any>null;
            this.postcode = _data["postcode"] !== undefined ? _data["postcode"] : <any>null;
            this.country = _data["country"] !== undefined ? _data["country"] : <any>null;
        }
    }

    static fromJS(data: any): UpdateAddressRequest {
        data = typeof data === 'object' ? data : {};
        let result = new UpdateAddressRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["streetAddress"] = this.streetAddress !== undefined ? this.streetAddress : <any>null;
        data["secondaryAddress"] = this.secondaryAddress !== undefined ? this.secondaryAddress : <any>null;
        data["locality"] = this.locality !== undefined ? this.locality : <any>null;
        data["region"] = this.region !== undefined ? this.region : <any>null;
        data["postcode"] = this.postcode !== undefined ? this.postcode : <any>null;
        data["country"] = this.country !== undefined ? this.country : <any>null;
        return data;
    }
}

export interface IUpdateAddressRequest {
    streetAddress: string;
    secondaryAddress?: string | null;
    locality: string;
    region: string;
    postcode: string;
    country: string;
}

export class UpdateOperatingScheduleRequest implements IUpdateOperatingScheduleRequest {
    timeOfOpen!: string;
    timeOfClose!: string;
    isClosed!: boolean;

    constructor(data?: IUpdateOperatingScheduleRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.timeOfOpen = _data["timeOfOpen"] !== undefined ? _data["timeOfOpen"] : <any>null;
            this.timeOfClose = _data["timeOfClose"] !== undefined ? _data["timeOfClose"] : <any>null;
            this.isClosed = _data["isClosed"] !== undefined ? _data["isClosed"] : <any>null;
        }
    }

    static fromJS(data: any): UpdateOperatingScheduleRequest {
        data = typeof data === 'object' ? data : {};
        let result = new UpdateOperatingScheduleRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["timeOfOpen"] = this.timeOfOpen !== undefined ? this.timeOfOpen : <any>null;
        data["timeOfClose"] = this.timeOfClose !== undefined ? this.timeOfClose : <any>null;
        data["isClosed"] = this.isClosed !== undefined ? this.isClosed : <any>null;
        return data;
    }
}

export interface IUpdateOperatingScheduleRequest {
    timeOfOpen: string;
    timeOfClose: string;
    isClosed: boolean;
}

export class UpdateSpecialRequest implements IUpdateSpecialRequest {
    content!: string;
    type!: SpecialTypes;
    startDate!: string;
    startTime!: string;
    endTime?: string | null;
    expirationDate?: string | null;
    isRecurring!: boolean;
    cronSchedule?: string | null;

    constructor(data?: IUpdateSpecialRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.content = _data["content"] !== undefined ? _data["content"] : <any>null;
            this.type = _data["type"] !== undefined ? _data["type"] : <any>null;
            this.startDate = _data["startDate"] !== undefined ? _data["startDate"] : <any>null;
            this.startTime = _data["startTime"] !== undefined ? _data["startTime"] : <any>null;
            this.endTime = _data["endTime"] !== undefined ? _data["endTime"] : <any>null;
            this.expirationDate = _data["expirationDate"] !== undefined ? _data["expirationDate"] : <any>null;
            this.isRecurring = _data["isRecurring"] !== undefined ? _data["isRecurring"] : <any>null;
            this.cronSchedule = _data["cronSchedule"] !== undefined ? _data["cronSchedule"] : <any>null;
        }
    }

    static fromJS(data: any): UpdateSpecialRequest {
        data = typeof data === 'object' ? data : {};
        let result = new UpdateSpecialRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["content"] = this.content !== undefined ? this.content : <any>null;
        data["type"] = this.type !== undefined ? this.type : <any>null;
        data["startDate"] = this.startDate !== undefined ? this.startDate : <any>null;
        data["startTime"] = this.startTime !== undefined ? this.startTime : <any>null;
        data["endTime"] = this.endTime !== undefined ? this.endTime : <any>null;
        data["expirationDate"] = this.expirationDate !== undefined ? this.expirationDate : <any>null;
        data["isRecurring"] = this.isRecurring !== undefined ? this.isRecurring : <any>null;
        data["cronSchedule"] = this.cronSchedule !== undefined ? this.cronSchedule : <any>null;
        return data;
    }
}

export interface IUpdateSpecialRequest {
    content: string;
    type: SpecialTypes;
    startDate: string;
    startTime: string;
    endTime?: string | null;
    expirationDate?: string | null;
    isRecurring: boolean;
    cronSchedule?: string | null;
}

export class UpdateVenueRequest implements IUpdateVenueRequest {
    name!: string;
    description?: string | null;
    phoneNumber?: string | null;
    website?: string | null;
    email?: string | null;
    profileImage?: string | null;
    address!: UpdateAddressRequest;

    constructor(data?: IUpdateVenueRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            this.address = data.address && !(<any>data.address).toJSON ? new UpdateAddressRequest(data.address) : <UpdateAddressRequest>this.address;
        }
        if (!data) {
            this.address = new UpdateAddressRequest();
        }
    }

    init(_data?: any) {
        if (_data) {
            this.name = _data["name"] !== undefined ? _data["name"] : <any>null;
            this.description = _data["description"] !== undefined ? _data["description"] : <any>null;
            this.phoneNumber = _data["phoneNumber"] !== undefined ? _data["phoneNumber"] : <any>null;
            this.website = _data["website"] !== undefined ? _data["website"] : <any>null;
            this.email = _data["email"] !== undefined ? _data["email"] : <any>null;
            this.profileImage = _data["profileImage"] !== undefined ? _data["profileImage"] : <any>null;
            this.address = _data["address"] ? UpdateAddressRequest.fromJS(_data["address"]) : new UpdateAddressRequest();
        }
    }

    static fromJS(data: any): UpdateVenueRequest {
        data = typeof data === 'object' ? data : {};
        let result = new UpdateVenueRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name !== undefined ? this.name : <any>null;
        data["description"] = this.description !== undefined ? this.description : <any>null;
        data["phoneNumber"] = this.phoneNumber !== undefined ? this.phoneNumber : <any>null;
        data["website"] = this.website !== undefined ? this.website : <any>null;
        data["email"] = this.email !== undefined ? this.email : <any>null;
        data["profileImage"] = this.profileImage !== undefined ? this.profileImage : <any>null;
        data["address"] = this.address ? this.address.toJSON() : <any>null;
        return data;
    }
}

export interface IUpdateVenueRequest {
    name: string;
    description?: string | null;
    phoneNumber?: string | null;
    website?: string | null;
    email?: string | null;
    profileImage?: string | null;
    address: IUpdateAddressRequest;
}

export class VenueDetail implements IVenueDetail {
    id?: string | null;
    name!: string;
    description?: string | null;
    phoneNumber?: string | null;
    website?: string | null;
    email?: string | null;
    profileImage?: string | null;
    streetAddress!: string;
    secondaryAddress?: string | null;
    locality!: string;
    region!: string;
    postcode!: string;
    country!: string;
    latitude?: number | null;
    longitude?: number | null;
    businessHours!: OperatingScheduleListItem[];
    createdAt?: DateTime;
    updatedAt?: DateTime | null;

    constructor(data?: IVenueDetail) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            if (data.businessHours) {
                this.businessHours = [];
                for (let i = 0; i < data.businessHours.length; i++) {
                    let item = data.businessHours[i];
                    this.businessHours[i] = item && !(<any>item).toJSON ? new OperatingScheduleListItem(item) : <OperatingScheduleListItem>item;
                }
            }
        }
        if (!data) {
            this.businessHours = [];
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"] !== undefined ? _data["id"] : <any>null;
            this.name = _data["name"] !== undefined ? _data["name"] : <any>null;
            this.description = _data["description"] !== undefined ? _data["description"] : <any>null;
            this.phoneNumber = _data["phoneNumber"] !== undefined ? _data["phoneNumber"] : <any>null;
            this.website = _data["website"] !== undefined ? _data["website"] : <any>null;
            this.email = _data["email"] !== undefined ? _data["email"] : <any>null;
            this.profileImage = _data["profileImage"] !== undefined ? _data["profileImage"] : <any>null;
            this.streetAddress = _data["streetAddress"] !== undefined ? _data["streetAddress"] : <any>null;
            this.secondaryAddress = _data["secondaryAddress"] !== undefined ? _data["secondaryAddress"] : <any>null;
            this.locality = _data["locality"] !== undefined ? _data["locality"] : <any>null;
            this.region = _data["region"] !== undefined ? _data["region"] : <any>null;
            this.postcode = _data["postcode"] !== undefined ? _data["postcode"] : <any>null;
            this.country = _data["country"] !== undefined ? _data["country"] : <any>null;
            this.latitude = _data["latitude"] !== undefined ? _data["latitude"] : <any>null;
            this.longitude = _data["longitude"] !== undefined ? _data["longitude"] : <any>null;
            if (Array.isArray(_data["businessHours"])) {
                this.businessHours = [] as any;
                for (let item of _data["businessHours"])
                    this.businessHours!.push(OperatingScheduleListItem.fromJS(item));
            }
            else {
                this.businessHours = <any>null;
            }
            this.createdAt = _data["createdAt"] ? DateTime.fromISO(_data["createdAt"].toString()) : <any>null;
            this.updatedAt = _data["updatedAt"] ? DateTime.fromISO(_data["updatedAt"].toString()) : <any>null;
        }
    }

    static fromJS(data: any): VenueDetail {
        data = typeof data === 'object' ? data : {};
        let result = new VenueDetail();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id !== undefined ? this.id : <any>null;
        data["name"] = this.name !== undefined ? this.name : <any>null;
        data["description"] = this.description !== undefined ? this.description : <any>null;
        data["phoneNumber"] = this.phoneNumber !== undefined ? this.phoneNumber : <any>null;
        data["website"] = this.website !== undefined ? this.website : <any>null;
        data["email"] = this.email !== undefined ? this.email : <any>null;
        data["profileImage"] = this.profileImage !== undefined ? this.profileImage : <any>null;
        data["streetAddress"] = this.streetAddress !== undefined ? this.streetAddress : <any>null;
        data["secondaryAddress"] = this.secondaryAddress !== undefined ? this.secondaryAddress : <any>null;
        data["locality"] = this.locality !== undefined ? this.locality : <any>null;
        data["region"] = this.region !== undefined ? this.region : <any>null;
        data["postcode"] = this.postcode !== undefined ? this.postcode : <any>null;
        data["country"] = this.country !== undefined ? this.country : <any>null;
        data["latitude"] = this.latitude !== undefined ? this.latitude : <any>null;
        data["longitude"] = this.longitude !== undefined ? this.longitude : <any>null;
        if (Array.isArray(this.businessHours)) {
            data["businessHours"] = [];
            for (let item of this.businessHours)
                data["businessHours"].push(item ? item.toJSON() : <any>null);
        }
        data["createdAt"] = this.createdAt ? this.createdAt.toString() : <any>null;
        data["updatedAt"] = this.updatedAt ? this.updatedAt.toString() : <any>null;
        return data;
    }
}

export interface IVenueDetail {
    id?: string | null;
    name: string;
    description?: string | null;
    phoneNumber?: string | null;
    website?: string | null;
    email?: string | null;
    profileImage?: string | null;
    streetAddress: string;
    secondaryAddress?: string | null;
    locality: string;
    region: string;
    postcode: string;
    country: string;
    latitude?: number | null;
    longitude?: number | null;
    businessHours: IOperatingScheduleListItem[];
    createdAt?: DateTime;
    updatedAt?: DateTime | null;
}

export class VenueDetailApiResponse implements IVenueDetailApiResponse {
    success?: boolean;
    message?: string | null;
    data?: VenueDetail;

    constructor(data?: IVenueDetailApiResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            this.data = data.data && !(<any>data.data).toJSON ? new VenueDetail(data.data) : <VenueDetail>this.data;
        }
    }

    init(_data?: any) {
        if (_data) {
            this.success = _data["success"] !== undefined ? _data["success"] : <any>null;
            this.message = _data["message"] !== undefined ? _data["message"] : <any>null;
            this.data = _data["data"] ? VenueDetail.fromJS(_data["data"]) : <any>null;
        }
    }

    static fromJS(data: any): VenueDetailApiResponse {
        data = typeof data === 'object' ? data : {};
        let result = new VenueDetailApiResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["success"] = this.success !== undefined ? this.success : <any>null;
        data["message"] = this.message !== undefined ? this.message : <any>null;
        data["data"] = this.data ? this.data.toJSON() : <any>null;
        return data;
    }
}

export interface IVenueDetailApiResponse {
    success?: boolean;
    message?: string | null;
    data?: IVenueDetail;
}

export class VenueListItem implements IVenueListItem {
    id?: string | null;
    name!: string;
    description?: string | null;
    locality!: string;
    region!: string;
    profileImage?: string | null;
    latitude?: number | null;
    longitude?: number | null;

    constructor(data?: IVenueListItem) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"] !== undefined ? _data["id"] : <any>null;
            this.name = _data["name"] !== undefined ? _data["name"] : <any>null;
            this.description = _data["description"] !== undefined ? _data["description"] : <any>null;
            this.locality = _data["locality"] !== undefined ? _data["locality"] : <any>null;
            this.region = _data["region"] !== undefined ? _data["region"] : <any>null;
            this.profileImage = _data["profileImage"] !== undefined ? _data["profileImage"] : <any>null;
            this.latitude = _data["latitude"] !== undefined ? _data["latitude"] : <any>null;
            this.longitude = _data["longitude"] !== undefined ? _data["longitude"] : <any>null;
        }
    }

    static fromJS(data: any): VenueListItem {
        data = typeof data === 'object' ? data : {};
        let result = new VenueListItem();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id !== undefined ? this.id : <any>null;
        data["name"] = this.name !== undefined ? this.name : <any>null;
        data["description"] = this.description !== undefined ? this.description : <any>null;
        data["locality"] = this.locality !== undefined ? this.locality : <any>null;
        data["region"] = this.region !== undefined ? this.region : <any>null;
        data["profileImage"] = this.profileImage !== undefined ? this.profileImage : <any>null;
        data["latitude"] = this.latitude !== undefined ? this.latitude : <any>null;
        data["longitude"] = this.longitude !== undefined ? this.longitude : <any>null;
        return data;
    }
}

export interface IVenueListItem {
    id?: string | null;
    name: string;
    description?: string | null;
    locality: string;
    region: string;
    profileImage?: string | null;
    latitude?: number | null;
    longitude?: number | null;
}

export class VenueListItemPagedApiResponse implements IVenueListItemPagedApiResponse {
    success?: boolean;
    message?: string | null;
    data?: VenueListItem[] | null;
    pagination!: PaginationData;

    constructor(data?: IVenueListItemPagedApiResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            if (data.data) {
                this.data = [];
                for (let i = 0; i < data.data.length; i++) {
                    let item = data.data[i];
                    this.data[i] = item && !(<any>item).toJSON ? new VenueListItem(item) : <VenueListItem>item;
                }
            }
            this.pagination = data.pagination && !(<any>data.pagination).toJSON ? new PaginationData(data.pagination) : <PaginationData>this.pagination;
        }
        if (!data) {
            this.pagination = new PaginationData();
        }
    }

    init(_data?: any) {
        if (_data) {
            this.success = _data["success"] !== undefined ? _data["success"] : <any>null;
            this.message = _data["message"] !== undefined ? _data["message"] : <any>null;
            if (Array.isArray(_data["data"])) {
                this.data = [] as any;
                for (let item of _data["data"])
                    this.data!.push(VenueListItem.fromJS(item));
            }
            else {
                this.data = <any>null;
            }
            this.pagination = _data["pagination"] ? PaginationData.fromJS(_data["pagination"]) : new PaginationData();
        }
    }

    static fromJS(data: any): VenueListItemPagedApiResponse {
        data = typeof data === 'object' ? data : {};
        let result = new VenueListItemPagedApiResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["success"] = this.success !== undefined ? this.success : <any>null;
        data["message"] = this.message !== undefined ? this.message : <any>null;
        if (Array.isArray(this.data)) {
            data["data"] = [];
            for (let item of this.data)
                data["data"].push(item ? item.toJSON() : <any>null);
        }
        data["pagination"] = this.pagination ? this.pagination.toJSON() : <any>null;
        return data;
    }
}

export interface IVenueListItemPagedApiResponse {
    success?: boolean;
    message?: string | null;
    data?: IVenueListItem[] | null;
    pagination: IPaginationData;
}

export class VenueSpecials implements IVenueSpecials {
    venueId?: string | null;
    venueName!: string;
    specials!: SpecialListItem[];

    constructor(data?: IVenueSpecials) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            if (data.specials) {
                this.specials = [];
                for (let i = 0; i < data.specials.length; i++) {
                    let item = data.specials[i];
                    this.specials[i] = item && !(<any>item).toJSON ? new SpecialListItem(item) : <SpecialListItem>item;
                }
            }
        }
        if (!data) {
            this.specials = [];
        }
    }

    init(_data?: any) {
        if (_data) {
            this.venueId = _data["venueId"] !== undefined ? _data["venueId"] : <any>null;
            this.venueName = _data["venueName"] !== undefined ? _data["venueName"] : <any>null;
            if (Array.isArray(_data["specials"])) {
                this.specials = [] as any;
                for (let item of _data["specials"])
                    this.specials!.push(SpecialListItem.fromJS(item));
            }
            else {
                this.specials = <any>null;
            }
        }
    }

    static fromJS(data: any): VenueSpecials {
        data = typeof data === 'object' ? data : {};
        let result = new VenueSpecials();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["venueId"] = this.venueId !== undefined ? this.venueId : <any>null;
        data["venueName"] = this.venueName !== undefined ? this.venueName : <any>null;
        if (Array.isArray(this.specials)) {
            data["specials"] = [];
            for (let item of this.specials)
                data["specials"].push(item ? item.toJSON() : <any>null);
        }
        return data;
    }
}

export interface IVenueSpecials {
    venueId?: string | null;
    venueName: string;
    specials: ISpecialListItem[];
}

export class VenueSpecialsApiResponse implements IVenueSpecialsApiResponse {
    success?: boolean;
    message?: string | null;
    data?: VenueSpecials;

    constructor(data?: IVenueSpecialsApiResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            this.data = data.data && !(<any>data.data).toJSON ? new VenueSpecials(data.data) : <VenueSpecials>this.data;
        }
    }

    init(_data?: any) {
        if (_data) {
            this.success = _data["success"] !== undefined ? _data["success"] : <any>null;
            this.message = _data["message"] !== undefined ? _data["message"] : <any>null;
            this.data = _data["data"] ? VenueSpecials.fromJS(_data["data"]) : <any>null;
        }
    }

    static fromJS(data: any): VenueSpecialsApiResponse {
        data = typeof data === 'object' ? data : {};
        let result = new VenueSpecialsApiResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["success"] = this.success !== undefined ? this.success : <any>null;
        data["message"] = this.message !== undefined ? this.message : <any>null;
        data["data"] = this.data ? this.data.toJSON() : <any>null;
        return data;
    }
}

export interface IVenueSpecialsApiResponse {
    success?: boolean;
    message?: string | null;
    data?: IVenueSpecials;
}

export class ApiResponse<TResult> {
    status: number;
    headers: { [key: string]: any; };
    result: TResult;

    constructor(status: number, headers: { [key: string]: any; }, result: TResult)
    {
        this.status = status;
        this.headers = headers;
        this.result = result;
    }
}

export class ApiException extends Error {
    override message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    if (result !== null && result !== undefined)
        throw result;
    else
        throw new ApiException(message, status, response, headers, null);
}