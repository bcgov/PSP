import cx from 'classnames';
import * as React from 'react';
import { Col } from 'react-bootstrap';
import { FaCaretRight } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import { StyledIconWrapper } from '@/components/common/styles';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { getFilePropertyName } from '@/utils/mapPropertyUtils';

import { StyledRow } from './FileMenuView';

interface IFileMenuRowProps {
  index: number;
  currentPropertyIndex: number;
  property: ApiGen_Concepts_FileProperty;
  onSelectProperty: (propertyId: number) => void;
}

export const FileMenuRow: React.FunctionComponent<IFileMenuRowProps> = ({
  index,
  currentPropertyIndex,
  property,
  onSelectProperty,
}) => {
  const propertyName = getFilePropertyName(property);
  return (
    <StyledRow
      key={`menu-item-row-${index}`}
      data-testid={`menu-item-row-${index}`}
      className={cx('no-gutters', { selected: currentPropertyIndex === index })}
      onClick={() => {
        if (currentPropertyIndex !== index) {
          onSelectProperty(property.id);
        }
      }}
    >
      <Col xs="1">{currentPropertyIndex === index && <FaCaretRight />}</Col>
      <Col xs="auto" className="pr-2">
        {property?.isActive !== false ? (
          <StyledIconWrapper className={cx({ selected: currentPropertyIndex === index })}>
            {index + 1}
          </StyledIconWrapper>
        ) : (
          <StyledDisabledIconWrapper>{index + 1}</StyledDisabledIconWrapper>
        )}
      </Col>
      <Col>
        {currentPropertyIndex === index ? (
          <span title="View">{propertyName.value}</span>
        ) : (
          <LinkButton title="View">{propertyName.value}</LinkButton>
        )}
      </Col>
    </StyledRow>
  );
};

const StyledDisabledIconWrapper = styled.div`
  &.selected {
    border-color: ${props => props.theme.bcTokens.themeGray110};
  }
  border: solid 0.3rem;
  border-color: ${props => props.theme.bcTokens.themeGray100};
  font-size: 1.5rem;
  border-radius: 20%;
  width: 3.25rem;
  height: 3.25rem;
  padding: 1rem;
  display: flex;
  justify-content: center;
  align-items: center;
  color: black;
  font-family: 'BCSans-Bold';
`;

export default FileMenuRow;
