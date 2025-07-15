import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import L, { DivIcon, GeoJSON, LatLngExpression, Layer, Map, Marker } from 'leaflet';
import ReactDOMServer from 'react-dom/server';
import Supercluster from 'supercluster';

import disposedImage from '@/assets/images/pins/disposed.png';
import disposedHightlightImage from '@/assets/images/pins/disposed-highlight.png';
import landPoiImage from '@/assets/images/pins/land-poi.png';
import landPoiHighlightImage from '@/assets/images/pins/land-poi-highlight.png';
import landRegImage from '@/assets/images/pins/land-reg.png';
import landRegHighlightImage from '@/assets/images/pins/land-reg-highlight.png';
import infoImage from '@/assets/images/pins/marker-info-orange.png';
import infoHighlightImage from '@/assets/images/pins/marker-info-orange-highlight.png';
import shadowImage from '@/assets/images/pins/marker-shadow.png';
import otherInterestImage from '@/assets/images/pins/other-interest.png';
import otherInterestHighlightImage from '@/assets/images/pins/other-interest-highlight.png';
import retiredImage from '@/assets/images/pins/retired.png';
import { ICluster } from '@/components/maps/types';
import DisabledDraftCircleNumber from '@/components/propertySelector/selectedPropertyList/DisabledDraftCircleNumber';
import { DraftCircleNumber } from '@/components/propertySelector/selectedPropertyList/DraftCircleNumber';
import { TANTALIS_CrownSurveyParcels_Feature_Properties } from '@/models/layers/crownLand';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import {
  PIMS_Property_Boundary_View,
  PIMS_Property_Location_Lite_View,
  PIMS_Property_Location_View,
} from '@/models/layers/pimsPropertyLocationView';

