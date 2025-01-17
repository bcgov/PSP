import { FormikProps } from 'formik';
import { forwardRef } from 'react';

import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';

import { useUpdatePropertyResearch } from '../hooks/useUpdatePropertyResearch';
import { UpdatePropertyFormModel } from './models';
import { IUpdatePropertyResearchFormProps } from './UpdatePropertyForm';
import { UpdatePropertyYupSchema } from './UpdatePropertyYupSchema';

export interface IUpdatePropertyResearchContainerProps {
  researchFileProperty: ApiGen_Concepts_ResearchFileProperty;
  onSuccess: () => void;
  View: React.FC<IUpdatePropertyResearchFormProps>;
}

export const UpdatePropertyResearchContainer = forwardRef<
  FormikProps<any>,
  IUpdatePropertyResearchContainerProps
>(({ researchFileProperty, onSuccess, View }, formikRef) => {
  const { updatePropertyResearchFile } = useUpdatePropertyResearch();

  const savePropertyFile = async (researchFile: ApiGen_Concepts_ResearchFileProperty) => {
    const response = await updatePropertyResearchFile(researchFile);
    if (typeof formikRef === 'function' || formikRef === null) {
      throw Error('unexpected ref prop');
    }
    formikRef.current?.setSubmitting(false);
    if (response?.fileName) {
      formikRef.current?.resetForm();
      onSuccess();
    }
  };

  return (
    <StyledFormWrapper>
      <View
        formikRef={formikRef as React.RefObject<FormikProps<UpdatePropertyFormModel>>}
        initialValues={UpdatePropertyFormModel.fromApi(researchFileProperty)}
        validationSchema={UpdatePropertyYupSchema}
        onSubmit={async (values: UpdatePropertyFormModel) => {
          const researchFile: ApiGen_Concepts_ResearchFileProperty = values.toApi();
          await savePropertyFile(researchFile);
        }}
      />
    </StyledFormWrapper>
  );
});

export default UpdatePropertyResearchContainer;
