import { FaExternalLinkAlt, FaEye } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { StyledIconButton } from '@/components/common/buttons';
import { Input } from '@/components/common/form';
import { TypeaheadField } from '@/components/common/form/Typeahead';
import { InlineFlexDiv } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { ColumnWithProps } from '@/components/Table';
import { AreaUnitTypes, Claims } from '@/constants/index';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { convertArea, formatApiAddress, formatNumber, mapLookupCode } from '@/utils';

export const ColumnDiv = styled.div`
  display: flex;
  flex-flow: column;
  padding-right: 0.5rem;
`;

type Props = {
  municipalities: ILookupCode[];
};

export const columns = ({ municipalities }: Props): ColumnWithProps<ApiGen_Concepts_Property>[] => [
  {
    Header: 'PID',
    accessor: 'pid',
    align: 'right',
    width: 40,
    Cell: (props: CellProps<ApiGen_Concepts_Property>) => {
      return (
        <>
          {props.row.original.pid}
          <span style={{ width: '2rem' }}>
            {props.row.original.isRetired ? (
              <TooltipIcon
                variant="warning"
                toolTipId="retired-tooltip"
                toolTip="RETIRED"
                placement="right"
              />
            ) : null}
          </span>
        </>
      );
    },
  },
  {
    Header: 'PIN',
    accessor: 'pin',
    align: 'right',
    width: 40,
  },
  {
    Header: 'Civic Address',
    accessor: p => formatApiAddress(p.address),
    align: 'left',
    minWidth: 100,
    width: 150,
  },
  {
    Header: 'Location',
    accessor: p => p.address?.municipality,
    align: 'left',
    width: 50,
    sortable: true,
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
    Cell: (props: CellProps<ApiGen_Concepts_Property>) => {
      const landArea = props.row.original.landArea ?? 0;
      const landUnitCode = props.row.original.areaUnit?.id;
      const hectars = convertArea(
        landArea,
        landUnitCode ?? AreaUnitTypes.SquareMeters,
        AreaUnitTypes.Hectares,
      );
      return <> {formatNumber(hectars, 0, 3)} </>;
    },
    align: 'right',
    width: 20,
    sortable: true,
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
    Header: 'Ownership',
    align: 'left',
    sortable: true,
    width: 20,
    Cell: (cellProps: CellProps<ApiGen_Concepts_Property>) => {
      const { hasClaim } = useKeycloakWrapper();

      const ownershipText = cellProps.row.original.isOwned
        ? 'Core Inventory'
        : cellProps.row.original.isPropertyOfInterest
        ? 'Property of Interest'
        : cellProps.row.original.isOtherInterest
        ? 'Other Interest'
        : cellProps.row.original.isDisposed
        ? 'Disposed'
        : '';
      return (
        <StyledDiv>
          {hasClaim(Claims.PROPERTY_VIEW) && (
            <>
              <span> {ownershipText}</span>
            </>
          )}
        </StyledDiv>
      );
    },
  },
  {
    Header: 'Actions',
    accessor: 'controls' as any, // this column is not part of the data model
    align: 'right',
    sortable: false,
    width: 20,
    Cell: (cellProps: CellProps<ApiGen_Concepts_Property, number>) => {
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
                to={`/mapview/sidebar/property/${cellProps.row.original.id}?pid=${cellProps.row.original.pid}`}
              >
                <FaEye />
              </Link>
            </StyledIconButton>
          )}

          {hasClaim(Claims.PROPERTY_VIEW) && (
            <StyledIconButton data-testid="view-prop-ext" title="View Property" variant="light">
              <Link
                to={`/mapview/sidebar/property/${cellProps.row.original.id}?pid=${cellProps.row.original.pid}`}
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
