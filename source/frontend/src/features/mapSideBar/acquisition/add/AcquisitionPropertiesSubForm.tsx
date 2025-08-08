import { FieldArray, FormikProps } from 'formik';
import { LatLngLiteral } from 'leaflet';
import isNumber from 'lodash/isNumber';
import { Col, Row } from 'react-bootstrap';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import MapSelectorContainer from '@/components/propertySelector/MapSelectorContainer';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';
import { useModalContext } from '@/hooks/useModalContext';
import { isLatLngInFeatureSetBoundary } from '@/utils';

import { AddressForm, PropertyForm } from '../../shared/models';
import { AcquisitionForm } from './models';

export interface IAcquisitionPropertiesProps {
  formikProps: FormikProps<AcquisitionForm>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

export const AcquisitionPropertiesSubForm: React.FunctionComponent<IAcquisitionPropertiesProps> = ({
  formikProps,
  confirmBeforeAdd,
}) => {
  const { values } = formikProps;
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();
  const { setModalContent, setDisplayModal } = useModalContext();

  return (
    <>
      <div className="py-2">
        Select one or more properties that you want to include in this acquisition. You can choose a
        location from the map, or search by other criteria.
      </div>

      <FieldArray name="properties">
        {({ push, remove, replace }) => (
          <>
            <LoadingBackdrop show={bcaLoading} />
            <Row className="py-3 no-gutters">
              <Col>
                <MapSelectorContainer
                  addSelectedProperties={(newProperties: SelectedFeatureDataset[]) => {
                    newProperties.reduce(async (promise, property, index) => {
                      return promise.then(async () => {
                        const formProperty = PropertyForm.fromFeatureDataset(property);
                        if (formProperty.pid) {
                          const bcaSummary = await getPrimaryAddressByPid(formProperty.pid, 30000);
                          formProperty.address = bcaSummary?.address
                            ? AddressForm.fromBcaAddress(bcaSummary?.address)
                            : undefined;
                        }

                        if (await confirmBeforeAdd(formProperty)) {
                          // Require user confirmation before adding property to file
                          setModalContent({
                            variant: 'warning',
                            title: 'User Override Required',
                            message: (
                              <>
                                <p>
                                  This property has already been added to one or more acquisition
                                  files.
                                </p>
                                <p>Do you want to acknowledge and proceed?</p>
                              </>
                            ),
                            okButtonText: 'Yes',
                            cancelButtonText: 'No',
                            handleOk: () => {
                              if (
                                values.properties?.length === 0 &&
                                index === 0 &&
                                formProperty.regionName !== 'Cannot determine'
                              ) {
                                formikProps.setFieldValue(`region`, formProperty.region);
                              }
                              push(formProperty);
                              setDisplayModal(false);
                            },
                            handleCancel: () => setDisplayModal(false),
                          });
                          setDisplayModal(true);
                        } else {
                          // No confirmation needed - just add the property to the file
                          if (
                            values.properties?.length === 0 &&
                            index === 0 &&
                            formProperty.regionName !== 'Cannot determine'
                          ) {
                            formikProps.setFieldValue(`region`, formProperty.region);
                          }
                          push(formProperty);
                        }
                      });
                    }, Promise.resolve());
                  }}
                  repositionSelectedProperty={(
                    featureset: SelectedFeatureDataset,
                    latLng: LatLngLiteral,
                    index: number | null,
                  ) => {
                    // As long as the marker is repositioned within the boundary of the originally selected property simply reposition the marker without further notification.
                    if (
                      isNumber(index) &&
                      index >= 0 &&
                      isLatLngInFeatureSetBoundary(latLng, featureset)
                    ) {
                      const formProperty = formikProps.values.properties[index];
                      const updatedFormProperty = new PropertyForm(formProperty);
                      updatedFormProperty.fileLocation = latLng;

                      // Find property within formik values and reposition it based on incoming file marker position
                      replace(index, updatedFormProperty);
                    }
                  }}
                  modifiedProperties={values.properties.map(p => p.toFeatureDataset())}
                />
              </Col>
            </Row>
            <Section header="Selected Properties">
              <SelectedPropertyHeaderRow />
              {formikProps.values.properties.map((property, index) => (
                <SelectedPropertyRow
                  key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                  onRemove={() => remove(index)}
                  nameSpace={`properties.${index}`}
                  index={index}
                  property={property.toFeatureDataset()}
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

export default AcquisitionPropertiesSubForm;
