import { FieldArray, FormikProps, useFormikContext } from 'formik';
import { useState } from 'react';
import { FaTrash } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton, StyledRemoveLinkButton } from '@/components/common/buttons';
import { FastCurrencyInput } from '@/components/common/form';
import { Select, SelectOption } from '@/components/common/form/Select';
import GenericModal from '@/components/common/GenericModal';
import { SectionField } from '@/components/common/Section/SectionField';
import { getCurrencyCleanValue, stringToBoolean } from '@/utils/formUtils';

import { CompensationRequisitionFormModel, FinancialActivityFormModel } from '../models';

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
                    <StyledRemoveLinkButton
                      title="Delete financial activity"
                      data-testid={`activity[${index}].delete-button`}
                      variant="light"
                      onClick={() => {
                        setRowToDelete(index);
                        setShowModal(true);
                      }}
                    >
                      <FaTrash size="2rem" />
                    </StyledRemoveLinkButton>
                  </StyledSubHeader>

                  <SectionField label="Code & Description" labelWidth="4" required>
                    <Select
                      field={`financials[${index}].financialActivityCodeId`}
                      options={financialActivityOptions}
                      placeholder="Select..."
                      required
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
                      onBlurChange={(e: React.ChangeEvent<HTMLInputElement>) => {
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
              display={showModal}
              title="Remove financial activity"
              message={'Are you sure you want to remove this financial activity?'}
              okButtonText="Remove"
              cancelButtonText="Cancel"
              handleOk={() => {
                setShowModal(false);
                arrayHelpers.remove(rowToDelete!);
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
  border-bottom: solid 0.2rem ${props => props.theme.css.primaryColor};
  margin-bottom: 2rem;

  label {
    color: ${props => props.theme.css.primaryColor};
    font-family: 'BCSans-Bold';
    font-size: 1.75rem;
    width: 100%;
    text-align: left;
  }

  button {
    margin-bottom: 1rem;
  }
`;
