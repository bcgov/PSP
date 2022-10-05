import { AreaUnitTypes } from 'constants/areaUnitTypes';
import { VolumeUnitTypes } from 'constants/volumeUnitTypes';
import { GeoJsonProperties } from 'geojson';
import { Api_Property } from 'models/api/Property';
import Api_TypeCode from 'models/api/TypeCode';
import { ReactNode } from 'react';
import { convertArea, convertVolume } from 'utils/convertUtils';
import { booleanToString } from 'utils/formUtils';

export interface IPropertyDetailsForm
  extends ExtendOverride<
    Api_Property,
    {
      electoralDistrict?: GeoJsonProperties;
      isALR?: boolean;
      firstNations?: {
        bandName?: string;
        reserveName?: string;
      };
      isVolumetricParcel: string; // radio buttons only support string values, not booleans
      landMeasurementTable?: Array<{ value: number; unit: string }>;
      volumetricMeasurementTable?: Array<{ value: number; unit: ReactNode }>;
      anomalies: (Api_TypeCode<string> | undefined)[];
      tenures: (Api_TypeCode<string> | undefined)[];
      adjacentLands: (Api_TypeCode<string> | undefined)[];
      roadTypes: (Api_TypeCode<string> | undefined)[];
    }
  > {}

export function toFormValues(apiData?: Api_Property): IPropertyDetailsForm {
  return {
    ...apiData,
    isALR: false,
    firstNations: {
      bandName: '',
      reserveName: '',
    },
    anomalies: apiData?.anomalies?.map(a => a.propertyAnomalyTypeCode) ?? [],
    tenures: apiData?.tenures?.map(t => t.propertyTenureTypeCode) ?? [],
    adjacentLands: apiData?.adjacentLands?.map(a => a.propertyAdjacentLandTypeCode) ?? [],
    roadTypes: apiData?.roadTypes?.map(a => a.propertyRoadTypeCode) ?? [],
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

export const defaultPropertyInfo: Partial<Api_Property> = {
  anomalies: [],
  tenures: [],
  roadTypes: [],
  adjacentLands: [],
  dataSourceEffectiveDate: '',
  isSensitive: false,
  isProvincialPublicHwy: false,
  pid: 0,
  pin: undefined,
  areaUnit: undefined,
  landArea: undefined,
  isVolumetricParcel: true,
  volumetricMeasurement: undefined,
  volumetricUnit: undefined,
  volumetricType: undefined,
  municipalZoning: '',
  zoning: '',
  notes: '',
};
