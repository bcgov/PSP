import * as React from 'react';
import styled from 'styled-components';

import AquisitionSection from './AquisitionSection';

export interface IPropertyAssociationTabViewProps {
  researchFiles: any[];
  aquisitionFiles: any[];
  aquisitionLeases: any[];
  aquisitionDispotitions: any[];
}

const PropertyAssociationTabView: React.FunctionComponent<IPropertyAssociationTabViewProps> = props => {
  return (
    <StyledSummarySection>
      <AquisitionSection aquisitionFiles={props.aquisitionFiles} />
    </StyledSummarySection>
  );
};

export default PropertyAssociationTabView;

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;
