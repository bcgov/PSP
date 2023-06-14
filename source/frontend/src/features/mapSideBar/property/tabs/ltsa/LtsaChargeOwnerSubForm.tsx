import { getIn, useFormikContext } from 'formik';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

import { Input } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import {
  ChargeOwnershipGroup,
  ChargeOwnershipGroupChargeOwner,
  LtsaOrders,
} from '@/interfaces/ltsaModels';
import { withNameSpace } from '@/utils/formUtils';
export interface ILtsaChargeOwnerSubFormProps {
  nameSpace?: string;
}

export const LtsaChargeOwnerSubForm: React.FunctionComponent<
  React.PropsWithChildren<ILtsaChargeOwnerSubFormProps>
> = ({ nameSpace }) => {
  const { values } = useFormikContext<LtsaOrders>();
  const chargeOwnershipGroups: ChargeOwnershipGroup[] =
    getIn(values, withNameSpace(nameSpace, 'chargeOwnershipGroups')) ?? [];
  return (
    <>
      {chargeOwnershipGroups.map((chargeOwnershipGroup: ChargeOwnershipGroup, cogIndex: number) => {
        const chargeOwners: ChargeOwnershipGroupChargeOwner[] =
          getIn(chargeOwnershipGroup, `chargeOwners`) ?? [];
        return chargeOwners.map((chargeOwner: ChargeOwnershipGroupChargeOwner, coIndex: number) => {
          const innerNameSpace = withNameSpace(
            nameSpace,
            `chargeOwnershipGroups.${cogIndex}.chargeOwners.${coIndex}`,
          );
          return (
            <React.Fragment key={`charge-owner-sub-row-${innerNameSpace}`}>
              {getIn(values, withNameSpace(innerNameSpace, 'lastNameOrCorpName1')) && (
                <SectionField label="Registered owner">
                  <Input field={`${withNameSpace(innerNameSpace, 'lastNameOrCorpName1')}`} />
                </SectionField>
              )}
              {getIn(values, withNameSpace(innerNameSpace, 'lastNameOrCorpName2')) && (
                <Row>
                  <Col xs={4}></Col>
                  <Col>
                    <Input field={`${withNameSpace(innerNameSpace, 'lastNameOrCorpName2')}`} />
                  </Col>
                </Row>
              )}
              {getIn(values, withNameSpace(innerNameSpace, 'givenName')) && (
                <Row>
                  <Col xs={4}></Col>
                  <Col>
                    <Input field={`${withNameSpace(innerNameSpace, 'givenName')}`} />
                  </Col>
                </Row>
              )}
              {getIn(values, withNameSpace(innerNameSpace, 'incorporationNumber')) && (
                <Row>
                  <Col xs={4}></Col>
                  <Col>
                    <Input field={`${withNameSpace(innerNameSpace, 'incorporationNumber')}`} />
                  </Col>
                </Row>
              )}
              {chargeOwnershipGroup.jointTenancyIndication && (
                <Row>
                  <Col xs={4}></Col>
                  <Col>
                    {chargeOwnershipGroup.jointTenancyIndication && (
                      <p>
                        Joint tenancy {chargeOwnershipGroup.interestFractionNumerator}/
                        {chargeOwnershipGroup.interestFractionDenominator}
                      </p>
                    )}
                  </Col>
                </Row>
              )}
              {getIn(
                values,
                withNameSpace(nameSpace, `chargeOwnershipGroups.${cogIndex}.ownershipRemarks`),
              ) && (
                <Row>
                  <Col xs={4}></Col>
                  <Col>
                    <Input
                      field={`${withNameSpace(
                        nameSpace,
                        `chargeOwnershipGroups.${cogIndex}.ownershipRemarks`,
                      )}`}
                    />
                  </Col>
                </Row>
              )}
            </React.Fragment>
          );
        });
      })}
    </>
  );
};

export default LtsaChargeOwnerSubForm;
