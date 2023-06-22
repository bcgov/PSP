import { Feature, FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import debounce from 'lodash/debounce';
import isNumber from 'lodash/isNumber';
import * as React from 'react';
import { useState } from 'react';
import { toast } from 'react-toastify';

import { DistrictCodes, RegionCodes } from '@/constants/index';
import { IGeocoderResponse } from '@/hooks/pims-api/interfaces/IGeocoder';
import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { useGeocoderRepository } from '@/hooks/useGeocoderRepository';
import { MOT_DistrictBoundary_Feature_Properties } from '@/models/layers/motDistrictBoundary';
import { MOT_RegionalBoundary_Feature_Properties } from '@/models/layers/motRegionalBoundary';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { featuresToIdentifiedMapProperty } from '@/utils/mapPropertyUtils';

import { ILayerSearchCriteria, IMapProperty } from '../models';
import PropertySearchSelectorFormView from './PropertySearchSelectorFormView';

export interface IPropertySelectorSearchContainerProps {
  selectedProperties: IMapProperty[];
  setSelectedProperties: (properties: IMapProperty[]) => void;
}

export const PropertySelectorSearchContainer: React.FunctionComponent<
  React.PropsWithChildren<IPropertySelectorSearchContainerProps>
> = ({ selectedProperties, setSelectedProperties }) => {
  const [layerSearch, setLayerSearch] = useState<ILayerSearchCriteria | undefined>();
  const [searchResults, setSearchResults] = useState<IMapProperty[]>([]);
  const [addressResults, setAddressResults] = useState<IGeocoderResponse[]>([]);

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

  React.useEffect(() => {
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

      const foundProperties = featuresToIdentifiedMapProperty(result) ?? [];
      // match the region and district for all found properties
      if (result?.features?.length !== undefined && result?.features?.length <= 15) {
        var matchTask = foundProperties.map(p =>
          matchRegionAndDistrict(p, findRegion, findDistrict),
        );

        var getAddressTasks = foundProperties.map(p => getPropertyAddress(p, getNearestToPoint));

        var addresses = await Promise.all(getAddressTasks);
        await Promise.all(matchTask);
        foundProperties.forEach((p, i) => {
          p.address = addresses[i]?.fullAddress;
        });
      }

      setSearchResults(foundProperties);
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

      let propertyResults: IMapProperty[] = [];
      responses?.forEach((item: FeatureCollection<Geometry, GeoJsonProperties> | undefined) => {
        if (item) {
          propertyResults = propertyResults.concat(
            featuresToIdentifiedMapProperty(item, selectedItem.fullAddress) ?? [],
          );
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

  const debouncedSearch = React.useRef(
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
    <>
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
    </>
  );
};

// Not thread safe. Modifies the passed property.
async function matchRegionAndDistrict(
  property: IMapProperty,
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
  if (property?.latitude === undefined || property?.longitude === undefined) {
    return;
  }

  const latLng: LatLngLiteral = {
    lat: property.latitude,
    lng: property.longitude,
  };

  // call these APIs in parallel - notice there is no "await"
  const regionTask = regionSearch(latLng, 'GEOMETRY');
  const districtTask = districtSearch(latLng, 'GEOMETRY');

  const regionFeature = await regionTask;
  const districtFeature = await districtTask;

  const regionProperties = regionFeature?.properties;
  const districtProperties = districtFeature?.properties;

  property.region = isNumber(regionProperties?.REGION_NUMBER)
    ? regionProperties?.REGION_NUMBER
    : RegionCodes.Unknown;
  property.regionName = regionProperties?.REGION_NAME ?? 'Cannot determine';
  property.district = isNumber(districtProperties?.DISTRICT_NUMBER)
    ? districtProperties?.DISTRICT_NUMBER
    : DistrictCodes.Unknown;
  property.districtName = districtProperties?.DISTRICT_NAME ?? 'Cannot determine';
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
