import { Feature, Geometry } from 'geojson';

import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { EBC_ELECTORAL_DISTS_BS10_SVW_Feature_Properties } from '@/models/layers/electoralBoundaries';
import { exists } from '@/utils';
import { booleanToString } from '@/utils/formUtils';

export type IPropertyDetailsForm = ExtendOverride<
  ApiGen_Concepts_Property,
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
    anomalies: ApiGen_Base_CodeType<string>[];
    tenures: ApiGen_Base_CodeType<string>[];
    roadTypes: ApiGen_Base_CodeType<string>[];
  }
>;

export function toFormValues(apiData?: ApiGen_Concepts_Property): IPropertyDetailsForm {
  return {
    ...getEmptyProperty(),
    ...apiData,
    isALR: false,
    firstNations: {
      bandName: '',
      reserveName: '',
    },
    anomalies: apiData?.anomalies?.map(a => a.propertyAnomalyTypeCode).filter(exists) ?? [],
    tenures: apiData?.tenures?.map(t => t.propertyTenureTypeCode).filter(exists) ?? [],
    roadTypes: apiData?.roadTypes?.map(a => a.propertyRoadTypeCode).filter(exists) ?? [],
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

export const defaultPropertyInfo: Partial<ApiGen_Concepts_Property> = {
  anomalies: [],
  tenures: [],
  roadTypes: [],
  dataSourceEffectiveDateOnly: EpochIsoDateTime,
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
