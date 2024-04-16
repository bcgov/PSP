import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_InterestHolderProperty } from '@/models/api/generated/ApiGen_Concepts_InterestHolderProperty';
import { formatNames } from '@/utils/personUtils';

export class Api_GenerateH120InterestHolder {
  interestHolderName: string;
  interestHolderType: string;
  interestHolderString: string;

  constructor(
    interestHolder: ApiGen_Concepts_InterestHolder | null,
    matchingInterestHolderProperty: ApiGen_Concepts_InterestHolderProperty,
    interestTypeCode: ApiGen_Base_CodeType<string>,
  ) {
    this.interestHolderName = interestHolder?.person
      ? formatNames([
          interestHolder.person.firstName,
          interestHolder.person.middleNames,
          interestHolder.person.surname,
        ])
      : interestHolder?.organization?.name ?? '';

    this.interestHolderType = interestTypeCode.description ?? '';
    this.interestHolderString = `${this.interestHolderName}: ${this.interestHolderType}`;
  }
}
