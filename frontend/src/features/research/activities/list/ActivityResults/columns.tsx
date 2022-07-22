import { IconButton } from 'components/common/buttons';
import { InlineFlexDiv } from 'components/common/styles';
import { ColumnWithProps, renderTypeCode } from 'components/Table';
import { Api_Activity } from 'models/api/Activity';
import { ImEye } from 'react-icons/im';
import { MdDelete } from 'react-icons/md';
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
      Cell: renderTypeCode,
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
      accessor: 'controls' as any, // this column is not part of the data model
      align: 'right',
      sortable: false,
      width: 40,
      maxWidth: 40,
      Cell: (cellProps: CellProps<Api_Activity>) => {
        return (
          <StyledDiv>
            {
              <IconButton
                title="View Activity"
                variant="light"
                onClick={() => onShowActivity(cellProps.row.original)}
              >
                <ImEye size="2rem" />
              </IconButton>
            }

            {
              <IconButton
                title="Delete Note"
                variant="gray"
                onClick={() => onDelete(cellProps.row.original)}
              >
                <MdDelete size="2rem" />
              </IconButton>
            }
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
