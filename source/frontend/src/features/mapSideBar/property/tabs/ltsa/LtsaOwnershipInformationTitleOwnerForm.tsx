import { FieldArray, getIn, useFormikContext } from 'formik';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Input } from '@/components/common/form';
import { SectionField, StyledFieldLabel } from '@/components/common/Section/SectionField';
import { LtsaOrders, TitleOwner } from '@/interfaces/ltsaModels';
import { withNameSpace } from '@/utils/formUtils';

export interface ILtsaOwnershipInformationTitleOwnerFormProps {
  nameSpace?: string;
}

export const LtsaOwnershipInformationTitleOwnerForm: React.FunctionComponent<
  React.PropsWithChildren<ILtsaOwnershipInformationTitleOwnerFormProps>
> = ({ nameSpace }) => {
  const { values } = useFormikContext<LtsaOrders>();
  const titleOwners = getIn(values, withNameSpace(nameSpace, 'titleOwners')) ?? [];

  const getJoinedFieldValues = (input1?: string, input2?: string): string => {
    const retVal: string[] = [];
    if (input1) retVal.push(input1);
    if (input2) retVal.push(input2);
    if (retVal.length > 1) return retVal.join(', ');

    return retVal.join();
  };

  return (
    <React.Fragment key={`title-owners-info-main-row-${nameSpace}`}>
      <FieldArray
        name={withNameSpace(nameSpace, 'titleOwners')}
        render={() => (
          <React.Fragment key={`title-owner-info-row-${nameSpace}`}>
            {titleOwners.map((titleOwner: TitleOwner, index: number) => {
              const innerNameSpace = withNameSpace(nameSpace, `titleOwners.${index}`);
              const ownerName = [
                titleOwner.givenName ?? '',
                titleOwner.lastNameOrCorpName1 ?? '',
                titleOwner.lastNameOrCorpName2 ?? '',
              ];
              return (
                <React.Fragment key={`title-owner-info-sub-row-${innerNameSpace}`}>
                  <OwnershipTitleInfo>
                    <SectionField label="Owner name">
                      <p>{ownerName.join(' ')}</p>
                    </SectionField>
                    <SectionField label="Incorporation number">
                      <Input field={`${withNameSpace(innerNameSpace, 'incorporationNumber')}`} />
                    </SectionField>
                    <SectionField label="Occupation">
                      <Input field={`${withNameSpace(innerNameSpace, 'occupationDescription')}`} />
                    </SectionField>
                    <Row>
                      <Col xs={4}>
                        <StyledFieldLabel>Address:</StyledFieldLabel>
                      </Col>
                      <Col>
                        {getIn(values, withNameSpace(innerNameSpace, 'address.addressLine1')) && (
                          <Row>
                            <Col>
                              <Input
                                field={`${withNameSpace(innerNameSpace, 'address.addressLine1')}`}
                              />
                            </Col>
                          </Row>
                        )}
                        {getIn(values, withNameSpace(innerNameSpace, 'address.addressLine2')) && (
                          <Row>
                            <Col>
                              <Input
                                field={`${withNameSpace(innerNameSpace, 'address.addressLine2')}`}
                              />
                            </Col>
                          </Row>
                        )}
                        {(getIn(values, withNameSpace(innerNameSpace, 'address.city')) ||
                          getIn(values, withNameSpace(innerNameSpace, 'address.provinceName'))) && (
                          <Row>
                            <Col>
                              {getJoinedFieldValues(
                                titleOwner?.address?.city,
                                titleOwner?.address?.provinceName as any,
                              )}
                            </Col>
                          </Row>
                        )}
                        {(getIn(values, withNameSpace(innerNameSpace, 'address.country')) ||
                          getIn(values, withNameSpace(innerNameSpace, 'address.postalCode'))) && (
                          <Row>
                            <Col>
                              {getJoinedFieldValues(
                                titleOwner?.address?.country,
                                titleOwner?.address?.postalCode,
                              )}
                            </Col>
                          </Row>
                        )}
                      </Col>
                    </Row>
                  </OwnershipTitleInfo>
                  {index < titleOwners.length - 1 && <hr></hr>}
                </React.Fragment>
              );
            })}
          </React.Fragment>
        )}
      />
    </React.Fragment>
  );
};

export default LtsaOwnershipInformationTitleOwnerForm;

const OwnershipTitleInfo = styled.div`
  padding: 1rem 2rem;
`;
