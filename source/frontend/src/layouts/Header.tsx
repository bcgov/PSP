import React from 'react';
import styled from 'styled-components';

import { useTenant } from '@/tenants';

/**
 * Header component that applies tenant environmental styling.
 * @param param0 Header properties.
 * @returns Header component.
 */
export const Header: React.FC<React.PropsWithChildren<React.HTMLAttributes<HTMLElement>>> = ({
  ...rest
}) => {
  const tenant = useTenant();
  return <HeaderStyled {...rest} backgroundColor={tenant.colour}></HeaderStyled>;
};

interface IHeaderProps {
  // Background color of header.
  backgroundColor: string;
}

const HeaderStyled = styled.header<IHeaderProps>`
  background-color: ${props => props.backgroundColor};
  border: none;
  border-bottom: 0.2rem solid ${props => props.theme.css.accentColor};
  grid-area: header;
`;

export default Header;
