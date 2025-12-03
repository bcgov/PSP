import { dequal } from 'dequal';
import { LatLngLiteral } from 'leaflet';
import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { toast } from 'react-toastify';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import {
  emptyFeatureDataset,
  LocationFeatureDataset,
} from '@/components/common/mapFSM/useLocationFeatureLoader';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';
import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import {
  exists,
  getFeatureBoundedCenter,
  getRegionAndDistrictsResults,
  latLngToKey,
} from '@/utils';

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
    requestLocationFeatureAddition: requestAddition,
    isEditPropertiesMode,
  } = useMapStateMachine();

  const [propertySearchFilter, setPropertySearchFilter] = useState<IPropertyFilter | null>(null);

  const pathGenerator = usePathGenerator();

  const { findDistrict, findRegion } = useAdminBoundaryMapLayer();

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

  // Base dataset (no region/district yet)
  const baseDatasets = useMemo<LocationFeatureDataset[]>(() => {
    return (
      mapFeatureData?.fullyAttributedFeatures.features.map<LocationFeatureDataset>(pmbcParcel => {
        const center = getFeatureBoundedCenter(pmbcParcel);
        return {
          ...emptyFeatureDataset(),
          location: { lat: center[1], lng: center[0] },
          parcelFeatures: [pmbcParcel],
        };
      }) ?? []
    );
  }, [mapFeatureData?.fullyAttributedFeatures?.features]);

  // Enrich dataset with region/district info
  const [locationFeatureDatasets, setLocationFeatureDatasets] = useState<LocationFeatureDataset[]>(
    [],
  );

  useEffect(() => {
    let cancelled = false;

    async function fetchRegions() {
      if (baseDatasets.length === 0) {
        setLocationFeatureDatasets([]);
        return;
      }

      const results = await getRegionAndDistrictsResults(baseDatasets, findRegion, findDistrict);

      if (cancelled) return;

      const enriched = baseDatasets.map(dataset => {
        const key = latLngToKey(dataset.location);
        if (results.has(key)) {
          const { regionResult, districtResult } = results.get(key)!;
          return {
            ...dataset,
            regionFeature: regionResult,
            districtFeature: districtResult,
          };
        } else {
          return dataset;
        }
      });

      setLocationFeatureDatasets(enriched);
    }

    fetchRegions();

    // Cleanup function to avoid state updates if component unmounts
    return () => {
      cancelled = true;
    };
  }, [baseDatasets, findDistrict, findRegion]);

  // Actions for creating new files
  const onCreateResearchFile = useCallback(() => {
    requestAddition(locationFeatureDatasets);
    pathGenerator.newFile('research');
  }, [pathGenerator, requestAddition, locationFeatureDatasets]);

  const onCreateAcquisitionFile = useCallback(() => {
    requestAddition(locationFeatureDatasets);
    pathGenerator.newFile('acquisition');
  }, [pathGenerator, requestAddition, locationFeatureDatasets]);

  const onCreateDispositionFile = useCallback(() => {
    requestAddition(locationFeatureDatasets);
    pathGenerator.newFile('disposition');
  }, [pathGenerator, requestAddition, locationFeatureDatasets]);

  const onCreateLeaseFile = useCallback(() => {
    requestAddition(locationFeatureDatasets);
    pathGenerator.newFile('lease');
  }, [pathGenerator, requestAddition, locationFeatureDatasets]);

  const onCreateManagementFile = useCallback(() => {
    requestAddition(locationFeatureDatasets);
    pathGenerator.newFile('management');
  }, [pathGenerator, requestAddition, locationFeatureDatasets]);

  const onAddToOpenFile = useCallback(() => {
    // If in edit properties mode, prepare the parcel for addition to an open file
    if (isEditPropertiesMode) {
      requestAddition(locationFeatureDatasets);
    }
  }, [isEditPropertiesMode, requestAddition, locationFeatureDatasets]);

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
