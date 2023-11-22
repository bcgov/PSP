import { FormikProps, getIn } from 'formik';
import React, { useEffect } from 'react';
import styled from 'styled-components';

import {
  FastCurrencyInput,
  FastDatePicker,
  Input,
  Select,
  TextArea,
} from '@/components/common/form';
import { SectionField, StyledFieldLabel } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useModalContext } from '@/hooks/useModalContext';
import { AgreementStatusTypes } from '@/models/api/Agreement';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { mapLookupCode } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';

import { StyledSectionSubheader } from '../styles';
import { AgreementsFormModel } from './models';

export interface IAgreementSubFormProps {
  index: number;
  nameSpace: string;
  formikProps: FormikProps<AgreementsFormModel>;
  agreementTypes: ILookupCode[];
}

export const AgreementSubForm: React.FunctionComponent<IAgreementSubFormProps> = ({
  index,
  nameSpace,
  formikProps,
  agreementTypes,
}) => {
  const H0074Type = 'H0074';
  const { getOptionsByType } = useLookupCodeHelpers();

  const agreementStatusOptions = getOptionsByType(API.AGREEMENT_STATUS_TYPES);
  const agreement = getIn(formikProps.values, nameSpace);

  const { setDisplayModal, setModalContent } = useModalContext();
  const setFieldValue = formikProps.setFieldValue;
  useEffect(() => {
    if (
      agreement.agreementStatusTypeCode !== AgreementStatusTypes.CANCELLED &&
      !!agreement.cancellationNote
    ) {
      setModalContent({
        okButtonText: 'Yes',
        cancelButtonText: 'No',
        message:
          'Changing status to a status other than "Cancelled" will remove your "Cancellation reason". Are you sure you want to continue?',
        title: 'Warning',
        handleCancel: () => {
          setFieldValue(
            withNameSpace(nameSpace, 'agreementStatusTypeCode'),
            AgreementStatusTypes.CANCELLED,
          );
          setDisplayModal(false);
        },
        handleOk: () => {
          setFieldValue(withNameSpace(nameSpace, 'cancellationNote'), '');
          setDisplayModal(false);
        },
      });
      setDisplayModal(true);
    }
  }, [agreement, setFieldValue, nameSpace, setDisplayModal, setModalContent]);
  return (
    <>
      <StyledSectionSubheader>Agreement details</StyledSectionSubheader>
      <SectionField labelWidth="5" label="Agreement status">
        <Select
          options={agreementStatusOptions}
          field={withNameSpace(nameSpace, 'agreementStatusTypeCode')}
        />
      </SectionField>
      {agreement.agreementStatusTypeCode === AgreementStatusTypes.CANCELLED && (
        <SectionField labelWidth="5" label="Cancellation reason">
          <Input field={withNameSpace(nameSpace, 'cancellationNote')} />
        </SectionField>
      )}
      <SectionField labelWidth="5" label="Legal survey plan">
        <Input field={withNameSpace(nameSpace, 'legalSurveyPlanNum')} />
      </SectionField>
      <SectionField labelWidth="5" label="Agreement type" required>
        <Select
          field={withNameSpace(nameSpace, 'agreementTypeCode')}
          options={agreementTypes.map(mapLookupCode)}
          placeholder="Select Agreement Type"
          onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
            const selectedValue = [].slice
              .call(e.target.selectedOptions)
              .map((option: HTMLOptionElement & number) => option.value)[index];
            if (!!selectedValue && selectedValue !== H0074Type) {
              formikProps.setFieldValue(withNameSpace(nameSpace, 'commencementDate'), '');
            }
          }}
        />
      </SectionField>
      <SectionField labelWidth="5" label="Agreement date">
        <FastDatePicker
          field={withNameSpace(nameSpace, 'agreementDate')}
          formikProps={formikProps}
        />
      </SectionField>
      {formikProps.values.agreements[index].agreementTypeCode === H0074Type && (
        <SectionField labelWidth="5" label="Commencement date">
          <FastDatePicker
            field={withNameSpace(nameSpace, 'commencementDate')}
            formikProps={formikProps}
          />
        </SectionField>
      )}
      <SectionField labelWidth="5" label="Completion date">
        <FastDatePicker
          field={withNameSpace(nameSpace, 'completionDate')}
          formikProps={formikProps}
        />
      </SectionField>
      <SectionField labelWidth="5" label="Termination date">
        <FastDatePicker
          field={withNameSpace(nameSpace, 'terminationDate')}
          formikProps={formikProps}
        />
      </SectionField>
      <SectionField labelWidth="5" label="Possession date">
        <FastDatePicker
          field={withNameSpace(nameSpace, 'possessionDate')}
          formikProps={formikProps}
        />
      </SectionField>

      <StyledSectionSubheader>Financial</StyledSectionSubheader>
      <SectionField labelWidth="5" label="Purchase price">
        <FastCurrencyInput
          field={withNameSpace(nameSpace, 'purchasePrice')}
          formikProps={formikProps}
        />
      </SectionField>
      <SectionField
        labelWidth="5"
        label="Deposit due no later than"
        tooltip="Generally, if applicable, this is number of days from the execution of the agreement."
      >
        <StyledDiv>
          <Input field={withNameSpace(nameSpace, 'noLaterThanDays')} />
          <StyledFieldLabel>days.</StyledFieldLabel>
        </StyledDiv>
      </SectionField>
      <SectionField labelWidth="5" label="Deposit amount">
        <FastCurrencyInput
          field={withNameSpace(nameSpace, 'depositAmount')}
          formikProps={formikProps}
        />
      </SectionField>
    </>
  );
};

export default AgreementSubForm;

export const MediumTextArea = styled(TextArea)`
  textarea.form-control {
    min-width: 80rem;
    height: 7rem;
    resize: none;
  }
`;

const StyledDiv = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-start;
  label {
    margin-left: 0.5rem;
  }
`;
