import { Formik } from 'formik';
import noop from 'lodash/noop';
import styled from 'styled-components';

import { FormSection } from '@/components/common/form/styles';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { LeaseFormModel } from '@/features/leases/models';
import { ApiGen_Concepts_LeaseStakeholderType } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholderType';
import { withNameSpace } from '@/utils/formUtils';

import { FormStakeholder } from './models';
import TenantOrganizationContactInfo from './StakeholderOrganizationContactInfo';
import TenantPersonContactInfo from './StakeholderPersonContactInfo';

export interface ITenantProps {
  nameSpace?: string;
  stakeholders: FormStakeholder[];
  leaseStakeholderTypes?: ApiGen_Concepts_LeaseStakeholderType[];
  loading?: boolean;
  isPayableLease?: boolean;
}

/**
 * Tenant lease page displays all tenant information (persons and organizations)
 * @param {ITenantProps} param0
 */
export const ViewStakeholderForm: React.FunctionComponent<
  React.PropsWithChildren<ITenantProps>
> = ({ nameSpace, stakeholders, loading, leaseStakeholderTypes, isPayableLease }) => {
  return (
    <FormSectionOne>
      <Formik
        initialValues={{ ...new LeaseFormModel(), stakeholders }}
        onSubmit={noop}
        enableReinitialize
      >
        <>
          <LoadingBackdrop show={loading} parentScreen />

          {leaseStakeholderTypes
            .filter(stakeholderType => stakeholderType.isPayableRelated === isPayableLease)
            .map((stakeholderTypeCode: ApiGen_Concepts_LeaseStakeholderType) => {
              return (
                <Section
                  header={stakeholderTypeCode.description}
                  key={`stakeholder-type-${stakeholderTypeCode.code}`}
                >
                  {stakeholders.map((stakeholder: FormStakeholder, index) =>
                    stakeholder.stakeholderType === stakeholderTypeCode.code ? (
                      <div key={`stakeholders-${index}`}>
                        <>
                          {stakeholder.organizationId ? (
                            <TenantOrganizationContactInfo
                              disabled={true}
                              nameSpace={withNameSpace(nameSpace, `stakeholders.${index}`)}
                            />
                          ) : (
                            <TenantPersonContactInfo
                              disabled={true}
                              nameSpace={withNameSpace(nameSpace, `stakeholders.${index}`)}
                            />
                          )}
                        </>
                      </div>
                    ) : null,
                  )}
                </Section>
              );
            })}
          {stakeholders.length === 0 && (
            <StyledSection>
              <p>There are no stakeholders associated to this lease.</p>
              <p>Click the edit icon to add stakeholders.</p>
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
    color: ${props => props.theme.bcTokens.typographyColorPrimary};
  }
`;

const StyledSection = styled(Section)`
  background-color: transparent;
  padding-left: 0.5rem;
  margin-bottom: 0;
`;

export default ViewStakeholderForm;
