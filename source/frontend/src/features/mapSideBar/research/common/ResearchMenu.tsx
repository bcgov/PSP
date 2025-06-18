import { Col, Row } from 'react-bootstrap';
import { FaCaretRight } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import { EditButton } from '@/components/common/buttons/EditButton';
import { EditPropertiesIcon } from '@/components/common/buttons/EditPropertiesButton';
import { StyledIconWrapper } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';

import { cannotEditMessage } from '../../acquisition/common/constants';
import { StyledMenuWrapper } from '../../shared/FileMenuView';
import ResearchStatusUpdateSolver from '../tabs/fileDetails/ResearchStatusUpdateSolver';

export interface IResearchMenuProps {
  researchFile: ApiGen_Concepts_ResearchFile;
  items: string[];
  selectedIndex: number;
  onChange: (index: number) => void;
  onEdit: () => void;
}

const ResearchMenu: React.FunctionComponent<
  React.PropsWithChildren<IResearchMenuProps>
> = props => {
  const { hasClaim } = useKeycloakWrapper();
  const handleClick = (index: number) => {
    props.onChange(index);
  };
  const statusSolver = new ResearchStatusUpdateSolver(props.researchFile);
  return (
    <StyledMenuWrapper>
      {props.items.map((label: string, index: number) => (
        <StyledRow
          key={`menu-item-row-${index}`}
          data-testid={`menu-item-row-${index}`}
          className={`no-gutters ${props.selectedIndex === index ? 'selected' : ''}`}
          onClick={() => (props.selectedIndex !== index ? handleClick(index) : '')}
        >
          {index !== 0 && (
            <>
              <Col xs="1">{props.selectedIndex === index && <FaCaretRight />}</Col>
              <Col xs="auto" className="pr-2">
                <StyledIconWrapper className={props.selectedIndex === index ? 'selected' : ''}>
                  {index}
                </StyledIconWrapper>
              </Col>
              {props.selectedIndex === index ? (
                <StyledMenuCol>
                  <span title="View">{label}</span>
                </StyledMenuCol>
              ) : (
                <Col>
                  <LinkButton title="View" onClick={() => handleClick(index)}>
                    {label}
                  </LinkButton>
                </Col>
              )}
            </>
          )}

          {index === 0 && (
            <>
              {props.selectedIndex === index ? (
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
                {hasClaim(Claims.RESEARCH_EDIT) && statusSolver.canEditProperties() && (
                  <EditButton
                    title="Change properties"
                    icon={<EditPropertiesIcon />}
                    onClick={props.onEdit}
                  />
                )}
                {hasClaim(Claims.RESEARCH_EDIT) && !statusSolver.canEditProperties() && (
                  <TooltipIcon
                    toolTipId={`${props?.researchFile?.id || 0}-summary-cannot-edit-tooltip`}
                    toolTip={cannotEditMessage}
                  />
                )}
              </StyledMenuHeaderWrapper>
            </>
          )}
        </StyledRow>
      ))}
    </StyledMenuWrapper>
  );
};

export default ResearchMenu;

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
