export interface IAddress {
    streetAddress: string;
    secondaryAddress?: string | null;
    locality: string;
    region: string;
    postcode: string;
    country: string;
}

export class Address implements IAddress {
  id?: string;
  streetAddress: string;
  secondaryAddress?: string | null;
  locality: string;
  region: string;
  postcode: string;
  country: string;

  constructor(data: Partial<Address> = {}) {
    this.id = data.id;
    this.streetAddress = data.streetAddress || '';
    this.secondaryAddress = data.secondaryAddress;
    this.locality = data.locality || '';
    this.region = data.region || '';
    this.postcode = data.postcode || '';
    this.country = data.country || '';
  }

  static fromResponse(response: IAddress & { id?: string }): Address {
    return new Address({
      id: response.id,
      streetAddress: response.streetAddress,
      secondaryAddress: response.secondaryAddress,
      locality: response.locality,
      region: response.region,
      postcode: response.postcode,
      country: response.country
    });
  }

  getFormattedAddress(): string {
    const parts = [
      this.streetAddress,
      this.secondaryAddress,
      this.locality,
      `${this.region} ${this.postcode}`,
      this.country
    ].filter(part => part && part.trim().length > 0);
    
    return parts.join(', ');
  }
}