import { Formik } from 'formik';
import noop from 'lodash/noop';
import styled from 'styled-components';

import { FormSection } from '@/components/common/form/styles';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import * as API from '@/constants/API';
import { LeaseFormModel } from '@/features/leases/models';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { withNameSpace } from '@/utils/formUtils';
import { FormStakeholder } from '../stakeholders/models';
import TenantOrganizationContactInfo from '../stakeholders/StakeholderOrganizationContactInfo';
import TenantPersonContactInfo from '../stakeholders/StakeholderPersonContactInfo';

export interface ITenantProps {
  nameSpace?: string;
  stakeholders: FormStakeholder[];
  loading?: boolean;
}

/**
 * Tenant lease page displays all tenant information (persons and organizations)
 * @param {ITenantProps} param0
 */
export const ViewTenantForm: React.FunctionComponent<React.PropsWithChildren<ITenantProps>> = ({
  nameSpace,
  stakeholders,
  loading,
}) => {
  const { getPublicByType } = useLookupCodeHelpers();
  const stakeholderTypeCodes = getPublicByType(API.STAKEHOLDER_TYPES);
  return (
    <FormSectionOne>
      <Formik
        initialValues={{ ...new LeaseFormModel(), stakeholders }}
        onSubmit={noop}
        enableReinitialize
      >
        <>
          <LoadingBackdrop show={loading} parentScreen />

          {stakeholderTypeCodes.map((stakeholderTypeCode: ILookupCode) => {
            if (
              stakeholders.filter(
                stakeholder => stakeholder.stakeholderType === stakeholderTypeCode.id,
              ).length > 0
            ) {
              return (
                <Section
                  header={stakeholderTypeCode.name}
                  key={`stakeholder-type-${stakeholderTypeCode.id}`}
                >
                  {stakeholders.map(
                    (stakeholder: FormStakeholder, index) =>
                      stakeholder.stakeholderType === stakeholderTypeCode.id && (
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
                      ),
                  )}
                </Section>
              );
            }
            return null;
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

export default ViewTenantForm;
