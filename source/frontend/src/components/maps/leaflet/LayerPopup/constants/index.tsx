import { pidFormatter } from 'utils';

export const parcelLayerPopupConfig = {
  PID: { label: 'Parcel PID:', display: (data: any) => pidFormatter(data?.PID) },
  PIN: { label: 'Parcel PIN:', display: (data: any) => data.PIN },
  PLAN_NUMBER: { label: 'Plan number:', display: (data: any) => data.PLAN_NUMBER },
  OWNER_TYPE: { label: 'Owner type:', display: (data: any) => data.OWNER_TYPE },
  MUNICIPALITY: { label: 'Municipality:', display: (data: any) => data.MUNICIPALITY },
  FEATURE_AREA_SQM: {
    label: 'Area:',
    display: (data: any) => (
      <>
        {data.FEATURE_AREA_SQM} m<sup>2</sup>
      </>
    ),
  },
};

export const municipalityLayerPopupConfig = {
  ADMIN_AREA_GROUP_NAME: {
    label: 'Administration Area:',
    display: (data: any) => `${data.ADMIN_AREA_GROUP_NAME} (${data.CHANGE_REQUESTED_ORG})`,
  },
};
