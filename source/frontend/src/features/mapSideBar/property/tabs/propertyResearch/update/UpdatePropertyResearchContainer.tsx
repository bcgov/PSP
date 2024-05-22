import { Formik, FormikProps } from 'formik';
import { forwardRef } from 'react';

import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';

import { useUpdatePropertyResearch } from '../hooks/useUpdatePropertyResearch';
import { UpdatePropertyFormModel } from './models';
import UpdatePropertyForm from './UpdatePropertyForm';
import { UpdatePropertyYupSchema } from './UpdatePropertyYupSchema';

export interface IUpdatePropertyViewProps {
  researchFileProperty: ApiGen_Concepts_ResearchFileProperty;
  onSuccess: () => void;
}

export const UpdatePropertyResearchContainer = forwardRef<
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
