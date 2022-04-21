import { Input } from 'components/common/form';
import { FieldArray, getIn, useFormikContext } from 'formik';
import { ChargeOnTitle, LtsaOrders } from 'interfaces/ltsaModels';
import * as React from 'react';
import Collapse from 'react-bootstrap/Collapse';
import { withNameSpace } from 'utils/formUtils';

import { SectionField } from '../SectionField';
import { ArrowDropDownIcon, ArrowDropUpIcon, StyledSectionHeader } from '../SectionStyles';
import LtsaChargeOwnerSubForm from './LtsaChargeOwnerSubForm';

export interface ILtsaChargeSubFormProps {
  nameSpace?: string;
}

export const LtsaChargeSubForm: React.FunctionComponent<ILtsaChargeSubFormProps> = ({
  nameSpace,
}) => {
  const { values } = useFormikContext<LtsaOrders>();
  const charges = getIn(values, withNameSpace(nameSpace, 'chargesOnTitle')) ?? [];
  const [isExpanded, setIsExpanded] = React.useState<boolean>(true);

  return (
    <React.Fragment key={`charge-sub-row-${nameSpace}`}>
      <StyledSectionHeader>
        Charges, Liens and Interests
        {!isExpanded && (
          <ArrowDropDownIcon
            onClick={() => {
              setIsExpanded(!isExpanded);
            }}
          />
        )}
        {isExpanded && (
          <ArrowDropUpIcon
            onClick={() => {
              setIsExpanded(!isExpanded);
            }}
          />
        )}
      </StyledSectionHeader>
      <Collapse in={isExpanded}>
        <div>
          {charges.length === 0 && 'this title has no charges'}
          <FieldArray
            name={withNameSpace(nameSpace, 'chargesOnTitle')}
            render={({ push, remove, name }) => (
              <React.Fragment key={`charge-sub-row-${nameSpace}`}>
                {charges.map((charge: ChargeOnTitle, index: number) => {
                  const innerNameSpace = withNameSpace(nameSpace, `chargesOnTitle.${index}.charge`);
                  return (
                    <React.Fragment key={`charge-sub-row-${innerNameSpace}`}>
                      <SectionField label="Nature">
                        <Input field={`${withNameSpace(innerNameSpace, 'transactionType')}`} />
                      </SectionField>
                      <SectionField label="Registration #">
                        <Input field={`${withNameSpace(innerNameSpace, 'chargeNumber')}`} />
                      </SectionField>
                      <SectionField label="Registered date">
                        <Input
                          field={`${withNameSpace(innerNameSpace, 'applicationReceivedDate')}`}
                        />
                      </SectionField>
                      <LtsaChargeOwnerSubForm nameSpace={innerNameSpace} />
                      {index < charges.length - 1 && <hr></hr>}
                    </React.Fragment>
                  );
                })}
              </React.Fragment>
            )}
          />
        </div>
      </Collapse>
    </React.Fragment>
  );
};

export default LtsaChargeSubForm;
