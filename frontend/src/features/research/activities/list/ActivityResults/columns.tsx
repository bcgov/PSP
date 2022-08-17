import { IconButton, LinkButton, StyledRemoveLinkButton } from 'components/common/buttons';
import { InlineFlexDiv } from 'components/common/styles';
import { ColumnWithProps, renderTypeCode } from 'components/Table';
import Claims from 'constants/claims';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import ITypeCode from 'interfaces/ITypeCode';
import { Api_Activity } from 'models/api/Activity';
import { FaTrash } from 'react-icons/fa';
import { ImEye } from 'react-icons/im';
import { CellProps } from 'react-table';
import styled from 'styled-components';

export function createActivityTableColumns(
  onShowActivity: (activity: Api_Activity) => void,
  onDelete: (activity: Api_Activity) => void,
) {
  const columns: ColumnWithProps<Api_Activity>[] = [
    {
      Header: 'Activity type',
      accessor: 'activityTemplateTypeCode',
      align: 'left',
      sortable: true,
      minWidth: 30,
      maxWidth: 40,
      Cell: (cellProps: CellProps<any, ITypeCode<string>>) => {
        const { hasClaim } = useKeycloakWrapper();
        return hasClaim(Claims.NOTE_VIEW) ? (
          <LinkButton onClick={() => onShowActivity(cellProps.row.original)}>
            {cellProps.value?.description ?? ''}
          </LinkButton>
        ) : (
          cellProps.value?.description ?? ''
        );
      },
    },
    {
      Header: 'Description',
      accessor: 'description',
      sortable: true,
      width: 50,
      maxWidth: 100,
    },
    {
      Header: 'Status',
      accessor: 'activityStatusTypeCode',
      sortable: true,
      width: 10,
      maxWidth: 20,
      Cell: renderTypeCode,
    },
    {
      Header: 'Actions',
      align: 'right',
      sortable: false,
      width: 40,
      maxWidth: 40,
      Cell: (cellProps: CellProps<Api_Activity>) => {
        const { hasClaim } = useKeycloakWrapper();
        return (
          <StyledDiv>
            {hasClaim(Claims.NOTE_VIEW) ? (
              <IconButton
                title="View Activity"
                variant="light"
                onClick={() => onShowActivity(cellProps.row.original)}
              >
                <ImEye size="2rem" />
              </IconButton>
            ) : null}

            {hasClaim(Claims.NOTE_DELETE) ? (
              <StyledRemoveLinkButton onClick={() => onDelete(cellProps.row.original)}>
                <FaTrash title="Delete Activity" size="2rem" />
              </StyledRemoveLinkButton>
            ) : null}
          </StyledDiv>
        );
      },
    },
  ];

  return columns;
}

const StyledDiv = styled(InlineFlexDiv)`
  justify-content: space-around;
  width: 100%;
`;
