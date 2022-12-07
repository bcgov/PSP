import MapSelectorContainer from 'components/propertySelector/MapSelectorContainer';
import { IMapProperty } from 'components/propertySelector/models';
import SelectedPropertyHeaderRow from 'components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from 'components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { Section } from 'features/mapSideBar/tabs/Section';
import { FieldArray, FormikProps } from 'formik';
import { Col, Row } from 'react-bootstrap';

import { PropertyForm } from '../../shared/models';
import { AcquisitionForm } from './models';

interface AcquisitionPropertiesProp {
  formikProps: FormikProps<AcquisitionForm>;
}

export const AcquisitionProperties: React.FunctionComponent<
  React.PropsWithChildren<AcquisitionPropertiesProp>
> = ({ formikProps }) => {
  const { values } = formikProps;

  return (
    <>
      <div className="py-2">
        Select one or more properties that you want to include in this acquisition. You can choose a
        location from the map, or search by other criteria.
      </div>

      <FieldArray name="properties">
        {({ push, remove }) => (
          <>
            <Row className="py-3 no-gutters">
              <Col>
                <MapSelectorContainer
                  onSelectedProperty={(newProperty: IMapProperty) => {
                    const formProperty = PropertyForm.fromMapProperty(newProperty);
                    if (values.properties?.length === 0) {
                      formikProps.setFieldValue(`region`, formProperty.region);
                    }
                    push(formProperty);
                  }}
                  modifiedProperties={values.properties}
                />
              </Col>
            </Row>
            <Section header="Selected properties">
              <SelectedPropertyHeaderRow />
              {formikProps.values.properties.map((property, index) => (
                <SelectedPropertyRow
                  key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                  onRemove={() => remove(index)}
                  nameSpace={`properties.${index}`}
                  index={index}
                  property={property}
                />
              ))}
              {formikProps.values.properties.length === 0 && <span>No Properties selected</span>}
            </Section>
          </>
        )}
      </FieldArray>
    </>
  );
};

export default AcquisitionProperties;
