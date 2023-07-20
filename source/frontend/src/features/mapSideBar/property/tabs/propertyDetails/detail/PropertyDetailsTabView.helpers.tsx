import { Feature, Geometry } from 'geojson';

import { Api_Property } from '@/models/api/Property';
import Api_TypeCode from '@/models/api/TypeCode';
import { EBC_ELECTORAL_DISTS_BS10_SVW_Feature_Properties } from '@/models/layers/electoralBoundaries';
import { booleanToString } from '@/utils/formUtils';

export interface IPropertyDetailsForm
  extends ExtendOverride<
    Api_Property,
    {
      electoralDistrict:
        | Feature<Geometry, EBC_ELECTORAL_DISTS_BS10_SVW_Feature_Properties>
        | undefined;
      isALR?: boolean;
      firstNations?: {
        bandName: string;
        reserveName: string;
      };
      isVolumetricParcel: string; // radio buttons only support string values, not booleans
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
    electoralDistrict: undefined,
  };
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
