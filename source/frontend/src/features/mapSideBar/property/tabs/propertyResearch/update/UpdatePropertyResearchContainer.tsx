import { Formik, FormikProps } from 'formik';
import * as React from 'react';
import styled from 'styled-components';

import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';

import { useUpdatePropertyResearch } from '../hooks/useUpdatePropertyResearch';
import { UpdatePropertyFormModel } from './models';
import UpdatePropertyForm from './UpdatePropertyForm';
import { UpdatePropertyYupSchema } from './UpdatePropertyYupSchema';

export interface IUpdatePropertyViewProps {
  researchFileProperty: ApiGen_Concepts_ResearchFileProperty;
  onSuccess: () => void;
}

export const UpdatePropertyResearchContainer = React.forwardRef<
  FormikProps<any>,
  IUpdatePropertyViewProps
>((props, formikRef) => {
  const { updatePropertyResearchFile } = useUpdatePropertyResearch();

  const savePropertyFile = async (researchFile: ApiGen_Concepts_ResearchFileProperty) => {
    const response = await updatePropertyResearchFile(researchFile);
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
    <Formik<UpdatePropertyFormModel>
      enableReinitialize
      innerRef={formikRef}
      initialValues={UpdatePropertyFormModel.fromApi(props.researchFileProperty)}
      validationSchema={UpdatePropertyYupSchema}
      onSubmit={async (values: UpdatePropertyFormModel) => {
        const researchFile: ApiGen_Concepts_ResearchFileProperty = values.toApi();
        await savePropertyFile(researchFile);
      }}
    >
      {formikProps => (
        <StyledFormWrapper>
          <UpdatePropertyForm formikProps={formikProps} />
        </StyledFormWrapper>
      )}
    </Formik>
  );
});

export default UpdatePropertyResearchContainer;

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
