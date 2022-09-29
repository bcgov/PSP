import { PropertyClassificationTypes } from 'constants/propertyClassificationTypes';
import { PropertyDataSourceTypes } from 'constants/propertyDataSourceTypes';
import { PropertyStatusTypes } from 'constants/propertyStatusTypes';
import { PropertyTenureTypes } from 'constants/propertyTenureTypes';
import { IProperty } from 'interfaces';
import { mockAddress } from 'mocks';

import { Api_PropertyFile } from './../models/api/PropertyFile';

export const getMockProperties = () =>
  [
    {
      id: 1,
      pid: '000-000-000',
      pin: '',
      statusId: PropertyStatusTypes.UnderAdmin,
      dataSourceId: PropertyDataSourceTypes.PAIMS,
      dataSourceEffectiveDate: '2021-08-30T17:28:17.655Z',
      classificationId: PropertyClassificationTypes.CoreOperational,
      tenureId: PropertyTenureTypes.HighwayRoad,
      zoning: '',
      zoningPotential: '',
      encumbranceReason: '',
      isSensitive: false,
      latitude: 48,
      longitude: 123,
      name: 'test name',
      description: 'test',
      addressId: mockAddress.id,
      address: { ...mockAddress },
      landArea: 123,
      landLegalDescription: 'test description',
    },
    {
      id: 2,
      pid: '000-000-001',
      pin: '',
      statusId: PropertyStatusTypes.UnderAdmin,
      dataSourceId: PropertyDataSourceTypes.PAIMS,
      dataSourceEffectiveDate: '2021-08-30T18:14:13.170Z',
      classificationId: PropertyClassificationTypes.CoreOperational,
      tenureId: PropertyTenureTypes.HighwayRoad,
      zoning: '',
      zoningPotential: '',
      encumbranceReason: '',
      isSensitive: false,
      latitude: 49,
      longitude: 123,
      name: 'test name',
      description: 'test',
      addressId: mockAddress.id,
      address: { ...mockAddress },
      landArea: 123,
      landLegalDescription: 'test description',
    },
    {
      id: 100,
      pid: '000-000-002',
      pin: '',
      statusId: PropertyStatusTypes.UnderAdmin,
      dataSourceId: PropertyDataSourceTypes.PAIMS,
      dataSourceEffectiveDate: '2021-08-30T18:14:13.170Z',
      classificationId: PropertyClassificationTypes.CoreOperational,
      tenureId: PropertyTenureTypes.HighwayRoad,
      zoning: '',
      zoningPotential: '',
      encumbranceReason: '',
      isSensitive: false,
      latitude: 48,
      longitude: 123,
      name: 'test name',
      description: 'test',
      addressId: mockAddress.id,
      address: { ...mockAddress },
      landArea: 123,
      landLegalDescription: 'test description',
    },
  ] as IProperty[];

export const getMockApiPropertyFiles = (): Api_PropertyFile[] => [
  {
    id: 1,
    propertyName: 'test property name',
    property: {
      id: 1,
      anomalies: [],
      tenures: [],
      roadTypes: [],
      adjacentLands: [],
      region: {
        id: 1,
        description: 'South Coast Region',
        isDisabled: false,
      },
      district: {
        id: 1,
        description: 'Lower Mainland District',
        isDisabled: false,
      },
      dataSourceEffectiveDate: '2021-08-31T00:00:00',
      latitude: 54.794202998379006,
      longitude: -127.18413347053901,
      isSensitive: false,
      isRwyBeltDomPatent: false,
      pid: 7723385,
      pin: 90069930,
      landArea: 1,
      isVolumetricParcel: false,
      location: {
        coordinate: {
          x: -127.18413347053901,
          y: 54.794202998379006,
        },
      },
      rowVersion: 3,
    },
  },
  {
    id: 2,
    property: {
      id: 2,
      anomalies: [],
      tenures: [],
      roadTypes: [],
      adjacentLands: [],
      region: {
        id: 1,
        description: 'South Coast Region',
        isDisabled: false,
      },
      district: {
        id: 1,
        description: 'Lower Mainland District',
        isDisabled: false,
      },
      dataSourceEffectiveDate: '2021-08-31T00:00:00',
      latitude: 54.54882129837095,
      longitude: -128.60383100540952,
      isSensitive: false,
      isRwyBeltDomPatent: false,
      pid: 11041404,
      pin: 90072652,
      landArea: 1,
      isVolumetricParcel: false,
      location: {
        coordinate: {
          x: -128.60383100540952,
          y: 54.54882129837095,
        },
      },
      rowVersion: 5,
    },
  },
];
