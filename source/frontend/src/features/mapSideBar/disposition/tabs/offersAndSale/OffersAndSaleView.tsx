import React from 'react';
import { FaPlus } from 'react-icons/fa';
import { useHistory, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import EditButton from '@/components/common/buttons/EditButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import TooltipIcon from '@/components/common/TooltipIcon';
import { Claims } from '@/constants';
import { cannotEditMessage } from '@/features/mapSideBar/acquisition/common/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_DispositionFileAppraisal } from '@/models/api/generated/ApiGen_Concepts_DispositionFileAppraisal';
import { ApiGen_Concepts_DispositionFileOffer } from '@/models/api/generated/ApiGen_Concepts_DispositionFileOffer';
import { ApiGen_Concepts_DispositionFileSale } from '@/models/api/generated/ApiGen_Concepts_DispositionFileSale';
import { prettyFormatDate } from '@/utils/dateUtils';
import { formatMoney } from '@/utils/numberFormatUtils';
import { exists } from '@/utils/utils';

import {
  calculateNetProceedsAfterSppAmount,
  calculateNetProceedsBeforeSppAmount,
} from '../../models/DispositionSaleFormModel';
import DispositionOfferDetails from './dispositionOffer/dispositionOfferDetails/DispositionOfferDetails';
import DispositionSaleContactDetails from './dispositionOffer/dispositionSaleContactDetails/DispositionSaleContactDetails';

export interface IOffersAndSaleViewProps {
  loading: boolean;
  dispositionFile: ApiGen_Concepts_DispositionFile;
  dispositionOffers: ApiGen_Concepts_DispositionFileOffer[];
  dispositionSale: ApiGen_Concepts_DispositionFileSale | null;
  dispositionAppraisal: ApiGen_Concepts_DispositionFileAppraisal | null;
  isFileFinalStatus?: boolean;
  onDispositionOfferDeleted: (offerId: number) => void;
}

const OffersAndSaleView: React.FunctionComponent<IOffersAndSaleViewProps> = ({
  loading,
  dispositionFile,
  dispositionOffers,
  dispositionSale,
  dispositionAppraisal,
  isFileFinalStatus,
  onDispositionOfferDeleted,
}) => {
  const history = useHistory();
  const match = useRouteMatch();
  const keycloak = useKeycloakWrapper();

  const purchaserAgent = dispositionSale?.dispositionPurchaserAgent;
  const purchaserAgentSolicitor = dispositionSale?.dispositionPurchaserSolicitor;

  return (
    <>
      <LoadingBackdrop show={loading} />
      <Section
        isCollapsable={false}
        header={
          <StyledSubHeader>
            <div>Appraisal and Assessment</div>
            {keycloak.hasClaim(Claims.DISPOSITION_EDIT) && !isFileFinalStatus && (
              <div>
                <EditButton
                  title="Edit Appraisal"
                  data-testId={`appraisal-edit-btn`}
                  onClick={() => {
                    history.push(`${match.url}/appraisal/update`);
                  }}
                />
              </div>
            )}
            {keycloak.hasClaim(Claims.DISPOSITION_EDIT) && isFileFinalStatus && (
              <TooltipIcon
                toolTipId={`${dispositionFile?.id || 0}-values-summary-cannot-edit-tooltip`}
                toolTip={cannotEditMessage}
              />
            )}
          </StyledSubHeader>
        }
      >
        {dispositionAppraisal ? (
          <>
            <SectionField
              label="Appraisal value ($)"
              labelWidth={{ xs: 5 }}
              valueTestId="disposition-file.appraisedValueAmount"
            >
              {formatMoney(dispositionAppraisal.appraisedAmount)}
            </SectionField>
            <SectionField
              label="Appraisal date"
              labelWidth={{ xs: 5 }}
              valueTestId="disposition-file.appraisalDate"
            >
              {prettyFormatDate(dispositionAppraisal.appraisalDate)}
            </SectionField>
            <SectionField
              label="BC assessment value ($)"
              labelWidth={{ xs: 5 }}
              valueTestId="disposition-file.bcaValueAmount"
            >
              {formatMoney(dispositionAppraisal.bcaValueAmount)}
            </SectionField>
            <SectionField
              label="BC assessment roll year"
              labelWidth={{ xs: 5 }}
              valueTestId="disposition-file.bcaAssessmentRollYear"
            >
              {dispositionAppraisal.bcaRollYear ?? ''}
            </SectionField>
            <SectionField
              label="List price ($)"
              labelWidth={{ xs: 5 }}
              valueTestId="disposition-file.listPriceAmount"
            >
              {formatMoney(dispositionAppraisal.listPriceAmount)}
            </SectionField>
          </>
        ) : (
          <p>There are no Appraisal and Assessment details indicated with this disposition file.</p>
        )}
      </Section>

      <Section
        isCollapsable
        initiallyExpanded
        header={
          <SectionListHeader
            claims={[Claims.DISPOSITION_EDIT]}
            title="Offers"
            addButtonText="Add Offer"
            addButtonIcon={<FaPlus size={'2rem'} />}
            onButtonAction={() => {
              history.push(`${match.url}/offers/new`);
            }}
            cannotAddComponent={
              <TooltipIcon
                toolTipId={`deposit-notes-cannot-edit-tooltip`}
                toolTip={cannotEditMessage}
              />
            }
            isAddEnabled={!isFileFinalStatus}
          />
        }
      >
        {dispositionOffers.map((offer, index) => (
          <DispositionOfferDetails
            key={offer.id}
            dispositionOffer={offer}
            index={index}
            onDelete={onDispositionOfferDeleted}
            dispositionFile={dispositionFile}
            isFileFinalStatus={isFileFinalStatus}
          ></DispositionOfferDetails>
        ))}
        {dispositionOffers.length === 0 && (
          <p>There are no offers indicated with this disposition file.</p>
        )}
      </Section>

      <Section
        isCollapsable={false}
        header={
          <StyledSubHeader>
            <div>Sales Details</div>
            {keycloak.hasClaim(Claims.DISPOSITION_EDIT) && !isFileFinalStatus && (
              <EditButton
                title="Edit Sale"
                data-testId={`sale-edit-btn`}
                onClick={() => {
                  history.push(`${match.url}/sale/update`);
                }}
              />
            )}
            {keycloak.hasClaim(Claims.DISPOSITION_EDIT) && isFileFinalStatus && (
              <TooltipIcon
                toolTipId={`${dispositionFile?.id || 0}-sale-summary-cannot-edit-tooltip`}
                toolTip={cannotEditMessage}
              />
            )}
          </StyledSubHeader>
        }
      >
        {(dispositionSale && (
          <>
            <SectionField
              label="Purchaser name(s)"
              labelWidth={{ xs: 6 }}
              valueTestId="disposition-sale.purchasers"
            >
              {exists(dispositionSale.dispositionPurchasers) &&
                dispositionSale.dispositionPurchasers.map((purchaser, index) => (
                  <React.Fragment key={`purchaser-${index}`}>
                    <DispositionSaleContactDetails
                      contactInformation={purchaser}
                    ></DispositionSaleContactDetails>
                    {index !== dispositionSale.dispositionPurchasers!.length - 1 && (
                      <StyledSpacer className="my-3" />
                    )}
                  </React.Fragment>
                ))}
            </SectionField>
            <SectionField
              label="Purchaser agent"
              labelWidth={{ xs: 6 }}
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
              labelWidth={{ xs: 6 }}
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
              labelWidth={{ xs: 6 }}
              tooltip="For general sales, provide the date when the last condition(s) are to be removed. For road closures enter the condition precedent date"
              valueTestId="disposition-sale.finalConditionRemovalDate"
            >
              {prettyFormatDate(dispositionSale.finalConditionRemovalDate)}
            </SectionField>
            <SectionField
              label="Sale completion date"
              labelWidth={{ xs: 6 }}
              valueTestId="disposition-sale.saleCompletionDate"
            >
              {prettyFormatDate(dispositionSale.saleCompletionDate)}
            </SectionField>
            <SectionField
              label="Fiscal year of sale"
              labelWidth={{ xs: 6 }}
              valueTestId="disposition-sale.saleFiscalYear"
            >
              {dispositionSale.saleFiscalYear}
            </SectionField>
            <SectionField
              label="Final sale price, incl. GST ($)"
              labelWidth={{ xs: 6 }}
              valueTestId="disposition-sale.finalSaleAmount"
            >
              {formatMoney(dispositionSale.finalSaleAmount)}
            </SectionField>
            <SectionField
              label="Realtor commission ($)"
              labelWidth={{ xs: 6 }}
              valueTestId="disposition-sale.realtorCommissionAmount"
            >
              {formatMoney(dispositionSale.realtorCommissionAmount)}
            </SectionField>
            <SectionField
              label="GST required"
              labelWidth={{ xs: 6 }}
              valueTestId="disposition-sale.isGstRequired"
            >
              {dispositionSale?.isGstRequired ? 'Yes' : 'No'}
            </SectionField>
            {dispositionSale?.isGstRequired && (
              <SectionField
                label="GST collected ($)"
                labelWidth={{ xs: 6 }}
                tooltip="GST collected is calculated based upon Final Sales Price"
                valueTestId="disposition-sale.gstCollectedAmount"
              >
                {formatMoney(dispositionSale.gstCollectedAmount)}
              </SectionField>
            )}

            <SectionField
              label="Net Book Value ($)"
              labelWidth={{ xs: 6 }}
              valueTestId="disposition-sale.netBookAmount"
            >
              {formatMoney(dispositionSale.netBookAmount)}
            </SectionField>
            <SectionField
              label="Total cost of sales ($)"
              labelWidth={{ xs: 6 }}
              tooltip="Sum of all costs incurred to prepare property for sale (e.g., appraisal, environmental and other consultants, legal fees, First Nations accommodation, etc.)"
              valueTestId="disposition-sale.totalCostAmount"
            >
              {formatMoney(dispositionSale.totalCostAmount)}
            </SectionField>
            <SectionField
              label="Net proceeds before SPP cost ($)"
              labelWidth={{ xs: 6 }}
              tooltip="Net Proceeds before Surplus Property Program (SPP) Cost = Final Sales price, less Commissions, GST Total Cost of Sales, and Net Book Value"
              valueTestId="disposition-sale.netProceedsBeforeSppAmount"
            >
              {formatMoney(calculateNetProceedsBeforeSppAmount(dispositionSale))}
            </SectionField>
            <SectionField
              label="SPP Amount ($)"
              labelWidth={{ xs: 6 }}
              tooltip="Surplus Property Program (SPP) fee to be paid to CITZ"
              valueTestId="disposition-sale.sppAmount"
            >
              {formatMoney(dispositionSale.sppAmount)}
            </SectionField>
            <SectionField
              label="Net proceeds after SPP cost ($)"
              labelWidth={{ xs: 6 }}
              tooltip="Net Proceeds after SPP Cost = Final Sales price, less Commissions, GST, Net Book Value, Total Cost of Sales,  and SPP Amount"
              valueTestId="disposition-sale.netProceedsAfterSppAmount"
            >
              {formatMoney(calculateNetProceedsAfterSppAmount(dispositionSale))}
            </SectionField>
            <SectionField
              label="Remediation cost ($)"
              labelWidth={{ xs: 6 }}
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

export default OffersAndSaleView;

const StyledSpacer = styled.div`
  border-bottom: 0.1rem solid ${props => props.theme.css.borderOutlineColor};
`;

const StyledSubHeader = styled.div`
  display: flex;
  flex-direction: row;
  align-items: end;
  justify-content: space-between;

  label {
    color: ${props => props.theme.bcTokens.surfaceColorPrimaryButtonDefault};
    font-family: 'BCSans-Bold';
    font-size: 2rem;
    width: 100%;
    text-align: left;
    margin-bottom: 0;
  }
`;
