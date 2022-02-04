import { FormSection } from 'components/common/form/styles';
import OverflowTip from 'components/common/OverflowTip';
import * as Styled from 'features/leases/detail/styles';
import { FieldArray, getIn, useFormikContext } from 'formik';
import { ILease, IOrganization, IPerson, ITenant } from 'interfaces';
import { find, findIndex } from 'lodash';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';

import TenantNotes from './TenantNotes';
import TenantOrganizationContactInfo from './TenantOrganizationContactInfo';
import TenantPersonContactInfo from './TenantPersonContactInfo';

export interface ITenantProps {
  nameSpace?: string;
}

/**
 * Tenant lease page displays all tenant information (persons and organizations)
 * @param {ITenantProps} param0
 */
export const Tenant: React.FunctionComponent<ITenantProps> = ({ nameSpace }) => {
  const { values } = useFormikContext<ILease>();
  const persons: IPerson[] = getIn(values, withNameSpace(nameSpace, 'persons')) ?? [];
  const organizations: IOrganization[] =
    getIn(values, withNameSpace(nameSpace, 'organizations')) ?? [];

  return (
    <FormSectionOne>
      <FieldArray
        name={withNameSpace(nameSpace, 'properties')}
        render={renderProps => (
          <>
            {persons.map((person: IPerson, index) => (
              <Styled.SpacedInlineListItem key={`person-${index}`}>
                <FormSection>
                  <Row>
                    <Col>
                      <TenantPersonContactInfo
                        disabled={true}
                        nameSpace={withNameSpace(nameSpace, `persons.${index}`)}
                      />
                    </Col>
                    <Col>
                      <TenantNotes
                        disabled={true}
                        nameSpace={`tenants.${getTenantPersonIndex(values.tenants, person.id)}`}
                      />
                    </Col>
                  </Row>
                </FormSection>
              </Styled.SpacedInlineListItem>
            ))}
            {organizations.map((organization: IOrganization, index) => (
              <Styled.SpacedInlineListItem key={`organizations-${index}`}>
                <FormSection>
                  <Row>
                    <Col>
                      <TenantOrganizationContactInfo
                        disabled={true}
                        nameSpace={withNameSpace(nameSpace, `organizations.${index}`)}
                      />
                    </Col>
                    <Col>
                      <TenantNotes
                        disabled={true}
                        nameSpace={`tenants.${getTenantOrganizationIndex(
                          values.tenants,
                          organization.id,
                        )}`}
                      />
                    </Col>
                  </Row>
                </FormSection>
              </Styled.SpacedInlineListItem>
            ))}
            {persons.length === 0 && organizations.length === 0 && (
              <>
                <p>There are no tenants associated to this lease.</p>
                <p>Click the edit icon to add tenants.</p>
              </>
            )}
          </>
        )}
      />
    </FormSectionOne>
  );
};

const getTenantOrganizationIndex = (tenants: ITenant[], organizationId?: number) => {
  return findIndex(tenants, (tenant: ITenant) => tenant.organizationId === organizationId);
};

const getTenantPersonIndex = (tenants: ITenant[], personId?: number) => {
  return findIndex(tenants, (tenant: ITenant) => tenant.personId === personId);
};

export const FormSectionOne = styled(FormSection)`
  column-count: 1;
  & > * {
    break-inside: avoid-column;
  }
  column-gap: 10rem;
  background-color: white;
  li {
    list-style-type: none;
    padding: 2rem 0;
    margin: 0;
  }
  @media only screen and (max-width: 1500px) {
    column-count: 1;
  }
  min-width: 75rem;
  .form-control {
    color: ${props => props.theme.css.formTextColor};
  }
`;

export default Tenant;
