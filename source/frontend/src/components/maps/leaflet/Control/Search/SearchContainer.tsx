import { dequal } from 'dequal';
import { LatLngLiteral } from 'leaflet';
import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { toast } from 'react-toastify';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import {
  defaultPropertyFilter,
  IPropertyFilter,
} from '@/features/properties/filter/IPropertyFilter';
import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import {
  exists,
  featureSetToLatLngKey,
  getFeatureBoundedCenter,
  getRegionAndDistrictsResults,
} from '@/utils';

import { ISearchViewProps } from './SearchView';

export interface ISearchContainerProps {
  View: React.FunctionComponent<ISearchViewProps>;
}

const useEnrichedDatasets = (
  baseDatasets: SelectedFeatureDataset[],
  findRegion: ReturnType<typeof useAdminBoundaryMapLayer>['findRegion'],
  findDistrict: ReturnType<typeof useAdminBoundaryMapLayer>['findDistrict'],
): SelectedFeatureDataset[] => {
  const [selectedFeatureDatasets, setSelectedFeatureDatasets] = useState<SelectedFeatureDataset[]>(
    [],
  );

  useEffect(() => {
    let cancelled = false;

    async function fetchRegions() {
      if (baseDatasets.length === 0) {
        setSelectedFeatureDatasets([]);
        return;
      }

      const results = await getRegionAndDistrictsResults(baseDatasets, findRegion, findDistrict);

      if (cancelled) return;

      const enriched = baseDatasets.map(dataset => {
        const key = featureSetToLatLngKey(dataset);
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

      setSelectedFeatureDatasets(enriched);
    }

    fetchRegions();

    // Cleanup function to avoid state updates if component unmounts
    return () => {
      cancelled = true;
    };
  }, [baseDatasets, findDistrict, findRegion]);

  return selectedFeatureDatasets;
};

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

  const pathGenerator = usePathGenerator();

  const { findDistrict, findRegion } = useAdminBoundaryMapLayer();

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
        setMapSearchCriteria(filter);
      }
    } else if (filter !== null && !dequal(mapSearchCriteria, filter)) {
      mapClearLocationMark();
      setMapSearchCriteria(filter);
    }
  };

  // Base datasets (no region/district yet)
  const pimsBaseDatasets = useMemo<SelectedFeatureDataset[]>(() => {
    return (
      mapFeatureData?.pimsFeatures.features.map<SelectedFeatureDataset>(pimsParcel => {
        const center = getFeatureBoundedCenter(pimsParcel);
        return {
          parcelFeature: null,
          pimsFeature: pimsParcel,
          location:
            exists(center) && center.length >= 2 ? { lat: center[1], lng: center[0] } : null,
          regionFeature: null,
          fileLocation: null,
          fileBoundary: null,
          districtFeature: null,
          municipalityFeature: null,
          selectingComponentId: null,
        };
      }) ?? []
    );
  }, [mapFeatureData]);

  const pmbcBaseDatasets = useMemo<SelectedFeatureDataset[]>(() => {
    return (
      mapFeatureData?.fullyAttributedFeatures.features.map<SelectedFeatureDataset>(pmbcParcel => {
        const center = getFeatureBoundedCenter(pmbcParcel);
        return {
          parcelFeature: pmbcParcel,
          pimsFeature: null,
          location:
            exists(center) && center.length >= 2 ? { lat: center[1], lng: center[0] } : null,
          regionFeature: null,
          fileLocation: null,
          fileBoundary: null,
          districtFeature: null,
          municipalityFeature: null,
          selectingComponentId: null,
        };
      }) ?? []
    );
  }, [mapFeatureData]);

  const pimsSelectedFeatureDatasets = useEnrichedDatasets(
    pimsBaseDatasets,
    findRegion,
    findDistrict,
  );

  const pmbcSelectedFeatureDatasets = useEnrichedDatasets(
    pmbcBaseDatasets,
    findRegion,
    findDistrict,
  );

  // Actions for creating new files
  const onCreateResearchFile = useCallback(
    (isPims: boolean) => {
      const datasets = isPims ? pimsSelectedFeatureDatasets : pmbcSelectedFeatureDatasets;
      prepareForCreation(datasets);
      pathGenerator.newFile('research');
    },
    [pathGenerator, prepareForCreation, pimsSelectedFeatureDatasets, pmbcSelectedFeatureDatasets],
  );

  const onCreateAcquisitionFile = useCallback(
    (isPims: boolean) => {
      const datasets = isPims ? pimsSelectedFeatureDatasets : pmbcSelectedFeatureDatasets;
      prepareForCreation(datasets);
      pathGenerator.newFile('acquisition');
    },
    [pathGenerator, prepareForCreation, pimsSelectedFeatureDatasets, pmbcSelectedFeatureDatasets],
  );

  const onCreateDispositionFile = useCallback(
    (isPims: boolean) => {
      const datasets = isPims ? pimsSelectedFeatureDatasets : pmbcSelectedFeatureDatasets;
      prepareForCreation(datasets);
      pathGenerator.newFile('disposition');
    },
    [pathGenerator, prepareForCreation, pimsSelectedFeatureDatasets, pmbcSelectedFeatureDatasets],
  );

  const onCreateLeaseFile = useCallback(
    (isPims: boolean) => {
      const datasets = isPims ? pimsSelectedFeatureDatasets : pmbcSelectedFeatureDatasets;
      prepareForCreation(datasets);
      pathGenerator.newFile('lease');
    },
    [pathGenerator, prepareForCreation, pimsSelectedFeatureDatasets, pmbcSelectedFeatureDatasets],
  );

  const onCreateManagementFile = useCallback(
    (isPims: boolean) => {
      const datasets = isPims ? pimsSelectedFeatureDatasets : pmbcSelectedFeatureDatasets;
      prepareForCreation(datasets);
      pathGenerator.newFile('management');
    },
    [pathGenerator, prepareForCreation, pimsSelectedFeatureDatasets, pmbcSelectedFeatureDatasets],
  );

  const onAddToOpenFile = useCallback(
    (isPims: boolean) => {
      // If in edit properties mode, prepare the parcel for addition to an open file
      if (isEditPropertiesMode) {
        const datasets = isPims ? pimsSelectedFeatureDatasets : pmbcSelectedFeatureDatasets;
        prepareForCreation(datasets);
      }
    },
    [
      isEditPropertiesMode,
      prepareForCreation,
      pimsSelectedFeatureDatasets,
      pmbcSelectedFeatureDatasets,
    ],
  );

  return (
    <View
      propertyFilter={mapSearchCriteria ?? defaultPropertyFilter}
      onFilterChange={handleMapFilterChange}
      searchResult={mapFeatureData}
      pmbcSelectedFeatureDatasets={pmbcSelectedFeatureDatasets}
      pimsSelectedFeatureDatasets={pimsSelectedFeatureDatasets}
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
