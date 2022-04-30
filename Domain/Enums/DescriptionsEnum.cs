using System.ComponentModel;

namespace Domain.Enums
{
    public enum DiaryType : int
    {
        [Description("Table")]
        Table = 1,
        [Description("Scheme")]
        Scheme = 2,
        [Description("Matrix")]
        Matrix = 3,
    }

    public enum DiaryViewType : int
    {
        [Description("Day")]
        Day = 1,
        [Description("Week")]
        Week = 2,
    }

    public enum ColumnValueType : int
    {
        [Description("String")]
        String = 1,
        [Description("Int")]
        Int = 2,
        [Description("Bool")]
        Bool = 3,

        [Description("Date")]
        Date = 4,

        [Description("Json")]
        Json = 5,
    }
}