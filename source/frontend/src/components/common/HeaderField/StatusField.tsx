import { Col, Row } from 'react-bootstrap';
import { IconType } from 'react-icons';
import { BiDuplicate } from 'react-icons/bi';
import { FaCheck, FaPauseCircle, FaStopCircle } from 'react-icons/fa';
import { MdArchive, MdCancel, MdCircle, MdEdit } from 'react-icons/md';
import styled from 'styled-components';

import { Dictionary } from '@/interfaces/Dictionary';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { isValidIsoDateTime, prettyFormatDate } from '@/utils';

import { InlineFlexDiv } from '../styles';
import { StyledSmallText } from './AuditSection';

interface IStatusFieldProps {
  statusCodeType: ApiGen_Base_CodeType<string>;
  preText?: string;
  statusCodeDate?: string;
}

interface StatusStyle {
  icon: IconType;
  colorVariant: string;
}

const statusDictionary: Dictionary<StatusStyle> = {
  active: { icon: MdCircle, colorVariant: 'green' },
  archived: { icon: MdArchive, colorVariant: 'grey' },
  completed: { icon: FaCheck, colorVariant: 'blue' },
  hold: { icon: FaPauseCircle, colorVariant: 'yellow' },
  cancelled: { icon: MdCancel, colorVariant: 'red' },
  draft: { icon: MdEdit, colorVariant: 'yellow' },
  terminated: { icon: FaStopCircle, colorVariant: 'red' },
  duplicate: { icon: BiDuplicate, colorVariant: 'blue' },
};

const StatusField: React.FunctionComponent<React.PropsWithChildren<IStatusFieldProps>> = ({
  statusCodeType,
  preText,
  statusCodeDate,
}) => {
  const translateStatusCode = (statusCodeType: ApiGen_Base_CodeType<string>) => {
    switch (statusCodeType.id.toLowerCase()) {
      case 'active':
      case 'ac':
        return statusDictionary['active'];
      case 'archived':
      case 'cncn':
      case 'archiv':
        return statusDictionary['archived'];
      case 'cancelled':
      case 'ca':
      case 'cancel':
      case 'discard':
        return statusDictionary['cancelled'];
      case 'complete':
      case 'closed':
      case 'co':
      case 'complt':
        return statusDictionary['completed'];
      case 'draft':
      case 'pl':
        return statusDictionary['draft'];
      case 'duplicate':
        return statusDictionary['duplicate'];
      case 'hold':
      case 'inactive':
      case 'ho':
        return statusDictionary['hold'];
      case 'terminated':
        return statusDictionary['terminated'];
      default:
        break;
    }
    return statusDictionary['cancelled'];
  };

  const statusFound = translateStatusCode(statusCodeType);
  return (
    <StyledBottomContainer>
      <Row className="no-gutters justify-content-end align-items-end">
        <Col />
        <Col xs="auto" className="align-self-end d-flex">
          <StyledSmallText>
            <b>{preText}</b>
          </StyledSmallText>
          <RetiredWarning $variant={statusFound.colorVariant}>
            <statusFound.icon size={16} />
            {statusCodeType?.description.toUpperCase()}
          </RetiredWarning>
        </Col>
      </Row>
      {isValidIsoDateTime(statusCodeDate) && (
        <Row className="no-gutters justify-content-end align-items-end">
          <Col />
          <Col xs="auto" className="align-self-end d-flex">
            <StyledSmallText>{prettyFormatDate(statusCodeDate)}</StyledSmallText>
          </Col>
        </Row>
      )}
    </StyledBottomContainer>
  );
};

export default StatusField;

const RetiredWarning = styled(InlineFlexDiv)<{ $variant: string }>`
  //font-weight: bold; TODO: Bold should be enough, but atm the bcsans-bold is different
  font-family: 'BCSans-Bold';
  font-size: 1.4rem;

  border-radius: 0.4rem;

  letter-spacing: 0.1rem;
  align-items: center;

  padding: 0.2rem 0.5rem;
  margin-left: 0.4rem;

  gap: 0.5rem;

  ${props => {
    if (props.$variant === 'green') {
      return `
  color: ${props.theme.css.completedColor};
  background-color: ${props.theme.css.completedBackgroundColor};
    `;
    } else if (props.$variant === 'grey') {
      return `
  color: ${props.theme.css.fileStatusGreyColor};
  background-color: ${props.theme.css.fileStatusGreyBackgroundColor};
    `;
    } else if (props.$variant === 'blue') {
      return `
  color: ${props.theme.css.fileStatusBlueColor};
  background-color: ${props.theme.css.filterBoxColor};
    `;
    } else if (props.$variant === 'yellow') {
      return `
  color: ${props.theme.css.textWarningColor};
  background-color: ${props.theme.css.warningBackgroundColor};
    `;
    } else if (props.$variant === 'red') {
      return `
  color: ${props.theme.css.fontDangerColor};
  background-color: ${props.theme.css.dangerBackgroundColor};
    `;
    } else {
      return `color: red`;
    }
  }}
`;

const StyledBottomContainer = styled.div`
  margin-top: auto;
  padding-top: 1rem;
  padding-bottom: 1rem;
`;
