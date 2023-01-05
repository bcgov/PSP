import { Section } from 'features/mapSideBar/tabs/Section';
import { IInsurance } from 'interfaces';
import React from 'react';
import { useMemo } from 'react';
import { ILookupCode } from 'store/slices/lookupCodes';
import styled from 'styled-components';

import Policy from './Policy';
import { InsuranceTypeList } from './styles';

export interface InsuranceDetailsViewProps {
  insuranceList: IInsurance[];
  insuranceTypes: ILookupCode[];
}

const InsuranceDetailsView: React.FunctionComponent<
  React.PropsWithChildren<InsuranceDetailsViewProps>
> = ({ insuranceList, insuranceTypes }) => {
  const sortedInsuranceList = useMemo(
    () =>
      !!insuranceList?.length
        ? insuranceList.sort((a, b) => {
            return (
              insuranceTypes.findIndex(i => i.id === a.insuranceType.displayOrder) -
              insuranceTypes.findIndex(i => i.id === b.insuranceType.displayOrder)
            );
          })
        : [],
    [insuranceList, insuranceTypes],
  );
  return !!sortedInsuranceList.length ? (
    <StyledDiv data-testid="insurance-section">
      <Section header="Required insurance">
        <InsuranceTypeList>
          {sortedInsuranceList.map((insurance: IInsurance, index: number) => (
            <li key={index + insurance.id}>
              {insurance.insuranceType.description}
              {insurance.insuranceType.id === 'OTHER' && insurance.otherInsuranceType
                ? `: ${insurance.otherInsuranceType}`
                : ''}
            </li>
          ))}
        </InsuranceTypeList>
      </Section>

      {sortedInsuranceList.map((insurance: IInsurance, index: number) => (
        <div key={index + insurance.id} data-testid="insurance-section">
          <Policy insurance={insurance} />
          <br />
        </div>
      ))}
    </StyledDiv>
  ) : (
    <p>There are no insurance policies indicated with this lease/license</p>
  );
};

export default InsuranceDetailsView;

const StyledDiv = styled.div`
  margin-top: 4rem;
`;
