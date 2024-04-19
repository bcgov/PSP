import axios, { AxiosError } from 'axios';
import { FieldArray, Formik, FormikProps } from 'formik';
import { useContext, useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { toast } from 'react-toastify';

import GenericModal from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import MapSelectorContainer from '@/components/propertySelector/MapSelectorContainer';
import { IMapProperty } from '@/components/propertySelector/models';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { isValidId } from '@/utils';

import { AddressForm, FileForm, PropertyForm } from '../../models';
import SidebarFooter from '../../SidebarFooter';
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
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();

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

  const defaultConfirmBeforeAddMessage = (
    <>
      <p>This property has already been added to one or more files.</p>
      <p>Do you want to acknowledge and proceed?</p>
    </>
  );

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
                                const bcaSummary = await getPrimaryAddressByPid(
                                  property.pid,
                                  30000,
                                );
                                formProperty.address = bcaSummary?.address
                                  ? AddressForm.fromBcaAddress(bcaSummary?.address)
                                  : undefined;
                              }

                              if (await props.confirmBeforeAdd(formProperty)) {
                                // Require user confirmation before adding property to file
                                setModalContent({
                                  variant: 'warning',
                                  title: 'User Override Required',
                                  message:
                                    props.confirmBeforeAddMessage ?? defaultConfirmBeforeAddMessage,
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
                        modifiedProperties={formikProps.values.properties}
                      />
                    </Col>
                  </Row>
                  <Section header="Selected properties">
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
                        property={property.toMapProperty()}
                      />
                    ))}
                    {formikProps.values.properties.length === 0 && (
                      <span>No Properties selected</span>
                    )}
                  </Section>
                </>
              )}
            </FieldArray>
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
