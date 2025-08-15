import { Feature } from 'geojson';
import { FaExternalLinkAlt } from 'react-icons/fa';

import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { exists, formatApiAddress, getAreaUnit, pidFormatter } from '@/utils';

import { ContentConfig } from './LayerContent';

export const parcelLayerConfig: ContentConfig = {
  PID: { label: 'Parcel PID', display: (data: { [key: string]: any }) => pidFormatter(data?.PID) },
  PIN: { label: 'Parcel PIN', display: (data: { [key: string]: any }) => data.PIN },
  PLAN_NUMBER: {
    label: 'Plan number',
    display: (data: { [key: string]: any }) => data.PLAN_NUMBER,
  },
  LEGAL_DESCRIPTION: {
    label: 'Legal description',
    display: (data: { [key: string]: any }) => data.LEGAL_DESCRIPTION,
  },
  OWNER_TYPE: { label: 'Owner type', display: (data: { [key: string]: any }) => data.OWNER_TYPE },
  MUNICIPALITY: {
    label: 'Municipality',
    display: (data: { [key: string]: any }) => data.MUNICIPALITY,
  },
  REGIONAL_DISTRICT: {
    label: 'Regional district',
    display: (data: { [key: string]: any }) => data.REGIONAL_DISTRICT,
  },
  FEATURE_AREA_SQM: {
    label: 'Area',
    display: (data: { [key: string]: any }) => (
      <>
        {data.FEATURE_AREA_SQM} m<sup>2</sup>
      </>
    ),
  },
  PARCEL_CLASS: {
    label: 'Parcel class',
    display: (data: { [key: string]: any }) => data.PARCEL_CLASS,
  },
  SURVEY_DESIGNATION_1: {
    label: 'Survey designation one',
    display: (data: { [key: string]: any }) => data.SURVEY_DESIGNATION_1,
  },
  SURVEY_DESIGNATION_2: {
    label: 'Survey designation two',
    display: (data: { [key: string]: any }) => data.SURVEY_DESIGNATION_2,
  },
  SURVEY_DESIGNATION_3: {
    label: 'Survey designation three',
    display: (data: { [key: string]: any }) => data.SURVEY_DESIGNATION_3,
  },
};

// Utility function that dynamically creates a ContentConfig for a feature.
// Transforms "THE_PARAM" to "The param"
export const getDynamicFeatureConfig: (feature: Feature) => ContentConfig = (feature: Feature) => {
  // For debug enable this
  /*
  config['Feature Type'] = {
    label: 'Feature Type',
    display: (data: { [key: string]: any }) => feature.id,
  };
  */
  const config: ContentConfig = {};
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

export const municipalityLayerConfig: ContentConfig = {
  MUNICIPALITY_NAME: {
    label: 'Municipality Name',
    display: (data: { [key: string]: any }) =>
      `${data.ADMIN_AREA_GROUP_NAME} (${data.CHANGE_REQUESTED_ORG})`,
  },
};

export const pimsLayerConfig: ContentConfig = {
  PROPERTY_ID: {
    label: 'Hyperlink',
    display: (data: { [key: string]: any }) => (
      <a
        href={`/mapview/sidebar/property/${data.PROPERTY_ID}/details`}
        target="_blank"
        rel="noreferrer"
      >
        View PIMS property&nbsp;
        <FaExternalLinkAlt />
      </a>
    ),
  },
  PID: {
    label: 'PID',
    display: (data: { [key: string]: any }) => pidFormatter(data.PID_PADDED),
  },
  PIN: {
    label: 'PIN',
    display: (data: { [key: string]: any }) => data.PIN,
  },
  UNIQUE_ID: {
    label: 'Plan number',
    display: (data: { [key: string]: any }) => data.SURVEY_PLAN_NUMBER,
  },
  IS_RETIRED: {
    label: 'Retired ?',
    display: (data: { [key: string]: any }) => (data.IS_RETIRED ? 'Yes' : 'No'),
  },
  LAND_LEGAL_DESCRIPTION: {
    label: 'PIMS legal description',
    display: (data: { [key: string]: any }) => data.LAND_LEGAL_DESCRIPTION,
  },

  PIMS_ADDRESS: {
    label: 'Pims Address',
    display: (data: { [key: string]: any }) =>
      formatApiAddress({
        streetAddress1: data.STREET_ADDRESS_1,
        streetAddress2: data.STREET_ADDRESS_2,
        streetAddress3: data.STREET_ADDRESS_3,
        postal: data.POSTAL_CODE,
        province: { code: 'BC' },
        municipality: data.MUNICIPALITY_NAME,
      } as ApiGen_Concepts_Address), // force the type to avoid having to list unused fields.
  },
  LAND_AREA: {
    label: 'PIMS land area',
    display: (data: { [key: string]: any }) =>
      data.LAND_AREA + ' ' + getAreaUnit(data.PROPERTY_AREA_UNIT_TYPE_CODE.toLowerCase()),
  },
};

export const highwayLayerConfig: ContentConfig = {
  PROVINCIAL_PUBLIC_HIGHWAY_ID: {
    label: 'Id',
    display: (data: { [key: string]: any }) => data.PROVINCIAL_PUBLIC_HIGHWAY_ID,
  },
  GLOBALID: { label: 'Global Id', display: (data: { [key: string]: any }) => data.GLOBALID },
  UNIQUE_ID: { label: 'Unique Id', display: (data: { [key: string]: any }) => data.UNIQUE_ID },
  HYPERLINK: {
    label: 'Hyperlink to plan documents',
    display: (data: { [key: string]: any }) => (
      <a href={`${data.HYPERLINK}`} target="_blank" rel="noreferrer">
        {decodeURI(data.HYPERLINK)}
      </a>
    ),
  },
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
  RELATIVE_PATH: {
    label: 'Relative path',
    display: (data: { [key: string]: any }) => data.RELATIVE_PATH,
  },
};
