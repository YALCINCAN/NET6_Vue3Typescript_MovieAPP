import { QTableProps } from 'quasar';

/* eslint-disable @typescript-eslint/no-explicit-any */
export type QTableColumn<
  Row extends Record<string, any> = any,
  K = Row extends Record<string, any> ? keyof Row : string,
  Field = K | ((row: Row) => any)
> = Omit<NonNullable<QTableProps['columns']>[0], 'field' | 'format'> & {
  field: Field;
  format?: (val: any, row: Row) => string;
};


