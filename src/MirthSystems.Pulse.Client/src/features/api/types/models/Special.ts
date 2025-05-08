import { DateTime } from 'luxon';
import { SpecialTypes } from '../enums';

export interface ISpecial {
    content: string;
    type: SpecialTypes;
    startDate: string;
    startTime: string;
    endTime?: string | null;
    expirationDate?: string | null;
    isRecurring: boolean;
    cronSchedule?: string | null;
    venueId?: string;
    id?: string;
}
  
export interface ISpecialResponse {
    id: string;
    venueId: string;
    venueName?: string;
    content: string;
    type: SpecialTypes;
    startDate: string;
    startTime: string;
    endTime?: string | null;
    expirationDate?: string | null;
    isRecurring: boolean;
    cronSchedule?: string | null;
}

export class Special implements ISpecial {
  id?: string;
  venueId: string;
  venueName?: string;
  content: string;
  type: SpecialTypes;
  startDate: string;
  startTime: string;
  endTime?: string | null;
  expirationDate?: string | null;
  isRecurring: boolean;
  cronSchedule?: string | null;

  private _startDateTime?: DateTime;
  private _expirationDateTime?: DateTime | null;

  constructor(data: Partial<Special> = {}) {
    this.id = data.id;
    this.venueId = data.venueId || '';
    this.venueName = data.venueName;
    this.content = data.content || '';
    this.type = data.type !== undefined ? data.type : SpecialTypes.Food;
    this.startDate = data.startDate || '';
    this.startTime = data.startTime || '';
    this.endTime = data.endTime;
    this.expirationDate = data.expirationDate;
    this.isRecurring = data.isRecurring !== undefined ? data.isRecurring : false;
    this.cronSchedule = data.cronSchedule;
  }

  static fromResponse(response: ISpecialResponse): Special {
    return new Special({
      id: response.id,
      venueId: response.venueId,
      venueName: response.venueName,
      content: response.content,
      type: response.type,
      startDate: response.startDate,
      startTime: response.startTime,
      endTime: response.endTime,
      expirationDate: response.expirationDate,
      isRecurring: response.isRecurring,
      cronSchedule: response.cronSchedule
    });
  }

  get startDateTime(): DateTime | undefined {
    if (!this._startDateTime && this.startDate && this.startTime) {
      this._startDateTime = DateTime.fromFormat(
        `${this.startDate} ${this.startTime}`,
        'yyyy-MM-dd HH:mm'
      );
    }
    return this._startDateTime;
  }

  get expirationDateTime(): DateTime | null | undefined {
    if (this._expirationDateTime === undefined && this.expirationDate) {
      this._expirationDateTime = DateTime.fromFormat(this.expirationDate, 'yyyy-MM-dd');
    } else if (this.expirationDate === null) {
      this._expirationDateTime = null;
    }
    return this._expirationDateTime;
  }

  setStartDateTime(dateTime: DateTime): void {
    this._startDateTime = dateTime;
    this.startDate = dateTime.toFormat('yyyy-MM-dd');
    this.startTime = dateTime.toFormat('HH:mm');
  }

  setExpirationDate(dateTime: DateTime | null): void {
    this._expirationDateTime = dateTime;
    this.expirationDate = dateTime ? dateTime.toFormat('yyyy-MM-dd') : null;
  }

  isActive(currentDateTime: DateTime = DateTime.now()): boolean {
    if (this.expirationDateTime && this.expirationDateTime < currentDateTime) {
      return false;
    }

    if (!this.isRecurring && this.startDateTime) {
      return this.startDateTime.hasSame(currentDateTime, 'day');
    }

    return true;
  }
}