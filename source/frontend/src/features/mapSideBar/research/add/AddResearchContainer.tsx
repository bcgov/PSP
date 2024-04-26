import { Formik, FormikProps } from 'formik';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { MdTopic } from 'react-icons/md';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useInitialMapSelectorProperties } from '@/hooks/useInitialMapSelectorProperties';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, isValidId, isValidString } from '@/utils';
import { featuresetToMapProperty } from '@/utils/mapPropertyUtils';

import { PropertyForm } from '../../shared/models';
import SidebarFooter from '../../shared/SidebarFooter';
import { useAddResearch } from '../hooks/useAddResearch';
import { AddResearchFileYupSchema } from './AddResearchFileYupSchema';
import AddResearchForm from './AddResearchForm';
import { ResearchForm } from './models';

export interface IAddResearchContainerProps {
  onClose: () => void;
}

export const AddResearchContainer: React.FunctionComponent<IAddResearchContainerProps> = props => {
  const { onClose } = props;
  const history = useHistory();
  const formikRef = useRef<FormikProps<ResearchForm>>(null);
  const mapMachine = useMapStateMachine();
  const selectedFeatureDataset = mapMachine.selectedFeatureDataset;
  const { setModalContent, setDisplayModal } = useModalContext();
  const { execute: getPropertyAssociations } = usePropertyAssociations();
  const {
    getPropertyByPidWrapper: { execute: getPropertyByPid },
    getPropertyByPinWrapper: { execute: getPropertyByPin },
  } = usePimsPropertyRepository();
  const [needsUserConfirmation, setNeedsUserConfirmation] = useState<boolean>(true);

  // Warn user that property is part of an existing research file
  const confirmBeforeAdd = useCallback(
    async (propertyForm: PropertyForm): Promise<boolean> => {
      let apiId;
      try {
        if (isValidId(propertyForm.apiId)) {
          apiId = propertyForm.apiId;
        } else if (isValidString(propertyForm.pid)) {
          const result = await getPropertyByPid(propertyForm.pid);
          apiId = result?.id;
        } else if (isValidString(propertyForm.pin)) {
          const result = await getPropertyByPin(Number(propertyForm.pin));
          apiId = result?.id;
        }
      } catch (e) {
        apiId = 0;
      }

      if (isValidId(apiId)) {
        const response = await getPropertyAssociations(apiId);
        const researchAssociations = response?.researchAssociations ?? [];
        const otherResearchFiles = researchAssociations.filter(a => exists(a.id));
        return otherResearchFiles.length > 0;
      } else {
        // the property is not in PIMS db -> no need to confirm
        return false;
      }
    },
    [getPropertyAssociations, getPropertyByPid, getPropertyByPin],
  );

  const initialForm = useMemo(() => {
    const researchForm = new ResearchForm();
    if (selectedFeatureDataset) {
      researchForm.properties = [
        PropertyForm.fromMapProperty(featuresetToMapProperty(selectedFeatureDataset)),
      ];
    }
    return researchForm;
  }, [selectedFeatureDataset]);

  const { addResearchFile } = useAddResearch();
  const { bcaLoading, initialProperty } = useInitialMapSelectorProperties(selectedFeatureDataset);
  if (initialForm?.properties.length && initialProperty) {
    initialForm.properties[0].address = initialProperty.address;
  }
  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to add Research File');

  // Require user confirmation before adding a property to file
  // This is the flow for Map Marker -> right-click -> create Research File
  useEffect(() => {
    const runAsync = async () => {
      if (exists(initialForm) && exists(formikRef.current) && needsUserConfirmation) {
        if (initialForm.properties.length > 0) {
          const formProperty = initialForm.properties[0];
          if (await confirmBeforeAdd(formProperty)) {
            setModalContent({
              variant: 'warning',
              title: 'User Override Required',
              message: (
                <>
                  <p>This property has already been added to one or more research files.</p>
                  <p>Do you want to acknowledge and proceed?</p>
                </>
              ),
              okButtonText: 'Yes',
              cancelButtonText: 'No',
              handleOk: () => {
                // allow the property to be added to the file being created
                formikRef.current.resetForm();
                formikRef.current.setFieldValue('properties', initialForm.properties);
                setDisplayModal(false);
                // show the user confirmation modal only once when creating a file
                setNeedsUserConfirmation(false);
              },
              handleCancel: () => {
                // clear out the properties array as the user did not agree to the popup
                initialForm.properties.splice(0, initialForm.properties.length);
                formikRef.current.resetForm();
                formikRef.current.setFieldValue('properties', initialForm.properties);
                setDisplayModal(false);
                // show the user confirmation modal only once when creating a file
                setNeedsUserConfirmation(false);
              },
            });
            setDisplayModal(true);
          }
        }
      }
    };

    runAsync();
  }, [confirmBeforeAdd, initialForm, needsUserConfirmation, setDisplayModal, setModalContent]);

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
        history.replace(`/mapview/sidebar/research/${response.id}`);
        formikRef.current?.resetForm({ values: ResearchForm.fromApi(response) });
      }
    } finally {
      formikRef.current?.setSubmitting(false);
    }
  };

  const handleSave = () => {
    return formikRef.current?.submitForm() ?? Promise.resolve();
  };

  const cancelFunc = (resetForm: () => void, dirty: boolean) => {
    if (!dirty) {
      resetForm();
      onClose();
    } else {
      setModalContent({
        ...getCancelModalProps(),
        handleOk: () => {
          resetForm();
          setDisplayModal(false);
          onClose();
        },
      });
      setDisplayModal(true);
    }
  };

  return (
    <Formik<ResearchForm>
      innerRef={formikRef}
      initialValues={initialForm}
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
          icon={<MdTopic title="User Profile" size="2.5rem" className="mr-2" />}
          footer={
            <SidebarFooter
              isOkDisabled={formikProps?.isSubmitting || bcaLoading}
              onSave={handleSave}
              onCancel={() => cancelFunc(formikProps.resetForm, formikProps.dirty)}
              displayRequiredFieldError={!formikProps.isValid && !!formikProps.submitCount}
            />
          }
          showCloseButton
          onClose={() => cancelFunc(formikProps.resetForm, formikProps.dirty)}
        >
          <StyledFormWrapper>
            <AddResearchForm confirmBeforeAdd={confirmBeforeAdd} />
          </StyledFormWrapper>
        </MapSideBarLayout>
      )}
    </Formik>
  );
};

export default AddResearchContainer;

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
