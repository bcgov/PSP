import { Col, Row } from 'react-bootstrap';
import { FaCaretRight } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import { EditPropertiesIcon } from '@/components/common/buttons/EditPropertiesButton';
import { EditButton } from '@/components/common/EditButton';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';

export interface IResearchMenuProps {
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
                <Col>{label}</Col>
              ) : (
                <Col>
                  <LinkButton onClick={() => handleClick(index)}>{label}</LinkButton>
                </Col>
              )}
            </>
          )}

          {index === 0 && (
            <>
              {props.selectedIndex === index ? (
                <Col>{label}</Col>
              ) : (
                <Col>
                  <LinkButton onClick={() => handleClick(index)}>{label}</LinkButton>
                </Col>
              )}
              <StyledMenuHeaderWrapper>
                <StyledMenuHeader>Properties</StyledMenuHeader>
                {hasClaim(Claims.RESEARCH_EDIT) && (
                  <EditButton
                    title="Change properties"
                    icon={<EditPropertiesIcon />}
                    onClick={() => {
                      props.onEdit();
                    }}
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

const StyledMenuWrapper = styled.div`
  text-align: left;
  padding: 0px;
  margin: 0px;
  width: 100%;
  color: ${({ theme }) => theme.css.linkColor};
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
