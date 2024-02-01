import { ApiGen_Concepts_Address } from '../api/generated/ApiGen_Concepts_Address';
export class Api_GenerateAddress {
  line_1: string;
  line_2: string;
  line_3: string;
  city: string;
  province: string;
  postal: string;
  country: string;
  address_string: string;
  address_string_multiline_compressed: string;
  address_single_line_string: string;

  constructor(address: ApiGen_Concepts_Address | null) {
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

    this.address_single_line_string = [
      this.line_1,
      this.line_2,
      this.line_3,
      this.city,
      this.province,
      this.postal,
      this.country,
    ]
      .filter(a => !!a)
      .join(', ');

    this.address_string_multiline_compressed =
      [this.line_1, this.line_2, this.line_3].filter(a => !!a).join('\n') +
      '\n' +
      [this.city, this.province, this.postal, this.country].filter(a => !!a).join(', ');
  }
}
