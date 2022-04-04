import { Ref } from 'vue';
import { exportFile, Notify } from 'quasar';
import { QTableColumn } from 'src/composables/QTableColumn';
// ref: https://gist.github.com/IlCallo/08ee564b75f7f19d31b7b834e06859bd
/* eslint-disable @typescript-eslint/no-explicit-any */

function getCellValue<T extends Record<string, any>>(
  col: QTableColumn<T>,
  row: T
) {
  const val = (
    typeof col.field === 'function' ? col.field(row) : row[col.field]
  ) as unknown;

  return col.format !== void 0
    ? col.format(val, row)
    : (val as string | undefined);
}

function wrapCsvValue(val: string) {
  return `"${val.split('"').join('""')}"`;
}

export function useExportCsv<T extends Record<string, any>>(
  rows: Readonly<Ref<T[]>>,
  columns: QTableColumn<T>[],
  filename: string
) {
  function exportTable() {
    // naive encoding to csv format
    const content = [
      columns.map((col) => wrapCsvValue(col.label)),

      ...rows.value.map((row) =>
        columns
          .map((col) => getCellValue(col, row))
          .filter((value): value is string => value !== undefined)
          .map((value) => wrapCsvValue(value))
          .join(',')
      ),
    ].join('\r\n');

    const status = exportFile(`${filename}.csv`, content, 'text/csv');

    if (!status) {
      Notify.create({
        message: 'Browser denied file download...',
        color: 'negative',
        icon: 'warning',
      });
    }
  }

  return {
    exportTable,
  };
}
