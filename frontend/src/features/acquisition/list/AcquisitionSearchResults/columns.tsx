import { ColumnWithProps, renderTypeCode } from 'components/Table';
import { Claims } from 'constants/claims';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import AcquisitionProperties from './AcquisitionProperties';

export const columns: ColumnWithProps<Api_AcquisitionFile>[] = [
  {
    Header: 'Acquisition file #',
    accessor: 'fileNumber',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<Api_AcquisitionFile>) => {
      const { hasClaim } = useKeycloakWrapper();
      if (hasClaim(Claims.ACQUISITION_VIEW)) {
        return (
          <Link to={`/mapview/sidebar/acquisition/${props.row.original.id}`}>
            {props.row.original.fileNumber}
          </Link>
        );
      }
      return props.row.original.fileNumber;
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
    Cell: (props: CellProps<Api_AcquisitionFile>) => props.row.original.regionCode?.description,
  },
  {
    Header: 'Ministry project',
    accessor: 'ministryProjectNumber',
    align: 'left',
    clickable: true,
    width: 20,
    maxWidth: 30,
    Cell: (props: CellProps<Api_AcquisitionFile>) => {
      const { ministryProjectNumber, ministryProjectName } = props.row.original;
      const formattedValue = [ministryProjectNumber, ministryProjectName].filter(Boolean).join(' ');
      return formattedValue;
    },
  },
  {
    Header: 'Civic Address / PID / PIN',
    accessor: 'fileProperties',
    align: 'left',
    Cell: (props: CellProps<Api_AcquisitionFile>) => {
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
    accessor: 'fileStatusTypeCode',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: renderTypeCode,
  },
];
