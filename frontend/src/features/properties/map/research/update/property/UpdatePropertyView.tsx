import { Formik, FormikProps } from 'formik';
import { Api_ResearchFile, Api_ResearchFileProperty } from 'models/api/ResearchFile';
import * as React from 'react';
import { useRef } from 'react';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import { useUpdatePropertyResearch } from '../../hooks/useUpdatePropertyResearch';
import { UpdatePropertyFormModel } from './models';
import UpdatePropertyForm from './UpdatePropertyForm';

export interface IUpdatePropertyViewProps {
  researchFileProperty: Api_ResearchFileProperty;
  setFormikRef: (ref: React.RefObject<FormikProps<any>> | undefined) => void;
  onSuccess: () => void;
}

export const UpdatePropertyView: React.FunctionComponent<IUpdatePropertyViewProps> = props => {
  const formikRef = useRef<FormikProps<UpdatePropertyFormModel>>(null);

  props.setFormikRef(formikRef);

  const { updatePropertyResearchFile } = useUpdatePropertyResearch();

  const savePropertyFile = async (researchFile: Api_ResearchFileProperty) => {
    const response = await updatePropertyResearchFile(researchFile);
    if (!!response?.name) {
      formikRef.current?.resetForm();
      formikRef.current?.setSubmitting(false);
      props.onSuccess();
    }
  };

  return (
    <Formik<UpdatePropertyFormModel>
      enableReinitialize
      innerRef={formikRef}
      initialValues={UpdatePropertyFormModel.fromApi(props.researchFileProperty)}
      onSubmit={async (values: UpdatePropertyFormModel) => {
        const researchFile: Api_ResearchFile = values.toApi();
        await savePropertyFile(researchFile);
      }}
    >
      {formikProps => (
        <StyledFormWrapper>
          <UpdatePropertyForm formikProps={formikProps} />

          <Prompt
            when={formikProps.dirty && formikProps.submitCount === 0}
            message="You have made changes on this form. Do you wish to leave without saving?"
          />
        </StyledFormWrapper>
      )}
    </Formik>
  );
};

export default UpdatePropertyView;

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
