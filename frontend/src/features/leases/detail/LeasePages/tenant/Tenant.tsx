import { FormSection } from 'components/common/form/styles';
import * as Styled from 'features/leases/detail/styles';
import { FieldArray, getIn, useFormikContext } from 'formik';
import { ILease, IOrganization, IPerson } from 'interfaces';
import * as React from 'react';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';

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
    <StyledDetails>
      <TenantsFieldArray
        name={withNameSpace(nameSpace, 'properties')}
        render={renderProps => (
          <>
            {persons.map((person: IPerson, index) => (
              <Styled.SpacedInlineListItem>
                <FormSection>
                  <TenantPersonContactInfo
                    disabled={true}
                    nameSpace={withNameSpace(nameSpace, `persons.${index}`)}
                  />
                </FormSection>
              </Styled.SpacedInlineListItem>
            ))}
            {organizations.map((organization: IOrganization, index) => (
              <Styled.SpacedInlineListItem>
                <FormSection>
                  <TenantOrganizationContactInfo
                    disabled={true}
                    nameSpace={withNameSpace(nameSpace, `organizations.${index}`)}
                  />
                </FormSection>
              </Styled.SpacedInlineListItem>
            ))}
          </>
        )}
      />
    </StyledDetails>
  );
};

export const TenantsFieldArray = styled(FieldArray)`
  column-count: 2;
  & > * {
    break-inside: avoid-column;
  }
  column-gap: 10rem;
  li {
    list-style-type: none;
    padding: 2rem 0;
    margin: 0;
  }
  @media only screen and (max-width: 1500px) {
    column-count: 1;
  }
`;

const StyledDetails = styled.div`
  display: flex;
  flex-direction: column;
  width: 100%;
  gap: 2.5rem;
`;

export default Tenant;
