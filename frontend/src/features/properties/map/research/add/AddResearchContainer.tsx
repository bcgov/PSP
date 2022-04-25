import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { Formik, FormikProps } from 'formik';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';
import { useRef } from 'react';
import { MdTopic } from 'react-icons/md';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import ResearchFooter from '../common/ResearchFooter';
import { useAddResearch } from '../hooks/useAddResearch';
import { AddResearchFileYupSchema } from './AddResearchFileYupSchema';
import AddResearchForm from './AddResearchForm';
import { ResearchForm } from './models';

export interface IAddResearchContainerProps {
  onClose: () => void;
}

export const AddResearchContainer: React.FunctionComponent<IAddResearchContainerProps> = props => {
  const formikRef = useRef<FormikProps<ResearchForm>>(null);
  const initialForm = new ResearchForm();
  const { addResearchFile } = useAddResearch();

  const saveResearchFile = async (researchFile: Api_ResearchFile) => {
    const response = await addResearchFile(researchFile);
    if (!!response?.name) {
      formikRef.current?.resetForm();
      props.onClose();
    }
  };

  const handleSave = () => {
    formikRef.current?.setSubmitting(true);
    formikRef.current?.submitForm();
  };

  const handleCancel = () => {
    formikRef.current?.resetForm();
    props.onClose();
  };

  return (
    <MapSideBarLayout
      title="Create Research File"
      icon={<MdTopic title="User Profile" size="2.5rem" className="mr-2" />}
      footer={
        <ResearchFooter
          isSubmitting={formikRef.current?.isSubmitting}
          onSave={handleSave}
          onCancel={handleCancel}
        />
      }
      showCloseButton
      onClose={handleCancel}
    >
      <Formik<ResearchForm>
        innerRef={formikRef}
        initialValues={initialForm}
        onSubmit={async (values: ResearchForm, formikHelpers) => {
          const researchFile: Api_ResearchFile = values.toApi();
          saveResearchFile(researchFile);
          formikHelpers.setSubmitting(false);
        }}
        validationSchema={AddResearchFileYupSchema}
      >
        {formikProps => (
          <StyledFormWrapper>
            <AddResearchForm />

            <Prompt
              when={formikProps.dirty && formikProps.submitCount === 0}
              message="You have made changes on this form. Do you wish to leave without saving?"
            />
          </StyledFormWrapper>
        )}
      </Formik>
    </MapSideBarLayout>
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
