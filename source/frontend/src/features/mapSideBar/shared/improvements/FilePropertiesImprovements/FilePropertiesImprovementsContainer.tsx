import { useCallback, useEffect, useState } from 'react';

import { usePropertyImprovementRepository } from '@/hooks/repositories/usePropertyImprovementRepository';
import { ApiGen_CodeTypes_PropertyImprovementStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyImprovementStatusTypes';
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
          const sortArray = improvements.sort((a, b) => {
            const statusA = a.improvementStatusCode.id;
            const statusB = b.improvementStatusCode.id;
            if (statusA === statusB) return 0;
            if (statusA === ApiGen_CodeTypes_PropertyImprovementStatusTypes.ACTIVE) return -1;
            if (statusB !== ApiGen_CodeTypes_PropertyImprovementStatusTypes.ACTIVE) return 1;
            return 0;
          });
          propertiesImprovements.push({ property: fp, improvements: sortArray });
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
