import * as React from 'react';

interface IDraftMarkerProps {
  text?: string;
}

export const DraftMarker: React.FunctionComponent<IDraftMarkerProps> = ({ text, children }) => {
  return (
    <svg
      version="1.2"
      xmlns="http://www.w3.org/2000/svg"
      xmlnsXlink="http://www.w3.org/1999/xlink"
      overflow="visible"
      preserveAspectRatio="none"
      viewBox="0 0 29.36209262057153 43.98765018534512"
      width="29.36209262057153"
      height="43.98765018534512"
    >
      <g transform="translate(0, 0)">
        <g transform="translate(-0.000001071428573030421, 1.8534512574947746e-7) rotate(0)">
          <path
            style={{
              strokeWidth: 0,
              strokeLinecap: 'butt',
              strokeLinejoin: 'miter',
              fill: 'rgb(59, 153, 252)',
            }}
            d="M29.32826,14.6864c0.055,-3.905 -1.485,-7.645 -4.29,-10.3675c-2.7225,-2.805 -6.4625,-4.3725 -10.3675,-4.3175c-3.905,-0.055 -7.645,1.5125 -10.3675,4.3175c-2.805,2.7225 -4.345,6.4625 -4.29,10.34c-0.0825,1.76 0.22,3.52 0.9075,5.1425l10.45,22.165c0.275,0.6325 0.7425,1.1275 1.32,1.485c1.1825,0.715 2.695,0.715 3.8775,0c0.5775,-0.3575 1.045,-0.88 1.3475,-1.485l10.505,-22.165c0.6875,-1.6225 0.99,-3.355 0.935,-5.115v0z"
            vectorEffect="non-scaling-stroke"
          />
        </g>
        <defs>
          <path
            id="path-1648484224571569"
            d="M29.32826,14.6864c0.055,-3.905 -1.485,-7.645 -4.29,-10.3675c-2.7225,-2.805 -6.4625,-4.3725 -10.3675,-4.3175c-3.905,-0.055 -7.645,1.5125 -10.3675,4.3175c-2.805,2.7225 -4.345,6.4625 -4.29,10.34c-0.0825,1.76 0.22,3.52 0.9075,5.1425l10.45,22.165c0.275,0.6325 0.7425,1.1275 1.32,1.485c1.1825,0.715 2.695,0.715 3.8775,0c0.5775,-0.3575 1.045,-0.88 1.3475,-1.485l10.505,-22.165c0.6875,-1.6225 0.99,-3.355 0.935,-5.115v0z"
            vectorEffect="non-scaling-stroke"
          />
        </defs>
      </g>
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
              id="path-1648484224573577"
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
        {children}
      </svg>
      <title>{text}</title>
    </svg>
  );
};

export default DraftMarker;
