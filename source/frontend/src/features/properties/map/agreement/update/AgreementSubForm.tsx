import { FastCurrencyInput, FastDatePicker, Input, Select, TextArea } from 'components/common/form';
import { YesNoSelect } from 'components/common/form/YesNoSelect';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FormikProps, useFormikContext } from 'formik';
import React from 'react';
import { ILookupCode } from 'store/slices/lookupCodes';
import styled from 'styled-components';
import { mapLookupCode } from 'utils';
import { withNameSpace } from 'utils/formUtils';

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
  return (
    <>
      <StyledSectionSubheader>Agreement details</StyledSectionSubheader>
      <SectionField labelWidth="5" label="Agreement status">
        <YesNoSelect field={withNameSpace(nameSpace, 'agreementStatus')} />
      </SectionField>
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

      <StyledSectionSubheader>Financial</StyledSectionSubheader>
      <SectionField labelWidth="5" label="Purchase price">
        <FastCurrencyInput
          field={withNameSpace(nameSpace, 'purchasePrice')}
          formikProps={formikProps}
        />
      </SectionField>
      <SectionField labelWidth="5" label="Deposit due no later than">
        <Input field={withNameSpace(nameSpace, 'noLaterThanDays')} />
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
