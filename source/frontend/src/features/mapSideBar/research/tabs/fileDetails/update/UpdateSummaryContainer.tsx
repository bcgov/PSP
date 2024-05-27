import { Formik, FormikProps } from 'formik';
import { forwardRef } from 'react';

import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';

import { useUpdateResearch } from '../../../hooks/useUpdateResearch';
import { UpdateResearchSummaryFormModel } from './models';
import { UpdateResearchFileYupSchema } from './UpdateResearchFileYupSchema';
import UpdateResearchForm from './UpdateSummaryForm';

export interface IUpdateResearchViewProps {
  researchFile: ApiGen_Concepts_ResearchFile;
  onSuccess: () => void;
}

export const UpdateResearchContainer = forwardRef<FormikProps<any>, IUpdateResearchViewProps>(
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
