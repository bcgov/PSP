import cx from 'classnames';
import { useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaCaretRight } from 'react-icons/fa';
import { matchPath, useLocation } from 'react-router-dom';
import styled from 'styled-components';

import EditButton from '@/components/common/buttons/EditButton';
import { EditPropertiesIcon } from '@/components/common/buttons/EditPropertiesButton';
import { LinkButton } from '@/components/common/buttons/LinkButton';
import TooltipIcon from '@/components/common/TooltipIcon';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { exists, getFilePropertyName, sortFileProperties } from '@/utils';

export interface IFileMenuProps {
  properties: ApiGen_Concepts_FileProperty[];
  canEdit: boolean;
  onSelectFileSummary: () => void;
  onSelectProperty: (propertyId: number) => void;
  onEditProperties: () => void;
}

const FileMenu: React.FunctionComponent<React.PropsWithChildren<IFileMenuProps>> = ({
  properties,
  canEdit,
  onSelectFileSummary,
  onSelectProperty,
  onEditProperties,
  children,
}) => {
  const location = useLocation();

  const currentPropertyIndex = useMemo(() => {
    const match = matchPath(location.pathname, {
      path: '*/property/:menuIndex/',
      exact: false,
      strict: false,
    });
    if (exists(match)) {
      const propertyIndex = Number(match.params['menuIndex']);
      // the index on the url starts at 1, so remove one to make it match the index on the JS side.
      return propertyIndex - 1;
    }
    return null;
  }, [location.pathname]);

  // respect the order of properties as set by the user creating the file
  const sortedProperties = sortFileProperties(properties);
  const isSummary = useMemo(() => !exists(currentPropertyIndex), [currentPropertyIndex]);

  return (
    <StyledMenuWrapper>
      <StyledRow data-testid="menu-item-summary" className={cx({ selected: isSummary })}>
        <Col>
          {isSummary ? (
            <span title="File Details">File Summary</span>
          ) : (
            <LinkButton title="File Details" onClick={onSelectFileSummary}>
              File Summary
            </LinkButton>
          )}
        </Col>
      </StyledRow>
      <StyledMenuHeaderWrapper>
        <StyledMenuHeader>Properties</StyledMenuHeader>
        {canEdit && (
          <EditButton
            title="Change properties"
            icon={<EditPropertiesIcon />}
            onClick={onEditProperties}
          />
        )}
        {!canEdit && (
          <TooltipIcon
            toolTipId="summary-cannot-edit-tooltip"
            toolTip={'You cannot edit this, sorry'}
          />
        )}
      </StyledMenuHeaderWrapper>
      <div className={'p-1'} />
      <StyledMenuBodyWrapper>
        {sortedProperties.map((property: ApiGen_Concepts_FileProperty, index: number) => {
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
                <StyledIconWrapper className={cx({ selected: currentPropertyIndex === index })}>
                  {index + 1}
                </StyledIconWrapper>
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
        })}
      </StyledMenuBodyWrapper>
      <>{children}</>
    </StyledMenuWrapper>
  );
};

export default FileMenu;

export const StyledMenuWrapper = styled.div`
  flex: 1;
  text-align: left;
  padding: 0px;
  margin: 0px;
  width: 100%;
  color: ${props => props.theme.css.linkColor};

  display: flex;
  flex-direction: column;
`;

const StyledRow = styled(Row)`
  width: 100%;

  &.selected {
    font-weight: bold;
    cursor: default;
  }

  font-size: 1.4rem;
  font-weight: normal;
  cursor: pointer;
  padding-bottom: 0.5rem;

  div.Button__value {
    font-size: 1.4rem;
  }
`;

const StyledIconWrapper = styled.div`
  &.selected {
    background-color: ${props => props.theme.bcTokens.themeGold100};
  }

  background-color: ${props => props.theme.css.numberBackgroundColor};
  font-size: 1.5rem;
  border-radius: 50%;
  opacity: 0.8;
  width: 2.5rem;
  height: 2.5rem;
  padding: 1rem;
  display: flex;
  justify-content: center;
  align-items: center;
`;

const StyledMenuHeaderWrapper = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  width: 100%;
  border-bottom: 1px solid ${props => props.theme.css.borderOutlineColor};
`;

const StyledMenuHeader = styled.span`
  font-weight: bold;
  font-size: 1.6rem;
  color: ${props => props.theme.bcTokens.iconsColorSecondary};
  line-height: 2.2rem;
`;

const StyledMenuBodyWrapper = styled.div`
  flex-grow: 1;
`;
