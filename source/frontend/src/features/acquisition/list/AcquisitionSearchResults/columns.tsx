import { ColumnWithProps, renderTypeCode } from 'components/Table';
import { Claims } from 'constants/claims';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import { stringToFragment } from 'utils';

import AcquisitionProperties from './AcquisitionProperties';
import { AcquisitionSearchResultModel } from './models';

export const columns: ColumnWithProps<AcquisitionSearchResultModel>[] = [
  {
    Header: 'Acquisition file #',
    accessor: 'fileNumber',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<AcquisitionSearchResultModel>) => {
      const { hasClaim } = useKeycloakWrapper();
      if (hasClaim(Claims.ACQUISITION_VIEW)) {
        return (
          <Link to={`/mapview/sidebar/acquisition/${props.row.original.id}`}>
            {props.row.original.fileNumber}
          </Link>
        );
      }
      return stringToFragment(props.row.original.fileNumber);
    },
  },
  {
    Header: 'Acquisition file name',
    accessor: 'fileName',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 20,
    maxWidth: 40,
  },
  {
    Header: 'MOTI Region',
    accessor: 'regionCode',
    align: 'left',
    clickable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<AcquisitionSearchResultModel>) =>
      stringToFragment(props.row.original.regionCode),
  },
  {
    Header: 'Ministry project',
    accessor: 'ministryProjectNumber',
    align: 'left',
    clickable: true,
    width: 20,
    maxWidth: 30,
    Cell: (props: React.PropsWithChildren<CellProps<AcquisitionSearchResultModel>>) => {
      const { ministryProjectNumber, ministryProjectName } = props.row.original;
      const formattedValue = [ministryProjectNumber, ministryProjectName].filter(Boolean).join(' ');
      return stringToFragment(formattedValue);
    },
  },
  {
    Header: 'Civic Address / PID / PIN',
    accessor: 'fileProperties',
    align: 'left',
    Cell: (props: CellProps<AcquisitionSearchResultModel>) => {
      return (
        <AcquisitionProperties
          acquisitionProperties={props.row.original.fileProperties}
          maxDisplayCount={2}
        ></AcquisitionProperties>
      );
    },
  },
  {
    Header: 'Status',
    accessor: 'acquisitionFileStatusTypeCode',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: renderTypeCode,
  },
];
