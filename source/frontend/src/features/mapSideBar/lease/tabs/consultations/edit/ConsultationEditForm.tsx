import { Formik, FormikHelpers } from 'formik';
import styled from 'styled-components';

import { FastDatePicker, Input, Select, TextArea } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { PrimaryContactSelector } from '@/components/common/form/PrimaryContactSelector/PrimaryContactSelector';
import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledDivider } from '@/components/common/styles';
import * as API from '@/constants/API';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { isOrganizationResult } from '@/interfaces';
import { exists, isValidId } from '@/utils';

import { UpdateConsultationYupSchema } from './EditConsultationYupSchema';
import { ConsultationFormModel } from './models';

export interface IConsultationEditFormProps {
  isLoading: boolean;
  initialValues: ConsultationFormModel | null;
  onSubmit: (
    values: ConsultationFormModel,
    formikHelpers: FormikHelpers<ConsultationFormModel>,
  ) => Promise<void>;
  onCancel: () => void;
}

export const ConsultationEditForm: React.FunctionComponent<IConsultationEditFormProps> = ({
  isLoading,
  initialValues,
  onSubmit,
  onCancel,
}) => {
  const { setModalContent, setDisplayModal } = useModalContext();

  const { getOptionsByType } = useLookupCodeHelpers();
  const consultationTypeCodes = getOptionsByType(API.CONSULTATION_TYPES);

  const cancelFunc = (resetForm: () => void, dirty: boolean) => {
    if (!dirty) {
      resetForm();
      onCancel();
    } else {
      setModalContent({
        ...getCancelModalProps(),
        handleOk: () => {
          resetForm();
          setDisplayModal(false);
          onCancel();
        },
      });
      setDisplayModal(true);
    }
  };

  const headerTitle = !isValidId(initialValues.id) ? 'Add Consultation' : 'Update Consultation';

  return (
    initialValues && (
      <StyledFormWrapper>
        <Formik<ConsultationFormModel>
          enableReinitialize
          initialValues={initialValues}
          validationSchema={UpdateConsultationYupSchema}
          onSubmit={onSubmit}
        >
          {formikProps => {
            return (
              <>
                <LoadingBackdrop show={formikProps.isSubmitting || isLoading} parentScreen={true} />
                <StyledContent>
                  <Section header={headerTitle}>
                    <SectionField
                      required
                      labelWidth="4"
                      contentWidth="6"
                      label="Consultation type"
                    >
                      <Select
                        placeholder="Select"
                        options={consultationTypeCodes}
                        field="consultationTypeCode"
                      />
                    </SectionField>
                    {formikProps.values.consultationTypeCode === 'OTHER' && (
                      <SectionField required labelWidth="4" contentWidth="6" label="Description">
                        <Input field="otherDescription" />
                      </SectionField>
                    )}
                    <SectionField labelWidth="4" contentWidth="6" label="Requested on">
                      <FastDatePicker field="requestedOn" formikProps={formikProps} />
                    </SectionField>
                    <SectionField labelWidth="4" contentWidth="auto" label="Response received">
                      <YesNoSelect field="isResponseReceived" />
                    </SectionField>
                    {formikProps.values.isResponseReceived === true && (
                      <SectionField labelWidth="4" contentWidth="auto" label="Response received on">
                        <FastDatePicker field="responseReceivedDate" formikProps={formikProps} />
                      </SectionField>
                    )}
                    <SectionField labelWidth="4" contentWidth="12" label="Comments">
                      <TextArea field="comment" />
                    </SectionField>
                    <SectionField labelWidth="4" contentWidth="6" label="Contact">
                      <ContactInputContainer
                        field="contact"
                        View={ContactInputView}
                        displayErrorAsTooltip={false}
                      />
                    </SectionField>
                    {exists(formikProps.values.contact) &&
                      isOrganizationResult(formikProps.values.contact) && (
                        <SectionField label="Primary contact" labelWidth="4" contentWidth="6">
                          <PrimaryContactSelector
                            field={`primaryContactId`}
                            contactInfo={formikProps.values.contact}
                          />
                        </SectionField>
                      )}
                    <StyledDivider />
                    <StyledFooter>
                      <SidebarFooter
                        onSave={() => formikProps.submitForm()}
                        isOkDisabled={formikProps.isSubmitting || !formikProps.dirty}
                        onCancel={() => cancelFunc(formikProps.resetForm, formikProps.dirty)}
                        displayRequiredFieldError={
                          formikProps.isValid === false && !!formikProps.submitCount
                        }
                      />
                    </StyledFooter>
                  </Section>
                </StyledContent>
              </>
            );
          }}
        </Formik>
      </StyledFormWrapper>
    )
  );
};

export default ConsultationEditForm;

const StyledContent = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
`;

const StyledFooter = styled.div`
  margin-right: 1rem;
  padding-bottom: 1rem;
  z-index: 0;
`;
