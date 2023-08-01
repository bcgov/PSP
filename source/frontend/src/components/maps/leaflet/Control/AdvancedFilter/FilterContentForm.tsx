//import './Legend.scss';

import { Form, Formik, useFormikContext } from 'formik';
import { noop } from 'lodash';
import React, { useEffect, useMemo } from 'react';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';

import { ProjectSelector } from '@/components/common/form/ProjectSelector/ProjectSelector';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';

import { PropertyFilterFormModel } from './models';

interface IFormObserverProps {
  onChange: (model: PropertyFilterFormModel) => void;
}

const FormObserver: React.FC<IFormObserverProps> = ({ onChange }) => {
  const { values } = useFormikContext<PropertyFilterFormModel>();
  useEffect(() => {
    onChange(values);
  }, [onChange, values]);

  return null;
};

export interface IFilterContentFormProps {
  onChange: (model: PropertyFilterFormModel) => void;
  isLoading: boolean;
}

export const FilterContentForm: React.FC<React.PropsWithChildren<IFilterContentFormProps>> = ({
  onChange,
  isLoading,
}) => {
  const initialFilter = useMemo(() => {
    return new PropertyFilterFormModel();
  }, []);

  useEffect(() => {
    const firstLoad = async () => {
      await onChange(initialFilter);
    };
    firstLoad();
  }, [initialFilter, onChange]);

  return (
    <Formik<PropertyFilterFormModel> initialValues={initialFilter} onSubmit={noop}>
      <Form>
        <FormObserver onChange={onChange} />
        <LoadingBackdrop show={isLoading} parentScreen />
        <Section header="Project" isCollapsable initiallyExpanded>
          <SectionField label={null} contentWidth="12">
            <ProjectSelector field="projectPrediction" />
          </SectionField>
          <Row>
            <Col></Col>
          </Row>
        </Section>
      </Form>
    </Formik>
  );
};
