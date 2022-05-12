import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

interface IResearchFileLayoutProps {
  leftComponent: React.ReactNode;
  bodyComponent: React.ReactNode;
}

const ResearchFileLayout: React.FunctionComponent<IResearchFileLayoutProps> = ({
  leftComponent,
  bodyComponent,
}) => {
  return (
    <StyledRowContent className="no-gutters">
      <StyledColSidebar xs="3">{leftComponent}</StyledColSidebar>
      <StyledColContent xs="9">{bodyComponent}</StyledColContent>
    </StyledRowContent>
  );
};

const StyledRowContent = styled(Row)`
  height: 100%;
`;

const StyledColSidebar = styled(Col)`
  overflow: auto;
  height: 100%;
  width: 100%;
`;

const StyledColContent = styled(Col)`
  overflow: auto;
  height: 100%;
  width: 100%;
`;

export default ResearchFileLayout;
