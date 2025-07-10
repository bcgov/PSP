import { FieldArray, Form, Formik, getIn } from 'formik';
import noop from 'lodash/noop';
import moment from 'moment';
import { Fragment } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Input } from '@/components/common/form';
import { FormSection } from '@/components/common/form/styles';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import {
  InlineMessage,
  StyledInlineMessageSection,
} from '@/components/common/Section/SectionStyles';
import {
  ChargesOnStrataCommonProperty,
  LegalNotationsOnStrataCommonProperty,
  OrderParent,
  SpcpOrder,
} from '@/interfaces/ltsaModels';
import { exists } from '@/utils';
import { prettyFormatDate } from '@/utils/dateUtils';
import { withNameSpace } from '@/utils/formUtils';

import LtsaChargeOwnerSubForm from './LtsaChargeOwnerSubForm';

export interface ILtsaPlanTabViewProps {
  spcpData?: SpcpOrder;
  ltsaRequestedOn?: Date;
  loading: boolean;
  planNumber?: string;
}

export const LtsaPlanTabView: React.FunctionComponent<
  React.PropsWithChildren<ILtsaPlanTabViewProps>
> = ({ spcpData, ltsaRequestedOn, loading, planNumber }) => {
  const titleNameSpace = 'orderedProduct.fieldedData';

  const charges: ChargesOnStrataCommonProperty[] =
    getIn(spcpData, withNameSpace(titleNameSpace, 'chargesOnSCP')) ?? [];
  const legalNotations: LegalNotationsOnStrataCommonProperty[] =
    getIn(spcpData, withNameSpace(titleNameSpace, 'legalNotationsOnSCP')) ?? [];

  return (
    <>
      <LoadingBackdrop show={loading} parentScreen={true} />

      {!loading && exists(planNumber) && !spcpData ? (
        <FormSection>
          <b>
            Failed to load data from LTSA.
            <br />
            <br /> Refresh this page to try again, or select a different property. If this error
            persists, contact an administrator.
          </b>
        </FormSection>
      ) : (
        <Formik
          initialValues={spcpData ?? defaultLtsaSPCPData}
          onSubmit={noop}
          enableReinitialize={true}
        >
          <StyledForm>
            {exists(ltsaRequestedOn) && (
              <StyledInlineMessageSection>
                <InlineMessage>
                  This data was retrieved from LTSA on{' '}
                  {moment(ltsaRequestedOn).format('DD-MMM-YYYY h:mm A')}
                </InlineMessage>
              </StyledInlineMessageSection>
            )}

            <Section header="Strata Plan Common Property Details">
              <SectionField label="Strata plan number">
                <Input
                  disabled
                  field={withNameSpace(titleNameSpace, 'strataPlanIdentifier.strataPlanNumber')}
                />
              </SectionField>
              <SectionField label="Land title district">
                <Input
                  disabled
                  field={withNameSpace(titleNameSpace, 'strataPlanIdentifier.landTitleDistrict')}
                />
              </SectionField>
            </Section>

            <Section header="Legal Notations on Strata Common Property">
              <div>
                {legalNotations.length === 0 && 'None'}

                <FieldArray
                  name={withNameSpace(titleNameSpace, 'legalNotations')}
                  render={() => (
                    <Fragment key={`legal-notation-${titleNameSpace}`}>
                      {legalNotations.map(
                        (notation: LegalNotationsOnStrataCommonProperty, index: number) => {
                          const innerNameSpace = withNameSpace(
                            titleNameSpace,
                            `legalNotationsOnSCP.${index}`,
                          );

                          return (
                            <Fragment key={`legal-notation-sub-row-${innerNameSpace}`}>
                              <Row className="pb-2">
                                <Col>
                                  <SectionField
                                    label="Legal notations#"
                                    labelWidth={{ xs: 'auto' }}
                                  >
                                    {notation.legalNotationNumber}
                                  </SectionField>
                                </Col>
                                <Col>
                                  <SectionField label="Status" labelWidth={{ xs: 'auto' }}>
                                    {notation.status}
                                  </SectionField>
                                </Col>
                                <Col>
                                  <SectionField
                                    label="Cancellation date"
                                    labelWidth={{ xs: 'auto' }}
                                  >
                                    {prettyFormatDate(
                                      notation.legalNotation.applicationReceivedDate,
                                    )}
                                  </SectionField>
                                </Col>
                              </Row>
                              <SectionField label="Legal notations">
                                <p>{notation.legalNotation.legalNotationText}</p>
                              </SectionField>

                              {index < legalNotations.length - 1 && <hr></hr>}
                            </Fragment>
                          );
                        },
                      )}
                    </Fragment>
                  )}
                />
              </div>
            </Section>

            <Section header="Charges on Strata Common Property" isCollapsable initiallyExpanded>
              <div>
                {charges.length === 0 && 'None'}

                <FieldArray
                  name={withNameSpace(titleNameSpace, 'chargesOnTitle')}
                  render={() => (
                    <Fragment key={`charge-row-${titleNameSpace}`}>
                      {charges.map((charge: ChargesOnStrataCommonProperty, index: number) => {
                        const innerNameSpace = withNameSpace(titleNameSpace, `chargesOnSCP`);
                        const ownerNamespace = withNameSpace(
                          titleNameSpace,
                          `chargesOnSCP.${index}.charge`,
                        );
                        return (
                          <Fragment key={`charge-sub-row-${innerNameSpace}-${index}`}>
                            <SectionField label="Registration #">
                              {charge.chargeNumber}
                            </SectionField>
                            <SectionField label="Nature">
                              {charge.charge.transactionType}
                            </SectionField>
                            <SectionField label="Registered date">
                              {prettyFormatDate(charge.charge.applicationReceivedDate)}
                            </SectionField>

                            <LtsaChargeOwnerSubForm nameSpace={ownerNamespace} />

                            {index < charges.length - 1 && <hr></hr>}
                          </Fragment>
                        );
                      })}
                    </Fragment>
                  )}
                />
              </div>
            </Section>
          </StyledForm>
        </Formik>
      )}
    </>
  );
};

export const StyledForm = styled(Form)`
  position: relative;
  &&& {
    input,
    select,
    textarea {
      background: none;
      border: none;
      resize: none;
      height: fit-content;
      padding: 0;
    }
    .form-label {
      font-weight: bold;
    }
  }
`;

const defaultLtsaSPCPData: SpcpOrder = {
  fileReference: 'folio',
  productOrderParameters: {
    strataPlanNumber: '',
    includeCancelledInfo: false,
  },
  orderId: '',
  status: OrderParent.StatusEnum.Processing,
  orderedProduct: {
    fieldedData: {
      strataPlanIdentifier: {
        strataPlanNumber: '',
        landTitleDistrict: '',
      },
      legalNotationsOnSCP: [],
      chargesOnSCP: [],
    },
  },
  productType: OrderParent.ProductTypeEnum.CommonProperty,
};

export default LtsaPlanTabView;
