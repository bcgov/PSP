import { Feature, FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import debounce from 'lodash/debounce';
import isNumber from 'lodash/isNumber';
import { useCallback, useRef, useState } from 'react';
import { toast } from 'react-toastify';

import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { IGeocoderResponse } from '@/hooks/pims-api/interfaces/IGeocoder';
import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { useGeocoderRepository } from '@/hooks/useGeocoderRepository';
import { Dictionary } from '@/interfaces/Dictionary';
import { MOT_DistrictBoundary_Feature_Properties } from '@/models/layers/motDistrictBoundary';
import { MOT_RegionalBoundary_Feature_Properties } from '@/models/layers/motRegionalBoundary';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { exists, getFeatureBoundedCenter } from '@/utils';

import { ILayerSearchCriteria } from '../models';
import PropertySearchSelectorFormView from './PropertySearchSelectorFormView';

interface RegionDistrictTask {
  regionTask: Promise<Feature<Geometry, MOT_RegionalBoundary_Feature_Properties>>;
  districtTask: Promise<Feature<Geometry, MOT_DistrictBoundary_Feature_Properties>>;
}
interface RegionDistrictResult {
  regionResult: Feature<Geometry, MOT_RegionalBoundary_Feature_Properties>;
  districtResult: Feature<Geometry, MOT_DistrictBoundary_Feature_Properties>;
}

export interface IPropertySelectorSearchContainerProps {
  selectedProperties: SelectedFeatureDataset[];
  setSelectedProperties: (properties: SelectedFeatureDataset[]) => void;
}

export const PropertySelectorSearchContainer: React.FC<IPropertySelectorSearchContainerProps> = ({
  selectedProperties,
  setSelectedProperties,
}) => {
  const [searchResults, setSearchResults] = useState<SelectedFeatureDataset[]>([]);
  const [addressResults, setAddressResults] = useState<IGeocoderResponse[]>([]);

  const {
    getSitePids,
    isLoadingSitePids,
    searchAddress,
    isLoadingSearchAddress,
    isLoadingNearestToPoint,
  } = useGeocoderRepository();

  const { findDistrict, findDistrictLoading, findRegion, findRegionLoading } =
    useAdminBoundaryMapLayer();

  const {
    findByPid,
    findByPin,
    findByPlanNumber,
    findByLegalDescription,
    findByLoading: isMapLayerLoading,
    findMany,
    findManyLoading,
  } = useFullyAttributedParcelMapLayer();

  const searchFunc = useCallback(
    async (layerSearch: ILayerSearchCriteria) => {
      let result: FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties> | undefined =
        undefined;
      if (layerSearch?.searchBy === 'pid' && layerSearch.pid) {
        result = await findByPid(layerSearch.pid);
      } else if (
        layerSearch?.searchBy === 'pin' &&
        layerSearch.pin &&
        isNumber(+layerSearch?.pin)
      ) {
        result = await findByPin(layerSearch.pin);
      } else if (layerSearch?.searchBy === 'planNumber' && layerSearch.planNumber) {
        result = await findByPlanNumber(layerSearch.planNumber);
      } else if (layerSearch?.searchBy === 'legalDescription' && layerSearch.legalDescription) {
        result = await findByLegalDescription(layerSearch.legalDescription);
      } else if (layerSearch?.searchBy === 'address' && layerSearch.address) {
        // Ignore address searches
        return;
      } else if (layerSearch?.searchBy === 'coordinates' && layerSearch.coordinates) {
        result = await findMany(layerSearch.coordinates.toLatLng());
      }

      /*
      TODO: Address and PIMS search are not being currently used. PSP-10476
      // Address search takes much longer than the other fields. Avoid if there are a lot a properties.
      let hasAddressSearch = false;
      let getAddressTasks: Promise<IGeocoderResponse>[] = [];
      if (exists(result?.features?.length) && result?.features?.length <= 15) {
        hasAddressSearch = true;
        getAddressTasks = result.features.map(p => {
          const latLngArray = getFeatureBoundedCenter(p);
          return getPropertyAddress(
            { latitude: latLngArray[1], longitude: latLngArray[0] },
            getNearestToPoint,
          );
        });
      }

      const getPimsTasks = result.features.map(p => {
        // TODO: Add search by PLAN #
        if (exists(p?.properties?.PID_NUMBER) || exists(p?.properties?.PIN)) {
          return loadPimsProperties({
            PID: p?.properties?.PID_NUMBER?.toString(),
            PIN: p?.properties?.PIN?.toString(),
          });
        }
        return null;
      });
      */

      // match the region and district for all found properties
      const dataset: SelectedFeatureDataset[] = result.features.map(p =>
        featureToLocationFeatureDataset(p),
      );
      const regionDistrictTasks = getRegionAndDisctricts(dataset, findRegion, findDistrict);
      const regionDistrictResults: Dictionary<RegionDistrictResult> = {};

      // Await for all results
      for (const entry in regionDistrictTasks) {
        const districtResult = (await regionDistrictTasks[entry].districtTask) ?? null;
        const regionResult = (await regionDistrictTasks[entry].regionTask) ?? null;
        regionDistrictResults[entry] = { regionResult, districtResult };
      }

      /*
       * TODO: Not used. PSP-10476
      const pimsResults = await Promise.all(getPimsTasks);
      const addressResults = await Promise.all(getAddressTasks);
      */

      const locations = result.features.map<SelectedFeatureDataset>(p => {
        const foundProperty = featureToLocationFeatureDataset(p);
        const latLngKey = propertyToLatLngKey(foundProperty);
        if (exists(regionDistrictResults[latLngKey])) {
          foundProperty.regionFeature = regionDistrictResults[latLngKey].regionResult;
          foundProperty.districtFeature = regionDistrictResults[latLngKey].districtResult;
        }

        /*
        *  TODO: this is always false. This section needs to be analized. Otherwise, both the PIMS and Address search results are not being used at all
        * PSP-140476
        if (exists(foundProperty?.pimsFeature)) {
          foundProperty.pimsFeature = pimsResults[i]?.features?.length
            ? pimsResults[i]?.features[0]
            : ({
                properties: {
                  STREET_ADDRESS_1: addressResults[i]?.fullAddress,
                },
              } as Feature<Geometry, PIMS_Property_Location_View>);
        }
        */
        return foundProperty;
      });
      setSearchResults(locations ?? []);
    },
    [
      findByLegalDescription,
      findByPid,
      findByPin,
      findByPlanNumber,
      findRegion,
      findDistrict,
      findMany,
    ],
  );

  const handleOnAddressSelect = async (selectedItem: IGeocoderResponse) => {
    if (!selectedItem.siteId) {
      toast.info('No site id found for selected address');
      return;
    }
    const pidResults = await getSitePids(selectedItem.siteId);

    if (pidResults && pidResults?.pids) {
      if (pidResults.pids.length > 15) {
        toast.error('Maximum PID search size exceeded for selected address');
        return;
      }

      const findByPidCalls: Promise<FeatureCollection<Geometry, GeoJsonProperties> | undefined>[] =
        [];
      pidResults.pids.forEach(async (pid: string) => {
        findByPidCalls.push(findByPid(pid));
      });

      const responses = await Promise.all(findByPidCalls);

      let propertyResults: SelectedFeatureDataset[] = [];
      responses?.forEach((item: FeatureCollection<Geometry, GeoJsonProperties> | undefined) => {
        if (item) {
          item.features.forEach(feature => {
            if (feature) {
              propertyResults = propertyResults.concat(featureToLocationFeatureDataset(feature));
            }
          });
        }
      });

      // match the region and district for all found properties
      const regionDistrictTasks = getRegionAndDisctricts(propertyResults, findRegion, findDistrict);

      const regionDistrictResults: Dictionary<RegionDistrictResult> = {};
      for (const latLngKey in regionDistrictTasks) {
        const districtResult = await regionDistrictTasks[latLngKey].districtTask;
        const regionResult = await regionDistrictTasks[latLngKey].regionTask;
        regionDistrictResults[latLngKey] = { regionResult, districtResult };
      }

      propertyResults.forEach(foundProperty => {
        const latLngKey = propertyToLatLngKey(foundProperty);
        if (exists(regionDistrictResults[latLngKey])) {
          foundProperty.regionFeature = regionDistrictResults[latLngKey].regionResult;
          foundProperty.districtFeature = regionDistrictResults[latLngKey].districtResult;
        }
      });

      setSearchResults([...propertyResults]);
      setAddressResults([]);
    }
  };

  const debouncedSearch = useRef(
    debounce(
      async (val: string, abort: boolean) => {
        if (!abort) {
          const addresses = await searchAddress(
            val,
            `matchPrecisionNot=OCCUPANT,INTERSECTION,BLOCK,STREET,LOCALITY,PROVINCE,OCCUPANT`,
          );

          setAddressResults(addresses || []);
        }
      },
      500,
      { trailing: true },
    ),
  ).current;

  const handleOnAddressChange = async (val?: string) => {
    if (val && val.length >= 5) {
      debouncedSearch(val, false);
    } else {
      setAddressResults([]);
    }
  };

  return (
    <PropertySearchSelectorFormView
      onSearch={searchFunc}
      selectedProperties={selectedProperties}
      searchResults={searchResults}
      loading={
        isMapLayerLoading ||
        isLoadingSearchAddress ||
        isLoadingNearestToPoint ||
        isLoadingSitePids ||
        findRegionLoading ||
        findDistrictLoading ||
        findManyLoading
      }
      onSelectedProperties={setSelectedProperties}
      addressResults={addressResults}
      onAddressChange={handleOnAddressChange}
      onAddressSelect={handleOnAddressSelect}
    />
  );
};

export const featureToLocationFeatureDataset = (feature: Feature<Geometry, GeoJsonProperties>) => {
  const center = getFeatureBoundedCenter(feature);

  // TODO: This looks funky. Why is this reconstructing a location dataset from a feature?
  const locationDataSet: SelectedFeatureDataset = {
    parcelFeature: feature as Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>,
    pimsFeature: null,
    location: { lat: center[1], lng: center[0] },
    regionFeature: null,
    fileLocation: null,
    districtFeature: null,
    municipalityFeature: null,
    selectingComponentId: null,
  };
  return locationDataSet;
};

function getRegionAndDisctricts(
  properties: SelectedFeatureDataset[],
  regionSearch: (
    latlng: LatLngLiteral,
    geometryName?: string | undefined,
    spatialReferenceId?: number | undefined,
  ) => Promise<Feature<Geometry, MOT_RegionalBoundary_Feature_Properties> | undefined>,
  districtSearch: (
    latlng: LatLngLiteral,
    geometryName?: string | undefined,
    spatialReferenceId?: number | undefined,
  ) => Promise<Feature<Geometry, MOT_DistrictBoundary_Feature_Properties> | undefined>,
) {
  const latLngDictionary = properties.reduce((accumulator: Dictionary<LatLngLiteral>, property) => {
    if (!exists(property?.location?.lat) || !exists(property?.location?.lng)) {
      return accumulator;
    }

    const latLng: LatLngLiteral = {
      lat: property.location.lat,
      lng: property.location.lng,
    };

    const key = propertyToLatLngKey(property);

    if (!exists(accumulator[key])) {
      accumulator[key] = latLng;
    }

    return accumulator;
  }, {});

  const taskDictionary: Dictionary<RegionDistrictTask> = {};
  for (const entry in latLngDictionary) {
    const latLng = latLngDictionary[entry];

    // call these APIs in parallel - notice there is no "await"
    const regionTask = regionSearch(latLng, 'SHAPE');
    const districtTask = districtSearch(latLng, 'SHAPE');

    taskDictionary[entry] = {
      regionTask: regionTask,
      districtTask: districtTask,
    };
  }

  return taskDictionary;
}

function propertyToLatLngKey(property: SelectedFeatureDataset | null | undefined) {
  if (exists(property.location)) {
    const latLng: LatLngLiteral = {
      lat: property.location.lat,
      lng: property.location.lng,
    };

    const key = `${latLng.lat}-${latLng.lng}`;

    return key;
  }
  return '0-0';
}

/*
 * TODO: PSP-140476
async function getPropertyAddress(
  property: IMapProperty,
  getNearestToPoint: (lng: number, lat: number) => Promise<IGeocoderResponse | undefined>,
) {
  if (property?.latitude === undefined || property?.longitude === undefined) {
    return;
  }

  return await getNearestToPoint(property.longitude, property.latitude);
}
*/

export default PropertySelectorSearchContainer;
