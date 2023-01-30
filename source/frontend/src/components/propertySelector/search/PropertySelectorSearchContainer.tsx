import { useLayerQuery } from 'components/maps/leaflet/LayerPopup';
import { DistrictCodes, RegionCodes } from 'constants/index';
import { FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { IGeocoderResponse } from 'hooks/pims-api/interfaces/IGeocoder';
import { useFullyAttributedParcelMapLayer } from 'hooks/pims-api/useFullyAttributedParcelMapLayer';
import { useGeocoderRepository } from 'hooks/useGeocoderRepository';
import { LatLngLiteral } from 'leaflet';
import debounce from 'lodash/debounce';
import isNumber from 'lodash/isNumber';
import * as React from 'react';
import { useState } from 'react';
import { toast } from 'react-toastify';
import { useTenant } from 'tenants';
import { featuresToIdentifiedMapProperty } from 'utils/mapPropertyUtils';

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
  const { motiRegionLayerUrl, hwyDistrictLayerUrl } = useTenant();

  const {
    getSitePids,
    isLoadingSitePids,
    searchAddress,
    isLoadingSearchAddress,
    getNearestToPoint,
    isLoadingNearestToPoint,
  } = useGeocoderRepository();

  const {
    findOneWhereContainsWrapped: findOneWhereContainsRegion,
    findOneWhereContainsLoading: regionSearchLoading,
  } = useLayerQuery(motiRegionLayerUrl);
  const {
    findOneWhereContainsWrapped: findOneWhereContainsDistrict,
    findOneWhereContainsLoading: districtSearchLoading,
  } = useLayerQuery(hwyDistrictLayerUrl);

  const { parcelMapFullyAttributed } = useTenant();
  const {
    findByPid,
    findByPin,
    findByPlanNumber,
    findByLegalDescription,
    loadingIndicator: isMapLayerLoading,
  } = useFullyAttributedParcelMapLayer(parcelMapFullyAttributed.url, parcelMapFullyAttributed.name);

  React.useEffect(() => {
    const searchFunc = async () => {
      let result: FeatureCollection<Geometry, GeoJsonProperties> | undefined = undefined;
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
          matchRegionAndDistrict(p, findOneWhereContainsRegion, findOneWhereContainsDistrict),
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
    findOneWhereContainsRegion,
    findOneWhereContainsDistrict,
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
        propertyResults.map(p =>
          matchRegionAndDistrict(p, findOneWhereContainsRegion, findOneWhereContainsDistrict),
        ),
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
          regionSearchLoading ||
          districtSearchLoading
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
  ) => Promise<FeatureCollection<Geometry, GeoJsonProperties> | undefined>,
  districtSearch: (
    latlng: LatLngLiteral,
    geometryName?: string | undefined,
    spatialReferenceId?: number | undefined,
  ) => Promise<FeatureCollection<Geometry, GeoJsonProperties> | undefined>,
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

  const regionCollection = await regionTask;
  const districtCollection = await districtTask;

  const region = getProperties(regionCollection);
  const district = getProperties(districtCollection);

  property.region = isNumber(region?.REGION_NUMBER) ? region?.REGION_NUMBER : RegionCodes.Unknown;
  property.regionName = region?.REGION_NAME ?? 'Cannot determine';
  property.district = isNumber(district?.DISTRICT_NUMBER)
    ? district?.DISTRICT_NUMBER
    : DistrictCodes.Unknown;
  property.districtName = district?.DISTRICT_NAME ?? 'Cannot determine';
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

function getProperties(collection: FeatureCollection<Geometry, GeoJsonProperties> | undefined) {
  if (collection?.features?.length === undefined) {
    return {};
  }

  if (collection.features.length > 0) {
    return collection.features[0].properties || {};
  }
}

export default PropertySelectorSearchContainer;
