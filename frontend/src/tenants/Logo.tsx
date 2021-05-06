import React from 'react';
import { tenant } from '.';

/**
 * Logo properties.
 */
interface ILogoProps
  extends React.DetailedHTMLProps<React.ImgHTMLAttributes<HTMLImageElement>, HTMLImageElement> {
  // Whether to include the logo with text.
  withText?: boolean;
}

/**
 * A Logo component provides an image for the current tenant.
 * @param param0 Logo component properties.
 * @returns Image containing the
 */
export const Logo = ({ withText = false, ...rest }: ILogoProps) => {
  const { logo } = tenant;
  return (
    <img
      {...rest}
      src={withText ? logo.imageWithText : logo.image}
      alt={rest.alt ?? 'PIMS logo'}
      className={rest.className ?? 'pims-logo'}
    />
  );
};

export default Logo;
