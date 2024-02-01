import { ApiGen_Concepts_Geometry } from '@/models/api/generated/ApiGen_Concepts_Geometry';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { exists, pidFormatter } from '@/utils';

import { Api_GenerateAddress } from '../GenerateAddress';
import { Api_GenerateH120InterestHolder } from './GenerateH120InterestHolder';

export class Api_GenerateH120Property {
  location: ApiGen_Concepts_Geometry | null;
  pid: string;
  legal_description: string;
  region: string;
  address: Api_GenerateAddress | null;
  location_of_land: string;
  district: string;
  interest_holders_string: string;
  electoral_dist: string;

  constructor(
    property: ApiGen_Concepts_Property | null | undefined,
    interestHolders: Api_GenerateH120InterestHolder[],
  ) {
    this.location = exists(property?.location) ? property!.location : null;
    this.pid = pidFormatter(property?.pid?.toString()) ?? '';
    this.legal_description = property?.landLegalDescription ?? '';
    this.address = exists(property?.address) ? new Api_GenerateAddress(property!.address) : null;
    this.region = property?.region?.description ?? '';
    this.location_of_land = property?.generalLocation ?? '';
    this.district = property?.district?.description ?? '';
    this.interest_holders_string =
      interestHolders?.map(ih => ih.interestHolderString).join(', ') ?? [];
    this.electoral_dist = '';
  }
}
