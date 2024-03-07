import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { exists } from '@/utils';

import { Api_GenerateOrganization } from '../GenerateOrganization';
import { Api_GenerateProperty } from '../GenerateProperty';
import { Api_GenerateAcquisitionFile } from './GenerateAcquisitionFile';
import { Api_GenerateInterestHolder } from './GenerateInterestHolder';

export interface IApiGenerateExpropriationFormBaseInput {
  file: Api_GenerateAcquisitionFile | null;
  interestHolders?: ApiGen_Concepts_InterestHolder[];
  expropriationAuthority: ApiGen_Concepts_Organization | null;
  impactedProperties?: ApiGen_Concepts_AcquisitionFileProperty[];
}

export class Api_GenerateExpropriationFormBase {
  file: Api_GenerateAcquisitionFile | null;
  exp_authority: Api_GenerateOrganization | null;
  impacted_properties: Api_GenerateProperty[];
  impacted_interest_holders: Api_GenerateInterestHolder[];

  constructor({
    file,
    interestHolders = [],
    expropriationAuthority,
    impactedProperties = [],
  }: IApiGenerateExpropriationFormBaseInput) {
    this.file = file;
    this.exp_authority = new Api_GenerateOrganization(expropriationAuthority);
    this.impacted_properties = impactedProperties.map(fp => new Api_GenerateProperty(fp?.property));
    const filePropertyIds = new Set(impactedProperties.map(fp => fp?.id).filter(exists));
    const matchingInterestHolders = interestHolders.filter(ih =>
      ih?.interestHolderProperties?.some(
        ihp =>
          ihp?.propertyInterestTypes?.some(pit => pit.id !== 'NIP') &&
          filePropertyIds.has(Number(ihp.acquisitionFilePropertyId)),
      ),
    );
    this.impacted_interest_holders = matchingInterestHolders.map(
      ih => new Api_GenerateInterestHolder(ih),
    );
  }
}
