export interface IDiary {
  Id: number;
  DateTime: Date;
  Columns: Map<
    string,
    | (string | number | JSON | boolean | Date)
    | Array<string | number | JSON | boolean | Date>
  >;
}
