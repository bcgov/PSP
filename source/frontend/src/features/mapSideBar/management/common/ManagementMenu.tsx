import cx from 'classnames';
import { Col, Row } from 'react-bootstrap';
import { FaCaretRight } from 'react-icons/fa';
import styled from 'styled-components';

import EditButton from '@/components/common/buttons/EditButton';
import { EditPropertiesIcon } from '@/components/common/buttons/EditPropertiesButton';
import { LinkButton } from '@/components/common/buttons/LinkButton';
import TooltipIcon from '@/components/common/TooltipIcon';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';

import { cannotEditMessage } from '../../acquisition/common/constants';
import { StyledMenuWrapper } from '../../shared/FileMenuView';
import ManagementStatusUpdateSolver from '../tabs/fileDetails/detail/ManagementStatusUpdateSolver';

export interface IManagementMenuProps {
  managementFile: ApiGen_Concepts_ManagementFile;
  items: string[];
  selectedIndex: number;
  onChange: (index: number) => void;
  onShowPropertySelector: () => void;
}

const ManagementMenu: React.FunctionComponent<
  React.PropsWithChildren<IManagementMenuProps>
> = props => {
  const { hasClaim } = useKeycloakWrapper();
  const handleClick = (index: number) => {
    props.onChange(index);
  };
  const statusSolver = new ManagementStatusUpdateSolver(props.managementFile);
  const canEditDetails = () => {
    if (statusSolver?.canEditProperties()) {
      return true;
    }
    return false;
  };

  return (
    <StyledMenuWrapper>
      {props.items.map((label: string, index: number) => {
        const activeIndex = props.selectedIndex === index;
        if (index === 0) {
          return (
            <StyledRow
              key={`menu-item-row-${label ?? index}`}
              data-testid={`menu-item-row-${index}`}
              className={cx('no-gutters', { selected: props.selectedIndex === index })}
            >
              {activeIndex ? (
                <StyledMenuCol>
                  <span title="File Details">{label}</span>
                </StyledMenuCol>
              ) : (
                <Col>
                  <LinkButton title="File Details" onClick={() => handleClick(index)}>
                    {label}
                  </LinkButton>
                </Col>
              )}
              <StyledMenuHeaderWrapper>
                <StyledMenuHeader>Properties</StyledMenuHeader>
                {hasClaim(Claims.MANAGEMENT_EDIT) && canEditDetails() && (
                  <EditButton
                    title="Change properties"
                    icon={<EditPropertiesIcon />}
                    onClick={props.onShowPropertySelector}
                  />
                )}
                {hasClaim(Claims.MANAGEMENT_EDIT) && !canEditDetails() && (
                  <TooltipIcon
                    toolTipId={`${props?.managementFile?.id || 0}-summary-cannot-edit-tooltip`}
                    toolTip={cannotEditMessage}
                  />
                )}
              </StyledMenuHeaderWrapper>
            </StyledRow>
          );
        } else {
          return (
            <StyledRow
              key={`menu-item-row-${label ?? index}`}
              data-testid={`menu-item-row-${index}`}
              className={cx('no-gutters', { selected: props.selectedIndex === index })}
              onClick={() => (props.selectedIndex !== index ? handleClick(index) : '')}
            >
              <Col xs="1">{props.selectedIndex === index && <FaCaretRight />}</Col>
              <Col xs="auto" className="pr-2">
                <StyledIconWrapper className={cx({ selected: props.selectedIndex === index })}>
                  {index}
                </StyledIconWrapper>
              </Col>
              {activeIndex ? (
                <StyledMenuCol>
                  <span title="View">{label}</span>
                </StyledMenuCol>
              ) : (
                <Col>
                  <LinkButton title="View">{label}</LinkButton>
                </Col>
              )}
            </StyledRow>
          );
        }
      })}
    </StyledMenuWrapper>
  );
};

export default ManagementMenu;

const StyledMenuCol = styled(Col)`
  min-height: 2.5rem;
  line-height: 3rem;
`;

const StyledRow = styled(Row)`
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
