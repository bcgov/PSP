import { groupBy } from 'lodash';
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
import { ApiGen_CodeTypes_HistoricalFileNumberTypes } from '@/models/api/generated/ApiGen_CodeTypes_HistoricalFileNumberTypes';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { convertArea, formatNumber, formatSplitAddress, mapLookupCode } from '@/utils';

import { IPropertyResultRecord } from './PropertyListView';

export const ColumnDiv = styled.div`
  display: flex;
  flex-flow: column;
  padding-right: 0.5rem;
`;

type Props = {
  municipalities: ILookupCode[];
};

export const columns = ({ municipalities }: Props): ColumnWithProps<IPropertyResultRecord>[] => [
  {
    Header: 'PID',
    align: 'right',
    width: 30,
    Cell: (props: CellProps<IPropertyResultRecord>) => {
      return (
        <>
          {props.row.original.property.pid}
          <span style={{ width: '2rem' }}>
            {props.row.original.property.isRetired ? (
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
    accessor: p => p.property.pin,
    align: 'right',
    width: 25,
  },
  {
    Header: 'Historical File #',
    align: 'left',
    clickable: false,
    sortable: false,
    width: 30,
    maxWidth: 50,
    Cell: (props: CellProps<IPropertyResultRecord>) => {
      // File numbers types to display
      const numberTypes: string[] = [
        ApiGen_CodeTypes_HistoricalFileNumberTypes.LISNO.toString(),
        ApiGen_CodeTypes_HistoricalFileNumberTypes.PSNO.toString(),
        ApiGen_CodeTypes_HistoricalFileNumberTypes.OTHER.toString(),
      ];

      const filteredNumberTypes = props.row.original.fileNumbers.filter(x =>
        numberTypes.includes(x.historicalFileNumberTypeCode.id),
      );

      const groupByType = groupBy(filteredNumberTypes, x => x.historicalFileNumberTypeCode.id);

      let lisNumbers = '';
      let psNumbers = '';
      let otherNumbers = '';
      if (groupByType[ApiGen_CodeTypes_HistoricalFileNumberTypes.LISNO.toString()]?.length) {
        lisNumbers = groupByType[ApiGen_CodeTypes_HistoricalFileNumberTypes.LISNO.toString()]
          .map(x => x.historicalFileNumber)
          .join(', ');
      }

      if (groupByType[ApiGen_CodeTypes_HistoricalFileNumberTypes.PSNO.toString()]?.length) {
        psNumbers = groupByType[ApiGen_CodeTypes_HistoricalFileNumberTypes.PSNO.toString()]
          .map(x => x.historicalFileNumber)
          .join(', ');
      }

      if (groupByType[ApiGen_CodeTypes_HistoricalFileNumberTypes.OTHER.toString()]?.length) {
        otherNumbers = groupByType[ApiGen_CodeTypes_HistoricalFileNumberTypes.OTHER.toString()]
          .map(x => x.historicalFileNumber)
          .join(', ');
      }

      return (
        <FileNumbersDiv>
          {lisNumbers ? (
            <label>
              <span>LIS: </span>
              {lisNumbers};
            </label>
          ) : null}
          {psNumbers ? (
            <label>
              <span>PS: </span>
              {psNumbers};
            </label>
          ) : null}
          {otherNumbers ? (
            <label>
              <span>OTHER: </span>
              {otherNumbers}.
            </label>
          ) : null}
        </FileNumbersDiv>
      );
    },
  },
  {
    Header: 'Civic Address',
    accessor: p =>
      formatSplitAddress(
        p.property.streetAddress1,
        p.property.streetAddress2,
        p.property.streetAddress3,
        p.property.municipalityName,
        p.property.provinceName,
        p.property.postalCode,
      ),
    align: 'left',
    minWidth: 100,
    width: 150,
  },
  {
    Header: 'Location',
    accessor: p => p.property.municipalityName,
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
    Cell: (props: CellProps<IPropertyResultRecord>) => {
      const landArea = props.row.original.property.landArea ?? 0;
      const landUnitCode = props.row.original.property.propertyAreaUnitTypeCode;
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
    Cell: (cellProps: CellProps<IPropertyResultRecord>) => {
      const { hasClaim } = useKeycloakWrapper();

      const property = cellProps.row.original.property;
      const ownershipText = property.isOwned
        ? 'Core Inventory'
        : property.hasActiveResearchFile || property.hasActiveResearchFile
        ? 'Property of Interest'
        : property.isOtherInterest
        ? 'Other Interest'
        : property.isDisposed
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
    Cell: (cellProps: CellProps<IPropertyResultRecord, number>) => {
      const { hasClaim } = useKeycloakWrapper();
      const property = cellProps.row.original.property;

      return (
        <StyledDiv>
          {hasClaim(Claims.PROPERTY_VIEW) && (
            <StyledIconButton
              data-testid="view-prop-tab"
              title="View Property Info"
              variant="light"
            >
              <Link to={`/mapview/sidebar/property/${property.id}?pid=${property.pid}`}>
                <FaEye />
              </Link>
            </StyledIconButton>
          )}

          {hasClaim(Claims.PROPERTY_VIEW) && (
            <StyledIconButton data-testid="view-prop-ext" title="View Property" variant="light">
              <Link
                to={`/mapview/sidebar/property/${property.id}?pid=${property.pid}`}
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

const FileNumbersDiv = styled('div')`
  label {
    display: inline-block;

    span {
      font-weight: bold;
    }
  }
`;
