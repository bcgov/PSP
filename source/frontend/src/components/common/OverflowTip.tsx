import React, { useEffect, useRef, useState } from 'react';
import styled from 'styled-components';

import TooltipWrapper from './TooltipWrapper';

interface IOverflowTextProps {
  fullText?: string;
}

/** Creates a dynamic tooltip that displays over text when that text is overflowing and displaying an ellipsis via textOverflow css property */
const OverflowTip: React.FunctionComponent<
  React.PropsWithChildren<IOverflowTextProps & React.HTMLAttributes<HTMLDivElement>>
> = ({ fullText, className }) => {
  const textElementRef = useRef<HTMLDivElement>();

  const compareSize = () => {
    if (textElementRef?.current) {
      const compare = textElementRef?.current?.scrollWidth > textElementRef?.current?.clientWidth;
      setHover(compare);
    }
  };

  useEffect(() => {
    compareSize();
    window.addEventListener('resize', compareSize);
    return () => {
      window.removeEventListener('resize', compareSize);
    };
  }, [fullText]);

  // Define state and function to update the value
  const [hoverStatus, setHover] = useState(false);

  return (
    <TooltipWrapper toolTipId={`tooltip-title`} toolTip={hoverStatus ? fullText : ''}>
      <StyledOverflowDiv className={className} ref={textElementRef as any}>
        {fullText ?? ''}
      </StyledOverflowDiv>
    </TooltipWrapper>
  );
};

const StyledOverflowDiv = styled.div`
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  width: fit-content;
`;

export default OverflowTip;
