import React, { useEffect, useRef, useState } from 'react';
import styled from 'styled-components';

import TooltipWrapper from './TooltipWrapper';

interface IOverflowTextProps {
  fullText?: string;
  valueTestId?: string | null;
}

/** Creates a dynamic tooltip that displays over text when that text is overflowing and displaying an ellipsis via textOverflow css property */
const OverflowTip: React.FunctionComponent<
  React.PropsWithChildren<IOverflowTextProps & React.HTMLAttributes<HTMLDivElement>>
> = ({ fullText, className, valueTestId, children }) => {
  // Define state and function to update the value
  const [hoverStatus, setHover] = useState(false);
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
  }, [fullText, children]);

  return (
    <TooltipWrapper
      tooltipId={`tooltip-title`}
      trigger={['hover', 'focus']}
      tooltip={hoverStatus ? fullText : ''}
    >
      <StyledOverflowDiv
        className={className}
        ref={textElementRef as any}
        data-testid={valueTestId}
      >
        {children ?? fullText}
      </StyledOverflowDiv>
    </TooltipWrapper>
  );
};

const StyledOverflowDiv = styled.div`
  flex-grow: 1;
  margin: 0.15rem;
  witdh: 100%;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 1;
  -webkit-box-orient: vertical;
  word-break: break-all;
  line-break: break-all;
`;

export default OverflowTip;
