import { Api_Geometry, Api_Property } from '@/models/api/Property';
import { pidFormatter } from '@/utils';

import { Api_GenerateAddress } from './GenerateAddress';
export class Api_GenerateProperty {
  location: Api_Geometry | null;
  pid: string;
  legal_description: string;
  region: string;
  address: Api_GenerateAddress | null;
  location_of_land: string;
  district: string;
  electoral_dist: string;

  constructor(property: Api_Property | null | undefined) {
    this.location =
      property?.location !== null && property?.location !== undefined ? property.location : null;
    this.pid = pidFormatter(property?.pid?.toString()) ?? '';
    this.legal_description = property?.landLegalDescription ?? '';
    this.address =
      property?.address !== null && property?.address !== undefined
        ? new Api_GenerateAddress(property.address)
        : null;
    this.region = property?.region?.description ?? '';
    this.district = property?.district?.description ?? '';
    this.electoral_dist = '';
    this.location_of_land = property?.generalLocation ?? '';
  }
}
