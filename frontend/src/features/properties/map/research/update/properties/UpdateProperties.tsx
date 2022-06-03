import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import MapSelectorContainer from 'features/properties/selector/MapSelectorContainer';
import { IMapProperty } from 'features/properties/selector/models';
import { FieldArray, Formik, FormikProps } from 'formik';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import { useRef } from 'react';
import { Col, Row } from 'react-bootstrap';

import { PropertyForm, ResearchForm } from '../../add/models';
import ResearchFooter from '../../common/ResearchFooter';
import { useUpdateResearchProperties } from '../../hooks/useUpdateResearchProperties';

interface IUpdatePropertiesProps {
  researchFile: Api_ResearchFile;
  setIsShowingPropertySelector: (isShowing: boolean) => void;
  onSuccesss: () => void;
}

export const UpdateProperties: React.FunctionComponent<IUpdatePropertiesProps> = props => {
  const formikRef = useRef<FormikProps<ResearchForm>>(null);
  const formResearchFile = ResearchForm.fromApi(props.researchFile);

  console.log(props.researchFile);
  console.log(formResearchFile);

  const { updateResearchFileProperties } = useUpdateResearchProperties();

  const handleSave = async () => {
    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
    }
  };

  const handleCancel = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
    props.setIsShowingPropertySelector(false);
  };

  const saveResearchFile = async (researchFile: Api_ResearchFile) => {
    const response = await updateResearchFileProperties(researchFile);
    formikRef.current?.setSubmitting(false);
    if (!!response?.name) {
      formikRef.current?.resetForm();
      props.setIsShowingPropertySelector(false);
      props.onSuccesss();
    }
  };
  return (
    <MapSideBarLayout
      title={'Property selection'}
      icon={undefined}
      footer={
        <ResearchFooter
          isSubmitting={formikRef.current?.isSubmitting}
          onSave={handleSave}
          onCancel={handleCancel}
        />
      }
    >
      <Formik<ResearchForm>
        innerRef={formikRef}
        initialValues={formResearchFile}
        onSubmit={async (values: ResearchForm) => {
          const researchFile: Api_ResearchFile = values.toApi();
          await saveResearchFile(researchFile);
        }}
      >
        {formikProps => (
          <FieldArray name="properties">
            {({ push, remove }) => (
              <Row className="py-3 no-gutters">
                <Col>
                  <MapSelectorContainer
                    onSelectedProperty={(newProperty: IMapProperty) => {
                      const formProperty = PropertyForm.fromMapProperty(newProperty);
                      push(formProperty);
                    }}
                    existingProperties={formikProps.values.properties}
                    onRemoveProperty={remove}
                  />
                </Col>
              </Row>
            )}
          </FieldArray>
        )}
      </Formik>
    </MapSideBarLayout>
  );
};

export default UpdateProperties;
