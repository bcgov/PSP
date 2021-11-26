import { ReactComponent as Active } from 'assets/images/active.svg';
import { ReactComponent as Inactive } from 'assets/images/inactive.svg';
import { InlineFlexDiv } from 'components/common/styles';
import { ColumnWithProps } from 'components/Table';
import { IContactSearchResult } from 'interfaces';
import React from 'react';
import { FaRegBuilding, FaRegUser } from 'react-icons/fa';
import { MdContactMail, MdEdit } from 'react-icons/md';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import * as Styled from '../styles';

const columns: ColumnWithProps<IContactSearchResult>[] = [
  {
    Header: '',
    accessor: 'isDisabled',
    align: 'right',
    width: 20,
    maxWidth: 20,
    Cell: (props: CellProps<IContactSearchResult>) =>
      props.row.original.isDisabled ? <Inactive /> : <Active />,
  },
  {
    Header: '',
    accessor: 'id',
    align: 'right',
    width: 10,
    maxWidth: 10,
    Cell: (props: CellProps<IContactSearchResult>) =>
      props.row.original.personId !== undefined ? (
        <FaRegUser size={20} />
      ) : (
        <FaRegBuilding size={20} />
      ),
  },
  {
    Header: 'Summary',
    accessor: 'summary',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 80,
    maxWidth: 120,
    Cell: (props: CellProps<IContactSearchResult>) => (
      <Link to={`/contact/${props.row.original.id}`}>{props.row.original.summary}</Link>
    ),
  },
  {
    Header: 'Last Name',
    accessor: 'surname',
    sortable: true,
    align: 'left',
    width: 60,
    maxWidth: 100,
  },
  {
    Header: 'First Name',
    accessor: 'firstName',
    sortable: true,
    align: 'left',
    width: 60,
    maxWidth: 100,
  },
  {
    Header: 'Organization',
    accessor: 'organizationName',
    sortable: true,
    align: 'left',
    width: 80,
    maxWidth: 100,
  },
  {
    Header: 'E-mail',
    accessor: 'email',
    align: 'left',
    minWidth: 80,
    width: 100,
  },
  {
    Header: 'Mailing Address',
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
    Header: 'Update/View',
    accessor: 'controls' as any, // this column is not part of the data model
    width: 40,
    maxWidth: 40,
    Cell: (props: CellProps<IContactSearchResult>) => (
      <StyledDiv>
        <Styled.IconButton variant="light">
          <MdEdit size={22} />
        </Styled.IconButton>
        <Styled.IconButton variant="light">
          <MdContactMail size={22} />
        </Styled.IconButton>
      </StyledDiv>
    ),
  },
];
const StyledDiv = styled(InlineFlexDiv)`
  justify-content: space-around;
  width: 100%;
`;

export default columns;
