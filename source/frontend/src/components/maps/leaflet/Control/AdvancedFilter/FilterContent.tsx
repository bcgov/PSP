//import './Legend.scss';

import { Form, Formik, useFormikContext } from 'formik';
import { noop } from 'lodash';
import React, { useCallback, useEffect, useMemo, useState } from 'react';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';

import { Select, SelectOption } from '@/components/common/form/Select';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { Api_Project } from '@/models/api/Project';
import { Api_PropertyFilterCriteria } from '@/models/api/ProjectFilterCriteria';

class PropertyFilterFormModel {
  public projectId: string = '';

  public toApi(): Api_PropertyFilterCriteria {
    return { projectId: this.projectId !== '' ? Number(this.projectId) : null };
  }
}

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

export const FilterContent: React.FC<React.PropsWithChildren> = () => {
  const [projects, setProjects] = useState<Api_Project[]>([]);

  const mapMachine = useMapStateMachine();

  const setVisiblePimsProperties = mapMachine.setVisiblePimsProperties;

  const { getAllProjects } = useProjectProvider();

  const { getMatchingProperties } = usePimsPropertyRepository();

  const getProjects = getAllProjects.execute;
  const matchProperties = getMatchingProperties.execute;

  const initialFilter = useMemo(() => {
    return new PropertyFilterFormModel();
  }, []);

  const retrieveProjects = useCallback(async () => {
    const retrievedProjects = await getProjects();
    setProjects(retrievedProjects || []);
  }, [getProjects]);

  const filterProperties = useCallback(
    async (filter: Api_PropertyFilterCriteria) => {
      const retrievedProperties = await matchProperties(filter);

      if (retrievedProperties !== undefined) {
        setVisiblePimsProperties(retrievedProperties);
      }
    },
    [matchProperties, setVisiblePimsProperties],
  );

  useEffect(() => {
    const firstLoad = async () => {
      await retrieveProjects();
      await filterProperties(initialFilter.toApi());
    };
    firstLoad();
  }, [retrieveProjects, filterProperties, initialFilter]);

  const projectOptions = useMemo(() => {
    return projects.map<SelectOption>(p => {
      const label = `${p.code} - ${p.description}`;
      return { label, value: p.id?.toString() || '' };
    });
  }, [projects]);

  const onChange = useCallback(
    (model: PropertyFilterFormModel) => {
      filterProperties(model.toApi());
    },
    [filterProperties],
  );

  return (
    <Formik<PropertyFilterFormModel> initialValues={initialFilter} onSubmit={noop}>
      <Form>
        <FormObserver onChange={onChange} />
        <LoadingBackdrop
          show={getAllProjects.loading || getMatchingProperties.loading}
          parentScreen
        />
        <Section header="Project" isCollapsable initiallyExpanded>
          <SectionField label={null} contentWidth="12">
            <Select
              field="projectId"
              placeholder="Select a project"
              options={projectOptions}
            ></Select>
          </SectionField>
          <Row>
            <Col></Col>
          </Row>
        </Section>
      </Form>
    </Formik>
  );
};
