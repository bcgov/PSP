import clsx from 'classnames';
import { Form } from 'components/common/form';
import { InlineFlexDiv } from 'components/common/styles';
import * as React from 'react';
import styled from 'styled-components';
import { prettyFormatDate } from 'utils';

import * as Styled from '../styles';

export interface IDetailTermInformationBoxProps {
  title: string;
  inverted?: boolean;
  startDate?: string;
  expiryDate?: string;
}

/**
 * Display the start and expiry date of the term reprented by the passed title.
 * @param {IDetailTermInformationBoxProps} param0
 */
export const DetailTermInformationBox: React.FunctionComponent<IDetailTermInformationBoxProps> = ({
  title,
  inverted,
  startDate,
  expiryDate,
}) => {
  const formattedStartDate = prettyFormatDate(startDate);
  const formattedExpiryDate = prettyFormatDate(expiryDate);
  return (
    <StyledTermInformationBox className={clsx({ inverted: inverted })}>
      <Styled.LeaseH4>{title}</Styled.LeaseH4>
      <FullWidthInlineFlexDiv>
        <Form.Label>Start date:</Form.Label>
        <StyledDate>{formattedStartDate}</StyledDate>
      </FullWidthInlineFlexDiv>
      <FullWidthInlineFlexDiv>
        <Form.Label>Expiry:</Form.Label>
        <StyledDate>{formattedExpiryDate}</StyledDate>
      </FullWidthInlineFlexDiv>
    </StyledTermInformationBox>
  );
};

const StyledDate = styled.div`
  justify-self: flex-end;
  font-weight: 700;
`;

const StyledTermInformationBox = styled.div`
  width: 25.5rem;
  background-color: ${props => props.theme.css.primaryColor};
  color: white;
  padding: 1rem 2rem;
  border-radius: 0.4rem;
`;

const FullWidthInlineFlexDiv = styled(InlineFlexDiv)`
  justify-content: space-between;
`;

export default DetailTermInformationBox;
