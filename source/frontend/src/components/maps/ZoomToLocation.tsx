import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import { geoJSON, LatLngBounds } from 'leaflet';
import { useCallback, useMemo } from 'react';
import { FaSearchPlus } from 'react-icons/fa';
import { PiCornersOut } from 'react-icons/pi';

import { LinkButton } from '@/components/common/buttons/LinkButton';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';
import {
  boundaryFromFileProperty,
  exists,
  firstValidOrNull,
  latLngLiteralToGeometry,
  pimsGeomeryToGeometry,
} from '@/utils';

import { LocationFeatureDataset } from '../common/mapFSM/useLocationFeatureLoader';
import TooltipIcon from '../common/TooltipIcon';

export interface IZoomToLocationProps {
  formProperties?: PropertyForm[] | null;
  pimsProperties?: ApiGen_Concepts_Property[] | null;
  pimsFileProperties?: ApiGen_Concepts_FileProperty[] | null;
  locationFeatureDataset?: LocationFeatureDataset | null;
  featureCollection?: Feature<Geometry, GeoJsonProperties>[] | null;
  pimsFeatures?: Feature<Geometry, PIMS_Property_Location_View>[] | null;

  geometry?: Geometry | null;
  icon: ZoomIconType;
}

export enum ZoomIconType {
  single,
  area,
}

export const ZoomToLocation: React.FunctionComponent<IZoomToLocationProps> = ({
  formProperties,
  pimsProperties,
  pimsFileProperties,
  locationFeatureDataset,
  featureCollection,
  geometry,
  icon,
}) => {
  const { requestFlyToBounds } = useMapStateMachine();

  const bounds: LatLngBounds | null = useMemo(() => {
    const propertyLocations: Geometry[] =
      formProperties?.map(
        p => p?.polygon ?? latLngLiteralToGeometry({ lat: p?.latitude, lng: p?.longitude }),
      ) || [];

    if (exists(geometry)) {
      propertyLocations.push(geometry);
    }

    if (exists(pimsProperties)) {
      pimsProperties.forEach(pimsProperty => {
        const pimsGeometry =
          pimsProperty?.boundary ?? pimsGeomeryToGeometry(pimsProperty?.location);
        const pimsBounds = geoJSON(pimsGeometry)?.getBounds();
        if (exists(pimsBounds) && pimsBounds.isValid()) {
          propertyLocations.push(pimsGeometry);
        }
      });
    }

    if (exists(pimsFileProperties)) {
      pimsFileProperties.forEach(pimsFileProperty => {
        const pimsFileGeometry = boundaryFromFileProperty(pimsFileProperty);
        const pimsFileBounds = geoJSON(pimsFileGeometry)?.getBounds();
        if (exists(pimsFileBounds) && pimsFileBounds.isValid()) {
          propertyLocations.push(pimsFileGeometry);
        }
      });
    }

    const validParcelFeature = firstValidOrNull(locationFeatureDataset?.parcelFeatures, exists);
    const validPimsFeature = firstValidOrNull(locationFeatureDataset?.pimsFeatures, exists);
    if (exists(validParcelFeature?.geometry)) {
      const pmbcGeometry = validParcelFeature.geometry;
      const pmbcBounds = geoJSON(pmbcGeometry)?.getBounds();
      if (exists(pmbcBounds) && pmbcBounds.isValid()) {
        propertyLocations.push(pmbcGeometry);
      }
    } else if (exists(validPimsFeature?.geometry)) {
      const pimsFeatureGeometry = validPimsFeature.geometry;
      const pimsBounds = geoJSON(pimsFeatureGeometry)?.getBounds();
      if (exists(pimsBounds) && pimsBounds.isValid()) {
        propertyLocations.push(pimsFeatureGeometry);
      }
    } else if (exists(locationFeatureDataset?.location)) {
      propertyLocations.push(latLngLiteralToGeometry(locationFeatureDataset.location));
    }

    if (exists(featureCollection)) {
      featureCollection.forEach(feature => {
        const featureGeometry = feature?.geometry ?? null;
        const featureBounds = geoJSON(featureGeometry)?.getBounds();
        if (exists(featureBounds) && featureBounds.isValid()) {
          propertyLocations.push(featureGeometry);
        }
      });
    }

    if (!exists(propertyLocations)) {
      return null;
    }

    const newBounds = geoJSON(propertyLocations.filter(exists))?.getBounds() ?? null;
    return newBounds;
  }, [
    featureCollection,
    formProperties,
    geometry,
    locationFeatureDataset?.location,
    locationFeatureDataset?.parcelFeatures,
    locationFeatureDataset?.pimsFeatures,
    pimsFileProperties,
    pimsProperties,
  ]);

  const isValid = useMemo(() => {
    return exists(bounds) && bounds.isValid();
  }, [bounds]);

  const fitBoundaries = useCallback(() => {
    if (isValid) {
      requestFlyToBounds(bounds);
    }
  }, [isValid, bounds, requestFlyToBounds]);

  // Zoom to File Properties Boundary
  // Zoom to latlng
  // Zoom to boundary
  return (
    <TooltipWrapper tooltip="Zoom" tooltipId="zoom-tooltip">
      <>
        {isValid === true ? (
          <LinkButton title="Fit boundaries" onClick={fitBoundaries} disabled={!isValid}>
            {icon === ZoomIconType.single && <FaSearchPlus size={18} />}
            {icon === ZoomIconType.area && <PiCornersOut size={18} />}
          </LinkButton>
        ) : (
          <TooltipIcon
            toolTipId={'no-location-tooltip'}
            toolTip={'No valid location'}
            placement={'auto'}
          />
        )}
      </>
    </TooltipWrapper>
  );
};
