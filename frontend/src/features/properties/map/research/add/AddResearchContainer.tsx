import { SelectedPropertyContext } from 'components/maps/providers/SelectedPropertyContext';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { mapFeatureToProperty } from 'features/properties/selector/components/MapClickMonitor';
import { Formik, FormikProps } from 'formik';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';
import { useMemo } from 'react';
import { useEffect, useRef } from 'react';
import { MdTopic } from 'react-icons/md';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import ResearchFooter from '../common/ResearchFooter';
import { useAddResearch } from '../hooks/useAddResearch';
import { AddResearchFileYupSchema } from './AddResearchFileYupSchema';
import AddResearchForm from './AddResearchForm';
import { PropertyForm, ResearchForm } from './models';

export interface IAddResearchContainerProps {
  onClose: () => void;
}

export const AddResearchContainer: React.FunctionComponent<IAddResearchContainerProps> = props => {
  const formikRef = useRef<FormikProps<ResearchForm>>(null);
  const { selectedResearchFeature, setSelectedResearchFeature } = React.useContext(
    SelectedPropertyContext,
  );
  const initialForm = useMemo(() => {
    const researchForm = new ResearchForm();
    if (!!selectedResearchFeature) {
      researchForm.properties = [new PropertyForm(mapFeatureToProperty(selectedResearchFeature))];
    }
    return researchForm;
  }, [selectedResearchFeature]);
  const { addResearchFile } = useAddResearch();

  useEffect(() => {
    if (!!selectedResearchFeature && !!formikRef.current) {
      formikRef.current.resetForm();
      formikRef.current?.setFieldValue('properties', [
        new PropertyForm(mapFeatureToProperty(selectedResearchFeature)),
      ]);
    }
    return () => {
      setSelectedResearchFeature(null);
    };
  }, [initialForm, selectedResearchFeature, setSelectedResearchFeature]);

  const saveResearchFile = async (researchFile: Api_ResearchFile, onSuccess: () => void) => {
    const response = await addResearchFile(researchFile);
    if (!!response?.name) {
      onSuccess();
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
          await saveResearchFile(researchFile, () => {
            formikHelpers.setSubmitting(false);
            formikHelpers.resetForm();
          });
        }}
        validationSchema={AddResearchFileYupSchema}
      >
        {formikProps => (
          <StyledFormWrapper>
            <AddResearchForm />

            <Prompt
              when={
                (formikProps.dirty || formikProps.values.properties.length > 0) &&
                formikProps.submitCount === 0
              }
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
