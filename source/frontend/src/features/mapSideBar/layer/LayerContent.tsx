import { GeoJsonProperties } from 'geojson';
import keys from 'lodash/keys';
import React from 'react';
import styled from 'styled-components';

import { FormSection } from '@/components/common/form/styles';
import { SectionField } from '@/components/common/Section/SectionField';

/**
 * A configuration used to display the properties fields in the  content
 * @property
 * @example
 * {ADMIN_AREA_SQFT: (data: any) => `${data.ADMIN_AREA_SQFT} ft^2`}
 */
export type ContentConfig = {
  [key: string]: {
    label: string;
    display: (data: { [key: string]: any }) => string | React.ReactNode;
  };
};

export interface IContentProps {
  data: GeoJsonProperties;
  config: ContentConfig;
}

/**
 * A component to display layer details (data) using the provided configuration to control the display (config)
 * @param param0
 */
export const LayerContent: React.FC<React.PropsWithChildren<IContentProps>> = ({
  config,
  data,
}) => {
  const rows = React.useMemo(() => keys(config), [config]);

  return (
    <StyledFormSection>
      {rows === undefined || rows.length === 0 ? (
        <b>No layer information at this location</b>
      ) : null}
      {rows.map(key => (
        <SectionField
          key={key}
          label={config[key].label}
          labelWidth={{ xs: 4 }}
          contentWidth={{ xs: 8 }}
          noGutters
          className="no-gutters text-break pb-2"
        >
          {config[key].display(data ?? {})}
        </SectionField>
      ))}
    </StyledFormSection>
  );
};

const StyledFormSection = styled(FormSection)`
  background-color: white;
`;
