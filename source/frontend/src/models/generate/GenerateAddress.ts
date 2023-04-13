import { Api_Address } from 'models/api/Address';
export class GenerateAddress {
  line_1: string;
  line_2: string;
  line_3: string;
  city: string;
  province: string;
  postal: string;
  country: string;
  address_string: string;

  constructor(address: Api_Address | null) {
    this.line_1 = address?.streetAddress1 ?? '';
    this.line_2 = address?.streetAddress2 ?? '';
    this.line_3 = address?.streetAddress3 ?? '';
    this.city = address?.municipality ?? '';
    this.province = address?.province?.description ?? '';
    this.postal = address?.postal ?? '';
    this.country = address?.country?.description ?? '';
    this.address_string = [
      this.line_1,
      this.line_2,
      this.line_3,
      this.city,
      this.province,
      this.postal,
      this.country,
    ]
      .filter(a => !!a)
      .join('\n');
  }
}
