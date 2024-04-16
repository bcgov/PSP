import clsx from 'classnames';
import { Formik, FormikProps } from 'formik';
import React, { ChangeEvent, useMemo, useRef, useState } from 'react';
import ReactVisibilitySensor from 'react-visibility-sensor';
import styled from 'styled-components';

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
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import { PROP_MGMT_ACTIVITY_STATUS_TYPES, PROP_MGMT_ACTIVITY_TYPES } from '@/constants/API';
import SaveCancelButtons from '@/features/leases/SaveCancelButtons';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useModalManagement } from '@/hooks/useModalManagement';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { ApiGen_Concepts_PropertyActivitySubtype } from '@/models/api/generated/ApiGen_Concepts_PropertyActivitySubtype';
import { exists } from '@/utils';
import { mapLookupCode } from '@/utils/mapLookupCode';

import { ContactListForm } from './ContactListForm';
import { InvoiceListForm } from './InvoiceListForm';
import { InvoiceTotalsForm } from './InvoiceTotalsForm';
import { PropertyActivityFormModel } from './models';
import { PropertyActivityEditFormYupSchema } from './validation';

export interface IPropertyActivityEditFormProps {
  propertyId: number;
  activity?: ApiGen_Concepts_PropertyActivity;
  subtypes: ApiGen_Concepts_PropertyActivitySubtype[];
  gstConstant: number;
  pstConstant: number;
  onCancel: () => void;
  loading: boolean;
  show: boolean;
  setShow: (show: boolean) => void;
  onSave: (model: ApiGen_Concepts_PropertyActivity) => Promise<void>;
}

export const PropertyActivityEditForm: React.FunctionComponent<
  React.PropsWithChildren<IPropertyActivityEditFormProps>
> = props => {
  const formikRef = useRef<FormikProps<PropertyActivityFormModel>>(null);

  const [showConfirmModal, openConfirmModal, closeConfirmModal] = useModalManagement();

  const [activityType, setActivityType] = useState<string | null>(null);

  const initialForm = useMemo(() => {
    const initialModel = PropertyActivityFormModel.fromApi(props.activity);
    if (props.activity === undefined) {
      initialModel.activityStatusCode = 'NOTSTARTED';
    }
    setActivityType(initialModel.activityTypeCode);
    return initialModel;
  }, [props.activity]);

  const lookupCodes = useLookupCodeHelpers();

  const activityTypeOptions = lookupCodes
    .getByType(PROP_MGMT_ACTIVITY_TYPES)
    .map(c => mapLookupCode(c));

  const activitySubtypeOptions: SelectOption[] = useMemo(() => {
    return props.subtypes
      .filter(ast => ast.parentTypeCode === activityType)
      .map<SelectOption>(ast => {
        return {
          label: ast.description ?? '',
          value: ast.typeCode ?? '',
          code: ast.typeCode ?? undefined,
          parentId: ast.parentTypeCode ?? undefined,
        };
      });
  }, [activityType, props.subtypes]);

  const activityStatusOptions = lookupCodes
    .getByType(PROP_MGMT_ACTIVITY_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  const saveActivity = async (values: PropertyActivityFormModel) => {
    await props.onSave(values.toApi(props.propertyId));
    if (exists(formikRef.current)) {
      formikRef.current.isSubmitting = false;
    }
  };

  const onActivityTypeChange = async (changeEvent: ChangeEvent<HTMLInputElement>) => {
    const typeCode = changeEvent.target.value;
    setActivityType(typeCode ?? null);
  };

  const isEditMode = props.activity !== undefined;

  return (
    <ReactVisibilitySensor
      onChange={(isVisible: boolean) => {
        !isVisible && props.setShow(true);
      }}
    >
      <Styled.PopupTray className={clsx({ show: props.show })}>
        <Styled.TrayHeader>{isEditMode ? 'Edit ' : 'New '}Property Activity</Styled.TrayHeader>
        <Styled.TrayContent>
          <StyledFormWrapper>
            <StyledSummarySection>
              <LoadingBackdrop show={props.loading} />
              {initialForm !== undefined && (
                <Formik<PropertyActivityFormModel>
                  enableReinitialize
                  innerRef={formikRef}
                  validationSchema={PropertyActivityEditFormYupSchema}
                  initialValues={initialForm}
                  onSubmit={saveActivity}
                >
                  {formikProps => (
                    <>
                      <Section header="Activity Details">
                        <SectionField label="Activity type" contentWidth="7" required>
                          <Select
                            field="activityTypeCode"
                            options={activityTypeOptions}
                            placeholder="Select type"
                            onChange={onActivityTypeChange}
                          />
                        </SectionField>
                        <SectionField label="Sub-type" contentWidth="7" required>
                          <Select
                            field="activitySubtypeCode"
                            options={activitySubtypeOptions}
                            placeholder="Select subtype"
                          />
                        </SectionField>
                        <SectionField label="Activity status" contentWidth="7" required>
                          <Select
                            field="activityStatusCode"
                            options={activityStatusOptions}
                            placeholder="Select status"
                          />
                        </SectionField>
                        <SectionField label="Requested added date" contentWidth="7" required>
                          <FastDatePicker field="requestedDate" formikProps={formikProps} />
                        </SectionField>
                        <SectionField
                          label="Completion date"
                          contentWidth="7"
                          required={formikProps.values.activityStatusCode === 'COMPLETED'}
                        >
                          <FastDatePicker field="completionDate" formikProps={formikProps} />
                        </SectionField>
                        <SectionField label="Description" contentWidth="12" required>
                          <TextArea field="description" />
                        </SectionField>

                        <SectionField label="Ministry contacts" contentWidth="8">
                          <ContactListForm
                            field="ministryContacts"
                            formikProps={formikProps}
                            contactType={RestrictContactType.ONLY_INDIVIDUALS}
                          />
                        </SectionField>
                        <SectionField label="Requested source" contentWidth="7">
                          <Input field="requestedSource" />
                        </SectionField>
                        <SectionField label="Involved parties" contentWidth="8">
                          <ContactListForm
                            field="involvedParties"
                            formikProps={formikProps}
                            contactType={RestrictContactType.ALL}
                          />
                        </SectionField>
                        <SectionField label="Service provider" contentWidth="7">
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
                      <InvoiceTotalsForm formikProps={formikProps} />
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

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
`;
