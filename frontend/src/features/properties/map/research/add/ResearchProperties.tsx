import MapSelectorContainer from 'features/properties/selector/MapSelectorContainer';
import { IMapProperty } from 'features/properties/selector/models';
import { FieldArray, useFormikContext } from 'formik';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { PropertyForm, ResearchForm } from './models';

const ResearchProperties: React.FunctionComponent = () => {
  const { values } = useFormikContext<ResearchForm>();

  return (
    <>
      <StyledSectionHeader>Properties to include in this file:</StyledSectionHeader>
      <div className="py-2">
        Select one or more properties that you want to include in this research file. You can choose
        a location from the map, or search by other criteria
      </div>

      <FieldArray name="properties">
        {({ push, remove }) => (
          <>
            <Row className="py-3 no-gutters">
              <Col>
                <MapSelectorContainer
                  onSelectedProperty={(newProperty: IMapProperty) => {
                    const formProperty = new PropertyForm(newProperty);
                    push(formProperty);
                  }}
                  selectedProperties={values.properties}
                  onRemoveProperty={remove}
                />
              </Col>
            </Row>
          </>
        )}
      </FieldArray>
    </>
  );
};

export default ResearchProperties;

const StyledSectionHeader = styled.h2`
  font-weight: bold;
  color: ${props => props.theme.css.primaryColor};
  border-bottom: 0.2rem ${props => props.theme.css.primaryColor} solid;
`;
