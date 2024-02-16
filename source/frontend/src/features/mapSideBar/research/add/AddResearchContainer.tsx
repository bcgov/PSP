import { Formik, FormikProps } from 'formik';
import * as React from 'react';
import { useEffect, useMemo, useRef } from 'react';
import { MdTopic } from 'react-icons/md';
import { Prompt, useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useInitialMapSelectorProperties } from '@/hooks/useInitialMapSelectorProperties';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
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

export const AddResearchContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddResearchContainerProps>
> = props => {
  const history = useHistory();
  const formikRef = useRef<FormikProps<ResearchForm>>(null);
  const mapMachine = useMapStateMachine();
  const selectedFeatureDataset = mapMachine.selectedFeatureDataset;

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

  useEffect(() => {
    if (!!initialForm && !!formikRef.current) {
      formikRef.current.resetForm();
      formikRef.current?.setFieldValue('properties', initialForm.properties);
    }
  }, [initialForm]);

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

  const handleCancel = () => {
    props.onClose();
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
              onCancel={handleCancel}
              displayRequiredFieldError={!formikProps.isValid && !!formikProps.submitCount}
            />
          }
          showCloseButton
          onClose={handleCancel}
        >
          <StyledFormWrapper>
            <AddResearchForm />

            <Prompt
              when={
                (formikProps.dirty ||
                  (formikProps.values.properties !== initialForm.properties &&
                    formikProps.submitCount === 0) ||
                  (!formikProps.values.id && formikProps.values.properties.length > 0)) &&
                !formikProps.isSubmitting
              }
              message="You have made changes on this form. Do you wish to leave without saving?"
            />
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
