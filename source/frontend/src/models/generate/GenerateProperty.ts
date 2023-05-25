import { Api_Property } from 'models/api/Property';
import { pidFormatter } from 'utils';

import { Api_GenerateAddress } from './GenerateAddress';
export class Api_GenerateProperty {
  pid: string;
  legal_description: string;
  region: string;
  address: Api_GenerateAddress | null;
  location_of_land: string;
  district: string;

  constructor(property: Api_Property | null | undefined) {
    this.pid = pidFormatter(property?.pid?.toString()) ?? '';
    this.legal_description = property?.landLegalDescription ?? '';
    this.address =
      property?.address !== null && property?.address !== undefined
        ? new Api_GenerateAddress(property.address)
        : null;
    this.region = property?.region?.description ?? '';
    this.location_of_land = ''; // TODO
    this.district = property?.district?.description ?? '';
  }
}
