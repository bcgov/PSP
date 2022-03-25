import { Table } from 'components/Table';

import { TableCaption } from '../../SectionStyles';

export const LandMeasurementTable: React.FC = () => {
  return (
    <>
      <TableCaption>Land measurement</TableCaption>
      <Table name="land-measurements" hideHeaders columns={[]} data={[]}></Table>
    </>
  );
};
