import React, { FunctionComponent, PropsWithChildren } from 'react';
import { Col, Collapse, Row } from 'react-bootstrap';
import { FaInfoCircle } from 'react-icons/fa';
import { MdArrowDropDown, MdArrowDropUp } from 'react-icons/md';
import styled from 'styled-components';

export interface IFormGuideViewProps {
  title: string;
  isCollapsed: boolean;
  guideBody: React.ReactNode;
  toggleCollapse: () => void;
}

const FormGuideView: FunctionComponent<PropsWithChildren<IFormGuideViewProps>> = ({
  title,
  isCollapsed,
  guideBody,
  toggleCollapse,
}) => {
  return (
    <>
      <StyledSectionHeader>
        <StyledRow className="no-gutters" onClick={() => toggleCollapse()}>
          <Col>
            <StyledFaInfoCircle />
            {title}
          </Col>
          <Col xs="1">
            {isCollapsed && <ArrowDropDownIcon />}
            {!isCollapsed && <ArrowDropUpIcon />}
          </Col>
        </StyledRow>
      </StyledSectionHeader>

      <Collapse in={!isCollapsed}>
        <StyledSection>{guideBody}</StyledSection>
      </Collapse>
    </>
  );
};

export default FormGuideView;

const StyledSectionHeader = styled.h2`
  font-size: 16px;
  color: #1a5a96;
  font-style: italic;
  font-weight: bold;
  text-decoration: none solid rgb(26, 90, 150);
  line-height: 24px;
  margin-bottom: 1rem;
`;

const StyledSection = styled.div`
  padding: 1.5rem 1.8rem;
  border-radius: 4px;
  background-color: #f0f7fc;
  margin-bottom: 2rem;
  font-size: 14px;
  color: ${props => props.theme.bcTokens.surfaceColorBackgroundDarkBlue};
  text-decoration: none solid rgb(49, 49, 50);
  line-height: 24px;
`;

const ArrowDropDownIcon = styled(MdArrowDropDown)`
  float: right;
  cursor: pointer;
  color: #1a5a96;
  font-size: 2.8rem;
`;
const ArrowDropUpIcon = styled(MdArrowDropUp)`
  float: right;
  cursor: pointer;
  color: #1a5a96;
  font-size: 2.8rem;
`;
const StyledFaInfoCircle = styled(FaInfoCircle)`
  margin-right: 1.5rem;
  font-size: 1.3rem;
`;

const StyledRow = styled(Row)`
  cursor: pointer;
`;
