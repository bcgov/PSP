import { FormikProps, getIn } from 'formik';
import { useMemo } from 'react';
import styled from 'styled-components';

import { FastCurrencyInput, FastDatePicker, Input, Select } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField, StyledFieldLabel } from '@/components/common/Section/SectionField';
import * as AGREEMENT from '@/constants/agreements';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useModalContext } from '@/hooks/useModalContext';
import { ApiGen_CodeTypes_AgreementStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AgreementStatusTypes';
import { ApiGen_CodeTypes_AgreementTypes } from '@/models/api/generated/ApiGen_CodeTypes_AgreementTypes';

import { AgreementFormModel } from '../models/AgreementFormModel';
import { StyledSectionSubheader } from '../styles';

export interface IAcquisitionFormProps {
  formikProps: FormikProps<AgreementFormModel>;
  fileType: string;
  isNew?: boolean;
}

const AgreementForm: React.FunctionComponent<React.PropsWithChildren<IAcquisitionFormProps>> = ({
  formikProps,
  fileType,
  isNew = false,
}) => {
  const { setModalContent, setDisplayModal } = useModalContext();
  const { getAgreementSelectOptions, getOptionsByType } = useLookupCodeHelpers();

  const agreementStatusSelectOptions = useMemo(() => {
    const options = getOptionsByType(API.AGREEMENT_STATUS_TYPES);
    if (isNew && fileType === 'disposition') {
      return options.filter(option => option.value === ApiGen_CodeTypes_AgreementStatusTypes.DRAFT);
    }
    return options;
  }, [getOptionsByType, isNew, fileType]);

  const agreementTypeSelectOptions = useMemo(() => {
    if (fileType === 'disposition') {
      return getAgreementSelectOptions(API.AGREEMENT_TYPES, fileType, AGREEMENT.DISPOSITION_FORMS);
    }

    const options = getAgreementSelectOptions(API.AGREEMENT_TYPES, fileType);
    return options.filter(option => option.value !== ApiGen_CodeTypes_AgreementTypes.H179RC);
  }, [getAgreementSelectOptions, fileType]);

  const agreementStatusTypeCodeValue: string | null = getIn(
    formikProps.values,
    'agreementStatusTypeCode',
  );

  const agreementTypeCodeValue: string | null = getIn(formikProps.values, 'agreementTypeCode');
  const agreementCancellationNoteValue: string | null = getIn(
    formikProps.values,
    'cancellationNote',
  );

  const onSelectChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    if (
      e.target.value !== ApiGen_CodeTypes_AgreementStatusTypes.CANCELLED &&
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
  };

  return (
    <Section header="Agreement Details">
      <SectionField labelWidth={{ xs: 5 }} label="Agreement status">
        <Select
          options={agreementStatusSelectOptions}
          onChange={onSelectChange}
          field="agreementStatusTypeCode"
        />
      </SectionField>
      {agreementStatusTypeCodeValue === ApiGen_CodeTypes_AgreementStatusTypes.CANCELLED && (
        <SectionField labelWidth={{ xs: 5 }} label="Cancellation reason">
          <Input field="cancellationNote" />
        </SectionField>
      )}
      <SectionField labelWidth={{ xs: 5 }} label="Legal survey plan">
        <Input field="legalSurveyPlanNum" />
      </SectionField>
      <SectionField labelWidth={{ xs: 5 }} label="Agreement type" required>
        <Select
          field="agreementTypeCode"
          options={agreementTypeSelectOptions}
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
      <SectionField labelWidth={{ xs: 5 }} label="Agreement date">
        <FastDatePicker field="agreementDate" formikProps={formikProps} />
      </SectionField>
      {agreementTypeCodeValue === ApiGen_CodeTypes_AgreementTypes.H0074 && (
        <SectionField labelWidth={{ xs: 5 }} label="Commencement date">
          <FastDatePicker field="commencementDate" formikProps={formikProps} />
        </SectionField>
      )}
      <SectionField labelWidth={{ xs: 5 }} label="Completion date">
        <FastDatePicker field="completionDate" formikProps={formikProps} />
      </SectionField>
      <SectionField labelWidth={{ xs: 5 }} label="Termination date">
        <FastDatePicker field="terminationDate" formikProps={formikProps} />
      </SectionField>
      <SectionField labelWidth={{ xs: 5 }} label="Possession date">
        <FastDatePicker field="possessionDate" formikProps={formikProps} />
      </SectionField>

      <StyledSectionSubheader>Financial</StyledSectionSubheader>
      <SectionField labelWidth={{ xs: 5 }} label="Purchase price">
        <FastCurrencyInput field="purchasePrice" formikProps={formikProps} />
      </SectionField>
      <SectionField
        labelWidth={{ xs: 5 }}
        label="Deposit due no later than"
        tooltip="Generally, if applicable, this is number of days from the execution of the agreement"
      >
        <StyledDiv>
          <Input field="noLaterThanDays" />
          <StyledFieldLabel>days.</StyledFieldLabel>
        </StyledDiv>
      </SectionField>
      <SectionField labelWidth={{ xs: 5 }} label="Deposit amount">
        <FastCurrencyInput field="depositAmount" formikProps={formikProps} />
      </SectionField>
    </Section>
  );
};

export default AgreementForm;

const StyledDiv = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-start;
  label {
    margin-left: 0.5rem;
  }
`;
