import { Api_InterestHolder, Api_InterestHolderProperty } from '@/models/api/InterestHolder';
import Api_TypeCode from '@/models/api/TypeCode';
import { formatNames } from '@/utils/personUtils';

export class Api_GenerateH120InterestHolder {
  interestHolderName: string;
  interestHolderType: string;
  interestHolderString: string;

  constructor(
    interestHolder: Api_InterestHolder | null,
    matchingInterestHolderProperty: Api_InterestHolderProperty,
    interestTypeCode: Api_TypeCode<string>,
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
