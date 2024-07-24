import { getIn, useFormikContext } from 'formik';

import { FastCurrencyInput, FastDatePicker } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { FastDateYearPicker } from '@/components/common/form/FastDateYearPicker';
import { PrimaryContactSelector } from '@/components/common/form/PrimaryContactSelector/PrimaryContactSelector';
import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { DispositionSaleFormModel } from '@/features/mapSideBar/disposition/models/DispositionSaleFormModel';
import { useModalContext } from '@/hooks/useModalContext';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { getCurrencyCleanValue, stringToBoolean } from '@/utils/formUtils';

import { useCalculateNetProceeds } from '../hooks/useCalculateNetProceeds';
import DispositionSalePurchaserSubForm from './DispositionSalePurchasersSubForm';

export interface IDispositionSaleFormProps {
  dispositionSaleId: number | null;
}

const DispositionSaleForm: React.FunctionComponent<
  React.PropsWithChildren<IDispositionSaleFormProps>
> = ({ dispositionSaleId }) => {
  const formikProps = useFormikContext<DispositionSaleFormModel>();
  const { setModalContent, setDisplayModal } = useModalContext();

  const isGstRequired = getIn(formikProps.values, 'isGstRequired');
  const dispositionPurchaserAgent = getIn(formikProps.values, 'dispositionPurchaserAgent');
  const dispositionPurchaserSolicitor = getIn(formikProps.values, 'dispositionPurchaserSolicitor');

  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) / 100 : undefined;

  useCalculateNetProceeds(isGstRequired);

  // Functions
  const onFinalSaleAmountUpdated = (newValue: string): void => {
    const isGstRequired = getIn(formikProps.values, 'isGstRequired');
    const cleanValue = getCurrencyCleanValue(newValue);

    setGSTDerivedAmountFields(isGstRequired, cleanValue);
  };

  const onUpdateGstApplicable = (gstOption: string): void => {
    const isGstRequired = stringToBoolean(gstOption);
    const taxCollectedAmountValue = getIn(formikProps.values, 'gstCollectedAmount');

    if (!isGstRequired) {
      setModalContent({
        variant: 'warning',
        title: 'Confirm Change',
        message: 'The GST, if provided, will be cleared. Do you wish to proceed?',
        okButtonText: 'Yes',
        cancelButtonText: 'No',
        handleOk: () => {
          formikProps.setFieldValue(`isGstRequired`, false);
          setGSTDerivedAmountFields(isGstRequired, formikProps.values.finalSaleAmount ?? 0);
          setDisplayModal(false);
        },
        handleCancel: () => {
          formikProps.setFieldValue(`isGstRequired`, true);
          formikProps.setFieldValue(`gstCollectedAmount`, taxCollectedAmountValue);
          setDisplayModal(false);
        },
      });
      setDisplayModal(true);
    } else {
      formikProps.setFieldValue(`isGstRequired`, gstOption);
      setGSTDerivedAmountFields(isGstRequired, formikProps.values.finalSaleAmount ?? 0);
    }
  };

  const setGSTDerivedAmountFields = (gstRequired: boolean, salesAmount: number): void => {
    if (gstRequired && gstDecimal) {
      const taxAmount = salesAmount - salesAmount / (1 + gstDecimal);
      formikProps.setFieldValue(`gstCollectedAmount`, taxAmount);
    } else {
      formikProps.setFieldValue(`gstCollectedAmount`, '');
    }
  };

  return (
    <Section header="Sales Details">
      <SectionField label="Purchaser name(s)" labelWidth="5" contentWidth="7">
        <DispositionSalePurchaserSubForm dispositionSaleId={dispositionSaleId} />
      </SectionField>

      <SectionField label="Purchaser agent" labelWidth="5" contentWidth="6">
        <ContactInputContainer
          field="dispositionPurchaserAgent.contact"
          View={ContactInputView}
          displayErrorAsTooltip={false}
        ></ContactInputContainer>
      </SectionField>

      {dispositionPurchaserAgent.contact?.organizationId &&
        !dispositionPurchaserAgent.contact?.personId && (
          <SectionField label="Primary contact" labelWidth="5" contentWidth="5">
            <PrimaryContactSelector
              field={`dispositionPurchaserAgent.primaryContactId`}
              contactInfo={dispositionPurchaserAgent?.contact}
            ></PrimaryContactSelector>
          </SectionField>
        )}

      <SectionField label="Purchaser solicitor" labelWidth="5" contentWidth="6">
        <ContactInputContainer
          field="dispositionPurchaserSolicitor.contact"
          View={ContactInputView}
        ></ContactInputContainer>
      </SectionField>

      {dispositionPurchaserSolicitor.contact?.organizationId &&
        !dispositionPurchaserSolicitor.contact?.personId && (
          <SectionField label="Primary contact" labelWidth="5" contentWidth="5">
            <PrimaryContactSelector
              field={`dispositionPurchaserSolicitor.primaryContactId`}
              contactInfo={dispositionPurchaserSolicitor?.contact}
            ></PrimaryContactSelector>
          </SectionField>
        )}

      <SectionField
        label="Last condition removal date"
        labelWidth="5"
        contentWidth="5"
        tooltip="For general sales, provide the date when the last condition(s) are to be removed. For road closures enter the condition precedent date"
      >
        <FastDatePicker field="finalConditionRemovalDate" formikProps={formikProps} />
      </SectionField>
      <SectionField label="Sale completion date" labelWidth="5" contentWidth="5">
        <FastDatePicker field="saleCompletionDate" formikProps={formikProps} />
      </SectionField>
      <SectionField label="Fiscal year of sale" labelWidth="5" contentWidth="5">
        <FastDateYearPicker field="saleFiscalYear" formikProps={formikProps} />
      </SectionField>
      <SectionField label="Final sale price, incl. GST ($)" labelWidth="5" contentWidth="5">
        <FastCurrencyInput
          formikProps={formikProps}
          field="finalSaleAmount"
          onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
            onFinalSaleAmountUpdated(e.target.value);
          }}
        />
      </SectionField>
      <SectionField label="Realtor commission ($)" labelWidth="5" contentWidth="5">
        <FastCurrencyInput formikProps={formikProps} field="realtorCommissionAmount" />
      </SectionField>

      <SectionField label="GST required" labelWidth="5" contentWidth="5">
        <YesNoSelect
          field="isGstRequired"
          notNullable={true}
          onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
            const selectedValue = [].slice
              .call(e.target.selectedOptions)
              .map((option: HTMLOptionElement & number) => option.value)[0];
            onUpdateGstApplicable(selectedValue);
          }}
        ></YesNoSelect>
      </SectionField>
      {isGstRequired && (
        <SectionField
          label="GST collected ($)"
          labelWidth="5"
          contentWidth="5"
          tooltip="GST collected is calculated based upon Final Sales Price"
        >
          <FastCurrencyInput allowNegative formikProps={formikProps} field="gstCollectedAmount" />
        </SectionField>
      )}

      <SectionField label="Net Book Value ($)" labelWidth="5" contentWidth="5">
        <FastCurrencyInput formikProps={formikProps} field="netBookAmount" />
      </SectionField>

      <SectionField
        label="Total cost of sales ($)"
        labelWidth="5"
        contentWidth="5"
        tooltip="Sum of all costs incurred to prepare property for sale (e.g., appraisal, environmental and other consultants, legal fees, First Nations accommodation, etc.)"
      >
        <FastCurrencyInput formikProps={formikProps} field="totalCostAmount" />
      </SectionField>

      <SectionField
        label="Net proceeds before SPP cost ($)"
        labelWidth="5"
        contentWidth="5"
        tooltip="Net Proceeds before Surplus Property Program (SPP) Cost = Final Sales price, less Commissions, GST Total Cost of Sales, and Net Book Value"
      >
        <FastCurrencyInput
          formikProps={formikProps}
          field="netProceedsBeforeSppAmount"
          allowNegative
          disabled
        />
      </SectionField>
      <SectionField
        label="SPP Amount ($)"
        labelWidth="5"
        contentWidth="5"
        tooltip="Surplus Property Program (SPP) fee to be paid to CITZ"
      >
        <FastCurrencyInput formikProps={formikProps} field="sppAmount" />
      </SectionField>

      <SectionField
        label="Net proceeds after SPP cost ($)"
        labelWidth="5"
        contentWidth="5"
        tooltip="Net Proceeds after SPP Cost = Final Sales price, less Commissions, GST, Net Book Value, Total Cost of Sales,  and SPP Amount"
      >
        <FastCurrencyInput
          formikProps={formikProps}
          field="netProceedsAfterSppAmount"
          allowNegative
          disabled
        />
      </SectionField>
      <SectionField label="Remediation cost ($)" labelWidth="5" contentWidth="5">
        <FastCurrencyInput formikProps={formikProps} field="remediationAmount" />
      </SectionField>
    </Section>
  );
};

export default DispositionSaleForm;
