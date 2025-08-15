import { dequal } from 'dequal';
import { LatLngLiteral } from 'leaflet';
import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { toast } from 'react-toastify';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';
import { exists, getFeatureBoundedCenter } from '@/utils';

import { ISearchViewProps } from './SearchView';

export interface ISearchContainerProps {
  View: React.FunctionComponent<ISearchViewProps>;
}

export const SearchContainer: React.FC<ISearchContainerProps> = ({ View }) => {
  const {
    mapSearchCriteria,
    setMapSearchCriteria,
    mapClick,
    requestCenterToLocation,
    mapFeatureData,
    mapMarkLocation,
    mapClearLocationMark,
    prepareForCreation,
    isEditPropertiesMode,
  } = useMapStateMachine();

  const [propertySearchFilter, setPropertySearchFilter] = useState<IPropertyFilter | null>(null);

  const pathGenerator = usePathGenerator();

  useEffect(() => {
    if (propertySearchFilter !== null && !dequal(mapSearchCriteria, propertySearchFilter)) {
      setMapSearchCriteria(propertySearchFilter);
    }
  }, [propertySearchFilter, mapSearchCriteria, setMapSearchCriteria]);

  const handleMapFilterChange = (filter: IPropertyFilter) => {
    if (['coordinates', 'name', 'address'].includes(filter.searchBy)) {
      let latLng: LatLngLiteral = undefined;
      switch (filter.searchBy) {
        case 'name':
        case 'address':
          if (exists(filter.latitude)) {
            latLng = { lat: +filter.latitude, lng: +filter.longitude };
          } else {
            toast.warn('No valid location found for address - unable to zoom to location.');
          }
          break;
        case 'coordinates':
          latLng = filter.coordinates?.toLatLng();
      }
      if (latLng) {
        mapMarkLocation(latLng);
        requestCenterToLocation(latLng);
        mapClick(latLng);
      }
    } else {
      mapClearLocationMark();
      setPropertySearchFilter(filter);
    }
  };

  // Convert to an object that can be consumed by the file creation process
  const selectedFeatureDatasets = useMemo<SelectedFeatureDataset[]>(() => {
    return (
      mapFeatureData?.fullyAttributedFeatures.features.map<SelectedFeatureDataset>(pmbcParcel => {
        const center = getFeatureBoundedCenter(pmbcParcel);
        return {
          parcelFeature: pmbcParcel,
          pimsFeature: null,
          location: { lat: center[1], lng: center[0] },
          regionFeature: null,
          fileLocation: null,
          districtFeature: null,
          municipalityFeature: null,
          selectingComponentId: null,
        };
      }) ?? []
    );
  }, [mapFeatureData?.fullyAttributedFeatures?.features]);

  const onCreateResearchFile = useCallback(() => {
    prepareForCreation(selectedFeatureDatasets);
    pathGenerator.newFile('research');
  }, [pathGenerator, prepareForCreation, selectedFeatureDatasets]);

  const onCreateAcquisitionFile = useCallback(() => {
    prepareForCreation(selectedFeatureDatasets);
    pathGenerator.newFile('acquisition');
  }, [pathGenerator, prepareForCreation, selectedFeatureDatasets]);

  const onCreateDispositionFile = useCallback(() => {
    prepareForCreation(selectedFeatureDatasets);
    pathGenerator.newFile('disposition');
  }, [pathGenerator, prepareForCreation, selectedFeatureDatasets]);

  const onCreateLeaseFile = useCallback(() => {
    prepareForCreation(selectedFeatureDatasets);
    pathGenerator.newFile('lease');
  }, [pathGenerator, prepareForCreation, selectedFeatureDatasets]);

  const onCreateManagementFile = useCallback(() => {
    prepareForCreation(selectedFeatureDatasets);
    pathGenerator.newFile('management');
  }, [pathGenerator, prepareForCreation, selectedFeatureDatasets]);

  const onAddToOpenFile = useCallback(() => {
    // If in edit properties mode, prepare the parcel for addition to an open file
    if (isEditPropertiesMode) {
      prepareForCreation(selectedFeatureDatasets);
    }
  }, [isEditPropertiesMode, prepareForCreation, selectedFeatureDatasets]);

  return (
    <View
      propertyFilter={propertySearchFilter}
      onFilterChange={handleMapFilterChange}
      searchResult={mapFeatureData}
      canAddToOpenFile={isEditPropertiesMode}
      onCreateResearchFile={onCreateResearchFile}
      onCreateAcquisitionFile={onCreateAcquisitionFile}
      onCreateDispositionFile={onCreateDispositionFile}
      onCreateLeaseFile={onCreateLeaseFile}
      onCreateManagementFile={onCreateManagementFile}
      onAddToOpenFile={onAddToOpenFile}
    />
  );
};
