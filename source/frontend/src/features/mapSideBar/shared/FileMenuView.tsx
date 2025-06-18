import cx from 'classnames';
import { useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { matchPath, useLocation } from 'react-router-dom';
import styled from 'styled-components';

import EditButton from '@/components/common/buttons/EditButton';
import { EditPropertiesIcon } from '@/components/common/buttons/EditPropertiesButton';
import { LinkButton } from '@/components/common/buttons/LinkButton';
import TooltipIcon from '@/components/common/TooltipIcon';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { exists } from '@/utils';

import FileMenuRow from './FileMenuRow';

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

  const isSummary = useMemo(() => !exists(currentPropertyIndex), [currentPropertyIndex]);

  const activeProperties = [];
  const inactiveProperties = [];
  properties.forEach(p => {
    if (p.isActive !== false) {
      activeProperties.push(p);
    } else {
      inactiveProperties.push(p);
    }
  });

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
        <h4>Active</h4>
        {activeProperties.map((property: ApiGen_Concepts_FileProperty, index: number) => {
          return (
            <FileMenuRow
              key={`menu-item-row-parent-${property?.id ?? index}`}
              index={index}
              currentPropertyIndex={currentPropertyIndex}
              property={property}
              onSelectProperty={onSelectProperty}
            />
          );
        })}
        <h4>Inactive</h4>
        {inactiveProperties.map((property: ApiGen_Concepts_FileProperty, index: number) => {
          return (
            <FileMenuRow
              key={`menu-item-row-parent-${property?.id ?? index}`}
              index={index}
              currentPropertyIndex={currentPropertyIndex}
              property={property}
              onSelectProperty={onSelectProperty}
            />
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

export const StyledRow = styled(Row)`
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
