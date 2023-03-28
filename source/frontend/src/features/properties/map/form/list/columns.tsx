import { LinkButton, StyledIconButton, StyledRemoveIconButton } from 'components/common/buttons';
import { InlineFlexDiv } from 'components/common/styles';
import { ColumnWithProps } from 'components/Table';
import Claims from 'constants/claims';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_FileForm } from 'models/api/Form';
import { FaTrash } from 'react-icons/fa';
import { ImEye } from 'react-icons/im';
import { CellProps } from 'react-table';
import styled from 'styled-components';
import { stringToFragment } from 'utils';

export function createFormTableColumns(
  onShowForm: (form: Api_FileForm) => void,
  onDelete: (formFileId: number) => void,
) {
  const columns: ColumnWithProps<Api_FileForm>[] = [
    {
      Header: 'Form type',
      accessor: 'formTypeCode',
      align: 'left',
      sortable: true,
      minWidth: 40,
      maxWidth: 50,
      Cell: (cellProps: CellProps<Api_FileForm>) => {
        const { hasClaim } = useKeycloakWrapper();
        return hasClaim(Claims.FORM_VIEW) ? (
          <LinkButton onClick={() => onShowForm(cellProps.row.original)}>
            {cellProps.row.original?.formTypeCode?.name ?? ''}
          </LinkButton>
        ) : (
          stringToFragment(cellProps.row.original?.formTypeCode?.name ?? '')
        );
      },
    },
    {
      Header: 'Actions',
      align: 'left',
      sortable: false,
      width: 20,
      maxWidth: 20,
      Cell: (cellProps: CellProps<Api_FileForm>) => {
        const { hasClaim } = useKeycloakWrapper();
        return (
          <StyledDiv>
            {hasClaim(Claims.FORM_VIEW) ? (
              <StyledIconButton
                title="View Template Form"
                variant="light"
                onClick={() => onShowForm(cellProps.row.original)}
              >
                <ImEye size="2rem" />
              </StyledIconButton>
            ) : null}

            {hasClaim(Claims.FORM_DELETE) ? (
              <StyledRemoveIconButton
                onClick={() => cellProps.row.original.id && onDelete(cellProps.row.original.id)}
                title="Delete Template Form"
              >
                <FaTrash size="2rem" />
              </StyledRemoveIconButton>
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
