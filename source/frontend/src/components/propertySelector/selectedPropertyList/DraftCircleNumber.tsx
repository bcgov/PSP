import * as React from 'react';
import styled from 'styled-components';

import DraftMarker from '@/components/maps/leaflet/SvgMarkers/DraftMarker';

interface IDraftCircleNumberProps {
  text?: string;
}

export const DraftCircleNumber: React.FunctionComponent<
  React.PropsWithChildren<IDraftCircleNumberProps>
> = ({ text }) => {
  return (
    <DraftMarker text={text}>
      <StyledText x={text?.length === 1 ? 7 : 4} y="15" textLength="1.25rem">
        {text?.length ?? 0 <= 2 ? text : '..'}
      </StyledText>
    </DraftMarker>
  );
};

const StyledText = styled.text`
  font-size: 1.2rem;
`;

export default DraftCircleNumber;