// parcel icon (green)
export const parcelIcon = L.icon({
  iconUrl: landRegImage,
  shadowUrl: shadowImage,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

// parcel icon (green) highlighted
export const parcelIconSelect = L.icon({
  iconUrl: landRegHighlightImage,
  shadowUrl: shadowImage,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

// property of interest icon (blue) highlighted
export const propertyOfInterestIcon = L.icon({
  iconUrl: landPoiImage,
  shadowUrl: shadowImage,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

// property of interest icon (blue) highlighted
export const propertyOfInterestIconSelect = L.icon({
  iconUrl: landPoiHighlightImage,
  shadowUrl: shadowImage,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

// other interest icon (purple)
export const otherInterestIcon = L.icon({
  iconUrl: otherInterestImage,
  shadowUrl: shadowImage,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

// other interest icon (purple) highlighted
export const otherInterestIconSelect = L.icon({
  iconUrl: otherInterestHighlightImage,
  shadowUrl: shadowImage,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

// disposed icon (grey)
export const disposedIcon = L.icon({
  iconUrl: disposedImage,
  shadowUrl: shadowImage,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

// disposed icon (grey) highlighted
export const disposedIconSelect = L.icon({
  iconUrl: disposedHightlightImage,
  shadowUrl: shadowImage,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

// retired icon (grey)
export const retiredIcon = L.icon({
  iconUrl: retiredImage,
  shadowUrl: shadowImage,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});
// not owned property icon (orange)
export const notOwnedPropertyIcon = L.icon({
  iconUrl: infoImage,
  shadowUrl: shadowImage,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

// not owned property icon (orange) highlighted
export const notOwnedPropertyIconSelect = L.icon({
  iconUrl: infoHighlightImage,
  shadowUrl: shadowImage,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

type MarkerFeature =
  | PIMS_Property_Location_Lite_View
  | PIMS_Property_Boundary_View
  | PMBC_FullyAttributed_Feature_Properties
  | TANTALIS_CrownSurveyParcels_Feature_Properties;

/**
 * This function defines how GeoJSON points spawn Leaflet layers on the map.
 * It is called internally by the `GeoJSON` leaflet component.
 * @param feature
 * @param latlng
 */
export function pointToLayer<P extends MarkerFeature, C extends Supercluster.ClusterProperties>(
  point: Supercluster.ClusterFeature<C> | Supercluster.PointFeature<P>,
  latlng: LatLngExpression,
): Layer {
  if ('cluster' in point.properties) {
    const cluster = point as Supercluster.ClusterFeature<C>;
    return createClusterMarker(cluster, latlng);
  } else {
    const feature = point as Supercluster.PointFeature<P>;
    // we have a single point to render
    return createSingleMarker(feature, latlng);
  }
}

/**
 * Get an icon type for the specified cluster property details.
 *
 * Precedence (map viewing map markers)
 *
 * 1. Retired (only if advanced filter is open). This will take precedence over other ownership statuses.
 * 2. Core Inventory
 * 3. Other Interest
 * 4. Property of Interest
 * 5. Disposed (only if advanced filter is open)
 */
export function getMarkerIcon(
  feature:
    | Supercluster.PointFeature<PIMS_Property_Location_Lite_View | PIMS_Property_Boundary_View>
    | Feature<Geometry, PIMS_Property_Location_Lite_View>,
  selected: boolean,
  showDisposed = false,
  showRetired = false,
): L.Icon<L.IconOptions> | null {
  if (feature?.properties?.IS_RETIRED === true) {
    if (showRetired) {
      return retiredIcon;
    } else {
      return null;
    }
  } else if (showDisposed && feature?.properties?.IS_DISPOSED === true) {
    if (selected) {
      return disposedIconSelect;
    } else {
      return disposedIcon;
    }
  } else if (feature?.properties?.IS_OWNED === true) {
    if (selected) {
      return parcelIconSelect;
    }
    return parcelIcon;
  } else if (feature?.properties?.IS_OTHER_INTEREST === true) {
    if (selected) {
      return otherInterestIconSelect;
    } else {
      return otherInterestIcon;
    }
  } else if (
    feature?.properties?.HAS_ACTIVE_ACQUISITION_FILE === true ||
    feature?.properties?.HAS_ACTIVE_RESEARCH_FILE === true
  ) {
    if (selected) {
      return propertyOfInterestIconSelect;
    } else {
      return propertyOfInterestIcon;
    }
  }

  return null;
}

/**
 * Get an icon type for the specified cluster property details.
 */
export function getNotOwnerMarkerIcon(selected: boolean): L.Icon<L.IconOptions> {
  if (selected) {
    return notOwnedPropertyIconSelect;
  }
  return notOwnedPropertyIcon;
}

// parcel icon (blue with number) highlighted
export const getDraftIcon = (text: string) => {
  return L.divIcon({
    iconSize: [29, 45],
    iconAnchor: [15, 45],
    popupAnchor: [1, -34],
    shadowSize: [41, 41],
    html: ReactDOMServer.renderToStaticMarkup(<DraftCircleNumber text={text} />),
  });
};

// disabled parcel icon (grey with number) highlighted
export const getDisabledDraftIcon = (text: string) => {
  return L.divIcon({
    iconSize: [29, 40],
    iconAnchor: [15, 40],
    popupAnchor: [1, -34],
    shadowSize: [41, 41],
    html: ReactDOMServer.renderToStaticMarkup(<DisabledDraftCircleNumber text={text} />),
  });
};

/**
 * Creates a map pin for a single point; e.g. a parcel
 * @param feature the geojson object
 * @param latlng the point position
 */
export const createSingleMarker = <P extends MarkerFeature>(
  feature: Supercluster.PointFeature<P>,
  latlng: LatLngExpression,
): Layer => {
  const isOwned = isPimsFeature(feature);

  if (isOwned) {
    const icon = getMarkerIcon(feature, false);
    return icon ? new Marker(latlng, { icon }) : (null as unknown as Layer);
  } else {
    const icon = getNotOwnerMarkerIcon(false);
    return new Marker(latlng, { icon });
  }
};

export const isPimsFeature = (
  feature: Supercluster.PointFeature<MarkerFeature>,
): feature is Supercluster.PointFeature<
  PIMS_Property_Location_View | PIMS_Property_Location_Lite_View | PIMS_Property_Boundary_View
> => {
  return isPimsLocation(feature) || isPimsBoundary(feature) || isPimsPropertyLite(feature);
};

export const isPimsLocation = (
  feature: Supercluster.PointFeature<MarkerFeature>,
): feature is Supercluster.PointFeature<
  PIMS_Property_Location_View | PIMS_Property_Location_Lite_View
> => {
  return feature.id?.toString().startsWith('PIMS_PROPERTY_LOCATION_') ?? false;
};

export const isPimsBoundary = (
  feature: Supercluster.PointFeature<MarkerFeature>,
): feature is Supercluster.PointFeature<PIMS_Property_Boundary_View> => {
  return feature.id?.toString().startsWith('PIMS_PROPERTY_BOUNDARY_') ?? false;
};

export const isPimsPropertyLite = (
  feature: Supercluster.PointFeature<MarkerFeature>,
): feature is Supercluster.PointFeature<PIMS_Property_Boundary_View> => {
  return feature.id?.toString().startsWith('PIMS_PROPERTY_LITE_') ?? false;
};

export const isFaParcelMap = (
  feature: Supercluster.PointFeature<MarkerFeature>,
): feature is Supercluster.PointFeature<PMBC_FullyAttributed_Feature_Properties> => {
  return feature.id?.toString().startsWith('WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_') ?? false;
};

// Internal cache of cluster icons to avoid re-creating the same icon over and over again.
const iconsCache: Record<number, DivIcon> = {};

/**
 * Creates a marker for clusters on the map
 * @param feature the cluster geojson object
 * @param latlng the cluster position
 */
export function createClusterMarker<P extends GeoJsonProperties>(
  feature: ICluster<P>,
  latlng: LatLngExpression,
): Layer {
  const {
    cluster: isCluster,
    point_count: count,
    point_count_abbreviated: displayValue,
  } = feature?.properties as Supercluster.ClusterProperties;

  if (!isCluster) {
    return null as unknown as Layer;
  }

  const size = count < 100 ? 'small' : count < 1000 ? 'medium' : 'large';

  if (!iconsCache[count]) {
    iconsCache[count] = new DivIcon({
      html: `<div><span>${displayValue}</span></div>`,
      className: `marker-cluster marker-cluster-${size}`,
      iconSize: [40, 40],
    });
  }

  const icon = iconsCache[count];
  return new Marker(latlng, { icon });
}

/** Zooms to a cluster */
export function zoomToCluster<P extends GeoJsonProperties>(
  cluster: ICluster<P>,
  expansionZoom: number,
  map: Map,
) {
  const latlng = GeoJSON.coordsToLatLng(cluster?.geometry?.coordinates as [number, number]);
  map?.setView(latlng, expansionZoom, { animate: true });
}
