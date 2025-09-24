import axios, { AxiosError } from 'axios';
import { FieldArray, Formik, FormikProps } from 'formik';
import { geoJSON, LatLngLiteral } from 'leaflet';
import noop from 'lodash/noop';
import { useCallback, useContext, useEffect, useMemo, useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { PiCornersOut } from 'react-icons/pi';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { LinkButton } from '@/components/common/buttons/LinkButton';
import GenericModal from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import MapClickMonitor from '@/components/propertySelector/MapClickMonitor';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { useEditPropertiesMode } from '@/hooks/useEditPropertiesMode';
import { useFeatureDatasetsWithAddresses } from '@/hooks/useFeatureDatasetsWithAddresses';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import {
  exists,
  firstOrNull,
  isLatLngInFeatureSetBoundary,
  isNumber,
  isValidId,
  latLngLiteralToGeometry,
} from '@/utils';
import { addPropertiesToCurrentFile } from '@/utils/propertyUtils';

import { FileForm, PropertyForm } from '../../models';
import SidebarFooter from '../../SidebarFooter';
import AddPropertiesGuide from './AddPropertiesGuide';
import { UpdatePropertiesYupSchema } from './UpdatePropertiesYupSchema';

export interface IUpdatePropertiesProps {
  file: ApiGen_Concepts_File;
  setIsShowingPropertySelector: (isShowing: boolean) => void;
  onSuccess: (updateProperties?: boolean, updateFile?: boolean) => void;
  updateFileProperties: (
    file: ApiGen_Concepts_File,
    userOverrideCodes: UserOverrideCode[],
  ) => Promise<ApiGen_Concepts_File | undefined>;
  canRemove: (propertyId: number) => Promise<boolean>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
  confirmBeforeAddMessage?: React.ReactNode;
  formikRef?: React.RefObject<FormikProps<any>>;
  disableProperties?: boolean;
}

export const UpdateProperties: React.FunctionComponent<IUpdatePropertiesProps> = props => {
  const localRef = useRef<FormikProps<FileForm>>(null);
  const formikRef = props.formikRef ? props.formikRef : localRef;
  const formFile = FileForm.fromApi(props.file);

  const [showSaveConfirmModal, setShowSaveConfirmModal] = useState<boolean>(false);
  const [showAssociatedEntityWarning, setShowAssociatedEntityWarning] = useState<boolean>(false);
  const [isValid, setIsValid] = useState<boolean>(true);
  const { setModalContent, setDisplayModal } = useModalContext();
  const { resetFilePropertyLocations } = useContext(SideBarContext);
  const {
    requestFlyToBounds,
    selectedFeatures,
    processCreation,
    mapLocationFeatureDataset,
    prepareForCreation,
  } = useMapStateMachine();

  useEditPropertiesMode();

  // Get PropertyForms with addresses for all selected features
  const { featuresWithAddresses, bcaLoading } = useFeatureDatasetsWithAddresses(
    selectedFeatures ?? [],
  );

  // Convert SelectedFeatureDataset to PropertyForm
  const propertyForms = useMemo(
    () =>
      featuresWithAddresses.map(obj => {
        const property = PropertyForm.fromFeatureDataset(obj.feature);
        if (exists(obj.address)) {
          property.address = obj.address;
        }
        return property;
      }),
    [featuresWithAddresses],
  );

  // This effect is used to update the file properties when "add to open file" is clicked in the worklist.
  useEffect(() => {
    if (exists(formikRef.current) && propertyForms.length > 0) {
      addPropertiesToCurrentFile(formikRef, 'properties', propertyForms, noop);
      processCreation();
    }
  }, [formikRef, processCreation, propertyForms]);

  const fitBoundaries = () => {
    const fileProperties = formikRef?.current?.values?.properties;

    if (exists(fileProperties)) {
      const locations = fileProperties.map(
        p => p?.polygon ?? latLngLiteralToGeometry(p?.fileLocation),
      );
      const bounds = geoJSON(locations).getBounds();

      if (exists(bounds) && bounds.isValid()) {
        requestFlyToBounds(bounds);
      }
    }
  };

  const handleSaveClick = async () => {
    await formikRef?.current?.validateForm();
    if (!formikRef?.current?.isValid) {
      setIsValid(false);
    } else {
      setIsValid(true);
    }
    setShowSaveConfirmModal(true);
  };

  const handleCancelClick = () => {
    if (formikRef !== undefined) {
      if (formikRef.current?.dirty) {
        setModalContent({
          ...getCancelModalProps(),
          handleOk: () => {
            handleCancelConfirm();
            setDisplayModal(false);
          },
          handleCancel: () => setDisplayModal(false),
        });
        setDisplayModal(true);
      } else {
        handleCancelConfirm();
      }
    } else {
      handleCancelConfirm();
    }
  };

  const handleSaveConfirm = async () => {
    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
    }
  };

  const handleCancelConfirm = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
    resetFilePropertyLocations();
    props.setIsShowingPropertySelector(false);
  };

  const saveFile = async (file: ApiGen_Concepts_File) => {
    try {
      const response = await props.updateFileProperties(file, []);

      formikRef.current?.setSubmitting(false);
      if (isValidId(response?.id)) {
        if (file.fileProperties?.find(fp => !fp.property?.address && !fp.property?.id)) {
          toast.warn(
            'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
            { autoClose: 15000 },
          );
        }
        formikRef.current?.resetForm();
        props.setIsShowingPropertySelector(false);
        props.onSuccess(true);
      }
    } catch (e) {
      if (axios.isAxiosError(e) && (e as AxiosError).code === '409') {
        setShowAssociatedEntityWarning(true);
      }
    }
  };

  const selectedFeatureDataset = useMemo<SelectedFeatureDataset>(() => {
    return {
      selectingComponentId: mapLocationFeatureDataset?.selectingComponentId ?? null,
      location: mapLocationFeatureDataset?.location,
      fileLocation: mapLocationFeatureDataset?.fileLocation ?? null,
      parcelFeature: firstOrNull(mapLocationFeatureDataset?.parcelFeatures),
      pimsFeature: firstOrNull(mapLocationFeatureDataset?.pimsFeatures),
      regionFeature: mapLocationFeatureDataset?.regionFeature ?? null,
      districtFeature: mapLocationFeatureDataset?.districtFeature ?? null,
      municipalityFeature: firstOrNull(mapLocationFeatureDataset?.municipalityFeatures),
      isActive: true,
      displayOrder: 0,
    };
  }, [
    mapLocationFeatureDataset?.selectingComponentId,
    mapLocationFeatureDataset?.location,
    mapLocationFeatureDataset?.fileLocation,
    mapLocationFeatureDataset?.parcelFeatures,
    mapLocationFeatureDataset?.pimsFeatures,
    mapLocationFeatureDataset?.regionFeature,
    mapLocationFeatureDataset?.districtFeature,
    mapLocationFeatureDataset?.municipalityFeatures,
  ]);

  const handleAddToSelection = useCallback(() => {
    prepareForCreation([selectedFeatureDataset]);
  }, [prepareForCreation, selectedFeatureDataset]);

  return (
    <>
      <LoadingBackdrop show={bcaLoading} />
      <MapSideBarLayout
        title={'Property selection'}
        icon={undefined}
        footer={
          <SidebarFooter
            isOkDisabled={formikRef.current?.isSubmitting}
            onSave={handleSaveClick}
            onCancel={handleCancelClick}
            displayRequiredFieldError={isValid === false}
          />
        }
      >
        <>
          <AddPropertiesGuide />
          {exists(selectedFeatureDataset?.parcelFeature) && (
            <StyledButtonWrapper>
              <Button onClick={handleAddToSelection}>Add selected property</Button>
            </StyledButtonWrapper>
          )}
          <Formik<FileForm>
            innerRef={formikRef}
            initialValues={formFile}
            validationSchema={UpdatePropertiesYupSchema}
            onSubmit={async (values: FileForm) => {
              const file: ApiGen_Concepts_File = values.toApi();
              await saveFile(file);
            }}
          >
            {formikProps => (
              <FieldArray name="properties">
                {({ remove, replace }) => (
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
                    <Row className="py-3 no-gutters">
                      <Col>
                        <MapClickMonitor
                          selectedComponentId={null}
                          addProperty={noop}
                          repositionProperty={(
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
                            } else if (!isLatLngInFeatureSetBoundary(latLng, featureset)) {
                              toast.warn(
                                'Please choose a location that is within the (highlighted) boundary of this property.',
                              );
                            }
                          }}
                          modifiedProperties={formikProps.values.properties.map(p =>
                            p.toFeatureDataset(),
                          )}
                        />
                      </Col>
                    </Row>
                    <SelectedPropertyHeaderRow />
                    {formikProps.values.properties.map((property, index) => (
                      <SelectedPropertyRow
                        key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                        onRemove={async () => {
                          if (!property.apiId || (await props.canRemove(property.apiId))) {
                            remove(index);
                          } else {
                            setShowAssociatedEntityWarning(true);
                          }
                        }}
                        nameSpace={`properties.${index}`}
                        index={index}
                        property={property.toFeatureDataset()}
                        showDisable={props.disableProperties}
                      />
                    ))}
                    {formikProps.values.properties.length === 0 && (
                      <span>No Properties selected</span>
                    )}
                  </Section>
                )}
              </FieldArray>
            )}
          </Formik>
        </>
      </MapSideBarLayout>
      <GenericModal
        variant="info"
        display={showSaveConfirmModal}
        title={'Confirm changes'}
        message={
          <>
            <div>You have made changes to the properties in this file.</div>
            <br />
            <strong>Do you want to save these changes?</strong>
          </>
        }
        handleOk={handleSaveConfirm}
        handleCancel={() => setShowSaveConfirmModal(false)}
        okButtonText="Save"
        cancelButtonText="Cancel"
        show
      />
      <GenericModal
        variant="info"
        display={showAssociatedEntityWarning}
        title={'Property with associations'}
        message={
          <>
            <div>
              This property can not be removed from the file. This property is related to one or
              more entities in the file, only properties that are not linked to any entities in the
              file can be removed.
            </div>
          </>
        }
        handleOk={() => setShowAssociatedEntityWarning(false)}
        okButtonText="Close"
        show
      />
    </>
  );
};

export default UpdateProperties;

const StyledButtonWrapper = styled.div`
  margin: 0 1.6rem;
  padding-left: 1.6rem;
  text-align: left;
  text-underline-offset: 2px;

  button {
    font-size: 14px;
  }
`;
