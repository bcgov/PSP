import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { Formik, FormikProps } from 'formik';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';
import { useEffect, useRef, useState } from 'react';
import { MdTopic } from 'react-icons/md';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import ResearchFooter from '../common/ResearchFooter';
import { useGetResearch } from '../hooks/useGetResearch';
import { useUpdateResearch } from '../hooks/useUpdateResearch';
import { UpdateResearchFormModel } from './models';
import UpdateResearchForm from './UpdateResearchForm';
import UpdateResearchHeader from './UpdateResearchHeader';

export interface IUpdateResearchContainerProps {
  researchFileId: number;
  onClose: () => void;
}

export const UpdateResearchContainer: React.FunctionComponent<IUpdateResearchContainerProps> = props => {
  const formikRef = useRef<FormikProps<UpdateResearchFormModel>>(null);
  const { retrieveResearchFile } = useGetResearch();
  const { updateResearchFile } = useUpdateResearch();

  const [researchFile, setResearchFile] = useState<Api_ResearchFile | undefined>(undefined);
  const [initialForm, setForm] = useState<UpdateResearchFormModel | undefined>(undefined);

  useEffect(() => {
    async function fetchResearchFile() {
      var retrieved = await retrieveResearchFile(props.researchFileId);
      setResearchFile(retrieved);
      if (retrieved !== undefined) {
        setForm(UpdateResearchFormModel.fromApi(retrieved));
      }
    }
    fetchResearchFile();
  }, [props.researchFileId, retrieveResearchFile]);

  const saveResearchFile = async (researchFile: Api_ResearchFile) => {
    const response = await updateResearchFile(researchFile);
    if (!!response?.name) {
      props.onClose();
      formikRef.current?.resetForm();
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

  if (initialForm === undefined) {
    return <></>;
  }

  return (
    <MapSideBarLayout
      title="Update Research File"
      icon={<MdTopic title="User Profile" size="2.5rem" className="mr-2" />}
      footer={
        <ResearchFooter
          isSubmitting={formikRef.current?.isSubmitting}
          onSave={handleSave}
          onCancel={handleCancel}
        />
      }
      header={<UpdateResearchHeader researchFile={researchFile} />}
      showCloseButton
      onClose={handleCancel}
    >
      <Formik<UpdateResearchFormModel>
        enableReinitialize
        innerRef={formikRef}
        initialValues={initialForm}
        onSubmit={async (values: UpdateResearchFormModel, formikHelpers) => {
          const researchFile: Api_ResearchFile = values.toApi();
          console.log(researchFile);
          saveResearchFile(researchFile);
          formikHelpers.setSubmitting(false);
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
    </MapSideBarLayout>
  );
};

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
