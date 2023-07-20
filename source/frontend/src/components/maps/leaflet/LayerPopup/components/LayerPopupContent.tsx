import keys from 'lodash/keys';
import React from 'react';
import ListGroup from 'react-bootstrap/ListGroup';

import { LayerPopupInformation } from '../LayerPopupContainer';

/**
 * A configuration used to display the properties fields in the popup content
 * @property
 * @example
 * {ADMIN_AREA_SQFT: (data: any) => `${data.ADMIN_AREA_SQFT} ft^2`}
 */
export type PopupContentConfig = {
  [key: string]: {
    label: string;
    display: (data: { [key: string]: any }) => string | React.ReactNode;
  };
};

export interface IPopupContentProps {
  layerPopup: LayerPopupInformation;
}

/**
 * A component to display the layer details in a popup
 * @param param0
 */
export const LayerPopupContent: React.FC<React.PropsWithChildren<IPopupContentProps>> = ({
  layerPopup,
}) => {
  const { config, data } = { ...layerPopup };
  const rows = React.useMemo(() => keys(config), [config]);

  return (
    <ListGroup>
      {!rows.length && <b>No layer information at this location</b>}
      {rows.map(key => (
        <ListGroup.Item key={key}>
          <b>{config[key].label}</b> {config[key].display(data ?? {})}
        </ListGroup.Item>
      ))}
    </ListGroup>
  );
};
