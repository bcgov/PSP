import { FastDatePicker, Select, SelectOption, TextArea } from 'components/common/form';
import { UnsavedChangesPrompt } from 'components/common/form/UnsavedChangesPrompt';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikProps } from 'formik';
import { getCancelModalProps, useModalContext } from 'hooks/useModalContext';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_Compensation } from 'models/api/Compensation';
import moment from 'moment';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import SidebarFooter from '../../shared/SidebarFooter';
import { CompensationRequisitionYupSchema } from '../CompensationRequisitionYupSchema';
import { CompensationRequisitionFormModel, PayeeOption } from '../models';

export interface CompensationRequisitionFormProps {
  isLoading: boolean;
  formikRef: React.Ref<FormikProps<CompensationRequisitionFormModel>>;
  acquisitionFile: Api_AcquisitionFile;
  payeeOptions: PayeeOption[];
  initialValues: CompensationRequisitionFormModel;
  onSave: (compensation: CompensationRequisitionFormModel) => Promise<Api_Compensation | undefined>;
  onCancel: () => void;
}

const UpdateCompensationRequisitionForm: React.FC<CompensationRequisitionFormProps> = ({
  isLoading,
  formikRef,
  acquisitionFile,
  payeeOptions,
  initialValues,
  onSave,
  onCancel,
}) => {
  const { setModalContent, setDisplayModal } = useModalContext();
  let fiscalYearOptions = generateFiscalYearOptions();

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

  return (
    <StyledFormWrapper>
      <Formik<CompensationRequisitionFormModel>
        enableReinitialize
        innerRef={formikRef}
        initialValues={initialValues}
        onSubmit={async values => {
          await onSave(values);
        }}
        validationSchema={CompensationRequisitionYupSchema}
      >
        {formikProps => (
          <>
            <LoadingBackdrop show={isLoading}></LoadingBackdrop>
            <UnsavedChangesPrompt />
            <StyledContent>
              <Section header="Requisition details">
                <SectionField label="Status" labelWidth="7" contentWidth="4">
                  <Select
                    field="status"
                    options={[
                      {
                        label: 'Draft',
                        value: 'draft',
                      },
                      {
                        label: 'Final',
                        value: 'final',
                      },
                    ]}
                    placeholder="Select..."
                  />
                </SectionField>
                <SectionField label="Agreement date" labelWidth="7" contentWidth="4">
                  <FastDatePicker field="agreementDateTime" formikProps={formikProps} />
                </SectionField>
                <SectionField
                  label="Expropriation notice served date"
                  labelWidth="7"
                  contentWidth="4"
                >
                  <FastDatePicker
                    field="expropriationNoticeServedDateTime"
                    formikProps={formikProps}
                  />
                </SectionField>
                <SectionField label="Expropriation vesting date" labelWidth="7" contentWidth="4">
                  <FastDatePicker field="expropriationVestingDateTime" formikProps={formikProps} />
                </SectionField>
                <SectionField label="Special instructions" labelWidth="12">
                  <MediumTextArea field="specialInstruction" />
                </SectionField>
              </Section>

              <Section header="Financial coding">
                <SectionField label="Product" labelWidth="4">
                  {acquisitionFile.product?.code ?? ''}
                </SectionField>
                <SectionField label="Business function" labelWidth="4">
                  {acquisitionFile.project?.businessFunctionCode?.code ?? ''}
                </SectionField>
                <SectionField label="Work activity" labelWidth="4">
                  {acquisitionFile.project?.workActivityCode?.code ?? ''}
                </SectionField>
                <SectionField label="Cost type" labelWidth="4">
                  {acquisitionFile.project?.costTypeCode?.code ?? ''}
                </SectionField>
                <SectionField label="Fiscal year" labelWidth="4" contentWidth="4" required>
                  <Select field="fiscalYear" options={fiscalYearOptions} placeholder="Select..." />
                </SectionField>
              </Section>

              <Section header="Payment">
                <SectionField label="Payee" labelWidth="3" required>
                  <Select
                    field="payeeKey"
                    options={payeeOptions.map<SelectOption>(x => {
                      return { label: x.text, value: x.value };
                    })}
                    placeholder="Select..."
                  />
                </SectionField>
              </Section>

              <Section>
                <SectionField label="Detailed remarks" labelWidth="12">
                  <MediumTextArea field="detailedRemarks" />
                </SectionField>
              </Section>
            </StyledContent>

            <Prompt
              when={formikProps.dirty}
              message="You have made changes on this form. Do you wish to leave without saving?"
            />
            <StyledFooter>
              <SidebarFooter
                onSave={() => formikProps.submitForm()}
                isOkDisabled={formikProps.isSubmitting || !formikProps.dirty}
                onCancel={() => cancelFunc(formikProps.resetForm, formikProps.dirty)}
              />
            </StyledFooter>
          </>
        )}
      </Formik>
    </StyledFormWrapper>
  );
};

export default UpdateCompensationRequisitionForm;

const generateFiscalYearOptions = () => {
  let sysdate = moment();
  let pivotYear = sysdate.month() >= 4 ? moment().year() : moment().subtract(1, 'years').year();

  const options: SelectOption[] = [];

  options.push({ label: `${pivotYear - 1}/${pivotYear}`, value: `${pivotYear - 1}/${pivotYear}` });
  options.push({ label: `${pivotYear}/${pivotYear + 1}`, value: `${pivotYear}/${pivotYear + 1}` });
  options.push({
    label: `${pivotYear + 1}/${pivotYear + 2}`,
    value: `${pivotYear + 1}/${pivotYear + 2}`,
  });

  return options;
};

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-bottom: 1rem;
`;

const StyledContent = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

const StyledFooter = styled.div`
  margin-right: 1rem;
  padding-bottom: 1rem;
`;

const MediumTextArea = styled(TextArea)`
  textarea.form-control {
    min-width: 100%;
    height: 7rem;
    resize: none;
  }
`;
