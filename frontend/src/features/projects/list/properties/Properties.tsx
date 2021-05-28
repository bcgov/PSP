import { Table } from 'components/Table';
import * as React from 'react';

import { IProperty } from '../../common';
import { columns } from './columns';

export interface IProps {
  data: IProperty[];
  hideHeaders?: boolean;
}

export const Properties: React.FC<IProps> = ({ data, hideHeaders }) => {
  return (
    <Table<IProperty>
      hideHeaders={hideHeaders}
      name="nestedPropertiesTable"
      columns={columns}
      data={data}
      pageCount={1}
      hideToolbar={true}
    />
  );
};
