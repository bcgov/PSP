import React from 'react';

const SvgrMock = React.forwardRef<HTMLElement>((props, ref) => (
  <svg {...props}>
    <span ref={ref} />
  </svg>
));
SvgrMock.displayName = 'SvgrMock';

export const ReactComponent = SvgrMock;
export default SvgrMock;
