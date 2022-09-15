import GenericModal from 'components/common/GenericModal';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import SidebarFooter from 'features/properties/map/shared/SidebarFooter';
import MapSelectorContainer from 'features/properties/selector/MapSelectorContainer';
import { IMapProperty } from 'features/properties/selector/models';
import { FieldArray, Formik, FormikProps } from 'formik';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import { useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';

import { PropertyForm, ResearchForm } from '../../add/models';
import { useUpdateResearchProperties } from '../../hooks/useUpdateResearchProperties';
import { UpdatePropertiesYupSchema } from './UpdatePropertiesYupSchema';

interface IUpdatePropertiesProps {
  researchFile: Api_ResearchFile;
  setIsShowingPropertySelector: (isShowing: boolean) => void;
  onSuccess: () => void;
}

export const UpdateProperties: React.FunctionComponent<IUpdatePropertiesProps> = props => {
  const formikRef = useRef<FormikProps<ResearchForm>>(null);
  const formResearchFile = ResearchForm.fromApi(props.researchFile);

  const [showSaveConfirmModal, setShowSaveConfirmModal] = useState<boolean>(false);
  const [showCancelConfirmModal, setShowCancelConfirmModal] = useState<boolean>(false);

  const { updateResearchFileProperties } = useUpdateResearchProperties();

  const handleSaveClick = async () => {
    setShowSaveConfirmModal(true);
  };

  const handleCancelClick = () => {
    if (formikRef !== undefined) {
      if (formikRef.current?.dirty) {
        setShowCancelConfirmModal(true);
      } else {
        handleCancelConfirm();
      }
    } else {
      handleCancelConfirm();
    }
  };

  const handleSaveConfirm = async () => {
    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
    }
  };

  const handleCancelConfirm = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
    setShowCancelConfirmModal(false);
    props.setIsShowingPropertySelector(false);
  };

  const saveResearchFile = async (researchFile: Api_ResearchFile) => {
    const response = await updateResearchFileProperties(researchFile);
    formikRef.current?.setSubmitting(false);
    if (!!response?.fileName) {
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
          <SidebarFooter
            isOkDisabled={formikRef.current?.isSubmitting}
            onSave={handleSaveClick}
            onCancel={handleCancelClick}
          />
        }
      >
        <Formik<ResearchForm>
          innerRef={formikRef}
          initialValues={formResearchFile}
          validationSchema={UpdatePropertiesYupSchema}
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
        display={showSaveConfirmModal}
        title={'Confirm changes'}
        message={
          <>
            <div>You have made changes to the properties in this Research File.</div>
            <br />
            <strong>Do you want to save these changes?</strong>
          </>
        }
        handleOk={handleSaveConfirm}
        handleCancel={() => setShowSaveConfirmModal(false)}
        okButtonText="Save"
        cancelButtonText="Cancel"
        show
      />

      <GenericModal
        display={showCancelConfirmModal}
        title={'Confirm changes'}
        message={
          <>
            <div>If you cancel now, this research file will not be saved.</div>
            <br />
            <strong>Are you sure you want to Cancel?</strong>
          </>
        }
        handleOk={handleCancelConfirm}
        handleCancel={() => setShowCancelConfirmModal(false)}
        okButtonText="Ok"
        cancelButtonText="Resume editing"
        show
      />
    </>
  );
};

export default UpdateProperties;
