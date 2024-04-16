import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';

import { Api_GenerateProperty } from '../GenerateProperty';

export class Api_GenerateLeaseProperty {
  lease_area: string;
  property: Api_GenerateProperty;
  constructor(propertyLease: ApiGen_Concepts_PropertyLease) {
    this.lease_area = propertyLease.leaseArea
      ? (propertyLease.leaseArea?.toString() ?? '') +
        ' ' +
        (propertyLease?.areaUnitType?.description ?? 'm²')
      : '';
    this.property = new Api_GenerateProperty(propertyLease.property);
  }
}
