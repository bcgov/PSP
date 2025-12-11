import { Feature, Geometry } from 'geojson';
import { CellProps } from 'react-table';

import { ColumnWithProps } from '@/components/Table';
import { WHSE_Municipalities_Feature_Properties } from '@/models/layers/municipalities';
import {
  addressFromFeatureSet,
  pidFormatter,
  pidFromFeatureSet,
  pinFromFeatureSet,
  planFromFeatureSet,
} from '@/utils';

import { IIdentifiedLocationFeatureDataset } from './PropertySearchSelectorFormView';

const columns: ColumnWithProps<IIdentifiedLocationFeatureDataset>[] = [
  {
    Header: 'PID',
    align: 'left',
    maxWidth: 20,
    minWidth: 20,
    Cell: (
      props: CellProps<
        IIdentifiedLocationFeatureDataset,
        Feature<Geometry, WHSE_Municipalities_Feature_Properties>
      >,
    ) => {
      return <>{pidFormatter(pidFromFeatureSet(props.row.original))}</>;
    },
  },
  {
    Header: 'PIN',
    align: 'left',
    width: 20,
    maxWidth: 20,
    Cell: (
      props: CellProps<
        IIdentifiedLocationFeatureDataset,
        Feature<Geometry, WHSE_Municipalities_Feature_Properties>
      >,
    ) => {
      return <>{pinFromFeatureSet(props.row.original)}</>;
    },
  },
  {
    Header: 'Plan #',
    align: 'left',
    width: 20,
    maxWidth: 20,
    Cell: (
      props: CellProps<
        IIdentifiedLocationFeatureDataset,
        Feature<Geometry, WHSE_Municipalities_Feature_Properties>
      >,
    ) => {
      return <>{planFromFeatureSet(props.row.original)}</>;
    },
  },
  {
    Header: 'Address',
    align: 'left',
    width: 20,
    maxWidth: 20,
    Cell: (
      props: CellProps<
        IIdentifiedLocationFeatureDataset,
        Feature<Geometry, WHSE_Municipalities_Feature_Properties>
      >,
    ) => {
      return <>{addressFromFeatureSet(props.row.original)}</>;
    },
  },
];

export default columns;
