import { GeoJsonProperties } from 'geojson';
import keys from 'lodash/keys';
import React from 'react';

import { SectionField } from '@/components/common/Section/SectionField';

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
  data: GeoJsonProperties;
  config: PopupContentConfig;
}

/**
 * A component to display the layer details in a popup
 * @param param0
 */
export const LayerPopupContent: React.FC<React.PropsWithChildren<IPopupContentProps>> = ({
  config,
  data,
}) => {
  const rows = React.useMemo(() => keys(config), [config]);

  return (
    <>
      {rows === undefined || rows.length === 0 ? (
        <b>No layer information at this location</b>
      ) : null}
      {rows.map(key => (
        <SectionField
          key={key}
          label={config[key].label}
          labelWidth="5"
          contentWidth="7"
          noGutters
          className="no-gutters text-break pb-2"
        >
          {config[key].display(data ?? {})}
        </SectionField>
      ))}
    </>
  );
};
