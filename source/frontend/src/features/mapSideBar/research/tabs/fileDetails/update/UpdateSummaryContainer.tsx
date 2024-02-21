import { Formik, FormikProps } from 'formik';
import * as React from 'react';
import styled from 'styled-components';

import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';

import { useUpdateResearch } from '../../../hooks/useUpdateResearch';
import { UpdateResearchSummaryFormModel } from './models';
import { UpdateResearchFileYupSchema } from './UpdateResearchFileYupSchema';
import UpdateResearchForm from './UpdateSummaryForm';

export interface IUpdateResearchViewProps {
  researchFile: ApiGen_Concepts_ResearchFile;
  onSuccess: () => void;
}

export const UpdateResearchContainer = React.forwardRef<FormikProps<any>, IUpdateResearchViewProps>(
  (props, formikRef) => {
    const { updateResearchFile } = useUpdateResearch();

    const saveResearchFile = async (researchFile: ApiGen_Concepts_ResearchFile) => {
      const response = await updateResearchFile(researchFile);
      if (typeof formikRef === 'function' || formikRef === null) {
        throw Error('unexpected ref prop');
      }
      formikRef.current?.setSubmitting(false);
      if (response?.fileName) {
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
          const researchFile: ApiGen_Concepts_ResearchFile = values.toApi();
          await saveResearchFile(researchFile);
        }}
      >
        {formikProps => (
          <StyledFormWrapper>
            <UpdateResearchForm formikProps={formikProps} />
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
