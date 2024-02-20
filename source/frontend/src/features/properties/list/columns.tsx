import { FaExternalLinkAlt, FaEye } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { StyledIconButton } from '@/components/common/buttons';
import { Input } from '@/components/common/form';
import { TypeaheadField } from '@/components/common/form/Typeahead';
import { InlineFlexDiv } from '@/components/common/styles';
import { ColumnWithProps } from '@/components/Table';
import { Claims } from '@/constants/index';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { IProperty } from '@/interfaces';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { formatNumber, formatStreetAddress, mapLookupCode, stringToFragment } from '@/utils';

export const ColumnDiv = styled.div`
  display: flex;
  flex-flow: column;
  padding-right: 0.5rem;
`;

const NumberCell = ({ cell: { value } }: CellProps<IProperty, number | undefined>) =>
  stringToFragment(formatNumber(value ?? 0));

type Props = {
  municipalities: ILookupCode[];
};

export const columns = ({ municipalities }: Props): ColumnWithProps<IProperty>[] => [
  {
    Header: 'PID',
    accessor: 'pid',
    align: 'right',
    width: 40,
  },
  {
    Header: 'PIN',
    accessor: 'pin',
    align: 'right',
    width: 40,
  },
  {
    Header: 'Civic Address',
    accessor: p => formatStreetAddress(p.address),
    align: 'left',
    minWidth: 100,
    width: 150,
  },
  {
    Header: 'Location',
    accessor: p => p.address?.municipality,
    align: 'left',
    width: 50,
    filter: {
      component: TypeaheadField,
      props: {
        name: 'municipality',
        placeholder: 'Filter by location',
        className: 'location-search',
        options: municipalities.map(mapLookupCode).map(x => x.label),
        clearButton: true,
        hideValidation: true,
      },
    },
  },
  {
    Header: 'Lot Size (in\u00A0ha)',
    accessor: 'landArea',
    Cell: NumberCell,
    align: 'right',
    width: 20,
    filter: {
      component: Input,
      props: {
        field: 'maxLotSize',
        name: 'maxLotSize',
        placeholder: 'Filter by Lot Size',
        className: 'filter-input-control',
        type: 'number',
      },
    },
  },
  {
    Header: '',
    accessor: 'controls' as any, // this column is not part of the data model
    align: 'right',
    sortable: false,
    width: 20,
    Cell: (cellProps: CellProps<IProperty, number>) => {
      const { hasClaim } = useKeycloakWrapper();

      return (
        <StyledDiv>
          {hasClaim(Claims.PROPERTY_VIEW) && (
            <StyledIconButton
              data-testid="view-prop-tab"
              title="View Property Info"
              variant="light"
            >
              <Link
                to={`/mapview/sidebar/property/${
                  cellProps.row.original.id
                }?pid=${cellProps.row.original.pid?.toString().replace(/-/g, '')}`}
              >
                <FaEye />
              </Link>
            </StyledIconButton>
          )}

          {hasClaim(Claims.PROPERTY_VIEW) && (
            <StyledIconButton data-testid="view-prop-ext" title="View Property" variant="light">
              <Link
                to={`/mapview/sidebar/property/${
                  cellProps.row.original.id
                }?pid=${cellProps.row.original.pid?.toString().replace(/-/g, '')}`}
                target="_blank"
                rel="noopener noreferrer"
              >
                <FaExternalLinkAlt />
              </Link>
            </StyledIconButton>
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
