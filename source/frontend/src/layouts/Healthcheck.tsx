import React from 'react';
import styled from 'styled-components';

const HealthCheckStyled: React.FC<React.PropsWithChildren<React.HTMLAttributes<HTMLElement>>> = ({
  ...rest
}) => {
  return <HealthcheckStyled {...rest}></HealthcheckStyled>;
};

const HealthcheckStyled = styled.div`
  grid-area: health;
  border: 1px solid ${props => props.theme.bcTokens.typographyColorDanger};
`;

export default HealthCheckStyled;
