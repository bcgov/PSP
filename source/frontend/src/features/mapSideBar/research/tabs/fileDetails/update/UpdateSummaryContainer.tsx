import { Formik, FormikProps } from 'formik';
import * as React from 'react';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import { Api_ResearchFile } from '@/models/api/ResearchFile';

import { useUpdateResearch } from '../../../hooks/useUpdateResearch';
import { UpdateResearchSummaryFormModel } from './models';
import { UpdateResearchFileYupSchema } from './UpdateResearchFileYupSchema';
import UpdateResearchForm from './UpdateSummaryForm';

export interface IUpdateResearchViewProps {
  researchFile: Api_ResearchFile;
  onSuccess: () => void;
}

export const UpdateResearchContainer = React.forwardRef<FormikProps<any>, IUpdateResearchViewProps>(
  (props, formikRef) => {
    const { updateResearchFile } = useUpdateResearch();

    const saveResearchFile = async (researchFile: Api_ResearchFile) => {
      const response = await updateResearchFile(researchFile);
      if (typeof formikRef === 'function' || formikRef === null) {
        throw Error('unexpected ref prop');
      }
      formikRef.current?.setSubmitting(false);
      if (!!response?.fileName) {
        formikRef.current?.resetForm();
        props.onSuccess();
      }
    };

    return (
      <Formik<UpdateResearchSummaryFormModel>
        enableReinitialize
        innerRef={formikRef}
        validationSchema={UpdateResearchFileYupSchema}
        initialValues={UpdateResearchSummaryFormModel.fromApi(props.researchFile)}
        onSubmit={async (values: UpdateResearchSummaryFormModel) => {
          const researchFile: Api_ResearchFile = values.toApi();
          await saveResearchFile(researchFile);
        }}
      >
        {formikProps => (
          <StyledFormWrapper>
            <UpdateResearchForm formikProps={formikProps} />

            <Prompt
              when={formikProps.dirty && formikProps.submitCount === 0}
              message="You have made changes on this form. Do you wish to leave without saving?"
            />
          </StyledFormWrapper>
        )}
      </Formik>
    );
  },
);

export default UpdateResearchContainer;

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
