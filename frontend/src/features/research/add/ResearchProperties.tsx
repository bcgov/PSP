import { LargeInlineInput } from 'features/leases/add/styles';
import { Button, Col, Row } from 'react-bootstrap';
import { MdClose } from 'react-icons/md';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';

import { PropertyForm } from './models';

export interface IResearchPropertiesProps {
  properties: PropertyForm[];
  namespace: string;
  onRemove: (id: string) => void;
}

const ResearchProperties: React.FunctionComponent<IResearchPropertiesProps> = props => {
  const labelColumnSize = 3;
  const inputColumnSize = 9;
  return (
    <>
      <StyledSectionHeader>Properties to include in this file:</StyledSectionHeader>
      <Row className="pb-5">
        <Col>
          <Button onClick={() => {}}>Add Property</Button>
        </Col>
      </Row>
      <Row>
        <Col xs={labelColumnSize}>
          <strong>Identifier</strong>
        </Col>
        <Col xs={inputColumnSize}>
          <strong>Provide a descriptive name for this land</strong>
        </Col>
      </Row>
      <Row>
        <Col>
          <StyledUnderline />
        </Col>
      </Row>
      {props.properties.length > 0 &&
        props.properties.map((property: PropertyForm, index: number) => (
          <Row key={`research-property-${index}`}>
            <Col xs={labelColumnSize}>PID: {property.pid}</Col>
            <Col xs={inputColumnSize}>
              <Row>
                <Col xs="auto" className="pr-0">
                  <LargeInlineInput field={withNameSpace(props.namespace, 'description')} />
                </Col>
                <Col xs="auto">
                  <StyledRemoveButton
                    variant="link"
                    onClick={() => {
                      props.onRemove(property.id);
                    }}
                  >
                    <MdClose />
                  </StyledRemoveButton>
                </Col>
              </Row>
            </Col>
          </Row>
        ))}
      {props.properties.length === 0 && <div>No properties added</div>}
    </>
  );
};

export default ResearchProperties;

const StyledSectionHeader = styled.h2`
  font-weight: bold;
  color: ${props => props.theme.css.primaryColor};
  border-bottom: 0.2rem ${props => props.theme.css.primaryColor} solid;
  margin-bottom: 2rem;
`;

const StyledUnderline = styled.div`
  border-bottom: 0.2rem ${props => props.theme.css.primaryColor} solid;
  margin-bottom: 2rem;
`;

const StyledRemoveButton = styled(Button)`
  position: inline;
  padding: 0;
  color: ${props => props.theme.css.formBackgroundColor};
  &:hover {
    color: red;
  }
`;
