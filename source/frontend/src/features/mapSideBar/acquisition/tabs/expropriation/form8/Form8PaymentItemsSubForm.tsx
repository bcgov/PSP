import { FieldArray, FormikProps, useFormikContext } from 'formik';
import { FaTrash } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton, StyledRemoveLinkButton } from '@/components/common/buttons';
import { FastCurrencyInput, Select } from '@/components/common/form';
import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { getCurrencyCleanValue, stringToBoolean } from '@/utils/formUtils';

import { Form8FormModel, Form8PaymentItemModel } from './models/Form8FormModel';

export interface IForm8PaymentItemsSubFormProps {
  form8Id: number | null;
  formikProps: FormikProps<Form8FormModel>;
  gstConstantPercentage: number;
}

export const Form8PaymentItemsSubForm: React.FunctionComponent<IForm8PaymentItemsSubFormProps> = ({
  form8Id,
  formikProps,
  gstConstantPercentage,
}) => {
  const { values, setFieldValue } = useFormikContext<Form8FormModel>();
  const { setModalContent, setDisplayModal } = useModalContext();

  const { getOptionsByType } = useLookupCodeHelpers();
  const paymentItemTypesOptions = getOptionsByType(API.PAYMENT_ITEM_TYPES);

  const onUpdateGstApplicable = (index: number, gstOption: string): void => {
    const isGstRequired = stringToBoolean(gstOption);
    setFieldValue(`paymentItems[${index}].isGstRequired`, gstOption);
    setAmountFields(index, isGstRequired, values.paymentItems[index].pretaxAmount);
  };

  const onPretaxAmountUpdated = (index: number, newValue: string): void => {
    const isGstRequired = stringToBoolean(values.paymentItems[index].isGstRequired);
    const cleanValue = getCurrencyCleanValue(newValue);

    setAmountFields(index, isGstRequired, cleanValue);
  };

  const onTaxAmountUpdated = (index: number, newValue: string): void => {
    let totalAmount = 0;
    const preTaxAmount = values.paymentItems[index].pretaxAmount;
    const cleanValue = getCurrencyCleanValue(newValue);

    totalAmount = preTaxAmount + cleanValue;
    setFieldValue(`paymentItems[${index}].totalAmount`, totalAmount);
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

    setFieldValue(`paymentItems[${index}].taxAmount`, taxAmount);
    setFieldValue(`paymentItems[${index}].totalAmount`, totalAmount);
  };

  return (
    <FieldArray
      name="paymentItems"
      render={arrayHelpers => {
        return (
          <>
            {values.paymentItems.map((item, index) => (
              <div key={index} data-testid={`paymentItems[${index}]`}>
                <StyledSubHeader>
                  <label>Payment Item {index + 1}</label>
                  <StyledRemoveLinkButton
                    title="Delete Payment Item"
                    data-testid={`paymentItems[${index}].delete-button`}
                    variant="light"
                    onClick={() => {
                      setModalContent({
                        ...getDeleteModalProps(),
                        title: 'Remove Payment Item',
                        message: 'Do you wish to remove this payment item?',
                        okButtonText: 'Remove',
                        handleOk: async () => {
                          arrayHelpers.remove(index);
                          setDisplayModal(false);
                        },
                        handleCancel: () => {
                          setDisplayModal(false);
                        },
                      });
                      setDisplayModal(true);
                    }}
                  >
                    <FaTrash size="2rem" />
                  </StyledRemoveLinkButton>
                </StyledSubHeader>

                <SectionField label="Item" labelWidth="4" required>
                  <Select
                    field={`paymentItems[${index}].paymentItemTypeCode`}
                    options={paymentItemTypesOptions}
                    placeholder="Select..."
                    required
                  />
                </SectionField>
                <SectionField label="Payment (w/o GST)" required>
                  <FastCurrencyInput
                    allowNegative
                    formikProps={formikProps}
                    field={`paymentItems[${index}].pretaxAmount`}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                      onPretaxAmountUpdated(index, e.target.value);
                    }}
                  />
                </SectionField>
                <SectionField label="GST applicable?" labelWidth="4">
                  <YesNoSelect
                    notNullable={true}
                    field={`paymentItems[${index}].isGstRequired`}
                    onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                      const selectedValue = [].slice
                        .call(e.target.selectedOptions)
                        .map((option: HTMLOptionElement & number) => option.value)[0];
                      onUpdateGstApplicable(index, selectedValue);
                    }}
                  />
                </SectionField>
                {item.isGstRequired === 'true' && (
                  <SectionField label="GST amount">
                    <FastCurrencyInput
                      allowNegative
                      formikProps={formikProps}
                      field={`paymentItems[${index}].taxAmount`}
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
                    field={`paymentItems[${index}].totalAmount`}
                    disabled
                  />
                </SectionField>
              </div>
            ))}
            <LinkButton
              data-testid="add-payment-item"
              onClick={() => {
                const newItem = new Form8PaymentItemModel(null, form8Id);
                arrayHelpers.push(newItem);
              }}
            >
              + Add payment item
            </LinkButton>
          </>
        );
      }}
    ></FieldArray>
  );
};

export default Form8PaymentItemsSubForm;

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
