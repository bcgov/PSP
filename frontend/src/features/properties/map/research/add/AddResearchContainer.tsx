import { SelectedPropertyContext } from 'components/maps/providers/SelectedPropertyContext';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { mapFeatureToProperty } from 'features/properties/selector/components/MapClickMonitor';
import { Formik, FormikProps } from 'formik';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';
import { useMemo } from 'react';
import { useEffect, useRef } from 'react';
import { MdTopic } from 'react-icons/md';
import { Prompt, useHistory } from 'react-router-dom';
import styled from 'styled-components';

import SidebarFooter from '../../shared/SidebarFooter';
import { useAddResearch } from '../hooks/useAddResearch';
import { AddResearchFileYupSchema } from './AddResearchFileYupSchema';
import AddResearchForm from './AddResearchForm';
import { PropertyForm, ResearchForm } from './models';

export interface IAddResearchContainerProps {
  onClose: () => void;
}

export const AddResearchContainer: React.FunctionComponent<IAddResearchContainerProps> = props => {
  const history = useHistory();
  const formikRef = useRef<FormikProps<ResearchForm>>(null);
  const { selectedResearchFeature, setSelectedResearchFeature } = React.useContext(
    SelectedPropertyContext,
  );
  const initialForm = useMemo(() => {
    const researchForm = new ResearchForm();
    if (!!selectedResearchFeature) {
      researchForm.properties = [
        PropertyForm.fromMapProperty(mapFeatureToProperty(selectedResearchFeature)),
      ];
    }
    return researchForm;
  }, [selectedResearchFeature]);
  const { addResearchFile } = useAddResearch();

  useEffect(() => {
    if (!!selectedResearchFeature && !!formikRef.current) {
      formikRef.current.resetForm();
      formikRef.current?.setFieldValue('properties', [
        PropertyForm.fromMapProperty(mapFeatureToProperty(selectedResearchFeature)),
      ]);
    }
    return () => {
      setSelectedResearchFeature(null);
    };
  }, [initialForm, selectedResearchFeature, setSelectedResearchFeature]);

  const saveResearchFile = async (researchFile: Api_ResearchFile) => {
    const response = await addResearchFile(researchFile);
    formikRef.current?.setSubmitting(false);
    if (!!response?.fileName) {
      formikRef.current?.resetForm();
      history.replace(`/mapview/sidebar/research/${response.id}`);
    }
  };

  const handleSave = () => {
    formikRef.current?.setSubmitting(true);
    formikRef.current?.submitForm();
  };

  const handleCancel = () => {
    props.onClose();
  };

  return (
    <MapSideBarLayout
      title="Create Research File"
      icon={<MdTopic title="User Profile" size="2.5rem" className="mr-2" />}
      footer={
        <SidebarFooter
          isOkDisabled={formikRef.current?.isSubmitting}
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
        onSubmit={async (values: ResearchForm) => {
          const researchFile: Api_ResearchFile = values.toApi();
          await saveResearchFile(researchFile);
        }}
        validationSchema={AddResearchFileYupSchema}
      >
        {formikProps => (
          <StyledFormWrapper>
            <AddResearchForm />

            <Prompt
              when={
                formikProps.dirty ||
                (formikProps.values.properties !== initialForm.properties &&
                  formikProps.submitCount === 0)
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
