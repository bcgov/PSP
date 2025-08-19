import { LatLngLiteral, LeafletMouseEvent } from 'leaflet';
import { useRef } from 'react';
import { Marker, useMap } from 'react-leaflet';
import { PointFeature } from 'supercluster';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { TANTALIS_CrownSurveyParcels_Feature_Properties } from '@/models/layers/crownLand';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import {
  PIMS_Property_Boundary_View,
  PIMS_Property_Location_Lite_View,
} from '@/models/layers/pimsPropertyLocationView';
import { useTenant } from '@/tenants';

import {
  getMarkerIcon,
  getNotOwnerMarkerIcon,
  isFaParcelMap,
  isPimsBoundary,
  isPimsFeature,
  isPimsLocation,
  isPimsPropertyLite,
} from '../Layers/util';

interface SinglePropertyMarkerProps {
  pointFeature: PointFeature<
    | PIMS_Property_Location_Lite_View
    | PIMS_Property_Boundary_View
    | PMBC_FullyAttributed_Feature_Properties
    | TANTALIS_CrownSurveyParcels_Feature_Properties
  >;
  markerPosition: LatLngLiteral;
  isSelected: boolean;
}

const SinglePropertyMarker: React.FC<React.PropsWithChildren<SinglePropertyMarkerProps>> = ({
  pointFeature,
  markerPosition,
  isSelected,
}) => {
  const mapMachine = useMapStateMachine();
  const map = useMap();

  const getIcon = (
    feature: PointFeature<
      | PIMS_Property_Location_Lite_View
      | PIMS_Property_Boundary_View
      | PMBC_FullyAttributed_Feature_Properties
      | TANTALIS_CrownSurveyParcels_Feature_Properties
    >,
    isSelected: boolean,
    showDisposed: boolean,
    showRetired: boolean,
  ): L.Icon<L.IconOptions> | null => {
    const isOwned = isPimsFeature(feature);
    if (isOwned) {
      return getMarkerIcon(feature, isSelected, showDisposed, showRetired);
    } else {
      return getNotOwnerMarkerIcon(isSelected);
    }
  };

  const timer = useRef(null);
  const { doubleClickInterval } = useTenant();

  const handleMarkerClickEvent = () => {
    if (timer?.current !== null) {
      return;
    }
    timer.current = setTimeout(() => {
      onMarkerClicked();
      timer.current = null;
    }, doubleClickInterval ?? 250);
  };

  const handleDoubleClickEvent = () => {
    clearTimeout(timer?.current);
    timer.current = null;
  };

  const onMarkerClicked = () => {
    const clusterId = pointFeature.id?.toString() || 'ERROR_NO_ID';
    const [longitude, latitude] = pointFeature.geometry.coordinates;

    const latlng = { lat: latitude, lng: longitude };
    if (isPimsLocation(pointFeature) || isPimsPropertyLite(pointFeature)) {
      mapMachine.mapMarkerClick({
        clusterId: clusterId,
        latlng: latlng,
        pimsLocationFeature: pointFeature.properties,
        pimsBoundaryFeature: null,
        fullyAttributedFeature: null,
      });
    } else if (isPimsBoundary(pointFeature)) {
      mapMachine.mapMarkerClick({
        clusterId: clusterId,
        latlng: latlng,
        pimsLocationFeature: null,
        pimsBoundaryFeature: pointFeature.properties,
        fullyAttributedFeature: null,
      });
    } else if (isFaParcelMap(pointFeature)) {
      mapMachine.mapMarkerClick({
        clusterId: clusterId,
        latlng: latlng,
        pimsLocationFeature: null,
        pimsBoundaryFeature: null,
        fullyAttributedFeature: pointFeature.properties,
      });
    }
    mapMachine.requestCenterToLocation(latlng);
  };

  const icon = getIcon(pointFeature, isSelected, mapMachine.showDisposed, mapMachine.showRetired);

  // render single marker, not in a cluster
  return icon ? (
    <Marker
      {...pointFeature.properties}
      position={markerPosition}
      icon={icon}
      eventHandlers={{
        dblclick: (event: LeafletMouseEvent) => {
          map.fireEvent('dblclick', event); // bubble up double click events to the map.
          handleDoubleClickEvent();
        },
        click: () => {
          handleMarkerClickEvent();
        },
      }}
    />
  ) : null;
};

export default SinglePropertyMarker;
