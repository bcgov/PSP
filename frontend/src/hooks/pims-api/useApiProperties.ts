import { IGeoSearchParams, IPropertySearchParams } from 'constants/API';
import * as pimsToasts from 'constants/toasts';
import { LifecycleToasts } from 'customAxios';
import { IPagedItems, IProperty } from 'interfaces';
import queryString from 'query-string';
import React from 'react';

import { useApi } from '.';

const propertyCreatingToasts: LifecycleToasts = {
  loadingToast: pimsToasts.parcel.PARCEL_CREATING,
  successToast: pimsToasts.parcel.PARCEL_CREATED,
  errorToast: pimsToasts.parcel.PARCEL_CREATING_ERROR,
};

const propertyUpdatingToasts: LifecycleToasts = {
  loadingToast: pimsToasts.parcel.PARCEL_UPDATING,
  successToast: pimsToasts.parcel.PARCEL_UPDATED,
  errorToast: pimsToasts.parcel.PARCEL_UPDATING_ERROR,
};

const propertyDeletingToasts: LifecycleToasts = {
  loadingToast: pimsToasts.parcel.PARCEL_DELETING,
  successToast: pimsToasts.parcel.PARCEL_DELETED,
  errorToast: pimsToasts.parcel.PARCEL_DELETING_ERROR,
};

/**
 * PIMS API wrapper to centralize all AJAX requests to the property endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiProperties = () => {
  const api = useApi();
  const apiWithPropertyCreatingToasts = useApi({ lifecycleToasts: propertyCreatingToasts });
  const apiWithPropertyUpdatingToasts = useApi({ lifecycleToasts: propertyUpdatingToasts });
  const apiWithPropertyDeletingToasts = useApi({ lifecycleToasts: propertyDeletingToasts });

  return React.useMemo(
    () => ({
      // TODO: Remove fake endpoint used by the map.
      getPropertiesWfs: (params?: IGeoSearchParams): Promise<any[]> => Promise.resolve([]),
      getProperties: (params: IPropertySearchParams | null) =>
        api.get<IPagedItems<IProperty>>(
          `/properties/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getPropertyWithPid: (pid: string) => api.get<IProperty>(`/properties/${pid}`),
      getProperty: (id: number) => api.get<IProperty>(`/properties/${id}`),
      postProperty: (property: IProperty) =>
        apiWithPropertyCreatingToasts.post<IProperty>(`/properties`, property),
      putProperty: (property: IProperty) =>
        apiWithPropertyUpdatingToasts.put<IProperty>(`/properties/${property.id}`, property),
      deleteProperty: (property: IProperty) =>
        apiWithPropertyDeletingToasts.delete<IProperty>(`/properties/${property.id}`, {
          data: property,
        }),
    }),
    [
      api,
      apiWithPropertyCreatingToasts,
      apiWithPropertyUpdatingToasts,
      apiWithPropertyDeletingToasts,
    ],
  );
};
