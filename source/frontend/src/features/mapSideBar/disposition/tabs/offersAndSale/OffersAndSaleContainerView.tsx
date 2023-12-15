import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import {
  Api_DispositionFile,
  Api_DispositionFileOffer,
  Api_DispositionFileSale,
} from '@/models/api/DispositionFile';
import { prettyFormatDate } from '@/utils/dateUtils';
import { formatMoney } from '@/utils/numberFormatUtils';

import DispositionOfferDetails from './dispositionOffer/dispositionOfferDetails/DispositionOfferDetails';

export interface IOffersAndSaleContainerViewProps {
  loading: boolean;
  dispositionFile: Api_DispositionFile;
  dispositionOffers: Api_DispositionFileOffer[];
  dispositionSale: Api_DispositionFileSale | null;
}

const OffersAndSaleContainerView: React.FunctionComponent<IOffersAndSaleContainerViewProps> = ({
  loading,
  dispositionFile,
  dispositionOffers,
  dispositionSale,
}) => {
  const getAppraisalHasData = (): boolean => {
    return (
      dispositionFile.appraisedValueAmount !== null ||
      dispositionFile.appraisalDate !== null ||
      dispositionFile.bcaValueAmount !== null ||
      dispositionFile.bcaRollYear !== null ||
      dispositionFile.listPriceAmount !== null
    );
  };

  const appraisalHasData = getAppraisalHasData();

  return (
    <>
      <LoadingBackdrop show={loading} />
      <Section header="Appraisal and Assessment">
        {appraisalHasData ? (
          <>
            <SectionField
              label="Appraisal value ($)"
              labelWidth="4"
              valueTestId="disposition-file.appraisedAmount"
            >
              {formatMoney(dispositionFile.appraisedValueAmount)}
            </SectionField>
            <SectionField
              label="Appraisal date"
              labelWidth="4"
              valueTestId="disposition-file.appraisalDate"
            >
              {prettyFormatDate(dispositionFile.appraisalDate)}
            </SectionField>
            <SectionField
              label="BC assessment value ($)"
              labelWidth="4"
              valueTestId="disposition-file.bcaValueAmount"
            >
              {formatMoney(dispositionFile.bcaValueAmount)}
            </SectionField>
            <SectionField
              label="BC assessment roll year"
              labelWidth="4"
              valueTestId="disposition-file.bcaAssessmentRollYear"
            >
              {dispositionFile.bcaRollYear ?? ''}
            </SectionField>
            <SectionField
              label="List price ($)"
              labelWidth="4"
              valueTestId="disposition-file.listPriceAmount"
            >
              {formatMoney(dispositionFile.listPriceAmount)}
            </SectionField>
          </>
        ) : (
          <p>There are no value details indicated with this disposition file.</p>
        )}
      </Section>

      <Section isCollapsable initiallyExpanded header={'Offers'}>
        {dispositionOffers.map((offer, index) => (
          <DispositionOfferDetails
            key={index}
            dispositionOffer={offer}
            index={index}
          ></DispositionOfferDetails>
        ))}
        {dispositionOffers.length === 0 && (
          <p>There are no offers indicated with this disposition file.</p>
        )}
      </Section>

      <Section header="Sales Details">
        {(dispositionSale && (
          <>
            <SectionField label="Last condition removal date">
              {prettyFormatDate(dispositionSale.finalConditionRemovalDate)}
            </SectionField>
            <SectionField label="Sale completion date">
              {prettyFormatDate(dispositionSale.saleCompletionDate)}
            </SectionField>
            <SectionField label="Fiscal year of sale">
              {dispositionSale.saleFiscalYear}
            </SectionField>
            <SectionField label="Final sale price ($)">
              {formatMoney(dispositionSale.finalSaleAmount)}
            </SectionField>
            <SectionField label="Realtor commission ($)">
              {formatMoney(dispositionSale.realtorCommissionAmount)}
            </SectionField>
            <SectionField label="GST required">
              {dispositionSale?.isGstRequired ? 'Yes' : 'No'}
            </SectionField>
            <SectionField label="GST collected ($)">
              {formatMoney(dispositionSale.gstCollectedAmount)}
            </SectionField>
            <SectionField label="Net Book Value ($)">
              {formatMoney(dispositionSale.netBookAmount)}
            </SectionField>
            <SectionField label="Total cost of sales ($)">
              {formatMoney(dispositionSale.totalCostAmount)}
            </SectionField>
            <SectionField label="Net proceeds before SPP cost ($)">
              {formatMoney(dispositionSale.netProceedsBeforeSppAmount)}
            </SectionField>
            <SectionField label="SPP Amount ($)">
              {formatMoney(dispositionSale.sppAmount)}
            </SectionField>
            <SectionField label="Net proceeds after SPP cost ($)">
              {formatMoney(dispositionSale.netProceedsAfterSppAmount)}
            </SectionField>
            <SectionField label="Remediation cost ($)">
              {formatMoney(dispositionSale.remediationAmount)}
            </SectionField>
          </>
        )) ?? <p>There are no sale details indicated with this disposition file.</p>}
      </Section>
    </>
  );
};

export default OffersAndSaleContainerView;
