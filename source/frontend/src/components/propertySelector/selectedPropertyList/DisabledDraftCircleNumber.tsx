import styled from 'styled-components';

import DisabledDraftMarker from '@/components/maps/leaflet/SvgMarkers/DisabledDraftMarker';

interface IDisabledDraftCircleNumberProps {
  text?: string;
}

export const DisabledDraftCircleNumber: React.FunctionComponent<
  React.PropsWithChildren<IDisabledDraftCircleNumberProps>
> = ({ text }) => {
  return (
    <DisabledDraftMarker text={text}>
      <StyledText x={text?.length === 1 ? 60.5 : 58.5} y="65">
        {text?.length ?? 0 <= 2 ? text : '..'}
      </StyledText>
    </DisabledDraftMarker>
  );
};

const StyledText = styled.text`
  font-size: 6rem;
  fill: black;
  text-anchor: middle;
  alignment-baseline: central;
`;

export default DisabledDraftCircleNumber;
