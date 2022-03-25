import { Table } from 'components/Table';

import { TableCaption } from '../../SectionStyles';

export const VolumetricMeasurementTable: React.FC = () => {
  return (
    <>
      <TableCaption>Volumetric measurement</TableCaption>
      <Table name="volumetric-measurements" hideHeaders columns={[]} data={[]}></Table>
    </>
  );
};
