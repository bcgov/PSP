import { AreaUnitTypes } from 'constants/areaUnitTypes';
import { VolumeUnitTypes } from 'constants/volumeUnitTypes';
import { GeoJsonProperties } from 'geojson';
import { Api_Property } from 'models/api/Property';
import { ReactNode } from 'react';
import { convertArea, convertVolume } from 'utils/convertUtils';
import { booleanToString } from 'utils/formUtils';

export interface IPropertyDetailsForm
  extends ExtendOverride<
    Api_Property,
    {
      motiRegion?: GeoJsonProperties;
      highwaysDistrict?: GeoJsonProperties;
      electoralDistrict?: GeoJsonProperties;
      isALR?: boolean;
      firstNations?: {
        bandName?: string;
        reserveName?: string;
      };
      isVolumetricParcel: string; // radio buttons only support string values, not booleans
      landMeasurementTable?: Array<{ value: number; unit: string }>;
      volumetricMeasurementTable?: Array<{ value: number; unit: ReactNode }>;
    }
  > {}

export function toFormValues(apiData: Api_Property): IPropertyDetailsForm {
  return {
    ...apiData,
    motiRegion: {},
    highwaysDistrict: {},
    electoralDistrict: {},
    isALR: false,
    firstNations: {
      bandName: '',
      reserveName: '',
    },
    isVolumetricParcel: booleanToString(apiData?.isVolumetricParcel),
    landMeasurementTable: generateLandMeasurements(apiData),
    volumetricMeasurementTable: generateVolumeMeasurements(apiData),
  };
}

function generateLandMeasurements(apiData?: Api_Property): Array<{ value: number; unit: string }> {
  const { landArea, areaUnit } = { ...apiData };
  const unitId = areaUnit?.id;
  if (typeof landArea === 'undefined' || typeof unitId === 'undefined') {
    return [];
  }

  return [
    {
      value: convertArea(landArea, unitId, AreaUnitTypes.SquareMeters),
      unit: 'sq. metres',
    },
    {
      value: convertArea(landArea, unitId, AreaUnitTypes.SquareFeet),
      unit: 'sq. feet',
    },
    {
      value: convertArea(landArea, unitId, AreaUnitTypes.Hectares),
      unit: 'hectares',
    },
    {
      value: convertArea(landArea, unitId, AreaUnitTypes.Acres),
      unit: 'acres',
    },
  ];
}

function generateVolumeMeasurements(
  apiData?: Api_Property,
): Array<{ value: number; unit: ReactNode }> {
  const { volumetricMeasurement, volumetricUnit } = { ...apiData };
  const unitId = volumetricUnit?.id;
  if (typeof volumetricMeasurement === 'undefined' || typeof unitId === 'undefined') {
    return [];
  }

  return [
    {
      value: convertVolume(volumetricMeasurement, unitId, VolumeUnitTypes.CubicMeters),
      unit: (
        <span>
          metres<sup>3</sup>
        </span>
      ),
    },
    {
      value: convertVolume(volumetricMeasurement, unitId, VolumeUnitTypes.CubicFeet),
      unit: (
        <span>
          feet<sup>3</sup>
        </span>
      ),
    },
  ];
}

export const readOnlyMultiSelectStyle = {
  multiselectContainer: {
    opacity: 1,
  },
  searchBox: {
    border: 'none',
    padding: 0,
  },
  chips: {
    opacity: 1,
    background: '#F2F2F2',
    borderRadius: '4px',
    color: 'black',
    fontSize: '16px',
    marginRight: '1em',
  },
};

export const defaultPropertyInfo: Api_Property = {
  id: 1,
  propertyType: {
    id: 'TITLED',
    description: 'Titled',
    isDisabled: false,
  },
  anomalies: [
    {
      id: 'ACCESS',
      description: 'Access',
      isDisabled: false,
    },
  ],
  tenure: [
    {
      id: 'ADJLAND',
      description: 'Adjacent Land',
      isDisabled: false,
    },
  ],
  roadType: [
    {
      id: 'GAZSURVD',
      description: 'Gazetted (Surveyed)',
      isDisabled: false,
    },
  ],
  adjacentLand: [
    {
      id: 'PRIVATE',
      description: 'Private (Fee Simple)',
      isDisabled: false,
    },
  ],
  status: {
    id: 'MOTIADMIN',
    description: 'Under MoTI administration',
    isDisabled: false,
  },
  dataSource: {
    id: 'PAIMS',
    description: 'Property Acquisition and Inventory Management System (PAIMS)',
    isDisabled: false,
  },
  dataSourceEffectiveDate: '2021-08-31T00:00:00',
  isSensitive: false,
  isProvincialPublicHwy: false,
  address: {
    id: 204,
    streetAddress1: '456 Souris Street',
    streetAddress2: 'PO Box 250',
    streetAddress3: 'A Hoot and a holler from the A&W',
    municipality: 'North Podunk',
    province: {
      id: 1,
      code: 'BC',
      description: 'British Columbia',
      displayOrder: 10,
    },
    country: {
      id: 1,
      code: 'CA',
      description: 'Canada',
      displayOrder: 1,
    },
    postal: 'IH8 B0B',
    rowVersion: 1,
  },
  pid: '007-723-385',
  pin: 90069930,
  areaUnit: {
    id: 'HA',
    description: 'Hectare',
    isDisabled: false,
  },
  landArea: 1,
  isVolumetricParcel: true,
  volumetricMeasurement: 150,
  volumetricUnit: {
    id: 'FEET3',
    description: 'Feet cubed',
    isDisabled: false,
  },
  volumetricType: {
    isDisabled: false,
  },
  municipalZoning: 'Some municipal zoning comments',
  zoning: 'Lorem ipsum',
  notes:
    'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam porttitor nisl at elit vestibulum vestibulum. Nullam eget consectetur felis, id porta eros. Proin at massa rutrum, molestie lorem a, congue lorem.',
  rowVersion: 6,
};
