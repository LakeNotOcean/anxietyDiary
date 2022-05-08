import { IColumn } from "@src/app/models/column";
import { IDescription } from "@src/app/models/description";
import MapToArray from "@src/lib/MapToArray";
import jsonDescription from "../../../data/descriptions.json";

export const CreateDescriptions = (): Map<string, IDescription> => {
  var result = new Map<string, IDescription>();
  jsonDescription.map((diary) => {
    let diaryDescr = {
      ShortName: diary.ShortName,
      Name: diary.Name,
      DiaryType: diary.DiaryType,
      Description: diary.Description,
      CategoryId: diary.CategoryId,
    } as IDescription;
    diaryDescr.Columns = new Map<string, IColumn>();
    diary.Columns.map((column) => {
      let columnDescr = {
        ShortName: column.ShortName,
        Name: column.Name,
        ValueType: column.ValueType,
        isOptional: column.isOptional,
        isArbitrary: false,
      } as IColumn;
      diaryDescr.Columns.set(columnDescr.ShortName, columnDescr);
    });
    diary.ArbitraryColumns?.map((column) => {
      let columnDescr = {
        ShortName: column.ShortName,
        Name: column.Name,
        ValueType: column.ValueType,
        isOptional: column.isOptional,
        isArbitrary: false,
      } as IColumn;
      diaryDescr.Columns.set(columnDescr.ShortName, columnDescr);
    });
    result.set(diaryDescr.ShortName, diaryDescr);
  });
  return result;
};

export const getDescriptionColumnArray = (name: string): Array<IColumn> => {
  const descriptionMap = CreateDescriptions();
  return MapToArray<string, IColumn>(descriptionMap.get(name).Columns);
};
