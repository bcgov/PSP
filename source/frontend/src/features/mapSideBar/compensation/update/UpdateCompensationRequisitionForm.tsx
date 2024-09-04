import { Formik, FormikProps } from 'formik';
import moment from 'moment';
import { useEffect, useRef, useState } from 'react';
import React from 'react';
import styled from 'styled-components';

import {
  Check,
  FastCurrencyInput,
  FastDatePicker,
  Input,
  ProjectSelector,
  Select,
  SelectOption,
  TextArea,
} from '@/components/common/form';
import { TypeaheadSelect } from '@/components/common/form/TypeaheadSelect';
import GenericModal from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import FilePropertiesTable from '@/components/filePropertiesTable/FilePropertiesTable';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { IAutocompletePrediction } from '@/interfaces/IAutocomplete';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { isValidId } from '@/utils';
import { prettyFormatDate } from '@/utils/dateUtils';
import { withNameSpace } from '@/utils/formUtils';

import { CompensationRequisitionFormModel } from '../models/CompensationRequisitionFormModel';
import { CompensationRequisitionYupSchema } from './CompensationRequisitionYupSchema';
import FinancialActivitiesSubForm from './financials/FinancialActivitiesSubForm';

export interface CompensationRequisitionFormProps {
  isLoading: boolean;
  fileType: ApiGen_CodeTypes_FileTypes;
  file: ApiGen_Concepts_AcquisitionFile | ApiGen_Concepts_Lease;
  payeeOptions: PayeeOption[];
  initialValues: CompensationRequisitionFormModel;
  gstConstant: number;
  financialActivityOptions: SelectOption[];
  chartOfAccountsOptions: SelectOption[];
  responsiblityCentreOptions: SelectOption[];
  yearlyFinancialOptions: SelectOption[];
  onSave: (
    compensation: CompensationRequisitionFormModel,
  ) => Promise<ApiGen_Concepts_CompensationRequisition | undefined>;
  onCancel: () => void;
  showAltProjectError: boolean;
  setShowAltProjectError: React.Dispatch<React.SetStateAction<boolean>>;
}

