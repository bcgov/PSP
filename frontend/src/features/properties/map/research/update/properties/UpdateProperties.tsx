import GenericModal from 'components/common/GenericModal';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import MapSelectorContainer from 'features/properties/selector/MapSelectorContainer';
import { IMapProperty } from 'features/properties/selector/models';
import { FieldArray, Formik, FormikProps } from 'formik';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import { useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';

import { PropertyForm, ResearchForm } from '../../add/models';
import ResearchFooter from '../../common/ResearchFooter';
import { useUpdateResearchProperties } from '../../hooks/useUpdateResearchProperties';

interface IUpdatePropertiesProps {
  researchFile: Api_ResearchFile;
  setIsShowingPropertySelector: (isShowing: boolean) => void;
  onSuccess: () => void;
}

export const UpdateProperties: React.FunctionComponent<IUpdatePropertiesProps> = props => {
  const formikRef = useRef<FormikProps<ResearchForm>>(null);
  const formResearchFile = ResearchForm.fromApi(props.researchFile);

  const [showConfirmModal, setShowConfirmModal] = useState<boolean>(false);

  const { updateResearchFileProperties } = useUpdateResearchProperties();

  const handleSave = async () => {
    setShowConfirmModal(true);
  };

  const handleSaveConfirm = async () => {
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
      props.onSuccess();
    }
  };
  return (
    <>
      <MapSideBarLayout
        title={'Property selection'}
        icon={undefined}
        footer={
          <ResearchFooter
            isOkDisabled={formikRef.current?.isSubmitting}
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
      <GenericModal
        display={showConfirmModal}
        title={'Confirm changes'}
        message={
          <>
            <div>You have made changes to the properties in this Research File.</div>
            <br />
            <strong>Do you want to save these changes?</strong>
          </>
        }
        handleOk={handleSaveConfirm}
        handleCancel={() => setShowConfirmModal(false)}
        okButtonText="Save"
        cancelButtonText="Cancel"
        show
      />
    </>
  );
};

export default UpdateProperties;
