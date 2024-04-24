import { FieldArray, FormikProps } from 'formik';
import { Col, Row } from 'react-bootstrap';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import MapSelectorContainer from '@/components/propertySelector/MapSelectorContainer';
import { IMapProperty } from '@/components/propertySelector/models';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';
import { useModalContext } from '@/hooks/useModalContext';

import { AddressForm, PropertyForm } from '../../shared/models';
import { DispositionFormModel } from '../models/DispositionFormModel';

export interface DispositionPropertiesSubFormProps {
  formikProps: FormikProps<DispositionFormModel>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const DispositionPropertiesSubForm: React.FunctionComponent<DispositionPropertiesSubFormProps> = ({
  formikProps,
  confirmBeforeAdd,
}) => {
  const { values } = formikProps;
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();
  const { setModalContent, setDisplayModal } = useModalContext();

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
                          const bcaSummary = await getPrimaryAddressByPid(property.pid, 30000);
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

                        if (await confirmBeforeAdd(formProperty)) {
                          // Require user confirmation before adding property to file
                          setModalContent({
                            variant: 'warning',
                            title: 'User Override Required',
                            message: (
                              <>
                                <p>
                                  This property has already been added to one or more disposition
                                  files.
                                </p>
                                <p>Do you want to acknowledge and proceed?</p>
                              </>
                            ),
                            okButtonText: 'Yes',
                            cancelButtonText: 'No',
                            handleOk: () => {
                              push(formProperty);
                              setDisplayModal(false);
                            },
                            handleCancel: () => setDisplayModal(false),
                          });
                          setDisplayModal(true);
                        } else {
                          // No confirmation needed - just add the property to the file
                          push(formProperty);
                        }
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
