import MapSelectorContainer from 'features/properties/selector/MapSelectorContainer';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { PropertyForm } from './models';

export interface IResearchPropertiesProps {
  properties: PropertyForm[];
  namespace: string;
  onRemove: (id: string) => void;
}

const ResearchProperties: React.FunctionComponent<IResearchPropertiesProps> = props => {
  return (
    <>
      <StyledSectionHeader>Properties to include in this file:</StyledSectionHeader>
      <div className="py-2">
        Select one or more properties that you want to include in this research file. You can choose
        a location from the map, or search by other criteria
      </div>
      <Row className="py-3 no-gutters">
        <Col>
          <MapSelectorContainer properties={[]} />
        </Col>
      </Row>
    </>
  );
};

export default ResearchProperties;

const StyledSectionHeader = styled.h2`
  font-weight: bold;
  color: ${props => props.theme.css.primaryColor};
  border-bottom: 0.2rem ${props => props.theme.css.primaryColor} solid;
`;
