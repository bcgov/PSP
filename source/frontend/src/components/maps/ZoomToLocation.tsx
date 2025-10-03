import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import { geoJSON, LatLngBounds } from 'leaflet';
import { useCallback, useMemo } from 'react';
import { FaSearchPlus } from 'react-icons/fa';
import { PiCornersOut } from 'react-icons/pi';

import { LinkButton } from '@/components/common/buttons/LinkButton';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { ParcelDataset } from '@/features/properties/parcelList/models';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';
import {
  boundaryFromFileProperty,
  exists,
  latLngLiteralToGeometry,
  pimsGeomeryToGeometry,
} from '@/utils';

export interface IUpdatePropertiesProps {
  formProperties?: PropertyForm[] | null;
  pimsProperties?: ApiGen_Concepts_Property[] | null;
  pimsFileProperties?: ApiGen_Concepts_FileProperty[] | null;
  parcelDataset?: ParcelDataset | null;
  featureCollection?: Feature<Geometry, GeoJsonProperties>[] | null;
  pimsFeatures?: Feature<Geometry, PIMS_Property_Location_View>[] | null;

  geometry?: Geometry | null;
  icon: ZoomIconType;
}

export enum ZoomIconType {
  single,
  area,
}

export const ZoomToLocation: React.FunctionComponent<IUpdatePropertiesProps> = ({
  formProperties,
  pimsProperties,
  pimsFileProperties,
  parcelDataset,
  featureCollection,
  geometry,
  icon,
}) => {
  const { requestFlyToBounds } = useMapStateMachine();

  const bounds: LatLngBounds | null = useMemo(() => {
    const propertyLocations: Geometry[] =
      formProperties?.map(p => p?.polygon ?? latLngLiteralToGeometry(p?.fileLocation)) || [];

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

    if (exists(parcelDataset?.pmbcFeature?.geometry)) {
      const pmbcGeometry = parcelDataset.pmbcFeature.geometry;
      const pmbcBounds = geoJSON(pmbcGeometry)?.getBounds();
      if (exists(pmbcBounds) && pmbcBounds.isValid()) {
        propertyLocations.push(pmbcGeometry);
      }
    } else if (exists(parcelDataset?.pimsFeature.geometry)) {
      const pimsFeatureGeometry = parcelDataset.pimsFeature.geometry;
      const pimsBounds = geoJSON(pimsFeatureGeometry)?.getBounds();
      if (exists(pimsBounds) && pimsBounds.isValid()) {
        propertyLocations.push(pimsFeatureGeometry);
      }
    } else if (exists(parcelDataset?.location)) {
      propertyLocations.push(latLngLiteralToGeometry(parcelDataset.location));
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
    parcelDataset?.location,
    parcelDataset?.pimsFeature,
    parcelDataset?.pmbcFeature,
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
      <LinkButton title="Fit boundaries button" onClick={fitBoundaries} disabled={!isValid}>
        {icon === ZoomIconType.single && <FaSearchPlus size={18} />}
        {icon === ZoomIconType.area && <PiCornersOut size={18} />}
      </LinkButton>
    </TooltipWrapper>
  );
};
