import { IPropertySearchParams } from 'constants/API';
import { IBuilding, IPagedItems, IParcel } from 'interfaces';
import React from 'react';
import * as pimsToasts from 'constants/toasts';
import queryString from 'query-string';

import { useApi } from '.';
import { LifecycleToasts } from 'customAxios';

/**
 * PIMS API wrapper to centralize all AJAX requests to the property endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

const parcelCreatingToasts: LifecycleToasts = {
  loadingToast: pimsToasts.parcel.PARCEL_CREATING,
  successToast: pimsToasts.parcel.PARCEL_CREATED,
  errorToast: pimsToasts.parcel.PARCEL_CREATING_ERROR,
};

const parcelDeletingToasts: LifecycleToasts = {
  loadingToast: pimsToasts.parcel.PARCEL_DELETING,
  successToast: pimsToasts.parcel.PARCEL_DELETED,
  errorToast: pimsToasts.parcel.PARCEL_DELETING_ERROR,
};

const parcelUpdatingToasts: LifecycleToasts = {
  loadingToast: pimsToasts.parcel.PARCEL_UPDATING,
  successToast: pimsToasts.parcel.PARCEL_UPDATED,
  errorToast: pimsToasts.parcel.PARCEL_UPDATING_ERROR,
};

const buildingDeletingToasts: LifecycleToasts = {
  loadingToast: pimsToasts.building.BUILDING_DELETING,
  successToast: pimsToasts.building.BUILDING_DELETED,
  errorToast: pimsToasts.building.BUILDING_DELETING_ERROR,
};

export const useApiProperties = () => {
  const api = useApi();
  const apiWithParcelCreatingToasts = useApi({ lifecycleToasts: parcelCreatingToasts });
  const apiWithParcelDeletingToasts = useApi({ lifecycleToasts: parcelDeletingToasts });
  const apiWithParcelUpdatingToasts = useApi({ lifecycleToasts: parcelUpdatingToasts });
  const apiWithBuildingDeletingToasts = useApi({ lifecycleToasts: buildingDeletingToasts });

  return React.useMemo(
    () => ({
      getParcelDetail: (id: number) => api.get<IParcel>(`/properties/parcels/${id}`),
      getParcelsDetail: (params: IPropertySearchParams) =>
        api.get<IPagedItems<IParcel>>(
          `/properties/parcels?${params ? queryString.stringify(params) : ''}`,
        ),
      getParcels: (params: IPropertySearchParams | null) =>
        api.get<IPagedItems<IParcel>>(
          `/properties/search?${params ? queryString.stringify(params) : ''}`,
        ),
      postParcel: (parcel: IParcel) =>
        apiWithParcelCreatingToasts.post<IParcel>(`/properties/parcels`, parcel),
      putParcel: (parcel: IParcel) =>
        apiWithParcelUpdatingToasts.put<IParcel>(`/properties/parcels/${parcel.id}`, parcel),
      deleteParcel: (parcel: IParcel) =>
        apiWithParcelDeletingToasts.delete<IParcel>(`/properties/parcels/${parcel.id}`, {
          data: parcel,
        }),
      getBuilding: (id: number) => api.get<IBuilding>(`/properties/buildings/${id}`),
      deleteBuilding: (building: IBuilding) =>
        apiWithBuildingDeletingToasts.delete<IBuilding>(`/properties/buildings/${building.id}`),
    }),
    [
      api,
      apiWithParcelCreatingToasts,
      apiWithParcelUpdatingToasts,
      apiWithParcelDeletingToasts,
      apiWithBuildingDeletingToasts,
    ],
  );
};
