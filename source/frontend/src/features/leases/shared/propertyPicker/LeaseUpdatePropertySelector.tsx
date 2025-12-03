import { AxiosError } from 'axios';
import { FieldArray, FieldArrayRenderProps, Formik, FormikProps } from 'formik';
import { useCallback, useContext, useEffect, useMemo, useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import GenericModal, { ModalProps } from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import { ZoomIconType, ZoomToLocation } from '@/components/maps/ZoomToLocation';
import AreaContainer from '@/components/measurements/AreaContainer';
import MapClickMonitor from '@/components/propertySelector/MapClickMonitor';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { ModalContext } from '@/contexts/modalContext';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import AddPropertiesGuide from '@/features/mapSideBar/shared/update/properties/AddPropertiesGuide';
import { UpdatePropertiesYupSchema } from '@/features/mapSideBar/shared/update/properties/UpdatePropertiesYupSchema';
import { usePropertyLeaseRepository } from '@/hooks/repositories/usePropertyLeaseRepository';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useLocationFeatureDatasetsWithAddresses } from '@/hooks/useLocationFeatureDatasetsWithAddresses';
import { getCancelModalProps } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import {
  arePropertyFormsEqual,
  exists,
  isEmptyOrNull,
  isLatLngInFeatureSetBoundary,
  isNumber,
  isValidId,
} from '@/utils';
import { withNameSpace } from '@/utils/formUtils';

import { useLeaseDetail } from '../../hooks/useLeaseDetail';
import { FormLeaseProperty, LeaseFormModel } from '../../models';
import SelectedPropertyHeaderRow from './selectedPropertyList/SelectedPropertyHeaderRow';
interface LeaseUpdatePropertySelectorProp {
  lease: ApiGen_Concepts_Lease;
}

export const LeaseUpdatePropertySelector: React.FunctionComponent<
  LeaseUpdatePropertySelectorProp
