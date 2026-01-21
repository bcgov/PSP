import { useCallback, useEffect, useState } from 'react';

import { usePropertyImprovementRepository } from '@/hooks/repositories/usePropertyImprovementRepository';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';

import { IFilePropertyImprovements } from '../models/FilePropertyImprovements';
import { IFilePropertiesImprovementsViewProps } from './FilePropertiesImprovementsView';

export interface IFilePropertiesImprovementsContainerProps {
  fileProperties: ApiGen_Concepts_Property[];
  View: React.FunctionComponent<IFilePropertiesImprovementsViewProps>;
}

export const FilePropertiesImprovementsContainer: React.FunctionComponent<
  IFilePropertiesImprovementsContainerProps
> = ({ fileProperties, View }) => {
  const [loading, setLoading] = useState<boolean>(false);
  const [filePropertiesImprovements, setFilePropertiesImprovements] = useState<
    IFilePropertyImprovements[] | null
  >(null);

  const {
    getPropertyImprovements: { execute: getPropertyImprovementsApi },
  } = usePropertyImprovementRepository();

  const fetchFilePropertiesImprovements = useCallback(async () => {
    setLoading(true);
    const propertiesImprovements: IFilePropertyImprovements[] = [];

    if (fileProperties && fileProperties.length > 0) {
      const improvementPromises = fileProperties.map(fp =>
        getPropertyImprovementsApi(fp.id).then(improvements => {
          propertiesImprovements.push({ property: fp, improvements: improvements });
        }),
      );

      await Promise.all(improvementPromises);
    }

    setFilePropertiesImprovements(propertiesImprovements);
    setLoading(false);
  }, [fileProperties, getPropertyImprovementsApi]);

  useEffect(() => {
    if (fileProperties && filePropertiesImprovements === null) {
      fetchFilePropertiesImprovements();
    }
  }, [fetchFilePropertiesImprovements, fileProperties, filePropertiesImprovements]);

  return (
    <View isLoading={loading} filePropertiesImprovements={filePropertiesImprovements ?? []}></View>
  );
};

export default FilePropertiesImprovementsContainer;
