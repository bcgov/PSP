import { pidFormatter } from '@/utils';

import { PopupContentConfig } from './components/LayerPopupContent';

export const parcelLayerPopupConfig: PopupContentConfig = {
  PID: { label: 'Parcel PID:', display: (data: { [key: string]: any }) => pidFormatter(data?.PID) },
  PIN: { label: 'Parcel PIN:', display: (data: { [key: string]: any }) => data.PIN },
  PLAN_NUMBER: {
    label: 'Plan number:',
    display: (data: { [key: string]: any }) => data.PLAN_NUMBER,
  },
  OWNER_TYPE: { label: 'Owner type:', display: (data: { [key: string]: any }) => data.OWNER_TYPE },
  MUNICIPALITY: {
    label: 'Municipality:',
    display: (data: { [key: string]: any }) => data.MUNICIPALITY,
  },
  FEATURE_AREA_SQM: {
    label: 'Area:',
    display: (data: { [key: string]: any }) => (
      <>
        {data.FEATURE_AREA_SQM} m<sup>2</sup>
      </>
    ),
  },
};

export const municipalityLayerPopupConfig: PopupContentConfig = {
  ADMIN_AREA_GROUP_NAME: {
    label: 'Administration Area:',
    display: (data: { [key: string]: any }) =>
      `${data.ADMIN_AREA_GROUP_NAME} (${data.CHANGE_REQUESTED_ORG})`,
  },
};
