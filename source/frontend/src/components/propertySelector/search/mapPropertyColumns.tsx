import { Feature, Geometry } from 'geojson';
import { CellProps } from 'react-table';

import { ColumnWithProps } from '@/components/Table';
import { AddressForm } from '@/features/mapSideBar/shared/models';
import { WHSE_Municipalities_Feature_Properties } from '@/models/layers/municipalities';
import { formatApiAddress, pidFormatter, pidFromFeatureSet, pinFromFeatureSet } from '@/utils';

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
      return (
        <>
          {props.row.original?.pimsFeature?.properties?.SURVEY_PLAN_NUMBER ??
            props.row.original?.parcelFeature?.properties?.PLAN_NUMBER}
        </>
      );
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
      return (
        <>
          {props.row.original?.pimsFeature
            ? formatApiAddress(
                AddressForm.fromPimsView(props.row.original?.pimsFeature?.properties).toApi(),
              )
            : ''}
        </>
      );
    },
  },
];

export default columns;
