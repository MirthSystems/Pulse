import { DateTime } from 'luxon';
import { DayOfWeek } from '../enums';

export interface IOperatingSchedule {
  timeOfOpen: string;
  timeOfClose: string;
  isClosed: boolean;
  venueId?: string;
  dayOfWeek?: DayOfWeek;
  id?: string;
}

export interface IOperatingScheduleResponse {
  id: string;
  venueId: string;
  dayOfWeek: DayOfWeek;
  timeOfOpen: string;
  timeOfClose: string;
  isClosed: boolean;
}

export class OperatingSchedule implements IOperatingSchedule {
  id?: string;
  venueId: string;
  dayOfWeek: DayOfWeek;
  timeOfOpen: string;
  timeOfClose: string;
  isClosed: boolean;

  private _openTime?: DateTime;
  private _closeTime?: DateTime;

  constructor(data: Partial<OperatingSchedule> = {}) {
    this.id = data.id;
    this.venueId = data.venueId || '';
    this.dayOfWeek = data.dayOfWeek !== undefined ? data.dayOfWeek : DayOfWeek.Sunday;
    this.timeOfOpen = data.timeOfOpen || '09:00';
    this.timeOfClose = data.timeOfClose || '17:00';
    this.isClosed = data.isClosed !== undefined ? data.isClosed : false;
  }

  static fromResponse(response: IOperatingScheduleResponse): OperatingSchedule {
    return new OperatingSchedule({
      id: response.id,
      venueId: response.venueId,
      dayOfWeek: response.dayOfWeek,
      timeOfOpen: response.timeOfOpen,
      timeOfClose: response.timeOfClose,
      isClosed: response.isClosed
    });
  }

  get dayName(): string {
    return DayOfWeek[this.dayOfWeek];
  }

  get openTime(): DateTime | undefined {
    if (!this._openTime && this.timeOfOpen) {
      this._openTime = DateTime.fromFormat(this.timeOfOpen, 'HH:mm');
    }
    return this._openTime;
  }

  get closeTime(): DateTime | undefined {
    if (!this._closeTime && this.timeOfClose) {
      this._closeTime = DateTime.fromFormat(this.timeOfClose, 'HH:mm');
    }
    return this._closeTime;
  }

  setOpenTime(time: DateTime): void {
    this._openTime = time;
    this.timeOfOpen = time.toFormat('HH:mm');
  }

  setCloseTime(time: DateTime): void {
    this._closeTime = time;
    this.timeOfClose = time.toFormat('HH:mm');
  }

  isOpenAt(dateTime: DateTime): boolean {
    if (this.isClosed) {
      return false;
    }

    if (dateTime.weekday % 7 !== this.dayOfWeek) {
      return false;
    }

    const timeString = dateTime.toFormat('HH:mm');
    return timeString >= this.timeOfOpen && timeString <= this.timeOfClose;
  }

  getFormattedSchedule(): string {
    if (this.isClosed) {
      return `${this.dayName}: Closed`;
    }
    return `${this.dayName}: ${this.timeOfOpen} - ${this.timeOfClose}`;
  }
}