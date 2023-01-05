import MapSelectorContainer from 'components/propertySelector/MapSelectorContainer';
import { IMapProperty } from 'components/propertySelector/models';
import SelectedPropertyHeaderRow from 'components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from 'components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { Section } from 'features/mapSideBar/tabs/Section';
import { FieldArray, useFormikContext } from 'formik';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { useBcaAddress } from '../../hooks/useBcaAddress';
import { AddressForm, PropertyForm } from '../../shared/models';
import { ResearchForm } from './models';

const ResearchProperties: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  const { values } = useFormikContext<ResearchForm>();
  const { getPrimaryAddressByPid } = useBcaAddress();

  return (
    <>
      <StyledSectionHeader>Properties to include in this file:</StyledSectionHeader>
      <div className="py-2">
        Select one or more properties that you want to include in this research file. You can choose
        a location from the map, or search by other criteria.
      </div>

      <FieldArray name="properties">
        {({ push, remove }) => (
          <>
            <Row className="py-3 no-gutters">
              <Col>
                <MapSelectorContainer
                  addSelectedProperties={(newProperties: IMapProperty[]) => {
                    newProperties.reduce(async (promise, property) => {
                      return promise.then(async () => {
                        const formProperty = PropertyForm.fromMapProperty(property);
                        if (property.pid) {
                          const bcaSummary = await getPrimaryAddressByPid(property.pid);
                          formProperty.address = bcaSummary?.address
                            ? AddressForm.fromBcaAddress(bcaSummary?.address)
                            : undefined;
                          formProperty.legalDescription = bcaSummary?.legalDescription?.LEGAL_TEXT;
                        }
                        push(formProperty);
                      });
                    }, Promise.resolve());
                  }}
                  modifiedProperties={values.properties}
                />
              </Col>
            </Row>
            <Section header="Selected properties">
              <SelectedPropertyHeaderRow />
              {values.properties.map((property, index) => (
                <SelectedPropertyRow
                  key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                  onRemove={() => remove(index)}
                  nameSpace={`properties.${index}`}
                  index={index}
                  property={property.toMapProperty()}
                />
              ))}
              {values.properties.length === 0 && <span>No Properties selected</span>}
            </Section>
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
