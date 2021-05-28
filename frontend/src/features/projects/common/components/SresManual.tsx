import variables from '_variables.module.scss';
import * as React from 'react';
import { FiBookOpen } from 'react-icons/fi';
import styled from 'styled-components';

const SresManualDiv = styled.div`
  text-align: center;
  cursor: pointer;
  div {
    border: solid 2px ${variables.accentColor};
    border-radius: 2rem;
    display: inline-block;
    background-color: white;
  }
  svg {
    color: ${variables.sresIconColor};
    margin: 0.5rem;
  }
  font-size: 10px;
`;

interface ISresManualProps {
  clickUrl?: string;
  hideText?: boolean;
}

export const SresManual: React.FunctionComponent<ISresManualProps> = ({
  clickUrl,
  hideText,
}: ISresManualProps) => {
  const link =
    clickUrl ??
    'https://intranet.gov.bc.ca/assets/intranet/mtics/real-property/sres/process_manual_for_the_surplus_properties_program_-_feb_2020_-_version_2.pdf?';
  return (
    <SresManualDiv className="SresManual" onClick={() => window.open(link, '_blank')}>
      <div>
        <FiBookOpen size={28} />
      </div>
      {!hideText && (
        <p>
          <b>Process Manual</b> <br />
          for the Surplus <br /> Properties Program
        </p>
      )}
    </SresManualDiv>
  );
};

export default SresManual;
