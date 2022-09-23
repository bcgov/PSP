type ExtendOverride<T, R> = Omit<T, keyof R> & R;
