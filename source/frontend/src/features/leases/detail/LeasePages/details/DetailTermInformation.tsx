import * as Styled from 'features/leases/detail/styles';
import { getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import { ILeaseTerm } from 'interfaces/ILeaseTerm';
import moment from 'moment';
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
export const DetailTermInformation: React.FunctionComponent<
  React.PropsWithChildren<IDetailTermInformationProps>
> = ({ nameSpace }) => {
  const { values } = useFormikContext<IFormLease>();
  const startDate = getIn(values, withNameSpace(nameSpace, 'startDate'));
  const expiryDate = getIn(values, withNameSpace(nameSpace, 'expiryDate'));
  const terms = getIn(values, withNameSpace(nameSpace, 'terms'));
  const currentTerm = terms.find((term: ILeaseTerm) =>
    moment().isSameOrBefore(moment(term.expiryDate), 'day'),
  );
  return (
    <Styled.SpacedInlineListItem>
      <DetailTermInformationBox
        title="Lease / License"
        startDate={startDate}
        expiryDate={expiryDate}
      />
      <DetailTermInformationBox
        title="Current Term"
        startDate={currentTerm?.startDate}
        expiryDate={currentTerm?.expiryDate}
        inverted
      />
    </Styled.SpacedInlineListItem>
  );
};

export default DetailTermInformation;
