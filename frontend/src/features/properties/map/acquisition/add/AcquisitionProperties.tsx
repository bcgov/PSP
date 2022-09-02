import MapSelectorContainer from 'features/properties/selector/MapSelectorContainer';
import { IMapProperty } from 'features/properties/selector/models';
import { FieldArray, useFormikContext } from 'formik';
import { Col, Row } from 'react-bootstrap';

import { AcquisitionForm, AcquisitionPropertyForm } from './models';

export const AcquisitionProperties: React.FunctionComponent = () => {
  const { values } = useFormikContext<AcquisitionForm>();

  return (
    <>
      <div className="py-2">
        Select one or more properties that you want to include in this acquisition. You can choose a
        location from the map, or search by other criteria.
      </div>

      <FieldArray name="properties">
        {({ push, remove }) => (
          <Row className="py-3 no-gutters">
            <Col>
              <MapSelectorContainer
                onSelectedProperty={(newProperty: IMapProperty) => {
                  const formProperty = AcquisitionPropertyForm.fromMapProperty(newProperty);
                  push(formProperty);
                }}
                existingProperties={values.properties}
                onRemoveProperty={remove}
              />
            </Col>
          </Row>
        )}
      </FieldArray>
    </>
  );
};

export default AcquisitionProperties;
