import { Feature } from 'geojson';

import { exists, pidFormatter } from '@/utils';

import { PopupContentConfig } from './components/LayerPopupContent';

export const parcelLayerPopupConfig: PopupContentConfig = {
  PID: { label: 'Parcel PID', display: (data: { [key: string]: any }) => pidFormatter(data?.PID) },
  PIN: { label: 'Parcel PIN', display: (data: { [key: string]: any }) => data.PIN },
  PLAN_NUMBER: {
    label: 'Plan number',
    display: (data: { [key: string]: any }) => data.PLAN_NUMBER,
  },
  OWNER_TYPE: { label: 'Owner type', display: (data: { [key: string]: any }) => data.OWNER_TYPE },
  MUNICIPALITY: {
    label: 'Municipality',
    display: (data: { [key: string]: any }) => data.MUNICIPALITY,
  },
  FEATURE_AREA_SQM: {
    label: 'Area',
    display: (data: { [key: string]: any }) => (
      <>
        {data.FEATURE_AREA_SQM} m<sup>2</sup>
      </>
    ),
  },
};

// Utility function that dynamically creates a PopupContentConfig for a feature.
// Transforms "THE_PARAM" to "The param"
export const getDynamicFeatureConfig: (feature: Feature) => PopupContentConfig = (
  feature: Feature,
) => {
  // For debug enable this
  /*
  config['Feature Type'] = {
    label: 'Feature Type',
    display: (data: { [key: string]: any }) => feature.id,
  };
  */
  const config: PopupContentConfig = {};
  Object.keys(feature.properties).forEach(key => {
    config[key] = {
      label: key.charAt(0).toUpperCase() + key.slice(1).toLowerCase().replaceAll('_', ' '),
      display: (data: { [key: string]: any }) => data[key],
    };
  });
  // This is not being mapped in the typed feature data
  if (exists(config['SE_ANNO_CAD_DATA'])) {
    delete config['SE_ANNO_CAD_DATA'];
  }
  return config;
};

export const municipalityLayerPopupConfig: PopupContentConfig = {
  ADMIN_AREA_GROUP_NAME: {
    label: 'Administration Area',
    display: (data: { [key: string]: any }) =>
      `${data.ADMIN_AREA_GROUP_NAME} (${data.CHANGE_REQUESTED_ORG})`,
  },
};

export const highwayLayerPopupConfig: PopupContentConfig = {
  PROVINCIAL_PUBLIC_HIGHWAY_ID: {
    label: 'Id',
    display: (data: { [key: string]: any }) => data.PROVINCIAL_PUBLIC_HIGHWAY_ID,
  },
  GLOBALID: { label: 'Global Id', display: (data: { [key: string]: any }) => data.GLOBALID },
  UNIQUE_ID: { label: 'Unique Id', display: (data: { [key: string]: any }) => data.UNIQUE_ID },
  PLAN_ANNOTATION: {
    label: 'Plan annotation',
    display: (data: { [key: string]: any }) => data.PLAN_ANNOTATION,
  },
  MOTI_PLAN: { label: 'MOTI plan', display: (data: { [key: string]: any }) => data.MOTI_PLAN },
  VETTING_STATUS: {
    label: 'Vetting Status',
    display: (data: { [key: string]: any }) => data.VETTING_STATUS,
  },
  SHAPE_TYPE: { label: 'Shape type', display: (data: { [key: string]: any }) => data.SHAPE_TYPE },
  PENDING_CLASSIFICATION: {
    label: 'Pending classification',
    display: (data: { [key: string]: any }) => data.PENDING_CLASSIFICATION,
  },
  MOTI_FILE: { label: 'MOTI file', display: (data: { [key: string]: any }) => data.MOTI_FILE },
  GAZETTE_PUBLISHED_DATE: {
    label: 'Gazette published date',
    display: (data: { [key: string]: any }) => data.GAZETTE_PUBLISHED_DATE,
  },
  GAZETTE_PUBLISHED_ON: {
    label: 'Gazette published on',
    display: (data: { [key: string]: any }) => data.GAZETTE_PUBLISHED_ON,
  },
  PROV_PUBLIC_HIGHWAY_TYPE: {
    label: 'Prov public highway type',
    display: (data: { [key: string]: any }) => data.PROV_PUBLIC_HIGHWAY_TYPE,
  },
  ORDER_IN_COUNCIL: {
    label: 'Order in council',
    display: (data: { [key: string]: any }) => data.ORDER_IN_COUNCIL,
  },
  SHORT_LEGAL_DESCRIPTION: {
    label: 'Short legal description',
    display: (data: { [key: string]: any }) => data.SHORT_LEGAL_DESCRIPTION,
  },
  SURVEY_PLAN: {
    label: 'Survey plan',
    display: (data: { [key: string]: any }) => data.SURVEY_PLAN,
  },
  PARENT_PARCEL: {
    label: 'Parent parcel',
    display: (data: { [key: string]: any }) => data.PARENT_PARCEL,
  },
  PARCELMAPBC_COMMENT: {
    label: 'Parcelmapbc comment',
    display: (data: { [key: string]: any }) => data.PARCELMAPBC_COMMENT,
  },
  CREATE_TIME: {
    label: 'Create time',
    display: (data: { [key: string]: any }) => data.CREATE_TIME,
  },
  UPDATE_TIME: {
    label: 'Update time',
    display: (data: { [key: string]: any }) => data.UPDATE_TIME,
  },
  CREATE_USER: {
    label: 'Create user',
    display: (data: { [key: string]: any }) => data.CREATE_USER,
  },
  UPDATE_USER: {
    label: 'Update user',
    display: (data: { [key: string]: any }) => data.UPDATE_USER,
  },
  LTSA_CREATE_TIME: {
    label: 'Ltsa create time',
    display: (data: { [key: string]: any }) => data.LTSA_CREATE_TIME,
  },
  LTSA_UPDATE_TIME: {
    label: 'Ltsa update time',
    display: (data: { [key: string]: any }) => data.LTSA_UPDATE_TIME,
  },
  LTSA_CREATE_USER: {
    label: 'Ltsa create user',
    display: (data: { [key: string]: any }) => data.LTSA_CREATE_USER,
  },
  LTSA_UPDATE_USER: {
    label: 'Ltsa update user',
    display: (data: { [key: string]: any }) => data.LTSA_UPDATE_USER,
  },
  RELATED_GAZETTES: {
    label: 'Related gazettes',
    display: (data: { [key: string]: any }) => data.RELATED_GAZETTES,
  },
  HYPERLINK: { label: 'Hyperlink', display: (data: { [key: string]: any }) => data.HYPERLINK },
  RELATIVE_PATH: {
    label: 'Relative path',
    display: (data: { [key: string]: any }) => data.RELATIVE_PATH,
  },
};
