import { Feature, FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import debounce from 'lodash/debounce';
import isNumber from 'lodash/isNumber';
import { useEffect, useRef, useState } from 'react';
import { toast } from 'react-toastify';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { IGeocoderResponse } from '@/hooks/pims-api/interfaces/IGeocoder';
import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { usePimsPropertyLayer } from '@/hooks/repositories/mapLayer/usePimsPropertyLayer';
import { useGeocoderRepository } from '@/hooks/useGeocoderRepository';
import { MOT_DistrictBoundary_Feature_Properties } from '@/models/layers/motDistrictBoundary';
import { MOT_RegionalBoundary_Feature_Properties } from '@/models/layers/motRegionalBoundary';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';
import { getFeatureBoundedCenter } from '@/utils';

import { ILayerSearchCriteria, IMapProperty } from '../models';
import PropertySearchSelectorFormView from './PropertySearchSelectorFormView';

export interface IPropertySelectorSearchContainerProps {
  selectedProperties: LocationFeatureDataset[];
  setSelectedProperties: (properties: LocationFeatureDataset[]) => void;
}

export const PropertySelectorSearchContainer: React.FC<IPropertySelectorSearchContainerProps> = ({
  selectedProperties,
  setSelectedProperties,
}) => {
  const [layerSearch, setLayerSearch] = useState<ILayerSearchCriteria | undefined>();
  const [searchResults, setSearchResults] = useState<LocationFeatureDataset[]>([]);
  const [addressResults, setAddressResults] = useState<IGeocoderResponse[]>([]);

  const pimsPropertyLayerService = usePimsPropertyLayer();
  const loadPimsProperties = pimsPropertyLayerService.loadPropertyLayer.execute;

  const {
    getSitePids,
    isLoadingSitePids,
    searchAddress,
    isLoadingSearchAddress,
    getNearestToPoint,
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
  } = useFullyAttributedParcelMapLayer();

  useEffect(() => {
    const searchFunc = async () => {
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
      }

      // match the region and district for all found properties
      if (result?.features?.length !== undefined && result?.features?.length <= 15) {
        const matchTasks = result.features.map(p =>
          matchRegionAndDistrict(featureToLocationFeatureDataset(p), findRegion, findDistrict),
        );

        const getAddressTasks = result.features.map(p => {
          const latLngArray = getFeatureBoundedCenter(p);
          return getPropertyAddress(
            { latitude: latLngArray[1], longitude: latLngArray[0] },
            getNearestToPoint,
          );
        });

        const getPimsTasks = result.features.map(p => {
          return loadPimsProperties({
            PID: p?.properties?.PID_NUMBER?.toString(),
            PIN: p?.properties?.PIN?.toString(),
          });
        });

        const addressResults = await Promise.all(getAddressTasks);
        const regionDistrictResults = await Promise.all(matchTasks);
        const pimsResults = await Promise.all(getPimsTasks);

        const locations = result.features.map((p, i) => {
          const foundProperty = featureToLocationFeatureDataset(p);
          foundProperty.regionFeature = regionDistrictResults[i]?.regionFeature;
          foundProperty.districtFeature = regionDistrictResults[i]?.districtFeature;
          foundProperty.pimsFeature = pimsResults[i]?.features?.length
            ? pimsResults[i]?.features[0]
            : ({
                properties: {
                  STREET_ADDRESS_1: addressResults[i]?.fullAddress,
                },
              } as Feature<Geometry, PIMS_Property_Location_View>);
          return foundProperty;
        }) as LocationFeatureDataset[];
        setSearchResults(locations);
      } else {
        const locations = result?.features?.map(p => featureToLocationFeatureDataset(p));
        setSearchResults(locations ?? []);
      }
    };
    searchFunc();
  }, [
    findByLegalDescription,
    findByPid,
    findByPin,
    findByPlanNumber,
    getNearestToPoint,
    layerSearch,
    findRegion,
    findDistrict,
    loadPimsProperties,
  ]);

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

      let propertyResults: LocationFeatureDataset[] = [];
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
      await Promise.all(
        propertyResults.map(p => matchRegionAndDistrict(p, findRegion, findDistrict)),
      );

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
      onSearch={setLayerSearch}
      selectedProperties={selectedProperties}
      search={layerSearch}
      searchResults={searchResults}
      loading={
        isMapLayerLoading ||
        isLoadingSearchAddress ||
        isLoadingNearestToPoint ||
        isLoadingSitePids ||
        findRegionLoading ||
        findDistrictLoading
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
  return {
    parcelFeature: feature,
    selectingComponentId: null,
    pimsFeature: null,
    location: { lat: center[1], lng: center[0] },
    regionFeature: null,
    districtFeature: null,
    municipalityFeature: null,
    highwayFeature: null,
  } as LocationFeatureDataset;
};

// Not thread safe. Modifies the passed property.
async function matchRegionAndDistrict(
  property: LocationFeatureDataset,
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
  if (property?.location?.lat === undefined || property?.location?.lng === undefined) {
    return;
  }

  const latLng: LatLngLiteral = {
    lat: property?.location?.lat,
    lng: property?.location?.lng,
  };

  // call these APIs in parallel - notice there is no "await"
  const regionTask = regionSearch(latLng, 'SHAPE');
  const districtTask = districtSearch(latLng, 'SHAPE');

  const regionFeature = await regionTask;
  const districtFeature = await districtTask;
  return {
    ...property,
    regionFeature: { ...regionFeature },
    districtFeature: { ...districtFeature },
  };
}

async function getPropertyAddress(
  property: IMapProperty,
  getNearestToPoint: (lng: number, lat: number) => Promise<IGeocoderResponse | undefined>,
) {
  if (property?.latitude === undefined || property?.longitude === undefined) {
    return;
  }

  return await getNearestToPoint(property.longitude, property.latitude);
}

export default PropertySelectorSearchContainer;
