import * as Styled from 'features/leases/detail/styles';
import { getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

import { DetailTermInformationBox } from './DetailTermInformationBox';

export interface IDetailTermInformationProps {
  nameSpace?: string;
}

/**
 * Sub-form displaying the original and renewal term information presented in styled boxes.
 * @param {IDetailTermInformationProps} param0
 */
export const DetailTermInformation: React.FunctionComponent<IDetailTermInformationProps> = ({
  nameSpace,
}) => {
  const { values } = useFormikContext<IFormLease>();
  const startDate = getIn(values, withNameSpace(nameSpace, 'startDate'));
  const expiryDate = getIn(values, withNameSpace(nameSpace, 'expiryDate'));
  return (
    <Styled.SpacedInlineListItem>
      <DetailTermInformationBox
        title="Lease / License"
        startDate={startDate}
        expiryDate={expiryDate}
      />
    </Styled.SpacedInlineListItem>
  );
};

export default DetailTermInformation;
