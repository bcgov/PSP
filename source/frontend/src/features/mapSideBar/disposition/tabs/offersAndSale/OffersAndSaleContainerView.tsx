import React from 'react';
import styled from 'styled-components';

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
import DispositionSaleContactDetails from './dispositionOffer/dispositionSaleContactDetails/DispositionSaleContactDetails';

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

  const purchaserAgent = dispositionSale?.dispositionPurchaserAgents[0] ?? null;
  const purchaserAgentSolicitor = dispositionSale?.dispositionPurchaserSolicitors[0] ?? null;

  return (
    <>
      <LoadingBackdrop show={loading} />
      <Section header="Appraisal and Assessment">
        {appraisalHasData ? (
          <>
            <SectionField
              label="Appraisal value ($)"
              labelWidth="5"
              valueTestId="disposition-file.appraisedValueAmount"
            >
              {formatMoney(dispositionFile.appraisedValueAmount)}
            </SectionField>
            <SectionField
              label="Appraisal date"
              labelWidth="5"
              valueTestId="disposition-file.appraisalDate"
            >
              {prettyFormatDate(dispositionFile.appraisalDate)}
            </SectionField>
            <SectionField
              label="BC assessment value ($)"
              labelWidth="5"
              valueTestId="disposition-file.bcaValueAmount"
            >
              {formatMoney(dispositionFile.bcaValueAmount)}
            </SectionField>
            <SectionField
              label="BC assessment roll year"
              labelWidth="5"
              valueTestId="disposition-file.bcaAssessmentRollYear"
            >
              {dispositionFile.bcaRollYear ?? ''}
            </SectionField>
            <SectionField
              label="List price ($)"
              labelWidth="5"
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
            <SectionField
              label="Purchaser name(s)"
              labelWidth="5"
              valueTestId="disposition-sale.purchasers"
            >
              {dispositionSale.dispositionPurchasers.map((purchaser, index) => (
                <React.Fragment key={`purchaser-${index}`}>
                  <DispositionSaleContactDetails
                    contactInformation={purchaser}
                  ></DispositionSaleContactDetails>
                  {index !== dispositionSale.dispositionPurchasers.length - 1 && (
                    <StyledSpacer className="my-3" />
                  )}
                </React.Fragment>
              ))}
            </SectionField>
            <SectionField
              label="Purchaser agent"
              labelWidth="5"
              valueTestId="disposition-sale.purchaser-agent"
            >
              {purchaserAgent && (
                <DispositionSaleContactDetails
                  contactInformation={purchaserAgent}
                ></DispositionSaleContactDetails>
              )}
            </SectionField>
            <SectionField
              label="Purchaser solicitor"
              labelWidth="5"
              valueTestId="disposition-sale.purchaser-solicitor"
            >
              {purchaserAgentSolicitor && (
                <DispositionSaleContactDetails
                  contactInformation={purchaserAgentSolicitor}
                ></DispositionSaleContactDetails>
              )}
            </SectionField>
            <SectionField
              label="Last condition removal date"
              labelWidth="5"
              tooltip="For general sales, provide the date when the last condition(s) are to be removed. For road closures enter the condition precedent date."
              valueTestId="disposition-sale.finalConditionRemovalDate"
            >
              {prettyFormatDate(dispositionSale.finalConditionRemovalDate)}
            </SectionField>
            <SectionField
              label="Sale completion date"
              labelWidth="5"
              valueTestId="disposition-sale.saleCompletionDate"
            >
              {prettyFormatDate(dispositionSale.saleCompletionDate)}
            </SectionField>
            <SectionField
              label="Fiscal year of sale"
              labelWidth="5"
              valueTestId="disposition-sale.saleFiscalYear"
            >
              {dispositionSale.saleFiscalYear}
            </SectionField>
            <SectionField
              label="Final sale price ($)"
              labelWidth="5"
              valueTestId="disposition-sale.finalSaleAmount"
            >
              {formatMoney(dispositionSale.finalSaleAmount)}
            </SectionField>
            <SectionField
              label="Realtor commission ($)"
              labelWidth="5"
              valueTestId="disposition-sale.realtorCommissionAmount"
            >
              {formatMoney(dispositionSale.realtorCommissionAmount)}
            </SectionField>
            <SectionField
              label="GST required"
              labelWidth="5"
              valueTestId="disposition-sale.isGstRequired"
            >
              {dispositionSale?.isGstRequired ? 'Yes' : 'No'}
            </SectionField>
            <SectionField
              label="GST collected ($)"
              labelWidth="5"
              tooltip="GST collected is calculated based upon Final Sales Price."
              valueTestId="disposition-sale.gstCollectedAmount"
            >
              {formatMoney(dispositionSale.gstCollectedAmount)}
            </SectionField>
            <SectionField
              label="Net Book Value ($)"
              labelWidth="5"
              valueTestId="disposition-sale.netBookAmount"
            >
              {formatMoney(dispositionSale.netBookAmount)}
            </SectionField>
            <SectionField
              label="Total cost of sales ($)"
              labelWidth="5"
              tooltip="Sum of all costs incurred to prepare property for sale (e.g., appraisal, environmental and other consultants, legal fees, First Nations accommodation, etc.)."
              valueTestId="disposition-sale.totalCostAmount"
            >
              {formatMoney(dispositionSale.totalCostAmount)}
            </SectionField>
            <SectionField
              label="Net proceeds before SPP cost ($)"
              labelWidth="5"
              tooltip="Surplus Property Program (SPP)."
              valueTestId="disposition-sale.netProceedsBeforeSppAmount"
            >
              {formatMoney(dispositionSale.netProceedsBeforeSppAmount)}
            </SectionField>
            <SectionField
              label="SPP Amount ($)"
              labelWidth="5"
              tooltip="Surplus Property Program (SPP) fee to be paid to CITZ."
              valueTestId="disposition-sale.sppAmount"
            >
              {formatMoney(dispositionSale.sppAmount)}
            </SectionField>
            <SectionField
              label="Net proceeds after SPP cost ($)"
              labelWidth="5"
              tooltip="Net Proceeds after SPP Cost = Final Sales price, less Commissions, GST, Net Book Value, Total Cost of Sales,  and SPP Amount."
              valueTestId="disposition-sale.netProceedsAfterSppAmount"
            >
              {formatMoney(dispositionSale.netProceedsAfterSppAmount)}
            </SectionField>
            <SectionField
              label="Remediation cost ($)"
              labelWidth="5"
              valueTestId="disposition-sale.remediationAmount"
            >
              {formatMoney(dispositionSale.remediationAmount)}
            </SectionField>
          </>
        )) ?? <p>There are no sale details indicated with this disposition file.</p>}
      </Section>
    </>
  );
};

export default OffersAndSaleContainerView;

const StyledSpacer = styled.div`
  border-bottom: 0.1rem solid ${props => props.theme.css.tableHoverColor};
`;
