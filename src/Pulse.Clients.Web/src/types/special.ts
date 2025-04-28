import { SpecialStatus } from './special-status';

/**
 * Basic special information
 */
export type Special = {
  id: number;
  title: string;
  venue: string;
  location: string;
  tag: string;
  image?: string;
  status: SpecialStatus;
  views?: number;
  scheduledDate?: string;
};