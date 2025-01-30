import { FaRegBuilding, FaRegUser } from 'react-icons/fa';
import { Link, useHistory } from 'react-router-dom';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import ActiveIndicator from '@/components/common/ActiveIndicator';
import EditButton from '@/components/common/buttons/EditButton';
import ViewButton from '@/components/common/buttons/ViewButton';
import { InlineFlexDiv } from '@/components/common/styles';
import { ColumnWithProps } from '@/components/Table';
import { Claims } from '@/constants/claims';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { IContactSearchResult, isPersonSummary } from '@/interfaces';
import { stringToFragment } from '@/utils';

const columns: ColumnWithProps<IContactSearchResult>[] = [
  {
    Header: '',
    id: 'isDisabled',
    align: 'right',
    width: 10,
    maxWidth: 10,
    minWidth: 10,
    Cell: (props: CellProps<IContactSearchResult>) => (
      <ActiveIndicator active={!props.row.original.isDisabled} />
    ),
  },
  {
    Header: '',
    id: 'id',
    align: 'center',
    width: 20,
    maxWidth: 20,
    Cell: (props: CellProps<IContactSearchResult>) =>
      isPersonSummary(props.row.original) ? (
        <StatusIndicators className={props.row.original.isDisabled ? 'inactive' : 'active'}>
          <FaRegUser size={20} />
        </StatusIndicators>
      ) : (
        <StatusIndicators className={props.row.original.isDisabled ? 'inactive' : 'active'}>
          <FaRegBuilding size={20} />
        </StatusIndicators>
      ),
  },
  {
    Header: 'Summary',
    id: 'summary',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 80,
    maxWidth: 120,
    Cell: (props: CellProps<IContactSearchResult>) => {
      const { hasClaim } = useKeycloakWrapper();
      if (hasClaim(Claims.CONTACT_VIEW)) {
        return (
          <SummaryLink>
            <Link to={`/contact/${props.row.original.id}`}>{props.row.original.summary}</Link>
          </SummaryLink>
        );
      }
      return stringToFragment(props.row.original.summary);
    },
  },
  {
    Header: 'First name',
    accessor: 'firstName',
    sortable: true,
    align: 'left',
    width: 60,
    maxWidth: 100,
  },
  {
    Header: 'Last name',
    accessor: 'surname',
    sortable: true,
    align: 'left',
    width: 60,
    maxWidth: 100,
  },
  {
    Header: 'Organization',
    id: 'organizationName',
    sortable: true,
    align: 'left',
    width: 80,
    maxWidth: 100,
    Cell: (props: CellProps<IContactSearchResult>) => <>{props.row.original.organizationName}</>,
  },
  {
    Header: 'E-mail',
    accessor: 'email',
    align: 'left',
    className: 'text-break',
    minWidth: 80,
    width: 100,
  },
  {
    Header: 'Mailing address',
    accessor: 'mailingAddress',
    align: 'left',
    minWidth: 80,
    width: 100,
  },
  {
    Header: 'City',
    accessor: 'municipalityName',
    sortable: true,
    align: 'left',
    minWidth: 50,
    width: 70,
  },
  {
    Header: 'Prov',
    accessor: 'provinceState',
    align: 'left',
    width: 30,
    maxWidth: 50,
  },
  {
    Header: 'Edit/View',
    accessor: 'controls' as any, // this column is not part of the data model
    width: 50,
    maxWidth: 50,
    Cell: (props: CellProps<IContactSearchResult>) => {
      const history = useHistory();
      const { hasClaim } = useKeycloakWrapper();
      return (
        <StyledDiv>
          {hasClaim(Claims.CONTACT_EDIT) && (
            <EditButton
              title="Edit Contact"
              onClick={() => history.push(`/contact/${props.row.original.id}/edit`)}
            />
          )}

          {hasClaim(Claims.CONTACT_VIEW) && (
            <ViewButton
              title="View Contact"
              onClick={() => history.push(`/contact/${props.row.original.id}`)}
            />
          )}
        </StyledDiv>
      );
    },
  },
];
const StyledDiv = styled(InlineFlexDiv)`
  justify-content: space-around;
  width: 100%;
`;

export const StatusIndicators = styled.div`
  color: ${props => props.theme.css.borderOutlineColor};
  &.active {
    color: ${props => props.theme.bcTokens.iconsColorSuccess};
  }
`;

export const SummaryLink = styled.div`
  a {
    font-weight: bold;
    color: ${props => props.theme.css.activeActionColor} !important;
  }
`;

export default columns;
