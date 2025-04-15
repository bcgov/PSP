import { FaBan } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons/LinkButton';
import { ModalSize } from '@/components/common/GenericModal';
import { useModalContext } from '@/hooks/useModalContext';
import HealthCheckStyled from '@/layouts/Healthcheck';

export interface IHealthCheckIssue {
  key: string;
  msg: string;
}

export interface IHealthCheckViewProps {
  systemChecked: boolean;
  systemDegraded: boolean;
  systemChecks: IHealthCheckIssue[] | null;
}

const HealthcheckView: React.FunctionComponent<IHealthCheckViewProps> = ({
  systemChecked,
  systemDegraded,
  systemChecks,
}) => {
  const { setModalContent, setDisplayModal } = useModalContext();

  return systemChecked && systemDegraded ? (
    <HealthCheckStyled>
      <StyledWrapperDiv>
        <StyledIconDiv>
          <FaBan size={24} />
        </StyledIconDiv>
        <StyledContainer>
          <label>
            <span>{systemChecks[0].key}: </span>
            {systemChecks[0].msg}
          </label>
          {systemChecks.length > 1 && (
            <LinkButton
              data-testid="healthcheck-full-list-lnk"
              onClick={() => {
                setModalContent({
                  variant: 'error',
                  title: 'Error',
                  modalSize: ModalSize.LARGE,
                  message: (
                    <StyledList>
                      {systemChecks.map(x => {
                        return (
                          <label key={x.key}>
                            <span>{x.key}: </span>
                            {x.msg}
                          </label>
                        );
                      })}
                    </StyledList>
                  ),
                  okButtonText: 'Close',
                  handleOk: async () => {
                    setDisplayModal(false);
                  },
                });
                setDisplayModal(true);
              }}
            >
              See the full list here...
            </LinkButton>
          )}
        </StyledContainer>
      </StyledWrapperDiv>
    </HealthCheckStyled>
  ) : null;
};

const StyledWrapperDiv = styled.div`
  display: flex;
  height: 100%;
  background-color: ${props => props.theme.css.dangerBackgroundColor};
`;

const StyledIconDiv = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  width: auto;
  min-width: 6rem;
  height: 100%;
  background-color: ${props => props.theme.bcTokens.typographyColorDanger};

  svg {
    color: ${props => props.theme.css.pimsWhite};
  }
`;

const StyledContainer = styled.div`
  display: flex;
  align-items: center;
  flex-direction: row;
  flex-grow: 1;
  justify-content: flex-start;
  padding-left: 6rem;
  background-color: ${props => props.theme.css.dangerBackgroundColor};

  label {
    display: list-item;
    margin-bottom: 0;

    span {
      font-weight: bolder;
    }
  }

  button {
    margin-left: 6rem;
  }
`;

const StyledList = styled.div`
  padding: 1rem 2rem;

  label {
    display: list-item;
    margin-bottom: 0;

    span {
      font-weight: bolder;
    }
  }

  button {
    margin-left: 6rem;
  }
`;

export default HealthcheckView;
