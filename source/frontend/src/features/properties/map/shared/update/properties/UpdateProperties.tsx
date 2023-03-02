import axios, { AxiosError } from 'axios';
import GenericModal from 'components/common/GenericModal';
import MapSelectorContainer from 'components/propertySelector/MapSelectorContainer';
import { IMapProperty } from 'components/propertySelector/models';
import SelectedPropertyHeaderRow from 'components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from 'components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { Section } from 'features/mapSideBar/tabs/Section';
import { useBcaAddress } from 'features/properties/map/hooks/useBcaAddress';
import SidebarFooter from 'features/properties/map/shared/SidebarFooter';
import { FieldArray, Formik, FormikProps } from 'formik';
import { Api_File } from 'models/api/File';
import { useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { toast } from 'react-toastify';

import { AddressForm, FileForm, PropertyForm } from '../../models';
import { UpdatePropertiesYupSchema } from './UpdatePropertiesYupSchema';

export interface IUpdatePropertiesProps {
  file: Api_File;
  setIsShowingPropertySelector: (isShowing: boolean) => void;
  onSuccess: () => void;
  updateFileProperties: (file: Api_File) => Promise<Api_File | undefined>;
  canRemove: (propertyId: number) => Promise<boolean>;
}

export const UpdateProperties: React.FunctionComponent<
  React.PropsWithChildren<IUpdatePropertiesProps>
> = props => {
  const formikRef = useRef<FormikProps<FileForm>>(null);
  const formFile = FileForm.fromApi(props.file);

  const [showSaveConfirmModal, setShowSaveConfirmModal] = useState<boolean>(false);
  const [showCancelConfirmModal, setShowCancelConfirmModal] = useState<boolean>(false);
  const [showAssociatedEntityWarning, setShowAssociatedEntityWarning] = useState<boolean>(false);

  const { getPrimaryAddressByPid } = useBcaAddress();

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

  const saveFile = async (file: Api_File) => {
    try {
      const response = await props.updateFileProperties(file);

      formikRef.current?.setSubmitting(false);
      if (!!response?.fileName) {
        if (file.fileProperties?.find(fp => !fp.property?.address && !fp.property?.id)) {
          toast.warn(
            'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
            { autoClose: 15000 },
          );
        }
        formikRef.current?.resetForm();
        props.setIsShowingPropertySelector(false);
        props.onSuccess();
      }
    } catch (e) {
      if (axios.isAxiosError(e) && (e as AxiosError).code === '409') {
        setShowAssociatedEntityWarning(true);
      }
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
                <>
                  <Row className="py-3 no-gutters">
                    <Col>
                      <MapSelectorContainer
                        addSelectedProperties={(newProperties: IMapProperty[]) => {
                          newProperties.reduce(async (promise, property) => {
                            return promise.then(async () => {
                              const formProperty = PropertyForm.fromMapProperty(property);
                              if (property.pid) {
                                const bcaSummary = await getPrimaryAddressByPid(property.pid);
                                formProperty.address = bcaSummary?.address
                                  ? AddressForm.fromBcaAddress(bcaSummary?.address)
                                  : undefined;
                              }
                              push(formProperty);
                            });
                          }, Promise.resolve());
                        }}
                        modifiedProperties={formikProps.values.properties}
                      />
                    </Col>
                  </Row>
                  <Section header="Selected properties">
                    <SelectedPropertyHeaderRow />
                    {formikProps.values.properties.map((property, index) => (
                      <SelectedPropertyRow
                        key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                        onRemove={async () => {
                          if (!property.apiId || (await props.canRemove(property.apiId))) {
                            remove(index);
                          } else {
                            setShowAssociatedEntityWarning(true);
                          }
                        }}
                        nameSpace={`properties.${index}`}
                        index={index}
                        property={property.toMapProperty()}
                      />
                    ))}
                    {formikProps.values.properties.length === 0 && (
                      <span>No Properties selected</span>
                    )}
                  </Section>
                </>
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
        display={showAssociatedEntityWarning}
        title={'Property with associations'}
        message={
          <>
            <div>
              This property can not be removed from the file. This property is related to one or
              more entities in the file, only properties that are not linked to any entities in the
              file can be removed.
            </div>
          </>
        }
        handleOk={() => setShowAssociatedEntityWarning(false)}
        okButtonText="Close"
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
