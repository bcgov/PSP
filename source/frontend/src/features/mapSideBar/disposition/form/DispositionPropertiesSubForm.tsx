import { FieldArray, FormikProps } from 'formik';
import { geoJSON, LatLngLiteral } from 'leaflet';
import isNumber from 'lodash/isNumber';
import { Col, Row } from 'react-bootstrap';
import { PiCornersOut } from 'react-icons/pi';

import { LinkButton } from '@/components/common/buttons/LinkButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import MapSelectorContainer from '@/components/propertySelector/MapSelectorContainer';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';
import { useModalContext } from '@/hooks/useModalContext';
import { exists, isLatLngInFeatureSetBoundary, latLngLiteralToGeometry } from '@/utils';

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

  const mapMachine = useMapStateMachine();

  const fitBoundaries = () => {
    const fileProperties = formikProps.values.fileProperties;

    if (exists(fileProperties)) {
      const locations = fileProperties.map(
        p => p?.polygon ?? latLngLiteralToGeometry(p?.fileLocation),
      );
      const bounds = geoJSON(locations).getBounds();

      mapMachine.requestFlyToBounds(bounds);
    }
  };

  return (
    <>
      <div className="py-2">
        Select one or more properties that you want to include in this disposition. You can choose a
        location from the map, or search by other criteria.
      </div>

      <FieldArray name="fileProperties">
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
                      const formProperty = formikProps.values.fileProperties[index];
                      const updatedFormProperty = new PropertyForm(formProperty);
                      updatedFormProperty.fileLocation = latLng;

                      // Find property within formik values and reposition it based on incoming file marker position
                      replace(index, updatedFormProperty);
                    }
                  }}
                  modifiedProperties={values.fileProperties.map(p => p.toFeatureDataset())}
                />
              </Col>
            </Row>
            <Section
              header={
                <Row>
                  <Col xs="11">Selected Properties</Col>
                  <Col>
                    <TooltipWrapper
                      tooltip="Fit map to the file properties"
                      tooltipId="property-selector-tooltip"
                    >
                      <LinkButton title="Fit boundaries button" onClick={fitBoundaries}>
                        <PiCornersOut size={18} className="mr-2" />
                      </LinkButton>
                    </TooltipWrapper>
                  </Col>
                </Row>
              }
            >
              <SelectedPropertyHeaderRow />
              {formikProps.values.fileProperties.map((property, index) => (
                <SelectedPropertyRow
                  key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                  onRemove={() => remove(index)}
                  nameSpace={`fileProperties.${index}`}
                  index={index}
                  property={property.toFeatureDataset()}
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
