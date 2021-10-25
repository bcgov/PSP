import styled from 'styled-components';

export const MapContainer = styled.div`
  grid-area: map;
`;

export const MapGrid = styled.div`
  width: 100%;
  display: grid;
  grid: ${props => props.theme.css.mapfilterHeight} 1fr / min-content 1fr;
  grid-template-areas:
    'filter filter'
    'sidetray map';
`;
