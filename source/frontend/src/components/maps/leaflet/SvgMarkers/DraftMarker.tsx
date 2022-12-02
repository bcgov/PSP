import * as React from 'react';
import styled from 'styled-components';

interface IDraftMarkerProps {
  text?: string;
}

export const DraftMarker: React.FunctionComponent<React.PropsWithChildren<IDraftMarkerProps>> = ({
  text,
  children,
}) => {
  return (
    <StyledMarker
      id="Layer_2"
      xmlns="http://www.w3.org/2000/svg"
      viewBox="0 0 20 30"
      width="30"
      height="45"
    >
      <defs>
        <style></style>
      </defs>
      <g id="PDF_preview">
        <path
          className="cls-1"
          fill="#2a81cb"
          d="M.02,9.77c.4,7.19,9.81,20.23,9.81,20.23,0,0,9.14-13.08,9.43-20.23C19.79-3.33-.7-3.19,.02,9.77Zm9.61,5.13c-3.31,0-6-2.69-6-6S6.32,2.91,9.63,2.91s6,2.69,6,6-2.68,6-6,6Z"
        />
      </g>
      <svg
        version="1.2"
        xmlns="http://www.w3.org/2000/svg"
        xmlnsXlink="http://www.w3.org/1999/xlink"
        overflow="visible"
        preserveAspectRatio="none"
        viewBox="0 0 22 21"
        width="16"
        height="16"
        x="2"
        y="2"
      >
        <g transform="translate(1, 1)">
          <defs>
            <path
              id="path-1648484224573577"
              d="M9.5 0 C14.743192732693075 0 19 4.25680726730706 19 9.5 C19 14.74319273269294 14.743192732693075 19 9.5 19 C4.256807267306925 19 0 14.74319273269294 0 9.5 C0 4.25680726730706 4.256807267306925 0 9.5 0 Z"
              vectorEffect="non-scaling-stroke"
            />
          </defs>
          <g transform="translate(0, 0)">
            <path
              className="path-2"
              strokeLinecap="butt"
              strokeLinejoin="miter"
              strokeWidth="1"
              stroke="rgb(255, 255, 255)"
              fill="rgb(252, 186, 25)"
              d="M9.5 0 C14.743192732693075 0 19 4.25680726730706 19 9.5 C19 14.74319273269294 14.743192732693075 19 9.5 19 C4.256807267306925 19 0 14.74319273269294 0 9.5 C0 4.25680726730706 4.256807267306925 0 9.5 0 Z"
              vectorEffect="non-scaling-stroke"
            />
          </g>
        </g>
        {children}
      </svg>
      <title>{text}</title>
    </StyledMarker>
  );
};

const StyledMarker = styled.svg`
  min-width: 3rem;
  min-height: 4.5rem;
`;

export default DraftMarker;
