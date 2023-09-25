import { Api_PropertyLease } from '@/models/api/PropertyLease';

import { Api_GenerateProperty } from '../GenerateProperty';

export class Api_GenerateLeaseProperty {
  lease_area: string;
  property: Api_GenerateProperty;
  constructor(propertyLease: Api_PropertyLease) {
    this.lease_area = propertyLease.leaseArea
      ? (propertyLease.leaseArea?.toString() ?? '') +
        ' ' +
        (propertyLease?.areaUnitType?.description ?? 'm²')
      : '';
    this.property = new Api_GenerateProperty(propertyLease.property);
  }
}
