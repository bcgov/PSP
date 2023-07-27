//import './Legend.scss';

import { Form, Formik, useFormikContext } from 'formik';
import { noop } from 'lodash';
import React, { useCallback, useEffect, useMemo, useState } from 'react';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';

import { Select, SelectOption } from '@/components/common/form/Select';
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

export const FilterContainer: React.FC<React.PropsWithChildren> = () => {
  const [projects, setProjects] = useState<Api_Project[]>([]);

  const mapMachine = useMapStateMachine();

  const setVisiblePimsProperties = mapMachine.setVisiblePimsProperties;

  const { getAllProjects } = useProjectProvider();

  const { getMatchingProperties } = usePimsPropertyRepository();

  const getProjects = getAllProjects.execute;
  const matchProperties = getMatchingProperties.execute;

  const retrieveProjects = useCallback(async () => {
    const retrievedProjects = await getProjects();
    setProjects(retrievedProjects || []);
  }, [getProjects]);

  const filterProperties = useCallback(
    async (filter: Api_PropertyFilterCriteria) => {
      const retrievedProjects = await matchProperties(filter);

      if (retrievedProjects !== undefined) {
        setVisiblePimsProperties(retrievedProjects);
      }
    },
    [matchProperties, setVisiblePimsProperties],
  );

  useEffect(() => {
    retrieveProjects();
  }, [retrieveProjects]);

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
    <div className="border">
      <Formik<PropertyFilterFormModel>
        initialValues={new PropertyFilterFormModel()}
        onSubmit={noop}
      >
        <Form>
          <FormObserver onChange={onChange} />
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
    </div>
  );
};
