import React, { useMemo } from 'react';

import { Section } from '@/components/common/Section/Section';
import { Api_Insurance } from '@/models/api/Insurance';
import { ILookupCode } from '@/store/slices/lookupCodes';

import Policy from './Policy';
import { InsuranceTypeList } from './styles';

export interface InsuranceDetailsViewProps {
  insuranceList: Api_Insurance[];
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
    <div data-testid="insurance-section">
      <Section header="Required insurance">
        <InsuranceTypeList>
          {sortedInsuranceList.map((insurance: Api_Insurance, index: number) => (
            <li key={`insurance-section-${insurance?.id?.toString() ?? index}`}>
              {insurance.insuranceType.description}
              {insurance.insuranceType.id === 'OTHER' && insurance.otherInsuranceType
                ? `: ${insurance.otherInsuranceType}`
                : ''}
            </li>
          ))}
        </InsuranceTypeList>
      </Section>

      {sortedInsuranceList.map((insurance: Api_Insurance, index: number) => (
        <div
          key={`insurance-${insurance?.id?.toString() ?? index}`}
          data-testid="insurance-section"
        >
          <Policy insurance={insurance} />
        </div>
      ))}
    </div>
  ) : (
    <Section>
      <p>There are no insurance policies indicated with this lease/license</p>
    </Section>
  );
};

export default InsuranceDetailsView;
