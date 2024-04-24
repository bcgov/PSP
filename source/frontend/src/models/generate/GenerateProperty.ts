import { exists, pidFormatter } from '@/utils';

import { ApiGen_Concepts_Geometry } from '../api/generated/ApiGen_Concepts_Geometry';
import { ApiGen_Concepts_Property } from '../api/generated/ApiGen_Concepts_Property';
import { Api_GenerateAddress } from './GenerateAddress';
export class Api_GenerateProperty {
  location: ApiGen_Concepts_Geometry | null;
  pid: string;
  legal_desc: string;
  region: string;
  address: Api_GenerateAddress | null;
  location_of_land: string;
  district: string;
  electoral_dist: string;

  constructor(property: ApiGen_Concepts_Property | null | undefined) {
    this.location = exists(property?.location) ? property!.location : null;
    this.pid = pidFormatter(property?.pid?.toString()) ?? '';
    this.legal_desc = property?.landLegalDescription ?? '';
    this.address = exists(property?.address) ? new Api_GenerateAddress(property!.address) : null;
    this.region = property?.region?.description ?? '';
    this.district = property?.district?.description ?? '';
    this.electoral_dist = '';
    this.location_of_land = property?.generalLocation ?? '';
  }
}
