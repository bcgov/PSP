import GenericModal from 'components/common/GenericModal';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import SidebarFooter from 'features/properties/map/shared/SidebarFooter';
import MapSelectorContainer from 'features/properties/selector/MapSelectorContainer';
import { IMapProperty } from 'features/properties/selector/models';
import { FieldArray, Formik, FormikProps } from 'formik';
import { Api_File } from 'models/api/File';
import { useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';

import { FileForm, PropertyForm } from '../../models';
import { UpdatePropertiesYupSchema } from './UpdatePropertiesYupSchema';

export interface IUpdatePropertiesProps {
  file: Api_File;
  setIsShowingPropertySelector: (isShowing: boolean) => void;
  onSuccess: () => void;
  updateFileProperties: (file: Api_File) => Promise<Api_File | undefined>;
}

export const UpdateProperties: React.FunctionComponent<IUpdatePropertiesProps> = props => {
  const formikRef = useRef<FormikProps<FileForm>>(null);
  const formFile = FileForm.fromApi(props.file);

  const [showSaveConfirmModal, setShowSaveConfirmModal] = useState<boolean>(false);
  const [showCancelConfirmModal, setShowCancelConfirmModal] = useState<boolean>(false);

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

  const saveFile = async (researchFile: Api_File) => {
    const response = await props.updateFileProperties(researchFile);
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
        <Formik<FileForm>
          innerRef={formikRef}
          initialValues={formFile}
          validationSchema={UpdatePropertiesYupSchema}
          onSubmit={async (values: FileForm) => {
            const file: Api_File = values.toApi();
            await saveFile(file);
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
            <div>You have made changes to the properties in this file.</div>
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
            <div>If you cancel now, this file will not be saved.</div>
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
