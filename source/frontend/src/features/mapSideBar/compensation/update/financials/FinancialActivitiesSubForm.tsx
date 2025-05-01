import { FieldArray, FormikProps, useFormikContext } from 'formik';
import { useState } from 'react';
import styled from 'styled-components';

import { LinkButton, RemoveIconButton } from '@/components/common/buttons';
import { FastCurrencyInput } from '@/components/common/form';
import { Select, SelectOption } from '@/components/common/form/Select';
import { TypeaheadSelect } from '@/components/common/form/TypeaheadSelect';
import GenericModal from '@/components/common/GenericModal';
import { SectionField } from '@/components/common/Section/SectionField';
import { getCurrencyCleanValue, stringToBoolean } from '@/utils/formUtils';

import { CompensationRequisitionFormModel } from '../../models/CompensationRequisitionFormModel';
import { FinancialActivityFormModel } from '../../models/FinancialActivityFormModel';

export interface IFinancialActivitiesSubFormProps {
  formikProps: FormikProps<CompensationRequisitionFormModel>;
  compensationRequisitionId: number;
  financialActivityOptions: SelectOption[];
  gstConstantPercentage: number;
  activitiesUpdated: () => void;
}

export const FinancialActivitiesSubForm: React.FunctionComponent<
  IFinancialActivitiesSubFormProps
> = ({
  formikProps,
  compensationRequisitionId,
  financialActivityOptions,
  gstConstantPercentage,
  activitiesUpdated,
}) => {
  const { values, setFieldValue } = useFormikContext<CompensationRequisitionFormModel>();
  const [showModal, setShowModal] = useState(false);
  const [rowToDelete, setRowToDelete] = useState<number | undefined>(undefined);

  const onUpdateGstApplicable = (index: number, gstOption: string): void => {
    const isGstRequired = stringToBoolean(gstOption);
    setAmountFields(index, isGstRequired, values.financials[index].pretaxAmount);
  };

  const onPretaxAmountUpdated = (index: number, newValue: string): void => {
    const isGstRequired = stringToBoolean(values.financials[index].isGstRequired);
    const cleanValue = getCurrencyCleanValue(newValue);

    setAmountFields(index, isGstRequired, cleanValue);
  };

  const onTaxAmountUpdated = (index: number, newValue: string): void => {
    let totalAmount = 0;
    const preTaxAmount = values.financials[index].pretaxAmount;
    const cleanValue = getCurrencyCleanValue(newValue);

    totalAmount = preTaxAmount + cleanValue;
    setFieldValue(`financials[${index}].totalAmount`, totalAmount);
    activitiesUpdated();
  };

  const setAmountFields = (index: number, gstRequired: boolean, pretaxAmount: number): void => {
    let totalAmount = 0;
    let taxAmount = 0;

    if (gstRequired) {
      taxAmount = pretaxAmount * gstConstantPercentage;
      totalAmount = taxAmount + pretaxAmount;
    } else {
      totalAmount = pretaxAmount;
    }

    setFieldValue(`financials[${index}].taxAmount`, taxAmount);
    setFieldValue(`financials[${index}].totalAmount`, totalAmount);

    activitiesUpdated();
  };

  return (
    <FieldArray
      name="financials"
      render={arrayHelpers => {
        return (
          <>
            {values.financials.map((financial, index) => (
              <div
                key={`financial-act-${financial._id}`}
                data-testid={`finacialActivity[${index}]`}
              >
                <>
                  <StyledSubHeader>
                    <label>Activity {index + 1}</label>
                    <RemoveIconButton
                      title="Delete Financial Activity"
                      data-testId={`activity[${index}].delete-button`}
                      onRemove={() => {
                        setRowToDelete(index);
                        setShowModal(true);
                      }}
                    />
                  </StyledSubHeader>

                  <SectionField label="Code & Description" labelWidth={{ xs: 4 }} required>
                    <TypeaheadSelect
                      field={`financials.${index}.financialActivityCodeId`}
                      options={financialActivityOptions}
                    />
                  </SectionField>

                  <SectionField label="Amount (before tax)" required>
                    <FastCurrencyInput
                      allowNegative
                      formikProps={formikProps}
                      field={`financials[${index}].pretaxAmount`}
                      onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                        onPretaxAmountUpdated(index, e.target.value);
                      }}
                      onBlurChange={() => {
                        activitiesUpdated();
                      }}
                    />
                  </SectionField>

                  <SectionField label="GST applicable?" required>
                    <Select
                      field={`financials[${index}].isGstRequired`}
                      options={[
                        { label: 'No', value: 'false' },
                        { label: 'Yes', value: 'true' },
                      ]}
                      onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                        const selectedValue = [].slice
                          .call(e.target.selectedOptions)
                          .map((option: HTMLOptionElement & number) => option.value)[0];
                        onUpdateGstApplicable(index, selectedValue);
                      }}
                    />
                  </SectionField>

                  {financial.isGstRequired === 'true' && (
                    <SectionField label="GST amount">
                      <FastCurrencyInput
                        allowNegative
                        formikProps={formikProps}
                        field={`financials[${index}].taxAmount`}
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                          onTaxAmountUpdated(index, e.target.value);
                        }}
                      />
                    </SectionField>
                  )}

                  <SectionField label="Total amount">
                    <FastCurrencyInput
                      allowNegative
                      formikProps={formikProps}
                      field={`financials[${index}].totalAmount`}
                      disabled
                    />
                  </SectionField>
                </>
                {index !== values.financials.length - 1 && <StyledSpacer className="my-3" />}
              </div>
            ))}
            <LinkButton
              data-testid="add-financial-activity"
              onClick={() => {
                const activity = new FinancialActivityFormModel(null, compensationRequisitionId);
                arrayHelpers.push(activity);
              }}
            >
              + Add an Activity
            </LinkButton>

            <GenericModal
              variant="warning"
              display={showModal}
              title="Remove financial activity"
              message={'Are you sure you want to remove this financial activity?'}
              okButtonText="Remove"
              cancelButtonText="Cancel"
              handleOk={() => {
                setShowModal(false);
                rowToDelete !== undefined && arrayHelpers.remove(rowToDelete);
                setRowToDelete(undefined);
                activitiesUpdated();
              }}
              handleCancel={() => {
                setShowModal(false);
                setRowToDelete(undefined);
              }}
            />
          </>
        );
      }}
    ></FieldArray>
  );
};

export default FinancialActivitiesSubForm;

const StyledSpacer = styled.div`
  border-bottom: 0.1rem solid grey;
`;

const StyledSubHeader = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  border-bottom: solid 0.2rem ${props => props.theme.css.headerBorderColor};
  margin-bottom: 2rem;

  label {
    color: ${props => props.theme.css.headerTextColor};
    font-family: 'BCSans-Bold';
    font-size: 1.75rem;
    width: 100%;
    text-align: left;
  }

  button {
    margin-bottom: 1rem;
  }
`;
