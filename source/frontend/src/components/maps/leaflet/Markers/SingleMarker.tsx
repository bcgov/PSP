import { LatLngLiteral } from 'leaflet';
import { Marker } from 'react-leaflet';
import { PointFeature } from 'supercluster';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { PMBC_Feature_Properties } from '@/models/layers/parcelMapBC';
import {
  PIMS_Property_Boundary_View,
  PIMS_Property_Location_View,
} from '@/models/layers/pimsPropertyLocationView';

import {
  getMarkerIcon,
  getNotOwnerMarkerIcon,
  isParcelMap,
  isPimsBoundary,
  isPimsFeature,
  isPimsLocation,
} from '../Layers/util';

interface SinglePropertyMarkerProps {
  pointFeature: PointFeature<
    PIMS_Property_Location_View | PIMS_Property_Boundary_View | PMBC_Feature_Properties
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

  const getIcon = (
    feature: PointFeature<
      PIMS_Property_Location_View | PIMS_Property_Boundary_View | PMBC_Feature_Properties
    >,
    isSelected: boolean,
    showDisposed: boolean,
  ): L.Icon<L.IconOptions> | null => {
    const isOwned = isPimsFeature(feature);
    if (isOwned) {
      return getMarkerIcon(feature, isSelected, showDisposed);
    } else {
      return getNotOwnerMarkerIcon(isSelected);
    }
  };

  const onMarkerClicked = () => {
    const clusterId = pointFeature.id?.toString() || 'ERROR_NO_ID';
    const [longitude, latitude] = pointFeature.geometry.coordinates;

    const latlng = { lat: latitude, lng: longitude };
    if (isPimsLocation(pointFeature)) {
      mapMachine.mapMarkerClick({
        clusterId: clusterId,
        latlng: latlng,
        pimsLocationFeature: pointFeature.properties,
        pimsBoundaryFeature: null,
        pmbcFeature: null,
      });
    } else if (isPimsBoundary(pointFeature)) {
      mapMachine.mapMarkerClick({
        clusterId: clusterId,
        latlng: latlng,
        pimsLocationFeature: null,
        pimsBoundaryFeature: pointFeature.properties,
        pmbcFeature: null,
      });
    } else if (isParcelMap(pointFeature)) {
      mapMachine.mapMarkerClick({
        clusterId: clusterId,
        latlng: latlng,
        pimsLocationFeature: null,
        pimsBoundaryFeature: null,
        pmbcFeature: pointFeature.properties,
      });
    }
  };

  const icon = getIcon(pointFeature, isSelected, mapMachine.showDisposed);

  // render single marker, not in a cluster
  return icon ? (
    <Marker
      {...pointFeature.properties}
      position={markerPosition}
      icon={icon}
      eventHandlers={{
        click: () => {
          onMarkerClicked();
        },
      }}
    />
  ) : null;
};

export default SinglePropertyMarker;
