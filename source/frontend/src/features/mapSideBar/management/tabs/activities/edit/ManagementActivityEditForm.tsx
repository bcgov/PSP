import clsx from 'classnames';
import { Formik, FormikProps } from 'formik';
import React, { ChangeEvent, useMemo, useRef, useState } from 'react';
import { Col } from 'react-bootstrap';
import ReactVisibilitySensor from 'react-visibility-sensor';

import { CancelConfirmationModal } from '@/components/common/CancelConfirmationModal';
import { FastDatePicker, Input, Select, SelectOption } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { TextArea } from '@/components/common/form/TextArea';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import * as Styled from '@/components/common/styles';
import { TrayHeaderContent } from '@/components/common/styles';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import FilePropertiesTable from '@/components/filePropertiesTable/FilePropertiesTable';
import { PROP_MGMT_ACTIVITY_STATUS_TYPES, PROP_MGMT_ACTIVITY_TYPES } from '@/constants/API';
import SaveCancelButtons from '@/features/leases/SaveCancelButtons';
import { ActivityPropertyFormModel } from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/edit/models';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useModalManagement } from '@/hooks/useModalManagement';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { ApiGen_Concepts_PropertyActivitySubtype } from '@/models/api/generated/ApiGen_Concepts_PropertyActivitySubtype';
import { exists, getCurrentIsoDate, isValidId } from '@/utils';
import { mapLookupCode } from '@/utils/mapLookupCode';

import { ContactListForm } from './ContactListForm';
import { InvoiceListForm } from './InvoiceListForm';
import { ManagementActivityEditFormYupSchema } from './ManagementActivityEditFormYupSchema';
import { ManagementActivityFormModel } from './models';

export interface IManagementActivityEditFormProps {
  managementFile: ApiGen_Concepts_ManagementFile;
  activity?: ApiGen_Concepts_PropertyActivity;
  subtypes: ApiGen_Concepts_PropertyActivitySubtype[];
  gstConstant: number;
  pstConstant: number;
  onCancel: () => void;
  onClose: () => void;
  loading: boolean;
  show: boolean;
  setShow: (show: boolean) => void;
  onSave: (model: ApiGen_Concepts_PropertyActivity) => Promise<void>;
}

export const ManagementActivityEditForm: React.FunctionComponent<
  React.PropsWithChildren<IManagementActivityEditFormProps>
