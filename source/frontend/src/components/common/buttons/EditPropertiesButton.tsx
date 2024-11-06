import styled from 'styled-components';

import EditMapMarkerIcon from '@/assets/images/edit-map-marker.svg?react';

export const EditPropertiesIcon: React.FC = () => {
  return <EditMapMarkerButton width="2.4rem" height="2.4rem" />;
};

const EditMapMarkerButton = styled(EditMapMarkerIcon)`
  fill: ${props => props.theme.css.iconBlueAction};
  &:hover {
    fill: ${props => props.theme.css.iconBlueHover};
  }
`;
