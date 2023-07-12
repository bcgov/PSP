import { LatLngLiteral } from 'leaflet';
import { Marker } from 'react-leaflet';
import { PointFeature } from 'supercluster';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';

import { getMarkerIcon, getNotOwnerMarkerIcon } from '../Layers/util';

interface SinlePropertyMarkerProps {
  pointFeature: PointFeature<PIMS_Property_Location_View | PMBC_FullyAttributed_Feature_Properties>;
  markerPosition: LatLngLiteral;
  isSelected: boolean;
  isOwned: boolean;
}

const SinglePropertyMarker: React.FC<React.PropsWithChildren<SinlePropertyMarkerProps>> = ({
  pointFeature,
  markerPosition,
  isSelected,
  isOwned,
}) => {
  const mapMachine = useMapStateMachine();

  const getIcon = () => {
    if (isOwned) {
      const pimsFeature = pointFeature as PointFeature<PIMS_Property_Location_View>;
      return getMarkerIcon(pimsFeature, isSelected);
    } else {
      return getNotOwnerMarkerIcon(isSelected);
    }
  };

  const onMarkerClicked = () => {
    const clusterId = pointFeature.id?.toString() || 'ERROR_NO_ID';
    const [longitude, latitude] = pointFeature.geometry.coordinates;

    const latlng = { lat: latitude, lng: longitude };
    if (isOwned) {
      mapMachine.mapMarkerClick({
        clusterId: clusterId,
        latlng: latlng,
        pimsFeature: null,
        fullyAttributedFeature: (
          pointFeature as PointFeature<PMBC_FullyAttributed_Feature_Properties>
        ).properties,
      });
    } else {
      mapMachine.mapMarkerClick({
        clusterId: clusterId,
        latlng: latlng,
        pimsFeature: (pointFeature as PointFeature<PIMS_Property_Location_View>).properties,
        fullyAttributedFeature: null,
      });
    }
  };

  return (
    // render single marker, not in a cluster
    <Marker
      {...pointFeature.properties}
      position={markerPosition}
      icon={getIcon()}
      eventHandlers={{
        click: e => {
          onMarkerClicked();
        },
      }}
    />
  );
};

export default SinglePropertyMarker;
