import axios, { AxiosError } from 'axios';
import { Formik, FormikProps } from 'formik';
import { useCallback, useContext, useRef, useState } from 'react';
import { toast } from 'react-toastify';

import GenericModal from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { usePropertyFormSyncronizer } from '@/hooks/usePropertyFormSyncronizer';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { isValidId } from '@/utils';

import { FileForm, PropertyForm } from '../../models';
import SidebarFooter from '../../SidebarFooter';
import PropertiesListContainer from './PropertiesListContainer';
import { UpdatePropertiesYupSchema } from './UpdatePropertiesYupSchema';

export interface IUpdatePropertiesContainerProps {
  formFile: FileForm;
  setIsShowingPropertySelector: (isShowing: boolean) => void;
  onSuccess: (updateProperties?: boolean, updateFile?: boolean) => void;
  updateFileProperties: (
    file: FileForm,
    userOverrideCodes: UserOverrideCode[],
  ) => Promise<ApiGen_Concepts_File | undefined>;
  canRemove: (propertyId: number) => Promise<boolean>;
  canAdd: (propertyForm: PropertyForm) => Promise<boolean>;
  confirmAddMessage: React.ReactNode;
  formikRef?: React.RefObject<FormikProps<any>>;
  showDisabledProperties?: boolean;
  canUploadShapefiles?: boolean;
  canReposition?: boolean;
  showArea?: boolean;
}

export const UpdatePropertiesContainer: React.FunctionComponent<
  IUpdatePropertiesContainerProps
> = props => {
  const localRef = useRef<FormikProps<FileForm>>(null);
  const formikRef = props.formikRef ? props.formikRef : localRef;
  const canAdd = props.canAdd;
  const confirmAddMessage = props.confirmAddMessage;

  const [showSaveConfirmModal, setShowSaveConfirmModal] = useState<boolean>(false);
  const [showAssociatedEntityWarning, setShowAssociatedEntityWarning] = useState<boolean>(false);
  const [isValid, setIsValid] = useState<boolean>(true);
  const { setModalContent, setDisplayModal, modalProps } = useModalContext();
  const { resetFilePropertyLocations } = useContext(SideBarContext);

  // Require user confirmation before adding a property to file
  const confirmBeforeAdd = useCallback(
    async (newPropertyForms: PropertyForm[], isValidCallback: (isValid: boolean) => void) => {
      const needsConfirmation = await Promise.all(
        newPropertyForms.map(formProperty => canAdd(formProperty)),
      );
      debugger;
      if (needsConfirmation.some(x => x === true) && !modalProps.display) {
        setModalContent({
          variant: 'warning',
          title: 'User Override Required',
          message: confirmAddMessage,
          okButtonText: 'Yes',
          cancelButtonText: 'No',
          handleOk: () => {
            // allow the property to be added to the file being created
            isValidCallback(true);
            setDisplayModal(false);
          },
          handleCancel: () => {
            // clear out the properties array as the user did not agree to the popup
            isValidCallback(false);
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      } else {
        isValidCallback(true);
      }
    },
    [modalProps.display, canAdd, setModalContent, confirmAddMessage, setDisplayModal],
  );

  const { isLoading } = usePropertyFormSyncronizer(formikRef, 'properties', confirmBeforeAdd);

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

  const saveFile = async (fileForm: FileForm) => {
    try {
      const response = await props.updateFileProperties(fileForm, []);

      formikRef?.current?.setSubmitting(false);
      if (isValidId(response?.id)) {
        if (response.fileProperties?.find(fp => !fp.property?.address && !fp.property?.id)) {
          toast.warn(
            'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
            { autoClose: 15000 },
          );
        }
        formikRef?.current?.resetForm();
        props.setIsShowingPropertySelector(false);
        props.onSuccess(true);
      }
    } catch (e) {
      if (axios.isAxiosError(e) && (e as AxiosError).code === '409') {
        setShowAssociatedEntityWarning(true);
      }
    }
  };

  const onRemoveClick = async (propertyApiId: number, removeCallback: () => void) => {
    if (await props.canRemove(propertyApiId)) {
      removeCallback();
    } else {
      setShowAssociatedEntityWarning(true);
    }
  };

  return (
    <>
      <LoadingBackdrop show={isLoading} />
      <MapSideBarLayout
        title={'Property selection'}
        icon={undefined}
        footer={
          <SidebarFooter
            isOkDisabled={formikRef?.current?.isSubmitting}
            onSave={handleSaveClick}
            onCancel={handleCancelClick}
            displayRequiredFieldError={isValid === false}
          />
        }
      >
        <Formik<FileForm>
          innerRef={formikRef}
          initialValues={props.formFile}
          validationSchema={UpdatePropertiesYupSchema}
          onSubmit={async (values: FileForm) => {
            await saveFile(values);
          }}
        >
          {formikProps => (
            <PropertiesListContainer
              properties={formikProps.values.properties}
              verifyCanRemove={onRemoveClick}
              needsConfirmationBeforeAdd={() => Promise.resolve(true)}
              canUploadShapefiles={props.canUploadShapefiles}
              canReposition={props.canReposition}
              showDisabledProperties={props.showDisabledProperties}
              showArea={props.showArea}
            />
          )}
        </Formik>
      </MapSideBarLayout>
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

export default UpdatePropertiesContainer;
