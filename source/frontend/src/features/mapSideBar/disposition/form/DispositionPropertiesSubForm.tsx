import { FieldArray, FormikProps } from 'formik';
import { Col, Row } from 'react-bootstrap';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import MapSelectorContainer from '@/components/propertySelector/MapSelectorContainer';
import { IMapProperty } from '@/components/propertySelector/models';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';

import { AddressForm, PropertyForm } from '../../shared/models';
import { DispositionFormModel } from '../models/DispositionFormModel';

export interface DispositionPropertiesSubFormProps {
  formikProps: FormikProps<DispositionFormModel>;
}

const DispositionPropertiesSubForm: React.FunctionComponent<
  React.PropsWithChildren<DispositionPropertiesSubFormProps>
> = ({ formikProps }) => {
  const { values } = formikProps;
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();

  return (
    <>
      <div className="py-2">
        Select one or more properties that you want to include in this disposition. You can choose a
        location from the map, or search by other criteria.
      </div>

      <FieldArray name="fileProperties">
        {({ push, remove }) => (
          <>
            <LoadingBackdrop show={bcaLoading} />
            <Row className="py-3 no-gutters">
              <Col>
                <MapSelectorContainer
                  addSelectedProperties={(newProperties: IMapProperty[]) => {
                    newProperties.reduce(async (promise, property, index) => {
                      return promise.then(async () => {
                        const formProperty = PropertyForm.fromMapProperty(property);
                        if (property.pid) {
                          const bcaSummary = await getPrimaryAddressByPid(property.pid, 3000);
                          formProperty.address = bcaSummary?.address
                            ? AddressForm.fromBcaAddress(bcaSummary?.address)
                            : undefined;
                        }
                        // auto-select file region based upon the location of the property
                        if (
                          values.fileProperties?.length === 0 &&
                          index === 0 &&
                          formProperty.regionName !== 'Cannot determine'
                        ) {
                          formikProps.setFieldValue(`regionCode`, formProperty.region);
                        }
                        push(formProperty);
                      });
                    }, Promise.resolve());
                  }}
                  modifiedProperties={values.fileProperties}
                />
              </Col>
            </Row>
            <Section header="Selected properties">
              <SelectedPropertyHeaderRow />
              {formikProps.values.fileProperties.map((property, index) => (
                <SelectedPropertyRow
                  key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                  onRemove={() => remove(index)}
                  nameSpace={`fileProperties.${index}`}
                  index={index}
                  property={property.toMapProperty()}
                />
              ))}
              {formikProps.values.fileProperties.length === 0 && (
                <span>No Properties selected</span>
              )}
            </Section>
          </>
        )}
      </FieldArray>
    </>
  );
};

export default DispositionPropertiesSubForm;
