import * as React from 'react';
import styled from 'styled-components';

interface IDraftCircleNumberProps {
  text?: string;
}

export const DraftCircleNumber: React.FunctionComponent<IDraftCircleNumberProps> = ({ text }) => {
  return (
    <svg
      version="1.2"
      xmlns="http://www.w3.org/2000/svg"
      xmlnsXlink="http://www.w3.org/1999/xlink"
      overflow="visible"
      preserveAspectRatio="none"
      viewBox="0 0 22 21"
      width="21"
      height="21"
      x="5"
      y="5"
    >
      <g transform="translate(1, 1)">
        <defs>
          <path
            // eslint-disable-next-line no-octal-escape
            id="path-16484842245735\77"
            d="M9.5 0 C14.743192732693075 0 19 4.25680726730706 19 9.5 C19 14.74319273269294 14.743192732693075 19 9.5 19 C4.256807267306925 19 0 14.74319273269294 0 9.5 C0 4.25680726730706 4.256807267306925 0 9.5 0 Z"
            vectorEffect="non-scaling-stroke"
          />
        </defs>
        <g transform="translate(0, 0)">
          <path
            style={{
              stroke: 'rgb(255, 255, 255)',
              strokeWidth: 1,
              strokeLinecap: 'butt',
              strokeLinejoin: 'miter',
              fill: 'rgb(252, 186, 25)',
            }}
            d="M9.5 0 C14.743192732693075 0 19 4.25680726730706 19 9.5 C19 14.74319273269294 14.743192732693075 19 9.5 19 C4.256807267306925 19 0 14.74319273269294 0 9.5 C0 4.25680726730706 4.256807267306925 0 9.5 0 Z"
            vectorEffect="non-scaling-stroke"
          />
        </g>
      </g>
      <StyledText x={text?.length === 1 ? 7 : 4} y="15" textLength="1.25rem">
        {text?.length ?? 0 <= 2 ? text : '..'}
      </StyledText>
      <title>{text}</title>
    </svg>
  );
};

const StyledText = styled.text`
  font-size: 1.2rem;
`;

export default DraftCircleNumber;
