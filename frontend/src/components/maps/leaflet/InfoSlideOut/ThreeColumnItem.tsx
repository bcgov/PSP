import { Label } from 'components/common/Label';
import * as React from 'react';
import Col from 'react-bootstrap/Col';
import ListGroup from 'react-bootstrap/ListGroup';
import Row from 'react-bootstrap/Row';
import styled from 'styled-components';

const InnerRow = styled(Row)`
  margin: 0rem;
  width: 30rem;
`;

const LeftCol = styled(Col)`
  width: 11rem;
  max-width: 13.5rem;
  padding-right: 1rem;
  padding-left: 0rem;
`;

const CenterCol = styled(Col)`
  max-width: 0.1rem;
  padding: 0rem;
  background-color: rgba(96, 96, 96, 0.2);
`;

const RightCol = styled(Col)`
  padding-left: 1rem;
  padding-right: 0rem;
`;

interface IThreeColItem {
  leftSideLabel: string;
  rightSideItem: string | number | React.ReactNode | undefined;
}

export const ThreeColumnItem: React.FC<IThreeColItem> = ({ leftSideLabel, rightSideItem }) => {
  return (
    <InnerRow>
      <LeftCol>
        <ListGroup.Item className="left-side">
          <Label>{leftSideLabel}</Label>
        </ListGroup.Item>
      </LeftCol>
      <CenterCol />
      <RightCol>
        <ListGroup.Item>{rightSideItem}</ListGroup.Item>
      </RightCol>
    </InnerRow>
  );
};