const UpdateCompensationRequisitionForm: React.FC<CompensationRequisitionFormProps> = ({
  isLoading,
  fileType,
  file,
  payeeOptions,
  initialValues,
  gstConstant,
  financialActivityOptions,
  chartOfAccountsOptions,
  responsiblityCentreOptions,
  yearlyFinancialOptions,
  onSave,
  onCancel,
  showAltProjectError,
  setShowAltProjectError,
}) => {
  const fiscalYearOptions = generateFiscalYearOptions();
  const { setModalContent, setDisplayModal } = useModalContext();
  const formikRef = useRef<FormikProps<CompensationRequisitionFormModel>>(null);
  const [activitiesUpdated, setActivitiesUpdated] = useState<boolean>(false);
  const [showModal, setShowModal] = useState(false);
  const [isValid, setIsValid] = useState<boolean>(true);

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

  const onFinancialActivitiesUpdated = (values: CompensationRequisitionFormModel) => {
    const pretaxAmount = values?.financials
      .map(f => f.pretaxAmount)
      .reduce((prev, next) => prev + next, 0);

    const taxAmount = values?.financials
      .map(f => f.taxAmount)
      .reduce((prev, next) => prev + next, 0);

    const totalAmount = values?.financials
      .map(f => f.totalAmount)
      .reduce((prev, next) => prev + next, 0);

    formikRef.current?.setFieldValue(`payee.pretaxAmount`, pretaxAmount);
    formikRef.current?.setFieldValue(`payee.taxAmount`, taxAmount);
    formikRef.current?.setFieldValue(`payee.totalAmount`, totalAmount);
  };

  useEffect(() => {
    if (activitiesUpdated && formikRef.current?.values) {
      onFinancialActivitiesUpdated(formikRef.current?.values);
      setActivitiesUpdated(false);
    }
  }, [activitiesUpdated]);

  const handleSubmit = async (values: CompensationRequisitionFormModel) => {
    if (initialValues.status !== 'final' && values.status === 'final') {
      setShowModal(true);
    } else {
      await onSave(values);
    }
  };

  const onMinistryProjectSelected = async (param: IAutocompletePrediction[]) => {
    if (param.length > 0 && fileType === ApiGen_CodeTypes_FileTypes.Acquisition) {
      if (
        isValidId(param[0].id) &&
        (file as ApiGen_Concepts_AcquisitionFile).projectId === param[0].id
      ) {
        setShowAltProjectError(true);
      }
    }
  };

  return (
    <Formik<CompensationRequisitionFormModel>
      enableReinitialize
      innerRef={formikRef}
      initialValues={initialValues}
      onSubmit={handleSubmit}
      validationSchema={CompensationRequisitionYupSchema}
      validateOnChange={true}
    >
      {formikProps => {
        return (
          <StyledFormWrapper>
            <LoadingBackdrop show={isLoading}></LoadingBackdrop>

            <StyledContent>
              <Section header="Requisition Details">
                <SectionField label="Status" labelWidth="5">
                  <Select
                    field="status"
                    placeholder="Select..."
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
                  />
                </SectionField>
                <SectionField label="Alternate project" labelWidth="5" contentWidth="6">
                  <ProjectSelector
                    field="alternateProject"
                    onChange={(vals: IAutocompletePrediction[]) => {
                      onMinistryProjectSelected(vals);
                    }}
                  ></ProjectSelector>
                </SectionField>
                <SectionField
                  label="Final date"
                  labelWidth="5"
                  contentWidth="4"
                  valueTestId="compensation-finalized-date"
                >
                  {prettyFormatDate(initialValues.finalizedDate)}
                </SectionField>
                <SectionField label="Agreement date" labelWidth="5" contentWidth="4">
                  <FastDatePicker field="agreementDateTime" formikProps={formikProps} />
                </SectionField>

                {fileType === ApiGen_CodeTypes_FileTypes.Acquisition && (
                  <>
                    <SectionField
                      label="Expropriation notice served date"
                      labelWidth="5"
                      contentWidth="4"
                    >
                      <FastDatePicker
                        field="expropriationNoticeServedDateTime"
                        formikProps={formikProps}
                      />
                    </SectionField>
                    <SectionField
                      label="Expropriation vesting date"
                      labelWidth="5"
                      contentWidth="4"
                    >
                      <FastDatePicker
                        field="expropriationVestingDateTime"
                        formikProps={formikProps}
                      />
                    </SectionField>
                    <SectionField
                      label="Advanced payment served date"
                      labelWidth="5"
                      contentWidth="4"
                    >
                      <FastDatePicker field="advancedPaymentServedDate" formikProps={formikProps} />
                    </SectionField>
                  </>
                )}

                <SectionField label="Special instructions" labelWidth="12">
                  <MediumTextArea field="specialInstruction" />
                </SectionField>
              </Section>

              <Section
                header={
                  <div className="d-flex align-items-center">
                    <span>Select File Properties</span>
                    <TooltipIcon
                      toolTipId="select-properties"
                      innerClassName="ml-4 mb-1"
                      toolTip="Select the properties that will be displayed on the generated document"
                    />
                  </div>
                }
              >
                <SectionField label="Selected Properties" labelWidth="5">
                  <FilePropertiesTable
                    disabledSelection={false}
                    fileProperties={file.fileProperties ?? []}
                    selectedFileProperties={formikProps.values.selectedProperties}
                    setSelectedFileProperties={(fileProperties: ApiGen_Concepts_FileProperty[]) => {
                      formikProps.setFieldValue('selectedProperties', fileProperties);
                    }}
                  ></FilePropertiesTable>
                </SectionField>
              </Section>

              <Section header="Financial Coding">
                <SectionField label="Product" labelWidth="4">
                  {file.product?.code ?? ''}
                </SectionField>
                <SectionField label="Business function" labelWidth="4">
                  {file.project?.businessFunctionCode?.code ?? ''}
                </SectionField>
                <SectionField label="Work activity" labelWidth="4">
                  {file.project?.workActivityCode?.code ?? ''}
                </SectionField>
                <SectionField label="Cost type" labelWidth="4">
                  {file.project?.costTypeCode?.code ?? ''}
                </SectionField>
                <SectionField label="Fiscal year" labelWidth="4" contentWidth="4" required>
                  <Select field="fiscalYear" options={fiscalYearOptions} placeholder="Select..." />
                </SectionField>
                <SectionField label="STOB" labelWidth="4" required>
                  <TypeaheadSelect field="stob" options={yearlyFinancialOptions} />
                </SectionField>
                <SectionField label="Service line" labelWidth="4" required>
                  <TypeaheadSelect field="serviceLine" options={chartOfAccountsOptions} />
                </SectionField>
                <SectionField label="Responsibility centre" labelWidth="4" required>
                  <TypeaheadSelect
                    field="responsibilityCentre"
                    options={responsiblityCentreOptions}
                  />
                </SectionField>
              </Section>

              <Section header="Payment" isCollapsable initiallyExpanded>
                <SectionField label="Payee" labelWidth="4" required>
                  <Select
                    field={withNameSpace('payee', 'payeeKey')}
                    title={
                      payeeOptions.find(p => p.value === formikProps.values.payee.payeeKey)
                        ?.fullText
                    }
                    options={payeeOptions.map<SelectOption>(x => {
                      return { label: x.text, value: x.value, title: x.fullText };
                    })}
                    placeholder="Select..."
                  />
                </SectionField>
                <SectionField label="Payment in Trust?">
                  <Check field={withNameSpace('payee', 'isPaymentInTrust')} />
                </SectionField>
                <SectionField label="GST number" tooltip="Include GST # if applicable">
                  <Input field={withNameSpace('payee', 'gstNumber')}></Input>
                </SectionField>
                <SectionField label="Amount (before tax)">
                  <FastCurrencyInput
                    allowNegative
                    field={withNameSpace('payee', 'pretaxAmount')}
                    formikProps={formikProps}
                    disabled
                  />
                </SectionField>
                <SectionField label="GST amount">
                  <FastCurrencyInput
                    field={withNameSpace('payee', 'taxAmount')}
                    allowNegative
                    formikProps={formikProps}
                    disabled
                  />
                </SectionField>
                <SectionField
                  label="Total amount"
                  tooltip="Calculated total of all activities in this compensation requisition"
                >
                  <FastCurrencyInput
                    field={withNameSpace('payee', 'totalAmount')}
                    allowNegative
                    formikProps={formikProps}
                    disabled
                  />
                </SectionField>
              </Section>

              <Section header="Activities" isCollapsable initiallyExpanded>
                {initialValues?.id !== null && initialValues.id >= 0 && (
                  <FinancialActivitiesSubForm
                    financialActivityOptions={financialActivityOptions}
                    compensationRequisitionId={initialValues.id}
                    formikProps={formikProps}
                    gstConstantPercentage={gstConstant}
                    activitiesUpdated={() => {
                      setActivitiesUpdated(true);
                    }}
                  ></FinancialActivitiesSubForm>
                )}
              </Section>

              <Section>
                <SectionField label="Detailed remarks" labelWidth="12">
                  <MediumTextArea field="detailedRemarks" />
                </SectionField>
              </Section>
            </StyledContent>

            <StyledFooter>
              <SidebarFooter
                onSave={async () => {
                  await formikProps.validateForm();
                  if (!formikProps.isValid) {
                    setIsValid(false);
                  } else {
                    setIsValid(true);
                  }
                  formikProps.submitForm();
                }}
                isOkDisabled={formikProps.isSubmitting || !formikProps.dirty}
                onCancel={() => cancelFunc(formikProps.resetForm, formikProps.dirty)}
                displayRequiredFieldError={isValid === false}
              />
            </StyledFooter>

            <GenericModal
              variant="info"
              display={showModal}
              title="Confirm status change"
              message={
                <>
                  You have selected to change the status from DRAFT to FINAL. <br />
                  <br />
                  We recommend that you only make this change status (draft to final) when printing
                  the final version, as
                  <br />
                  <br />
                  <strong key="">you will not be able to roll back to draft status </strong>
                  <br />
                  <br />
                  without system administrator privileges. The compensation requisition cannot be
                  changed again once it is saved as final.
                </>
              }
              okButtonText="Proceed"
              cancelButtonText="Cancel"
              handleOk={async () => {
                formikRef.current?.values && (await onSave(formikRef.current?.values));
                setShowModal(false);
              }}
              handleCancel={() => {
                setShowModal(false);
              }}
            />

            <GenericModal
              variant="error"
              display={showAltProjectError}
              className="error"
              title="Alternate Project Error"
              message={[
                `You have selected an alternate project that is the same as the file project, please select a different project.`,
              ]}
              okButtonText="Close"
              handleOk={() => {
                setShowAltProjectError(false);
                formikRef.current?.setFieldValue('alternateProject', '');
              }}
              handleCancel={() => {
                setShowAltProjectError(false);
              }}
            />
          </StyledFormWrapper>
        );
      }}
    </Formik>
  );
};

export default UpdateCompensationRequisitionForm;

const generateFiscalYearOptions = () => {
  const sysdate = moment();
  const pivotYear = sysdate.month() >= 4 ? moment().year() : moment().subtract(1, 'years').year();

  const options: SelectOption[] = [];

  options.push({ label: `${pivotYear - 1}/${pivotYear}`, value: `${pivotYear - 1}/${pivotYear}` });
  options.push({ label: `${pivotYear}/${pivotYear + 1}`, value: `${pivotYear}/${pivotYear + 1}` });
  options.push({
    label: `${pivotYear + 1}/${pivotYear + 2}`,
    value: `${pivotYear + 1}/${pivotYear + 2}`,
  });

  return options;
};

const StyledContent = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
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
