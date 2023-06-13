import { FieldArray, getIn, useFormikContext } from 'formik';
import * as React from 'react';
import styled from 'styled-components';

import { Input } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { ChargeOwnershipGroup, LtsaOrders } from '@/interfaces/ltsaModels';
import { withNameSpace } from '@/utils/formUtils';

import LtsaOwnershipInformationTitleOwnerForm from './LtsaOwnershipInformationTitleOwnerForm';

export interface ILtsaOwnershipInformationFormProps {
  nameSpace?: string;
}

export const LtsaOwnershipInformationForm: React.FunctionComponent<
  React.PropsWithChildren<ILtsaOwnershipInformationFormProps>
> = ({ nameSpace }) => {
  const { values } = useFormikContext<LtsaOrders>();
  const ownershipGroups = getIn(values, withNameSpace(nameSpace, 'ownershipGroups')) ?? [];
  return (
    <React.Fragment key={`ownership-info-main-row-${nameSpace}`}>
      {ownershipGroups.length === 0 && 'None'}
      <FieldArray
        name={withNameSpace(nameSpace, 'ownershipGroups')}
        render={({ push, remove, name }) => (
          <React.Fragment key={`ownership-info-row-${nameSpace}`}>
            {ownershipGroups.map((ownershipGroup: ChargeOwnershipGroup, index: number) => {
              const innerNameSpace = withNameSpace(nameSpace, `ownershipGroups.${index}`);
              return (
                <React.Fragment key={`ownership-info-sub-row-${innerNameSpace}`}>
                  <OwnershipInfoMain showBottomMargin={index < ownershipGroups.length - 1}>
                    <OwnershipSummary>
                      <OwnershipInfo>
                        <SectionField label="Fractional ownership">
                          <p>
                            {ownershipGroup.interestFractionNumerator}/
                            {ownershipGroup.interestFractionDenominator}
                          </p>
                        </SectionField>
                        <SectionField label="Joint tenancy">
                          <p>{ownershipGroup.jointTenancyIndication ? 'Yes' : 'No'}</p>
                        </SectionField>
                        <SectionField label="Ownership remarks">
                          <Input field={`${withNameSpace(innerNameSpace, 'ownershipRemarks')}`} />
                        </SectionField>
                      </OwnershipInfo>
                    </OwnershipSummary>
                    <LtsaOwnershipInformationTitleOwnerForm nameSpace={innerNameSpace} />
                  </OwnershipInfoMain>
                </React.Fragment>
              );
            })}
          </React.Fragment>
        )}
      />
    </React.Fragment>
  );
};

export default LtsaOwnershipInformationForm;

const OwnershipInfo = styled.div`
  padding: 1.5rem 2rem 0 2rem;
`;
const OwnershipSummary = styled.div`
  background-color: #f2f2f2;
`;

interface IOwnershipInfoMainProps {
  showBottomMargin: boolean;
}

const OwnershipInfoMain = styled.div<IOwnershipInfoMainProps>`
  border: 2px solid #909090;
  border-radius: 5px;
  margin-bottom: ${(props: any) => (props.showBottomMargin ? '1.5rem' : '0rem')};
`;
