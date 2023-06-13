import React from 'react';
import styled from 'styled-components';

import { useTenant } from '@/tenants';

/**
 * Footer component that applies tenant environmental styling.
 * @param param0 Footer properties.
 * @returns Footer component.
 */
export const Footer: React.FC<React.PropsWithChildren<React.HTMLAttributes<HTMLElement>>> = ({
  ...rest
}) => {
  const tenant = useTenant();
  return <FooterStyled {...rest} backgroundColor={tenant.colour}></FooterStyled>;
};

interface IFooterProps {
  // Background color of footer.
  backgroundColor: string;
}

const FooterStyled = styled.footer<IFooterProps>`
  // colors
  background-color: ${props => props.backgroundColor ?? props.theme.css.primaryColor};
  border: none;
  border-top: 0.2rem solid ${props => props.theme.css.accentColor};
  grid-area: footer;

  a,
  h2 {
    color: #fff;
  }
`;

export default Footer;
