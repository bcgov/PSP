import React from 'react';

import BClogoUrl from '@/assets/images/logo-banner.svg';

/**
 * BC Gov Logo image.
 * @param props BCGovLogo properties.
 * @returns BCGovLogo component.
 */
export const BCGovLogo = (
  props: React.DetailedHTMLProps<React.ImgHTMLAttributes<HTMLImageElement>, HTMLImageElement>,
) => {
  return (
    <img
      className={props.className ?? 'bc-gov-icon'}
      src={BClogoUrl}
      width={props.width ?? 156}
      height={props.width ?? 43}
      alt={props.alt ?? 'Government of BC logo'}
    />
  );
};

export default BCGovLogo;
