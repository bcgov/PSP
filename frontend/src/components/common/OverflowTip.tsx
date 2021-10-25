import React, { useEffect, useRef, useState } from 'react';
import styled from 'styled-components';

import TooltipWrapper from './TooltipWrapper';

interface IOverflowTextProps {
  title: string;
  fullText?: string;
}

/** Creates a dynamic tooltip that displays over text when that text is overflowing and displaying an ellipsis via textOverflow css property */
const OverflowTip: React.FunctionComponent<IOverflowTextProps> = ({ title, fullText }) => {
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
  }, []);

  useEffect(
    () => () => {
      window.removeEventListener('resize', compareSize);
    },
    [],
  );

  // Define state and function to update the value
  const [hoverStatus, setHover] = useState(false);

  return (
    <TooltipWrapper toolTipId={`tooltip-title`} toolTip={hoverStatus ? fullText : ''}>
      <StyledOverflowDiv ref={textElementRef as any}>{fullText ?? ''}</StyledOverflowDiv>
    </TooltipWrapper>
  );
};

const StyledOverflowDiv = styled.div`
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  width: 100%;
`;

export default OverflowTip;
