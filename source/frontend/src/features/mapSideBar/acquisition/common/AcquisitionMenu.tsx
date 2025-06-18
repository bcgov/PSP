import cx from 'classnames';
import { Col, Row } from 'react-bootstrap';
import { FaCaretRight } from 'react-icons/fa';
import styled from 'styled-components';

import EditButton from '@/components/common/buttons/EditButton';
import { EditPropertiesIcon } from '@/components/common/buttons/EditPropertiesButton';
import { LinkButton } from '@/components/common/buttons/LinkButton';
import { StyledIconWrapper } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';

import { StyledMenuWrapper } from '../../shared/FileMenuView';
import AcquisitionFileStatusUpdateSolver from '../tabs/fileDetails/detail/AcquisitionFileStatusUpdateSolver';
import { cannotEditMessage } from './constants';
import GenerateFormContainer from './GenerateForm/GenerateFormContainer';
import GenerateFormView from './GenerateForm/GenerateFormView';

export interface IAcquisitionMenuProps {
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  items: string[];
  selectedIndex: number;
  onChange: (index: number) => void;
  onShowPropertySelector: () => void;
}

const AcquisitionMenu: React.FunctionComponent<
  React.PropsWithChildren<IAcquisitionMenuProps>
> = props => {
  const { hasClaim } = useKeycloakWrapper();
  const handleClick = (index: number) => {
    props.onChange(index);
  };
  const statusSolver = new AcquisitionFileStatusUpdateSolver(
    props.acquisitionFile.fileStatusTypeCode,
  );
  const canEditDetails = () => {
    if (statusSolver.canEditProperties()) {
      return true;
    }
    return false;
  };
  return (
    <>
      <StyledMenuWrapper>
        {props.items.map((label: string, index: number) => {
          const activeIndex = props.selectedIndex === index;
          if (index === 0) {
            return (
              <StyledRow
                key={`menu-item-row-${index}`}
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
                  {hasClaim(Claims.ACQUISITION_EDIT) && canEditDetails() && (
                    <EditButton
                      title="Change properties"
                      icon={<EditPropertiesIcon />}
                      onClick={props.onShowPropertySelector}
                    />
                  )}
                  {hasClaim(Claims.ACQUISITION_EDIT) && !canEditDetails() && (
                    <TooltipIcon
                      toolTipId={`${props?.acquisitionFile?.id || 0}-summary-cannot-edit-tooltip`}
                      toolTip={cannotEditMessage}
                    />
                  )}
                </StyledMenuHeaderWrapper>
              </StyledRow>
            );
          } else {
            return (
              <StyledRow
                key={`menu-item-row-${index}`}
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
      {props.acquisitionFile?.id && (
        <GenerateFormContainer
          acquisitionFileId={props.acquisitionFile.id}
          View={GenerateFormView}
        />
      )}
    </>
  );
};

export default AcquisitionMenu;

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
