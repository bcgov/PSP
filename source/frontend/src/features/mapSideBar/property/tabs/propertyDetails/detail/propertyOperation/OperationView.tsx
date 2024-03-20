import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { Table } from '@/components/Table';
import { AreaUnitTypes } from '@/constants';
import { ApiGen_CodeTypes_PropertyOperationTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyOperationTypes';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getApiPropertyName, prettyFormatUTCDate, prettyFormatUTCTime } from '@/utils';

import getColumns from './columns';

export interface IOperationViewProps {
  operationType: ApiGen_CodeTypes_PropertyOperationTypes;
  operationTimeStamp: UtcIsoDateTime;
  sourceProperties: ApiGen_Concepts_Property[];
  destinationProperties: ApiGen_Concepts_Property[];
}

export interface PropertyOperationResult {
  id: number;
  isSource: boolean;
  identifier: string;
  plan: string;
  status: string;
  area: number;
  areaUnitCode: string;
}

export const OperationView: React.FunctionComponent<IOperationViewProps> = ({
  operationType,
  operationTimeStamp,
  sourceProperties,
  destinationProperties,
}) => {
  const toOperationResult = (property: ApiGen_Concepts_Property, isSource: boolean) => {
    const propertyName = getApiPropertyName(property);
    return {
      id: property.id,
      isSource: isSource,
      identifier: `${propertyName.label}: ${propertyName.value}`,
      plan: property.planNumber?.toString() ?? '',
      status: property.isRetired ? 'Retired' : 'Active',
      area: property.landArea ?? 0,
      areaUnitCode: property.areaUnit?.id ?? AreaUnitTypes.SquareMeters,
    };
  };

  let operationData = sourceProperties.map<PropertyOperationResult>(o => {
    return toOperationResult(o, true);
  });

  operationData = operationData.concat(
    destinationProperties.map<PropertyOperationResult>(o => {
      return toOperationResult(o, false);
    }),
  );

  return (
    <Section
      header={
        <SectionField label="Created on" labelWidth="auto" className="">
          {prettyFormatUTCDate(operationTimeStamp)} at {prettyFormatUTCTime(operationTimeStamp)}
        </SectionField>
      }
      isStyledHeader
      initiallyExpanded
      isCollapsable
      noPadding
    >
      <Table<PropertyOperationResult>
        name="propertyOperationTable"
        columns={getColumns(ApiGen_CodeTypes_PropertyOperationTypes.SUBDIVIDE === operationType)}
        data={operationData}
        hideToolbar
        manualPagination
        disableSelection={true}
      ></Table>
    </Section>
  );
};