> = ({ lease }) => {
  const pathSolver = usePathGenerator();
  const [showSaveConfirmModal, setShowSaveConfirmModal] = useState<boolean>(false);
  const [repositionPropertyIndex, setRepositionPropertyIndex] = useState<number | null>(null);
  const [isValid, setIsValid] = useState<boolean>(true);
  const hasWarnedRef = useRef(false);

  const { setModalContent, setDisplayModal } = useContext(ModalContext);
  const { resetFilePropertyLocations } = useContext(SideBarContext);

  const formikRef = useRef<FormikProps<LeaseFormModel>>();
  const arrayHelpersRef = useRef<FieldArrayRenderProps | null>(null);

  const { updateLeaseProperties } = usePropertyLeaseRepository();
  const { getCompleteLease } = useLeaseDetail(lease?.id ?? undefined);

  const {
    locationFeaturesForAddition,
    processLocationFeaturesAddition: processCreation,
    mapLocationFeatureDataset,
    requestLocationFeatureAddition: prepareForCreation,
    isRepositioning,
    finishReposition,
    setEditPropertiesMode,
    refreshMapProperties,
  } = useMapStateMachine();

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to update Lease File Properties');

  const addPropertiesToCurrentFile = useCallback(
    (
      formikRef: React.RefObject<FormikProps<LeaseFormModel>>,
      leasePropertyForms: FormLeaseProperty[],
    ) => {
      const existingProperties = formikRef.current?.values?.properties ?? [];
      const uniqueProperties = leasePropertyForms.filter(newProperty => {
        return !existingProperties.some(existingProperty =>
          arePropertyFormsEqual(existingProperty.property, newProperty.property),
        );
      });

      const duplicatesSkipped = leasePropertyForms.length - uniqueProperties.length;

      // If there are unique properties, add them to the formik values
      if (uniqueProperties.length > 0) {
        formikRef.current?.setFieldValue('properties', [
          ...existingProperties,
          ...uniqueProperties,
        ]);
        formikRef.current?.setFieldTouched('properties', true);
        toast.success(`Added ${uniqueProperties.length} new property(s) to the file.`);
      }

      if (duplicatesSkipped > 0) {
        toast.warn(`Skipped ${duplicatesSkipped} duplicate property(s).`);
      }
    },
    [],
  );

  // Get PropertyForms with addresses for all selected features
  const { locationFeaturesWithAddresses: featuresWithAddresses, bcaLoading } =
    useLocationFeatureDatasetsWithAddresses(locationFeaturesForAddition ?? []);

  // Convert SelectedFeatureDataset to FormLeaseProperty
  const propertyForms = useMemo<FormLeaseProperty[]>(
    () =>
      featuresWithAddresses.map(obj => {
        const formProperty = FormLeaseProperty.fromFeatureDataset(obj.feature);
        if (exists(obj.address)) {
          formProperty.property.address = obj.address;
        }
        return formProperty;
      }),
    [featuresWithAddresses],
  );

  // This effect is used to update the file properties when "add to open file" is clicked in the worklist.
  useEffect(() => {
    if (exists(formikRef.current) && propertyForms.length > 0 && !hasWarnedRef.current) {
      const needsWarning = propertyForms.some(
        formProperty => exists(formProperty.property) && !isValidId(formProperty.property.apiId),
      );

      if (needsWarning) {
        hasWarnedRef.current = true; // mark as shown
        setModalContent({
          variant: 'info',
          title: 'Not inventory property',
          message:
            'You have selected a property not previously in the inventory. Do you want to add this property to the lease?',
          okButtonText: 'Add',
          cancelButtonText: 'Cancel',
          handleOk: () => {
            setDisplayModal(false);
            addPropertiesToCurrentFile(formikRef, propertyForms);
          },
          handleCancel: () => {
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      } else {
        // If no warning is needed, simply add the properties to the current file.
        addPropertiesToCurrentFile(formikRef, propertyForms);
      }
      processCreation();
    }
  }, [
    formikRef,
    propertyForms,
    setModalContent,
    setDisplayModal,
    addPropertiesToCurrentFile,
    processCreation,
  ]);

  const cancelRemove = useCallback(() => {
    setDisplayModal(false);
  }, [setDisplayModal]);

  const confirmRemove = useCallback(
    (indexToRemove: number) => {
      if (indexToRemove !== undefined) {
        arrayHelpersRef.current?.remove(indexToRemove);
      }
      setDisplayModal(false);
    },
    [setDisplayModal],
  );

  const getRemoveModalProps = useCallback<(index: number) => ModalProps>(
    (index: number) => {
      return {
        variant: 'info',
        title: 'Removing Property from Lease/Licence',
        message: 'Are you sure you want to remove this property from this lease/licence?',
        display: false,
        okButtonText: 'Remove',
        cancelButtonText: 'Cancel',
        handleOk: () => confirmRemove(index),
        handleCancel: cancelRemove,
      };
    },
    [confirmRemove, cancelRemove],
  );

  const onRemoveClick = useCallback(
    (index: number) => {
      setModalContent(getRemoveModalProps(index));
      setDisplayModal(true);
    },
    [getRemoveModalProps, setDisplayModal, setModalContent],
  );

  const handleCancelConfirm = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
    processCreation();
    resetFilePropertyLocations();
    pathSolver.showFile('lease', lease.id);
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

  const saveFile = async (file: ApiGen_Concepts_Lease) => {
    return withUserOverride(
      (userOverrideCodes: UserOverrideCode[]) => {
        return updateLeaseProperties.execute(file, userOverrideCodes).then(async response => {
          formikRef.current?.setSubmitting(false);
          if (isValidId(response?.id)) {
            if (file.fileProperties?.find(fp => !fp.property?.address && !fp.property?.id)) {
              toast.warn(
                'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
                { autoClose: 15000 },
              );
            }
            formikRef.current?.resetForm();
            await getCompleteLease();
            refreshMapProperties();
            pathSolver.showFile('lease', lease.id);
          }
        });
      },
      [],
      (axiosError: AxiosError<IApiError>) => {
        setModalContent({
          variant: 'error',
          title: 'Error',
          message: axiosError?.response?.data.error,
          okButtonText: 'Close',
          handleOk: async () => {
            formikRef.current?.resetForm();
            await getCompleteLease();
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      },
    );
  };

  const initialValues = LeaseFormModel.fromApi(lease);

  const handleAddToSelection = useCallback(() => {
    prepareForCreation([mapLocationFeatureDataset]);
  }, [prepareForCreation, mapLocationFeatureDataset]);

  useEffect(() => {
    // Set the map state machine to edit properties mode so that the map selector knows what mode it is in.
    setEditPropertiesMode(true);
    return () => {
      setEditPropertiesMode(false);
    };
  }, [setEditPropertiesMode]);

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
        <Formik<LeaseFormModel>
          innerRef={formikRef}
          initialValues={initialValues}
          validationSchema={UpdatePropertiesYupSchema}
          onSubmit={async (values: LeaseFormModel) => {
            try {
              const file: ApiGen_Concepts_Lease = LeaseFormModel.toApi(values);
              await saveFile(file);
            } finally {
              processCreation();
            }
          }}
        >
          {formikProps => (
            <FieldArray
              name="properties"
              render={arrayHelpers => {
                arrayHelpersRef.current = arrayHelpers;
                return (
                  <>
                    <AddPropertiesGuide />
                    {!isEmptyOrNull(mapLocationFeatureDataset?.parcelFeatures) && (
                      <StyledButtonWrapper>
                        <Button onClick={handleAddToSelection}>Add selected property</Button>
                      </StyledButtonWrapper>
                    )}
                    <Section
                      header={
                        <Row>
                          <Col xs="11">Selected Properties</Col>
                          <Col>
                            <ZoomToLocation
                              icon={ZoomIconType.area}
                              formProperties={formikProps.values.properties.map(lp => lp.property)}
                            />
                          </Col>
                        </Row>
                      }
                    >
                      <MapClickMonitor
                        onNewLocation={(
                          locationDataSet: LocationFeatureDataset,
                          hasMultipleProperties: boolean,
                        ) => {
                          if (
                            isRepositioning &&
                            isNumber(repositionPropertyIndex) &&
                            repositionPropertyIndex >= 0 &&
                            !hasMultipleProperties
                          ) {
                            // As long as the marker is repositioned within the boundary of the originally selected property simply reposition the marker without further notification.
                            const formProperty =
                              formikRef?.current?.values?.properties[repositionPropertyIndex];

                            if (
                              isLatLngInFeatureSetBoundary(
                                locationDataSet.location,
                                formProperty.property.toLocationFeatureDataset(),
                              )
                            ) {
                              const updatedFormProperty =
                                FormLeaseProperty.fromFormLeaseProperty(formProperty);
                              updatedFormProperty.property.fileLocation = locationDataSet.location;

                              // Find property within formik values and reposition it based on incoming file marker position
                              arrayHelpers.replace(repositionPropertyIndex, updatedFormProperty);

                              // Reset the reposition state
                              finishReposition();
                              setRepositionPropertyIndex(null);
                            }
                          } else {
                            toast.warn(
                              'Please choose a location that is within the (highlighted) boundary of this property.',
                            );
                          }
                        }}
                      />
                      <SelectedPropertyHeaderRow />
                      {formikProps.values.properties.map((leaseProperty, index) => {
                        const property = leaseProperty?.property;
                        if (exists(property)) {
                          const nameSpace = `properties.${index}`;
                          return (
                            <>
                              <SelectedPropertyRow
                                key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                                onRemove={() => onRemoveClick(index)}
                                canReposition={false}
                                nameSpace={`${nameSpace}.property`}
                                index={index}
                                property={property}
                                showDisable={false}
                                canUploadShapefile={false}
                              />
                              <Row className="align-items-center mb-3 no-gutters">
                                <Col md={{ span: 9, offset: 3 }}>
                                  <AreaContainer
                                    isEditable
                                    field={withNameSpace(nameSpace, 'landArea')}
                                    landArea={leaseProperty.landArea}
                                    unitCode={leaseProperty.areaUnitTypeCode}
                                    onChange={(landArea, areaUnitTypeCode) => {
                                      formikProps.setFieldValue(
                                        withNameSpace(nameSpace, 'landArea'),
                                        landArea,
                                      );
                                      formikProps.setFieldValue(
                                        withNameSpace(nameSpace, 'areaUnitTypeCode'),
                                        areaUnitTypeCode,
                                      );
                                    }}
                                  />
                                </Col>
                              </Row>
                            </>
                          );
                        }
                        return <></>;
                      })}
                      {formikProps.values.properties.length === 0 && (
                        <span>No Properties selected</span>
                      )}
                    </Section>
                  </>
                );
              }}
            />
          )}
        </Formik>
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
    </>
  );
};

export default LeaseUpdatePropertySelector;

const StyledButtonWrapper = styled.div`
  margin: 0 1.6rem;
  padding-left: 1.6rem;
  text-align: left;
  text-underline-offset: 2px;

  button {
    font-size: 14px;
  }
`;
