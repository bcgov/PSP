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
import { Claims } from '@/constants/index';
import TenureCleanupContainer from '@/features/mapSideBar/shared/detail/PropertyTenureCleanupContainer';
import { TenureCleanupFieldView } from '@/features/mapSideBar/shared/detail/PropertyTenureCleanupFieldView';
import HistoricalNumbersContainer from '@/features/mapSideBar/shared/header/HistoricalNumberContainer';
import { HistoricalNumberFieldView } from '@/features/mapSideBar/shared/header/HistoricalNumberFieldView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_AreaUnitTypes } from '@/models/api/generated/ApiGen_CodeTypes_AreaUnitTypes';
import { ApiGen_Concepts_PropertyView } from '@/models/api/generated/ApiGen_Concepts_PropertyView';
import { ILookupCode } from '@/store/slices/lookupCodes';
import {
  convertArea,
  formatNumber,
  formatSplitAddress,
  mapLookupCode,
  pidFormatter,
} from '@/utils';

export const ColumnDiv = styled.div`
  display: flex;
  flex-flow: column;
  padding-right: 0.5rem;
`;

type Props = {
  municipalities: ILookupCode[];
};

export const columns = ({
  municipalities,
}: Props): ColumnWithProps<ApiGen_Concepts_PropertyView>[] => [
  {
    Header: 'PID',
    align: 'left',
    responsive: true,
    width: 7,
    Cell: (props: CellProps<ApiGen_Concepts_PropertyView>) => {
      return (
        <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
          {pidFormatter(props?.row?.original?.pid?.toString())}
          {props.row.original.isRetired && (
            <TooltipIcon
              variant="warning"
              toolTipId="retired-tooltip"
              toolTip="RETIRED"
              placement="right"
            />
          )}
        </div>
      );
    },
  },
  {
    Header: 'PIN',
    accessor: p => p.pin,
    align: 'left',
    responsive: true,
    width: 7,
  },
  {
    Header: 'Historical File #',
    align: 'left',
    clickable: false,
    sortable: false,
    responsive: true,
    width: 9,
    Cell: (props: CellProps<ApiGen_Concepts_PropertyView>) => {
      const propertyArrayId = [props.row.original.id];
      return (
        <HistoricalNumbersContainer
          propertyIds={propertyArrayId}
          View={HistoricalNumberFieldView}
        />
      );
    },
  },
  {
    Header: 'Civic Address',
    accessor: p =>
      formatSplitAddress(
        p.streetAddress1,
        p.streetAddress2,
        p.streetAddress3,
        p.municipalityName,
        p.provinceName,
        p.postalCode,
      ),
    align: 'left',
    responsive: true,
    width: 30,
  },
  {
    Header: 'Location',
    accessor: p => p.municipalityName,
    align: 'left',
    responsive: true,
    width: 12,
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
    Cell: (props: CellProps<ApiGen_Concepts_PropertyView>) => {
      const landArea = props.row.original.landArea ?? 0;
      const landUnitCode = props.row.original.propertyAreaUnitTypeCode;
      const hectars = convertArea(
        landArea,
        landUnitCode ?? ApiGen_CodeTypes_AreaUnitTypes.M2,
        ApiGen_CodeTypes_AreaUnitTypes.HA,
      );
      return <> {formatNumber(hectars, 0, 3)} </>;
    },
    align: 'left',
    responsive: true,
    width: 7,
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
    responsive: true,
    width: 12,
    Cell: (cellProps: CellProps<ApiGen_Concepts_PropertyView>) => {
      const { hasClaim } = useKeycloakWrapper();

      const property = cellProps.row.original;
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
    Header: 'Tenure Cleanup',
    align: 'left',
    sortable: false,
    responsive: true,
    width: 10,
    Cell: (props: CellProps<ApiGen_Concepts_PropertyView>) => {
      const propertyArrayId = [props.row.original.id];
      return <TenureCleanupContainer propertyIds={propertyArrayId} View={TenureCleanupFieldView} />;
    },
  },
  {
    Header: 'Actions',
    accessor: 'controls' as any, // this column is not part of the data model
    align: 'left',
    sortable: false,
    responsive: true,
    width: 6,
    Cell: (cellProps: CellProps<ApiGen_Concepts_PropertyView, number>) => {
      const { hasClaim } = useKeycloakWrapper();
      const property = cellProps.row.original;

      return (
        <StyledDiv>
          {hasClaim(Claims.PROPERTY_VIEW) && (
            <StyledIconButton
              data-testid="view-prop-tab"
              title="View Property Info"
              variant="primary"
            >
              <Link to={`/mapview/sidebar/property/${property.id}?pid=${property.pid}`}>
                <FaEye size={22} />
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
  justify-content: flex-start;
  width: 100%;
`;
