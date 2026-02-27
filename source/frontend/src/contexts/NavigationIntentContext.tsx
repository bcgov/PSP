import React, { createContext, ReactNode, useContext, useState } from 'react';

export interface NavigationIntent {
  action?: () => void;
}

interface NavigationIntentContextProps {
  intent: NavigationIntent;
  setIntent: (intent: NavigationIntent) => void;
  clearIntent: () => void;
}

const NavigationIntentContext = createContext<NavigationIntentContextProps | undefined>(undefined);

export const NavigationIntentProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [intent, setIntentState] = useState<NavigationIntent>({});

  const setIntent = (intent: NavigationIntent) => setIntentState(intent);
  const clearIntent = () => setIntentState({});

  return (
    <NavigationIntentContext.Provider value={{ intent, setIntent, clearIntent }}>
      {children}
    </NavigationIntentContext.Provider>
  );
};

export const useNavigationIntent = () => {
  const context = useContext(NavigationIntentContext);
  if (!context) {
    throw new Error('useNavigationIntent must be used within a NavigationIntentProvider');
  }
  return context;
};
