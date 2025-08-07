import clsx from 'classnames';
import { Formik, FormikProps } from 'formik';
import React, { ChangeEvent, useCallback, useEffect, useRef, useState } from 'react';
import { Col } from 'react-bootstrap';
import ReactVisibilitySensor from 'react-visibility-sensor';

import { CancelConfirmationModal } from '@/components/common/CancelConfirmationModal';
import { FastDatePicker, Input, Multiselect, Select } from '@/components/common/form';
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
import {
  MGMT_ACTIVITY_STATUS_TYPES,
  MGMT_ACTIVITY_SUBTYPES_TYPES,
  MGMT_ACTIVITY_TYPES,
} from '@/constants/API';
import SaveCancelButtons from '@/features/leases/SaveCancelButtons';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useModalManagement } from '@/hooks/useModalManagement';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { exists, isValidId } from '@/utils';
import { mapLookupCode } from '@/utils/mapLookupCode';

import { ManagementActivitySubTypeModel } from '../models/ManagementActivitySubType';
import { ContactListForm } from './ContactListForm';
import { InvoiceListForm } from './InvoiceListForm';
import { PropertyActivityFormModel } from './models';
import { PropertyActivityEditFormYupSchema } from './validation';

export interface IPropertyActivityEditFormProps {
  propertyId: number;
  initialValues: PropertyActivityFormModel;
  gstConstant: number;
  pstConstant: number;
  loading: boolean;
  show: boolean;
  onCancel: () => void;
  onClose: () => void;
  setShow: (show: boolean) => void;
  onSave: (model: ApiGen_Concepts_ManagementActivity) => Promise<void>;
}

export const PropertyActivityEditForm: React.FunctionComponent<
  React.PropsWithChildren<IPropertyActivityEditFormProps>
> = props => {
  const formikRef = useRef<FormikProps<PropertyActivityFormModel>>(null);
  const [showConfirmModal, openConfirmModal, closeConfirmModal] = useModalManagement();

  const [activitySubTypeOptions, setActivitySubTypeOptions] =
    useState<ManagementActivitySubTypeModel[]>(null);

  const lookupCodes = useLookupCodeHelpers();
  const activityTypeOptions = lookupCodes.getByType(MGMT_ACTIVITY_TYPES).map(c => mapLookupCode(c));
  const activitySubTypeCodes: ILookupCode[] = lookupCodes.getByType(MGMT_ACTIVITY_SUBTYPES_TYPES);
  const activityStatusOptions = lookupCodes
    .getByType(MGMT_ACTIVITY_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  const onActivityTypeChange = async (changeEvent: ChangeEvent<HTMLInputElement>) => {
    const typeCode = changeEvent.target.value;
    if (
      typeCode &&
      formikRef.current?.values.activityTypeCode &&
      typeCode !== formikRef.current?.values.activityTypeCode
    ) {
      if (exists(formikRef.current)) {
        formikRef.current?.setFieldValue('activitySubtypeCodes', []);
      }
    }

    setManagementActivitySubTypeOptions(typeCode);
  };
  const setManagementActivitySubTypeOptions = useCallback(
    async (activityTypeCode: string) => {
      const subTypeOptions: ManagementActivitySubTypeModel[] = activitySubTypeCodes
        .filter(x => x.parentId === activityTypeCode)
        .map(x => ManagementActivitySubTypeModel.fromLookup(null, x));

      setActivitySubTypeOptions(subTypeOptions);
    },
    [activitySubTypeCodes],
  );

  useEffect(() => {
    if (activitySubTypeOptions === null) {
      setManagementActivitySubTypeOptions(props.initialValues.activityTypeCode);
    }
  }, [
    activitySubTypeOptions,
    props.initialValues.activityTypeCode,
    setManagementActivitySubTypeOptions,
  ]);

  return (
    <ReactVisibilitySensor
      onChange={(isVisible: boolean) => {
        !isVisible && props.setShow(true);
      }}
    >
      <Styled.PopupTray className={clsx({ show: props.show })}>
        <TrayHeaderContent>
          <Styled.TrayHeader>
            {isValidId(props.initialValues?.id) ? 'Edit ' : 'New '}Property Activity
          </Styled.TrayHeader>
          <Col xs="auto" className="text-right">
            <Styled.CloseIcon
              id="close-tray"
              title="close"
              onClick={() => {
                props.setShow(false);
                props.onClose();
              }}
            ></Styled.CloseIcon>
          </Col>
        </TrayHeaderContent>
        <Styled.TrayContent>
          <StyledFormWrapper>
            <StyledSummarySection>
              <LoadingBackdrop show={props.loading} />
              {exists(props.initialValues) && (
                <Formik<PropertyActivityFormModel>
                  enableReinitialize
                  innerRef={formikRef}
                  validationSchema={PropertyActivityEditFormYupSchema}
                  initialValues={props.initialValues}
                  onSubmit={async (values, formikHelpers) => {
                    await props.onSave(values.toApi(props.propertyId));
                    if (exists(formikRef.current)) {
                      formikHelpers.setSubmitting(false);
                    }
                  }}
                >
                  {formikProps => (
                    <>
                      <Section header="Activity Details">
                        <SectionField label="Activity type" contentWidth={{ xs: 7 }} required>
                          <Select
                            field="activityTypeCode"
                            options={activityTypeOptions}
                            placeholder="Select type"
                            onChange={onActivityTypeChange}
                          />
                        </SectionField>
                        <SectionField label="Sub-type(s)" contentWidth={{ xs: 7 }} required>
                          <Multiselect
                            field="activitySubtypeCodes"
                            displayValue="subTypeCodeDescription"
                            options={activitySubTypeOptions ?? []}
                            placeholder=""
                          />
                        </SectionField>
                        <SectionField label="Activity status" contentWidth={{ xs: 7 }} required>
                          <Select
                            field="activityStatusCode"
                            options={activityStatusOptions}
                            placeholder=""
                          />
                        </SectionField>
                        <SectionField label="Commencement" contentWidth={{ xs: 7 }} required>
                          <FastDatePicker field="requestedDate" formikProps={formikProps} />
                        </SectionField>
                        <SectionField
                          label="Completion date"
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
                        gstConstant={props.gstConstant}
                        pstConstant={props.pstConstant}
                      />
                      <SaveCancelButtons
                        onCancel={() => {
                          if (formikProps.dirty === true) {
                            openConfirmModal();
                          } else {
                            props.onCancel();
                          }
                        }}
                        formikProps={formikProps}
                      />
                      <CancelConfirmationModal
                        display={showConfirmModal}
                        handleOk={props.onCancel}
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
