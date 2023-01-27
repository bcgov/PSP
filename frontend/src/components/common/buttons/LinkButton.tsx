import React, { forwardRef } from 'react';

import { Button, ButtonProps } from './Button';

/**
 * A simple wrapper around Button to display like links.
 */
export const LinkButton = forwardRef<typeof Button, ButtonProps>((props, ref) => {
  // discard "variant" property - as will always be "link"
  const { variant, ...rest } = props;
  return <Button {...rest} className={rest.className} variant="link" ref={ref as any} />;
});