> = ({
  managementFile,
  activity,
  subtypes,
  gstConstant,
  pstConstant,
  loading,
  show,
  setShow,
  onCancel,
  onClose,
  onSave,
}) => {
  const formikRef = useRef<FormikProps<ManagementActivityFormModel>>(null);
  const [activityType, setActivityType] = useState<string | null>(null);
  const [showConfirmModal, openConfirmModal, closeConfirmModal] = useModalManagement();
  const lookupCodes = useLookupCodeHelpers();

  const initialForm = useMemo(() => {
    let activityForm: ManagementActivityFormModel;

    // Update activity flow
    if (exists(activity)) {
      activityForm = ManagementActivityFormModel.fromApi(activity);
    } else {
      // Create activity flow
      activityForm = new ManagementActivityFormModel(null, managementFile.id);
      activityForm.activityStatusCode = 'NOTSTARTED';
      activityForm.requestedDate = getCurrentIsoDate();
      // By default, all properties are selected but user can unselect all or some
      activityForm.activityProperties = (managementFile.fileProperties ?? [])
        .filter(fp => fp.isActive !== false)
        .map(fp => {
          const newActivityProperty = new ActivityPropertyFormModel();
          newActivityProperty.propertyId = fp.propertyId;
          return newActivityProperty;
        });
    }

    setActivityType(activityForm.activityTypeCode);
    return activityForm;
  }, [activity, managementFile.fileProperties, managementFile.id]);

  const activityTypeOptions = lookupCodes
    .getByType(PROP_MGMT_ACTIVITY_TYPES)
    .map(c => mapLookupCode(c));

  const activitySubtypeOptions: SelectOption[] = useMemo(() => {
    return subtypes
      .filter(ast => ast.parentTypeCode === activityType)
      .map<SelectOption>(ast => {
        return {
          label: ast.description ?? '',
          value: ast.typeCode ?? '',
          code: ast.typeCode ?? undefined,
          parentId: ast.parentTypeCode ?? undefined,
        };
      });
  }, [activityType, subtypes]);

  const activityStatusOptions = lookupCodes
    .getByType(PROP_MGMT_ACTIVITY_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  const saveActivity = async (values: ManagementActivityFormModel) => {
    await onSave(values.toApi());
    if (exists(formikRef.current)) {
      formikRef.current.setSubmitting(false);
    }
  };

  const onActivityTypeChange = async (changeEvent: ChangeEvent<HTMLInputElement>) => {
    const typeCode = changeEvent.target.value;
    setActivityType(typeCode ?? null);
    if (exists(formikRef.current)) {
      formikRef.current.setFieldValue('activitySubtypeCode', '');
    }
  };

  const isEditMode = exists(activity);

  const onCloseClick = () => {
    setShow(false);
    onClose();
  };

  return (
    <ReactVisibilitySensor
      onChange={(isVisible: boolean) => {
        !isVisible && setShow(true);
      }}
    >
      <Styled.PopupTray className={clsx({ show })}>
        <TrayHeaderContent>
          <Styled.TrayHeader>{isEditMode ? 'Edit ' : 'New '}Property Activity</Styled.TrayHeader>
          <Col xs="auto" className="text-right">
            <Styled.CloseIcon
              id="close-tray"
              title="close"
              onClick={onCloseClick}
            ></Styled.CloseIcon>
          </Col>
        </TrayHeaderContent>
        <Styled.TrayContent>
          <StyledFormWrapper>
            <StyledSummarySection>
              <LoadingBackdrop show={loading} />
              {exists(initialForm) && (
                <Formik<ManagementActivityFormModel>
                  enableReinitialize
                  innerRef={formikRef}
                  validationSchema={ManagementActivityEditFormYupSchema}
                  initialValues={initialForm}
                  onSubmit={saveActivity}
                >
                  {formikProps => (
                    <>
                      <Section header="Select File Properties">
                        <SectionField label="Selected Properties" labelWidth={{ xs: 5 }}>
                          <FilePropertiesTable
                            disabledSelection={false}
                            fileProperties={managementFile.fileProperties ?? []}
                            selectedFileProperties={
                              formikProps.values.activityProperties
                                .map(ap =>
                                  managementFile.fileProperties?.find(
                                    fp => fp.propertyId === ap.propertyId,
                                  ),
                                )
                                .filter(exists) ?? []
                            }
                            setSelectedFileProperties={(
                              fileProperties: ApiGen_Concepts_FileProperty[],
                            ) => {
                              const activityProperties: ActivityPropertyFormModel[] =
                                fileProperties.map(fileProperty => {
                                  const matchingProperty =
                                    formikProps.values.activityProperties.find(
                                      ap => ap.propertyId === fileProperty.propertyId,
                                    );

                                  if (exists(matchingProperty)) {
                                    return matchingProperty;
                                  } else {
                                    const newActivityProperty = new ActivityPropertyFormModel();
                                    newActivityProperty.propertyId = fileProperty.propertyId;
                                    newActivityProperty.propertyActivityId = isValidId(activity?.id)
                                      ? activity?.id
                                      : 0;

                                    return newActivityProperty;
                                  }
                                });

                              formikProps.setFieldValue('activityProperties', activityProperties);
                            }}
                          ></FilePropertiesTable>
                        </SectionField>
                      </Section>
                      <Section header="Activity Details">
                        <SectionField label="Activity type" contentWidth={{ xs: 7 }} required>
                          <Select
                            field="activityTypeCode"
                            options={activityTypeOptions}
                            placeholder="Select type"
                            onChange={onActivityTypeChange}
                          />
                        </SectionField>
                        <SectionField label="Sub-type" contentWidth={{ xs: 7 }} required>
                          <Select
                            field="activitySubtypeCode"
                            options={activitySubtypeOptions}
                            placeholder="Select subtype"
                          />
                        </SectionField>
                        <SectionField label="Activity status" contentWidth={{ xs: 7 }} required>
                          <Select
                            field="activityStatusCode"
                            options={activityStatusOptions}
                            placeholder="Select status"
                          />
                        </SectionField>
                        <SectionField label="Commencement" contentWidth={{ xs: 7 }} required>
                          <FastDatePicker field="requestedDate" formikProps={formikProps} />
                        </SectionField>
                        <SectionField
                          label="Completion"
                          contentWidth={{ xs: 7 }}
                          required={formikProps.values.activityStatusCode === 'COMPLETED'}
                        >
                          <FastDatePicker field="completionDate" formikProps={formikProps} />
                        </SectionField>
                        <SectionField label="Description" contentWidth={{ xs: 12 }} required>
                          <TextArea field="description" />
                        </SectionField>

                        <SectionField label="Ministry contacts" contentWidth={{ xs: 8 }}>
                          <ContactListForm
                            field="ministryContacts"
                            formikProps={formikProps}
                            contactType={RestrictContactType.ONLY_INDIVIDUALS}
                          />
                        </SectionField>
                        <SectionField
                          label="Contact manager"
                          contentWidth={{ xs: 7 }}
                          tooltip="Document the source of the request by entering the name of the person, organization or other entity from which the request has been received"
                        >
                          <Input field="requestedSource" />
                        </SectionField>
                        <SectionField label="External contacts" contentWidth={{ xs: 8 }}>
                          <ContactListForm
                            field="involvedParties"
                            formikProps={formikProps}
                            contactType={RestrictContactType.ALL}
                          />
                        </SectionField>
                        <SectionField label="Service provider" contentWidth={{ xs: 7 }}>
                          <ContactInputContainer
                            field="serviceProvider"
                            View={ContactInputView}
                            restrictContactType={RestrictContactType.ALL}
                          />
                        </SectionField>
                      </Section>
                      <InvoiceListForm
                        field="invoices"
                        formikProps={formikProps}
                        gstConstant={gstConstant}
                        pstConstant={pstConstant}
                      />
                      <SaveCancelButtons
                        onCancel={() => {
                          if (formikProps.dirty === true) {
                            openConfirmModal();
                          } else {
                            onCancel();
                          }
                        }}
                        formikProps={formikProps}
                      />
                      <CancelConfirmationModal
                        display={showConfirmModal}
                        handleOk={onCancel}
                        handleCancel={closeConfirmModal}
                      />
                    </>
                  )}
                </Formik>
              )}
            </StyledSummarySection>
          </StyledFormWrapper>
        </Styled.TrayContent>
      </Styled.PopupTray>
    </ReactVisibilitySensor>
  );
};
