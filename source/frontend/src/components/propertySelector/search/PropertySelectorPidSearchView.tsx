import { useFormikContext } from 'formik';
import { Col, Row } from 'react-bootstrap';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { SelectInput } from '@/components/common/List/SelectInput';
import { Section } from '@/components/common/Section/Section';
import { SubdivisionFormModel } from '@/features/mapSideBar/subdivision/AddSubdivisionModel';

import { ILayerSearchCriteria } from '../models';

export interface IPropertySearchSelectorPidFormViewProps {
  onSearch: (search: ILayerSearchCriteria) => void;
  loading: boolean;
}

export const PropertySearchSelectorPidFormView: React.FunctionComponent<
  React.PropsWithChildren<IPropertySearchSelectorPidFormViewProps>
> = ({ onSearch, loading }) => {
  const formikProps = useFormikContext<SubdivisionFormModel>();
  return (
    <>
      <Section header={undefined}>
        <Row>
          <Col xs={8} className="pr-0">
            <SelectInput<{ pid: string }, SubdivisionFormModel>
              field="searchBy"
              defaultKey="pid"
              selectOptions={[
                {
                  key: 'pid',
                  placeholder: `Enter a PID`,
                  label: 'PID',
                },
              ]}
              autoSetting="off"
            />
          </Col>
          <Col xs={1} className="p-0">
            <Row>
              <Col className="p-0" xs={6}>
                <SearchButton
                  disabled={loading}
                  onClick={() => onSearch({ pid: formikProps.values.pid })}
                  type="button"
                />
              </Col>
              <Col className="p-0" xs={6}>
                <ResetButton
                  disabled={loading}
                  onClick={() => {
                    formikProps.setFieldValue('pid', '');
                  }}
                />
              </Col>
            </Row>
          </Col>
        </Row>
      </Section>
    </>
  );
};

export default PropertySearchSelectorPidFormView;
