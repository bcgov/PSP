import clsx from 'classnames';
import styled from 'styled-components';

import { Form } from '@/components/common/form';
import { InlineFlexDiv } from '@/components/common/styles';
import * as Styled from '@/features/leases/detail/styles';
import { prettyFormatDate } from '@/utils';

export interface IDetailPeriodInformationBoxProps {
  title: string;
  inverted?: boolean;
  startDate?: string;
  expiryDate?: string;
}

/**
 * Display the start and expiry date of the period reprented by the passed title.
 * @param {IDetailPeriodInformationBoxProps} param0
 */
export const DetailPeriodInformationBox: React.FunctionComponent<
  React.PropsWithChildren<IDetailPeriodInformationBoxProps>
> = ({ title, inverted, startDate, expiryDate }) => {
  const formattedStartDate = prettyFormatDate(startDate);
  const formattedExpiryDate = prettyFormatDate(expiryDate);
  return (
    <StyledPeriodInformationBox className={clsx({ inverted: inverted })}>
      <Styled.LeaseH4>{title}</Styled.LeaseH4>
      <FullWidthInlineFlexDiv>
        <Form.Label>Start date:</Form.Label>
        <StyledDate>{formattedStartDate}</StyledDate>
      </FullWidthInlineFlexDiv>
      <FullWidthInlineFlexDiv>
        <Form.Label>Expiry:</Form.Label>
        <StyledDate>{formattedExpiryDate}</StyledDate>
      </FullWidthInlineFlexDiv>
    </StyledPeriodInformationBox>
  );
};

const StyledDate = styled.div`
  justify-self: flex-end;
  font-weight: 700;
`;

const StyledPeriodInformationBox = styled.div`
  width: 25.5rem;
  background: rgba(0, 33, 66, 0.73);
  color: white;
  padding: 1rem 2rem;
  border-radius: 0.4rem;
  &.inverted {
    background: white;
    color: ${props => props.theme.css.headerTextColor};
    border: 0.1rem solid;
    h4 {
      font-weight: 700;
      color: ${props => props.theme.css.headerTextColor};
    }
  }
`;

const FullWidthInlineFlexDiv = styled(InlineFlexDiv)`
  justify-content: space-between;
`;

export default DetailPeriodInformationBox;
