import { AxiosError } from 'axios';
import { FieldArray, FieldArrayRenderProps, Formik, FormikProps } from 'formik';
import { geoJSON, LatLngLiteral } from 'leaflet';
import isNumber from 'lodash/isNumber';
import { useCallback, useContext, useEffect, useMemo, useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { PiCornersOut } from 'react-icons/pi';
import { toast } from 'react-toastify';

import { LinkButton } from '@/components/common/buttons';
import GenericModal, { ModalProps } from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import MapSelectorContainer from '@/components/propertySelector/MapSelectorContainer';
import { IMapProperty } from '@/components/propertySelector/models';
import { ModalContext } from '@/contexts/modalContext';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { AddressForm } from '@/features/mapSideBar/shared/models';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import { UpdatePropertiesYupSchema } from '@/features/mapSideBar/shared/update/properties/UpdatePropertiesYupSchema';
import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';
import { useProperties } from '@/hooks/repositories/useProperties';
import { usePropertyLeaseRepository } from '@/hooks/repositories/usePropertyLeaseRepository';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useFeatureDatasetsWithAddresses } from '@/hooks/useFeatureDatasetsWithAddresses';
import { getCancelModalProps } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_PropertyView } from '@/models/api/generated/ApiGen_Concepts_PropertyView';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import {
  arePropertyFormsEqual,
  exists,
  isLatLngInFeatureSetBoundary,
  isValidId,
  isValidString,
  latLngLiteralToGeometry,
} from '@/utils';

import { useLeaseDetail } from '../../hooks/useLeaseDetail';
import { FormLeaseProperty, LeaseFormModel } from '../../models';
import SelectedPropertyHeaderRow from './selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from './selectedPropertyList/SelectedPropertyRow';

interface LeaseUpdatePropertySelectorProp {
  lease: ApiGen_Concepts_Lease;
}

export const LeaseUpdatePropertySelector: React.FunctionComponent<
  LeaseUpdatePropertySelectorProp
