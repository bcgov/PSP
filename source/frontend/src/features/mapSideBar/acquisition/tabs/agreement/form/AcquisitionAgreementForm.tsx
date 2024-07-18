import { FormikProps, getIn } from 'formik';
import { useEffect } from 'react';
import styled from 'styled-components';

import { FastCurrencyInput, FastDatePicker, Input, Select } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField, StyledFieldLabel } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useModalContext } from '@/hooks/useModalContext';
import { ApiGen_CodeTypes_AgreementStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AgreementStatusTypes';
import { ApiGen_CodeTypes_AgreementTypes } from '@/models/api/generated/ApiGen_CodeTypes_AgreementTypes';
import { mapLookupCode } from '@/utils/mapLookupCode';

import { AcquisitionAgreementFormModel } from '../models/AcquisitionAgreementFormModel';
import { StyledSectionSubheader } from '../styles';

export interface IAcquisitionAgreementFormProps {
  formikProps: FormikProps<AcquisitionAgreementFormModel>;
}

const AcquisitionAgreementForm: React.FunctionComponent<
  React.PropsWithChildren<IAcquisitionAgreementFormProps>
> = ({ formikProps }) => {
  const { setModalContent, setDisplayModal } = useModalContext();
  const { getOptionsByType, getByType } = useLookupCodeHelpers();

  const agreementStatusOptions = getOptionsByType(API.AGREEMENT_STATUS_TYPES);
  const agreementTypeOptions = getByType(API.AGREEMENT_TYPES);

  const agreementStatusTypeCodeValue: string | null = getIn(
    formikProps.values,
    'agreementStatusTypeCode',
  );
  const agreementTypeCodeValue: string | null = getIn(formikProps.values, 'agreementTypeCode');
  const agreementCancellationNoteValue: string | null = getIn(
    formikProps.values,
    'cancellationNote',
  );
  const agreementStatusTypeCodeTouched: string | null = getIn(
    formikProps.touched,
    'agreementStatusTypeCode',
  );

  useEffect(() => {
    if (
      agreementStatusTypeCodeValue !== ApiGen_CodeTypes_AgreementStatusTypes.CANCELLED &&
      !!agreementCancellationNoteValue
    ) {
      setModalContent({
        variant: 'warning',
        okButtonText: 'Yes',
        cancelButtonText: 'No',
        message:
          'Changing status to a status other than "Cancelled" will remove your "Cancellation reason". Are you sure you want to continue?',
        title: 'Warning',
        handleCancel: () => {
          formikProps.setFieldValue(
            'agreementStatusTypeCode',
            ApiGen_CodeTypes_AgreementStatusTypes.CANCELLED,
          );
          setDisplayModal(false);
        },
        handleOk: () => {
          formikProps.setFieldValue('cancellationNote', '');
          setDisplayModal(false);
        },
      });
      setDisplayModal(true);
    }
  }, [
    agreementStatusTypeCodeValue,
    agreementCancellationNoteValue,
    setDisplayModal,
    setModalContent,
    formikProps,
    agreementStatusTypeCodeTouched,
  ]);

  return (
    <Section header="Agreement Details">
      <SectionField labelWidth="5" label="Agreement status">
        <Select options={agreementStatusOptions} field="agreementStatusTypeCode" />
      </SectionField>
      {agreementStatusTypeCodeValue === ApiGen_CodeTypes_AgreementStatusTypes.CANCELLED && (
        <SectionField labelWidth="5" label="Cancellation reason">
          <Input field="cancellationNote" />
        </SectionField>
      )}
      <SectionField labelWidth="5" label="Legal survey plan">
        <Input field="legalSurveyPlanNum" />
      </SectionField>
      <SectionField labelWidth="5" label="Agreement type" required>
        <Select
          field="agreementTypeCode"
          options={agreementTypeOptions.map(mapLookupCode)}
          placeholder="Select Agreement Type"
          onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
            const selectedValue = [].slice
              .call(e.target.selectedOptions)
              .map((option: HTMLOptionElement & number) => option.value)[0];
            if (!!selectedValue && selectedValue !== ApiGen_CodeTypes_AgreementTypes.H0074) {
              formikProps.setFieldValue('commencementDate', '');
            }
          }}
        />
      </SectionField>
      <SectionField labelWidth="5" label="Agreement date">
        <FastDatePicker field="agreementDate" formikProps={formikProps} />
      </SectionField>
      {agreementTypeCodeValue === ApiGen_CodeTypes_AgreementTypes.H0074 && (
        <SectionField labelWidth="5" label="Commencement date">
          <FastDatePicker field="commencementDate" formikProps={formikProps} />
        </SectionField>
      )}
      <SectionField labelWidth="5" label="Completion date">
        <FastDatePicker field="completionDate" formikProps={formikProps} />
      </SectionField>
      <SectionField labelWidth="5" label="Termination date">
        <FastDatePicker field="terminationDate" formikProps={formikProps} />
      </SectionField>
      <SectionField labelWidth="5" label="Possession date">
        <FastDatePicker field="possessionDate" formikProps={formikProps} />
      </SectionField>

      <StyledSectionSubheader>Financial</StyledSectionSubheader>
      <SectionField labelWidth="5" label="Purchase price">
        <FastCurrencyInput field="purchasePrice" formikProps={formikProps} />
      </SectionField>
      <SectionField
        labelWidth="5"
        label="Deposit due no later than"
        tooltip="Generally, if applicable, this is number of days from the execution of the agreement"
      >
        <StyledDiv>
          <Input field="noLaterThanDays" />
          <StyledFieldLabel>days.</StyledFieldLabel>
        </StyledDiv>
      </SectionField>
      <SectionField labelWidth="5" label="Deposit amount">
        <FastCurrencyInput field="depositAmount" formikProps={formikProps} />
      </SectionField>
    </Section>
  );
};

export default AcquisitionAgreementForm;

const StyledDiv = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-start;
  label {
    margin-left: 0.5rem;
  }
`;
