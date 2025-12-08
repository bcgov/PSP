import { Formik, FormikProps } from 'formik';
import { useCallback, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import ResearchFileIcon from '@/assets/images/research-icon.svg?react';
import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useModalContext } from '@/hooks/useModalContext';
import { usePropertyFormSyncronizer } from '@/hooks/usePropertyFormSyncronizer';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, isValidId } from '@/utils';

import { PropertyForm } from '../../shared/models';
import SidebarFooter from '../../shared/SidebarFooter';
import { StyledFormWrapper } from '../../shared/styles';
import { useAddResearch } from '../hooks/useAddResearch';
import { AddResearchFileYupSchema } from './AddResearchFileYupSchema';
import { ResearchForm } from './models';

export interface IAddResearchContainerProps {
  onClose: () => void;
  onSuccess: (newResearchId: number) => void;
  View: React.FC;
}

export const AddResearchContainer: React.FunctionComponent<IAddResearchContainerProps> = props => {
  const { onClose, onSuccess, View } = props;
  const history = useHistory();
  const formikRef = useRef<FormikProps<ResearchForm>>(null);
  const mapMachine = useMapStateMachine();
  const { setModalContent, setDisplayModal } = useModalContext();
  const { execute: getPropertyAssociations } = usePropertyAssociations();
  const { addResearchFile } = useAddResearch();

  const [needsFirstTimeConfirmation, setNeedsFirstTimeConfirmation] = useState<boolean>(true);

  const initialForm = new ResearchForm();

  // Warn user that property is part of an existing research file
  const confirmProperty = useCallback(
    async (propertyForm: PropertyForm): Promise<boolean> => {
      if (isValidId(propertyForm.apiId)) {
        const response = await getPropertyAssociations(propertyForm.apiId);
        const researchAssociations = response?.researchAssociations ?? [];
        const otherResearchFiles = researchAssociations.filter(a => exists(a.id));
        return otherResearchFiles.length > 0;
      } else {
        // the property is not in PIMS db -> no need to confirm
        return false;
      }
    },
    [getPropertyAssociations],
  );

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to add Research File');

  // Require user confirmation before adding a property to file
  const confirmBeforeAdd = useCallback(
    async (
      newPropertyForms: PropertyForm[],
      isValidCallback: (isValid: boolean, newProperties: PropertyForm[]) => void,
    ) => {
      const needsConfirmation = await Promise.all(
        newPropertyForms.map(formProperty => confirmProperty(formProperty)),
      );
      if (needsFirstTimeConfirmation && needsConfirmation.some(x => x === true)) {
        // show the user confirmation modal only once when creating a file
        setNeedsFirstTimeConfirmation(false);
        setModalContent({
          variant: 'warning',
          title: 'User Override Required',
          message: (
            <>
              <p>One or more properties have already been added to one or more research files.</p>
              <p>Do you want to acknowledge and proceed?</p>
            </>
          ),
          okButtonText: 'Yes',
          cancelButtonText: 'No',
          handleOk: () => {
            // allow the properties to be added to the file being created
            isValidCallback(true, newPropertyForms);
            setDisplayModal(false);
          },
          handleCancel: () => {
            // clear out the properties array as the user did not agree to the popup
            isValidCallback(false, []);
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      } else {
        isValidCallback(true, newPropertyForms);
      }
    },
    [confirmProperty, needsFirstTimeConfirmation, setDisplayModal, setModalContent],
  );

  // Get PropertyForms with addresses for all selected features
  const { isLoading } = usePropertyFormSyncronizer(formikRef, confirmBeforeAdd);

  const saveResearchFile = async (
    researchFile: ApiGen_Concepts_ResearchFile,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    formikRef.current?.setSubmitting(true);
    try {
      const response = await addResearchFile(researchFile, userOverrideCodes);

      if (response?.fileName) {
        if (researchFile.fileProperties?.find(fp => !fp.property?.address && !fp.property?.id)) {
          toast.warn(
            'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
            { autoClose: 15000 },
          );
        }
        mapMachine.refreshMapProperties();
        formikRef.current?.resetForm({ values: ResearchForm.fromApi(response) });
        onSuccess(response.id);
      }
    } finally {
      formikRef.current?.setSubmitting(false);
    }
  };

  const handleSave = () => {
    return formikRef.current?.submitForm() ?? Promise.resolve();
  };

  const cancelFunc = () => {
    onClose();
  };

  const checkState = useCallback(() => {
    return formikRef?.current?.dirty && !formikRef?.current?.isSubmitting;
  }, [formikRef]);

  return (
    <Formik<ResearchForm>
      innerRef={formikRef}
      initialValues={initialForm}
      enableReinitialize
      onSubmit={async (values: ResearchForm) => {
        const researchFile: ApiGen_Concepts_ResearchFile = values.toApi();
        return withUserOverride((userOverrideCodes: UserOverrideCode[]) =>
          saveResearchFile(researchFile, userOverrideCodes),
        );
      }}
      validationSchema={AddResearchFileYupSchema}
    >
      {formikProps => (
        <MapSideBarLayout
          title="Create Research File"
          icon={<ResearchFileIcon title="Research file Icon" fill="currentColor" />}
          footer={
            <SidebarFooter
              isOkDisabled={formikProps?.isSubmitting || isLoading}
              onSave={handleSave}
              onCancel={cancelFunc}
              displayRequiredFieldError={!formikProps.isValid && !!formikProps.submitCount}
            />
          }
          showCloseButton
          onClose={cancelFunc}
        >
          <StyledFormWrapper>
            <LoadingBackdrop show={isLoading} parentScreen={true} />
            <View />
          </StyledFormWrapper>
          <ConfirmNavigation navigate={history.push} shouldBlockNavigation={checkState} />
        </MapSideBarLayout>
      )}
    </Formik>
  );
};

export default AddResearchContainer;
