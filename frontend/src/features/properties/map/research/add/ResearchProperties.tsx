import * as Styled from 'components/common/styles';
import { StyledFormSection } from 'features/mapSideBar/tabs/SectionStyles';
import MapSelectorContainer from 'features/properties/selector/MapSelectorContainer';
import { IMapProperty } from 'features/properties/selector/models';
import { FieldArray, useFormikContext } from 'formik';
import { isEqual } from 'lodash';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { PropertyForm, ResearchForm } from './models';
import SelectedPropertyHeaderRow from './SelectedPropertyHeaderRow';
import SelectedPropertyRow from './SelectedPropertyRow';

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
                    if (!values.properties.some(property => isEqual(formProperty, property))) {
                      push(formProperty);
                    }
                  }}
                />
              </Col>
            </Row>
            <StyledFormSection>
              <Styled.H3>Selected properties</Styled.H3>
              <SelectedPropertyHeaderRow />
              {values.properties.map((property, index, properties) => (
                <SelectedPropertyRow
                  key={`property.${property.latitude}-${property.longitude}-${property.pid}`}
                  onRemove={() => remove(index)}
                  nameSpace={`properties.${index}`}
                  index={index}
                />
              ))}

              {values.properties.length === 0 && <span>No Properties selected</span>}
            </StyledFormSection>
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
