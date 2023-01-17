import { useMapSearch } from 'components/maps/hooks/useMapSearch';
import { MapStateContext } from 'components/maps/providers/MapStateContext';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { Formik, FormikProps } from 'formik';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';
import { useMemo } from 'react';
import { useEffect, useRef } from 'react';
import { MdTopic } from 'react-icons/md';
import { Prompt, useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';
import { mapFeatureToProperty } from 'utils/mapPropertyUtils';

import { PropertyForm } from '../../shared/models';
import SidebarFooter from '../../shared/SidebarFooter';
import { useAddResearch } from '../hooks/useAddResearch';
import { AddResearchFileYupSchema } from './AddResearchFileYupSchema';
import AddResearchForm from './AddResearchForm';
import { ResearchForm } from './models';

export interface IAddResearchContainerProps {
  onClose: () => void;
}

export const AddResearchContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddResearchContainerProps>
> = props => {
  const history = useHistory();
  const formikRef = useRef<FormikProps<ResearchForm>>(null);
  const { selectedFileFeature } = React.useContext(MapStateContext);
  const initialForm = useMemo(() => {
    const researchForm = new ResearchForm();
    if (!!selectedFileFeature) {
      researchForm.properties = [
        PropertyForm.fromMapProperty(mapFeatureToProperty(selectedFileFeature)),
      ];
    }
    return researchForm;
  }, [selectedFileFeature]);
  const { addResearchFile } = useAddResearch();
  const { search } = useMapSearch();

  useEffect(() => {
    if (!!selectedFileFeature && !!formikRef.current) {
      formikRef.current.resetForm();
      formikRef.current?.setFieldValue('properties', [
        PropertyForm.fromMapProperty(mapFeatureToProperty(selectedFileFeature)),
      ]);
    }
  }, [initialForm, selectedFileFeature]);

  const saveResearchFile = async (researchFile: Api_ResearchFile) => {
    const response = await addResearchFile(researchFile);

    if (!!response?.fileName) {
      if (researchFile.fileProperties?.find(fp => !fp.property?.address && !fp.property?.id)) {
        toast.warn(
          'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
          { autoClose: 15000 },
        );
      }
      await search();
      history.replace(`/mapview/sidebar/research/${response.id}`);
      formikRef.current?.resetForm({ values: ResearchForm.fromApi(response) });
    }
    formikRef.current?.setSubmitting(false);
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
                (formikProps.dirty ||
                  (formikProps.values.properties !== initialForm.properties &&
                    formikProps.submitCount === 0) ||
                  (!formikProps.values.id && formikProps.values.properties.length > 0)) &&
                !formikProps.isSubmitting
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
