import * as React from 'react';
import { Col, Collapse, Row } from 'react-bootstrap';
import { FaInfoCircle } from 'react-icons/fa';
import { MdArrowDropDown, MdArrowDropUp } from 'react-icons/md';
import styled from 'styled-components';

export const ResearchFileNameGuide: React.FunctionComponent<
  React.PropsWithChildren<unknown>
> = () => {
  const [isCollapsed, setIsCollapsed] = React.useState<boolean>(true);
  return (
    <>
      <StyledSectionHeader>
        <StyledRow
          className="no-gutters"
          onClick={() => {
            setIsCollapsed(!isCollapsed);
          }}
        >
          <Col>
            <StyledFaInfoCircle />
            Help with choosing a name
          </Col>
          <Col xs="1">
            {isCollapsed && <ArrowDropDownIcon />}
            {!isCollapsed && <ArrowDropUpIcon />}
          </Col>
        </StyledRow>
      </StyledSectionHeader>

      <Collapse in={!isCollapsed}>
        <StyledSection>
          <p className="mb-4">
            Provide a predictable research file name that will be easy to search for. Recommended
            format is to use the road name(s) followed by some descriptive text that may include
            (but not limited to) one or more of the following:
          </p>
          <ul>
            <li>Ministry project name</li>
            <li>Name of the area</li>
            <li>Name of the MoTI highway district</li>
            <li>Name of the enquirer</li>
            <li>Legal description</li>
          </ul>
          <p className="mb-4 mt-3">
            If the road name is not available /applicable, the descriptive text should make the
            research file easy to search for in the future
          </p>
          <p>
            <b>Note:</b> This name does not need to be entirely unique, as a unique file number will
            be generated for your research file when you save it.
          </p>
        </StyledSection>
      </Collapse>
    </>
  );
};

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
  font-size: 16px;
  color: ${props => props.theme.css.darkBlue};
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
