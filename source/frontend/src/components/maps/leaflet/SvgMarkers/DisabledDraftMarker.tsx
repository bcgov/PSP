import styled from 'styled-components';

interface IDraftMarkerProps {
  text?: string;
}

export const DisabledDraftMarker: React.FunctionComponent<
  React.PropsWithChildren<IDraftMarkerProps>
> = ({ text, children }) => {
  return (
    <StyledMarker
      width="29"
      height="40"
      viewBox="0 0 127 163"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <path
        d="M122.567 61.2836C122.567 89.1804 85.2225 138.846 68.8483 159.337C64.9223 164.221 57.6449 164.221 53.7189 159.337C37.3447 138.846 0 89.1804 0 61.2836C0 27.4499 27.4499 0 61.2836 0C95.1172 0 122.567 27.4499 122.567 61.2836Z"
        fill="#464646"
      />
      <mask
        id="mask0_496_1341"
        style={{ maskType: 'alpha' }}
        maskUnits="userSpaceOnUse"
        x="11"
        y="15"
        width="116"
        height="116"
      >
        <path d="M16.8594 124.867L120.859 20.8674" stroke="white" strokeWidth="15" />
      </mask>
      <g mask="url(#mask0_496_1341)">
        <path
          d="M127 61.2826C127 89.1794 87.0859 138.845 69.5852 159.336C65.3891 164.22 57.6109 164.22 53.4148 159.336C35.9141 138.845 -4 89.1794 -4 61.2826C-4 27.449 25.3385 -0.000976562 61.5 -0.000976562C97.6615 -0.000976562 127 27.449 127 61.2826Z"
          fill="white"
        />
      </g>
      <circle cx="60.5" cy="65.5" r="37.5" fill="white" />
      <title>{text}</title>
      {children}
    </StyledMarker>
  );
};

const StyledMarker = styled.svg`
  min-width: 2.9rem;
  max-width: 2.9rem;1a
`;

export default DisabledDraftMarker;
