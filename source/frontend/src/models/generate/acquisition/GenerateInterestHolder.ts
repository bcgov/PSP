import { Api_InterestHolder, Api_InterestHolderProperty } from '@/models/api/InterestHolder';
import { formatNames } from '@/utils/personUtils';

export class Api_GenerateInterestHolder {
  interestHolderName: string;
  interestHolderType: string;
  interestHolderString: string;

  constructor(
    interestHolder: Api_InterestHolder | null,
    matchingInterestHolderProperty: Api_InterestHolderProperty,
  ) {
    this.interestHolderName = interestHolder?.person
      ? formatNames([
          interestHolder.person.firstName,
          interestHolder.person.middleNames,
          interestHolder.person.surname,
        ])
      : interestHolder?.organization?.name ?? '';
    this.interestHolderType = matchingInterestHolderProperty.interestTypeCode?.description ?? '';
    this.interestHolderString = `${this.interestHolderName}: ${this.interestHolderType}`;
  }
}
