import { Formik } from 'formik';
import styled from 'styled-components';

import { FormSection } from '@/components/common/form/styles';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { LeaseFormModel } from '@/features/leases/models';
import { withNameSpace } from '@/utils/formUtils';

import { FormTenant } from './models';
import TenantOrganizationContactInfo from './TenantOrganizationContactInfo';
import TenantPersonContactInfo from './TenantPersonContactInfo';
import noop from 'lodash/noop';

export interface ITenantProps {
  nameSpace?: string;
  tenants: FormTenant[];
  loading?: boolean;
}

/**
 * Tenant lease page displays all tenant information (persons and organizations)
 * @param {ITenantProps} param0
 */
export const ViewTenantForm: React.FunctionComponent<React.PropsWithChildren<ITenantProps>> = ({
  nameSpace,
  tenants,
  loading,
}) => {
  return (
    <FormSectionOne>
      <Formik
        initialValues={{ ...new LeaseFormModel(), tenants: tenants }}
        onSubmit={noop}
        enableReinitialize
      >
        <>
          <LoadingBackdrop show={loading} parentScreen />
          <Section header="Assignee">
            {tenants.map(
              (tenant: FormTenant, index) =>
                tenant.tenantType === 'ASGN' && (
                  <div key={`tenants-${index}`}>
                    <>
                      {tenant.organizationId ? (
                        <TenantOrganizationContactInfo
                          disabled={true}
                          nameSpace={withNameSpace(nameSpace, `tenants.${index}`)}
                        />
                      ) : (
                        <TenantPersonContactInfo
                          disabled={true}
                          nameSpace={withNameSpace(nameSpace, `tenants.${index}`)}
                        />
                      )}
                    </>
                  </div>
                ),
            )}
          </Section>

          <Section header="Tenant">
            {tenants.map(
              (tenant: FormTenant, index) =>
                tenant.tenantType === 'TEN' && (
                  <div key={`tenants-${index}`}>
                    <>
                      {tenant.organizationId ? (
                        <TenantOrganizationContactInfo
                          disabled={true}
                          nameSpace={withNameSpace(nameSpace, `tenants.${index}`)}
                        />
                      ) : (
                        <TenantPersonContactInfo
                          disabled={true}
                          nameSpace={withNameSpace(nameSpace, `tenants.${index}`)}
                        />
                      )}
                    </>
                  </div>
                ),
            )}
          </Section>

          <Section header="Representative">
            {tenants.map(
              (tenant: FormTenant, index) =>
                tenant.tenantType === 'REP' && (
                  <div key={`tenants-${index}`}>
                    <>
                      {tenant.lessorTypeCode?.id === 'ORG' ? (
                        <TenantOrganizationContactInfo
                          disabled={true}
                          nameSpace={withNameSpace(nameSpace, `tenants.${index}`)}
                        />
                      ) : (
                        <TenantPersonContactInfo
                          disabled={true}
                          nameSpace={withNameSpace(nameSpace, `tenants.${index}`)}
                        />
                      )}
                    </>
                  </div>
                ),
            )}
          </Section>

          <Section header="Property Manager">
            {tenants.map(
              (tenant: FormTenant, index) =>
                tenant.tenantType === 'PMGR' && (
                  <div key={`tenants-${index}`}>
                    <>
                      {tenant.lessorTypeCode?.id === 'ORG' ? (
                        <TenantOrganizationContactInfo
                          disabled={true}
                          nameSpace={withNameSpace(nameSpace, `tenants.${index}`)}
                        />
                      ) : (
                        <TenantPersonContactInfo
                          disabled={true}
                          nameSpace={withNameSpace(nameSpace, `tenants.${index}`)}
                        />
                      )}
                    </>
                  </div>
                ),
            )}
          </Section>
          <Section header="Unknown">
            {tenants.map(
              (tenant: FormTenant, index) =>
                tenant.tenantType === 'UNK' && (
                  <div key={`tenants-${index}`}>
                    <>
                      {tenant.lessorTypeCode?.id === 'ORG' ? (
                        <TenantOrganizationContactInfo
                          disabled={true}
                          nameSpace={withNameSpace(nameSpace, `tenants.${index}`)}
                        />
                      ) : (
                        <TenantPersonContactInfo
                          disabled={true}
                          nameSpace={withNameSpace(nameSpace, `tenants.${index}`)}
                        />
                      )}
                    </>
                  </div>
                ),
            )}
          </Section>
          {tenants.length === 0 && (
            <StyledSection>
              <p>There are no tenants associated to this lease.</p>
              <p>Click the edit icon to add tenants.</p>
            </StyledSection>
          )}
        </>
      </Formik>
    </FormSectionOne>
  );
};

export const FormSectionOne = styled(FormSection)`
  padding: 0;
  column-count: 1;
  & > * {
    break-inside: avoid-column;
  }
  column-gap: 10rem;
  background-color: light-gray;
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

const StyledSection = styled(Section)`
  background-color: transparent;
  padding-left: 0.5rem;
  margin-bottom: 0;
`;

export default ViewTenantForm;
