import L from 'leaflet';
import keys from 'lodash/keys';
import queryString from 'query-string';
import * as React from 'react';
import Col from 'react-bootstrap/Col';
import ListGroup from 'react-bootstrap/ListGroup';
import Row from 'react-bootstrap/Row';
import { useMap } from 'react-leaflet';
import { Link, useLocation } from 'react-router-dom';
import styled from 'styled-components';

export const LayerPopupTitle = styled('div')`
  padding: 1.6rem;
  font-weight: 800;
`;

export const MenuRow = styled(Row)`
  text-align: center;
  padding-bottom: 1rem;
`;

export const StyledLink = styled(Link)`
  padding: 0 0.4rem;
`;

export type PopupContentConfig = {
  [key: string]: {
    label: string;
    display: (data: { [key: string]: string }) => string | React.ReactNode;
  };
};

export interface IPopupContentProps {
  /**
   * Data coming from the GeoJSON feature.properties
   * @property
   * @example
   * feature: {
   *  properties: {
   *    'ADMIN_AREA_ID': 1,
   *    'ADMIN_AERA_NAME: 'West Saanich'
   *  }
   * }
   */
  data: { [key: string]: string };
  /**
   * A configuration used to display the properties fields in the popup content
   * @property
   * @example
   * {ADMIN_AREA_SQFT: (data: any) => `${data.ADMIN_AREA_SQFT} ft^2`}
   */
  config: PopupContentConfig;
  center?: L.LatLng;
  onAddToParcel: (e: MouseEvent, data: { [key: string]: any }) => void;
  bounds?: L.LatLngBounds;
}

/**
 * A component to display the layer details in a popup
 * @param param0
 */
export const LayerPopupContent: React.FC<IPopupContentProps> = ({
  data,
  config,
  center,
  onAddToParcel,
  bounds,
}) => {
  const rows = React.useMemo(() => keys(config), [config]);
  const location = useLocation();
  const urlParsed = queryString.parse(location.search);
  const populateDetails = urlParsed.sidebar === 'true' ? true : false;

  const mapInstance = useMap();
  const curZoom = mapInstance.getZoom();
  const boundZoom = mapInstance.getBoundsZoom(bounds!);

  return (
    <>
      <ListGroup>
        {rows.map(key => (
          <ListGroup.Item key={key}>
            <b>{config[key].label}</b> {config[key].display(data)}
          </ListGroup.Item>
        ))}
      </ListGroup>
      <MenuRow>
        <Col>
          {populateDetails && (data.PID !== undefined || data.PIN !== undefined) ? (
            <StyledLink
              onClick={(e: any) => onAddToParcel(e, { ...data, CENTER: center })}
              to={{
                search: queryString.stringify({
                  ...queryString.parse(location.search),
                  sidebar: true,
                  disabled: false,
                  loadDraft: false,
                }),
              }}
            >
              Populate property details
            </StyledLink>
          ) : null}
          {bounds && curZoom !== boundZoom ? (
            <StyledLink
              to={{ ...location }}
              onClick={() => mapInstance.flyToBounds(bounds, { animate: false })}
            >
              Zoom
            </StyledLink>
          ) : null}
        </Col>
      </MenuRow>
    </>
  );
};
