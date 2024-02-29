import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { Table } from '@/components/Table';
import { AreaUnitTypes } from '@/constants';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getApiPropertyName, prettyFormatDateTime } from '@/utils';

import columns from './columns';

interface ISubdivisionViewProps {
  operationTimeStamp: UtcIsoDateTime;
  sourceProperties: ApiGen_Concepts_Property[];
  destinationProperties: ApiGen_Concepts_Property[];
}

export interface PropertySubdivisionResult {
  id: number;
  isSource: boolean;
  identifier: string;
  plan: string;
  status: string;
  area: number;
  areaUnitCode: string;
}

export const SubdivisionView: React.FunctionComponent<ISubdivisionViewProps> = ({
  operationTimeStamp,
  sourceProperties,
  destinationProperties,
}) => {
  const toSubdivistionResult = (property: ApiGen_Concepts_Property, isSource: boolean) => {
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

  let subdivisionColumns = sourceProperties.map<PropertySubdivisionResult>(o => {
    return toSubdivistionResult(o, true);
  });

  subdivisionColumns = subdivisionColumns.concat(
    destinationProperties.map<PropertySubdivisionResult>(o => {
      return toSubdivistionResult(o, false);
    }),
  );

  return (
    <Section
      header={
        <SectionField label="Created Date Time" labelWidth="auto">
          {prettyFormatDateTime(operationTimeStamp)}
        </SectionField>
      }
      isStyledHeader
      initiallyExpanded
      isCollapsable
    >
      <Table<PropertySubdivisionResult>
        name="subdivisionTable"
        columns={columns}
        data={subdivisionColumns}
        noRowsMessage="No subdivision history"
        hideToolbar
        manualPagination
        disableSelection={true}
      ></Table>
    </Section>
  );
};