> = ({ lease }) => {
  const [showSaveConfirmModal, setShowSaveConfirmModal] = useState<boolean>(false);

  const formikRef = useRef<FormikProps<LeaseFormModel>>(null);

  const { resetFilePropertyLocations } = useContext(SideBarContext);

  const [isValid, setIsValid] = useState<boolean>(true);
  const { getPropertiesFromView: getProperties } = useProperties();
  const { setModalContent, setDisplayModal } = useContext(ModalContext);
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();

  const pathSolver = usePathGenerator();

  const { updateLeaseProperties } = usePropertyLeaseRepository();

  const arrayHelpersRef = useRef<FieldArrayRenderProps | null>(null);

  const { getCompleteLease } = useLeaseDetail(lease?.id ?? undefined);

  const {
    requestFlyToBounds,
    refreshMapProperties,
    setEditPropertiesMode,
    selectedFeatures,
    processCreation,
  } = useMapStateMachine();

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to update Lease File Properties');

  useEffect(() => {
    // Set the map state machine to edit properties mode so that the map selector knows what mode it is in.
    setEditPropertiesMode(true);
    return () => {
      setEditPropertiesMode(false);
    };
  }, [setEditPropertiesMode]);

  // Get FormLeaseProperties with addresses for all selected features
  const { featuresWithAddresses } = useFeatureDatasetsWithAddresses(selectedFeatures ?? []);

  // Convert SelectedFeatureDataset to FormLeaseProperty
  const leasePropertyForms = useMemo<FormLeaseProperty[]>(
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

  // This effect is used to update the file properties when "add to open file" is clicked in the worklist.
  useEffect(() => {
    if (exists(formikRef.current) && leasePropertyForms.length > 0) {
      addPropertiesToCurrentFile(formikRef, leasePropertyForms);
      processCreation();
    }
  }, [addPropertiesToCurrentFile, formikRef, processCreation, leasePropertyForms]);

  const addProperties = useCallback((properties: FormLeaseProperty[]) => {
    const formikProps = formikRef.current;
    if (arrayHelpersRef.current !== null && properties.length > 0) {
      properties.forEach((leaseProperty, index) => {
        const property = leaseProperty?.property;
        if (
          formikProps.values.properties?.length === 0 &&
          index === 0 &&
          property !== undefined &&
          property.regionName !== 'Cannot determine'
        ) {
          formikProps.setFieldValue('regionId', property.region ? property.region.toString() : '');
        }

        arrayHelpersRef.current && arrayHelpersRef.current.push(leaseProperty);
      });
    }
  }, []);

  const searchProperty = async (
    newProperty: IMapProperty,
  ): Promise<ApiGen_Concepts_PropertyView[] | undefined> => {
    const params: IPropertyFilter = {
      pid: exists(newProperty.pid) ? newProperty.pid.toString() : '',
      pin: exists(newProperty.pin) ? newProperty.pin.toString() : '',
      searchBy: exists(newProperty.pid) ? 'pid' : 'pin',
      address: '',
      page: undefined,
      quantity: undefined,
      latitude: undefined,
      longitude: undefined,
      historical: '',
      ownership: '',
      coordinates: null,
      name: '',
      section: '',
      township: '',
      range: '',
      district: '',
    };

    const result = await getProperties.execute(params);
    return result?.items ?? undefined;
  };
  const fitBoundaries = () => {
    const fileProperties = formikRef?.current?.values?.properties;

    if (exists(fileProperties)) {
      const locations = fileProperties.map(
        p => p?.property?.polygon ?? latLngLiteralToGeometry(p?.property?.fileLocation),
      );
      const bounds = geoJSON(locations).getBounds();

      if (exists(bounds) && bounds.isValid()) {
        requestFlyToBounds(bounds);
      }
    }
  };

  const confirmAdd = useCallback(
    (propertiesToConfirm: FormLeaseProperty[]) => {
      setDisplayModal(false);
      addProperties(propertiesToConfirm);
    },
    [setDisplayModal, addProperties],
  );

  const cancelAdd = useCallback(() => {
    setDisplayModal(false);
  }, [setDisplayModal]);

  const getAddModalProps = useCallback<(properties: FormLeaseProperty[]) => ModalProps>(
    (properties: FormLeaseProperty[]) => {
      return {
        variant: 'info',
        title: 'Not inventory property',
        message:
          'You have selected a property not previously in the inventory. Do you want to add this property to the lease?',
        display: false,
        okButtonText: 'Add',
        cancelButtonText: 'Cancel',
        handleOk: () => confirmAdd(properties),
        handleCancel: cancelAdd,
      };
    },
    [confirmAdd, cancelAdd],
  );

  const processAddedProperties = async (newProperties: SelectedFeatureDataset[]) => {
    let needsWarning = false;
    const newFormProperties: FormLeaseProperty[] = [];

    await newProperties.reduce(async (promise, property) => {
      return promise.then(async () => {
        const formProperty = FormLeaseProperty.fromFeatureDataset(property);

        const bcaSummary = formProperty?.property?.pid
          ? await getPrimaryAddressByPid(formProperty?.property?.pid, 30000)
          : undefined;

        // Retrieve the pims id of the property if it exists
        if (exists(formProperty.property) && !isValidId(formProperty.property.apiId)) {
          needsWarning = true;

          formProperty.property.address = bcaSummary?.address
            ? AddressForm.fromBcaAddress(bcaSummary?.address)
            : undefined;

          if (
            isValidString(formProperty?.property?.pid) ||
            isValidString(formProperty?.property?.pin)
          ) {
            const result = await searchProperty(formProperty.property.toMapProperty());
            if (result !== undefined && result.length > 0) {
              formProperty.property.apiId = result[0].id;
            }
          }
        }

        newFormProperties.push(formProperty);
      });
    }, Promise.resolve());

    if (needsWarning) {
      setModalContent(getAddModalProps(newFormProperties));
      setDisplayModal(true);
    } else {
      addProperties(newFormProperties);
    }
  };

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
            const file: ApiGen_Concepts_Lease = LeaseFormModel.toApi(values);
            await saveFile(file);
          }}
        >
          {formikProps => (
            <FieldArray
              name="properties"
              render={arrayHelpers => {
                arrayHelpersRef.current = arrayHelpers;
                return (
                  <>
                    <Row className="py-3 no-gutters">
                      <Col>
                        <MapSelectorContainer
                          addSelectedProperties={processAddedProperties}
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
                              const updatedFormProperty =
                                FormLeaseProperty.fromFormLeaseProperty(formProperty);
                              updatedFormProperty.property.fileLocation = latLng;

                              // Find property within formik values and reposition it based on incoming file marker position
                              arrayHelpers.replace(index, updatedFormProperty);
                            }
                          }}
                          modifiedProperties={LeaseFormModel.getPropertiesAsForm(
                            formikProps.values,
                          ).map(p => p.toFeatureDataset())}
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
                      {formikProps.values.properties.map((leaseProperty, index) => {
                        const property = leaseProperty?.property;
                        if (property !== undefined) {
                          return (
                            <SelectedPropertyRow
                              formikProps={formikProps}
                              key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                              onRemove={() => onRemoveClick(index)}
                              nameSpace={`properties.${index}`}
                              index={index}
                              property={property.toFeatureDataset()}
                              showSeparator={index < formikProps.values.properties.length - 1}
                            />
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
