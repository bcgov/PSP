import React from 'react';
import styled from 'styled-components';
import { useTenant } from 'tenants';

/**
 * Footer component that applies tenant environmental styling.
 * @param param0 Footer properties.
 * @returns Footer component.
 */
export const Footer: React.FC<React.HTMLAttributes<HTMLElement>> = ({ ...rest }) => {
  const tenant = useTenant();
  return <FooterStyled {...rest} backgroundColor={tenant.colour}></FooterStyled>;
};

interface IFooterProps {
  // Background color of footer.
  backgroundColor: string;
}

const FooterStyled = styled.footer<IFooterProps>`
  background-color: ${props => props.backgroundColor};

  // colors
  background-color: ${props => props.theme.css.primaryColor};
  border: none;
  border-top: 2px solid ${props => props.theme.css.accentColor};

  // sizing
  flex-shrink: 0;

  a,
  h2 {
    color: #fff;
  }
`;

export default Footer;
