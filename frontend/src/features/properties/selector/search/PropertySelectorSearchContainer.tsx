import { AxiosResponse } from 'axios';
import {
  HWY_DISTRICT_LAYER_URL,
  IUserLayerQuery,
  MOTI_REGION_LAYER_URL,
  useLayerQuery,
} from 'components/maps/leaflet/LayerPopup';
import { DistrictCodes, RegionCodes } from 'constants/index';
import { FeatureCollection, GeoJsonProperties, Geometry, Polygon } from 'geojson';
import { useApiGeocoder } from 'hooks/pims-api/useApiGeocoder';
import { useFullyAttributedParcelMapLayer } from 'hooks/pims-api/useFullyAttributedParcelMapLayer';
import { IGeocoderResponse } from 'hooks/useApi';
import { LatLngLiteral } from 'leaflet';
import debounce from 'lodash/debounce';
import isNumber from 'lodash/isNumber';
import noop from 'lodash/noop';
import polylabel from 'polylabel';
import * as React from 'react';
import { useState } from 'react';
import { toast } from 'react-toastify';
import { useTenant } from 'tenants';

import { MapClickMonitor } from '../components/MapClickMonitor';
import { ILayerSearchCriteria, IMapProperty } from '../models';
import { getPropertyIdentifier } from '../utils';
import PropertySearchSelectorFormView from './PropertySearchSelectorFormView';

export interface IPropertySelectorSearchContainerProps {
  selectedProperties: IMapProperty[];
  setSelectedProperties: (properties: IMapProperty[]) => void;
}

export const PropertySelectorSearchContainer: React.FunctionComponent<IPropertySelectorSearchContainerProps> = ({
  selectedProperties,
  setSelectedProperties,
}) => {
  const [layerSearch, setLayerSearch] = useState<ILayerSearchCriteria | undefined>();
  const [searchResults, setSearchResults] = useState<IMapProperty[]>([]);
  const [addressResults, setAddressResults] = useState<IGeocoderResponse[]>([]);

  const { getSitePids, searchAddress, getNearestToPoint } = useApiGeocoder();

  const regionService = useLayerQuery(MOTI_REGION_LAYER_URL);
  const districtService = useLayerQuery(HWY_DISTRICT_LAYER_URL);

  const { parcelMapFullyAttributed } = useTenant();
  const {
    findByPid,
    findByPin,
    findByPlanNumber,
    findByLegalDescription,
    loadingIndicator,
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
      }

      // match the region and district for all found properties
      const foundProperties = featuresToIdentifiedMapProperty(result) ?? [];
      await Promise.all(
        foundProperties.map(p => matchRegionAndDistrict(p, regionService, districtService)),
      );

      await Promise.all(foundProperties.map(p => matchAddress(p, getNearestToPoint)));

      setSearchResults(foundProperties);
    };
    searchFunc();
  }, [
    findByLegalDescription,
    findByPid,
    findByPin,
    findByPlanNumber,
    layerSearch,
    districtService,
    regionService,
  ]);

  const handleOnAddressSelect = async (selectedItem: IGeocoderResponse) => {
    if (!selectedItem.siteId) {
      toast.info('No site id found for selected address');
      return;
    }
    const pidResults = await getSitePids(selectedItem.siteId);

    if (pidResults && pidResults.data?.pids) {
      if (pidResults.data.pids.length > 50) {
        toast.error('Maximum PID search size exceeded for selected address');
        return;
      }

      const findByPidCalls: Promise<
        FeatureCollection<Geometry, GeoJsonProperties> | undefined
      >[] = [];
      pidResults.data.pids.forEach(async (pid: string) => {
        findByPidCalls.push(findByPid(pid));
      });

      const responses = await Promise.all(findByPidCalls);

      let propertyResults: IMapProperty[] = [];
      responses?.forEach((item: FeatureCollection<Geometry, GeoJsonProperties> | undefined) => {
        if (item) {
          propertyResults = propertyResults.concat(featuresToIdentifiedMapProperty(item) ?? []);
        }
      });

      // match the region and district for all found properties
      await Promise.all(
        propertyResults.map(p => matchRegionAndDistrict(p, regionService, districtService)),
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
          setAddressResults(addresses.data);
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
        loading={loadingIndicator}
        onSelectedProperties={setSelectedProperties}
        addressResults={addressResults}
        onAddressChange={handleOnAddressChange}
        onAddressSelect={handleOnAddressSelect}
      />

      <MapClickMonitor addProperty={noop} />
    </>
  );
};

export const featuresToIdentifiedMapProperty = (
  values: FeatureCollection<Geometry, GeoJsonProperties> | undefined,
) =>
  values?.features
    ?.filter(feature => feature?.geometry?.type === 'Polygon')
    .map(
      (feature): IMapProperty => {
        const boundedCenter = polylabel((feature.geometry as Polygon).coordinates);
        const property: IMapProperty = {
          pid: feature?.properties?.PID?.toString() ?? '',
          pin: feature?.properties?.PIN?.toString() ?? '',
          planNumber: feature?.properties?.PLAN_NUMBER?.toString() ?? '',
          latitude: boundedCenter[1],
          longitude: boundedCenter[0],
          legalDescription: feature?.properties?.LEGAL_DESCRIPTION,
        };
        property.id = getPropertyIdentifier(property);
        return property;
      },
    );

async function matchRegionAndDistrict(
  property: IMapProperty,
  regionService: IUserLayerQuery,
  districtService: IUserLayerQuery,
) {
  if (property?.latitude === undefined || property?.longitude === undefined) {
    return;
  }

  const latLng: LatLngLiteral = {
    lat: property.latitude,
    lng: property.longitude,
  };

  // call these APIs in parallel - notice there is no "await"
  const regionTask = regionService.findMetadataByLocation(latLng, 'GEOMETRY');
  const districtTask = districtService.findMetadataByLocation(latLng, 'GEOMETRY');

  const region = await regionTask;
  const district = await districtTask;

  property.region = isNumber(region.REGION_NUMBER) ? region.REGION_NUMBER : RegionCodes.Unknown;
  property.regionName = region.REGION_NAME ?? 'Cannot determine';
  property.district = isNumber(district.DISTRICT_NUMBER)
    ? district.DISTRICT_NUMBER
    : DistrictCodes.Unknown;
  property.districtName = district.DISTRICT_NAME ?? 'Cannot determine';
}

async function matchAddress(
  property: IMapProperty,
  getNearestToPoint: (lng: number, lat: number) => Promise<AxiosResponse<IGeocoderResponse, any>>,
) {
  if (property?.latitude === undefined || property?.longitude === undefined) {
    return property;
  }

  const queryResult = await getNearestToPoint(property.longitude, property.latitude);
  property.address = queryResult?.data?.fullAddress;
}

export default PropertySelectorSearchContainer;
