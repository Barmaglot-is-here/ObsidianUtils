namespace Utils;

public static class MonthConverter
{
    public static string GetMonthName(this DateTime date) => ToString(date.Month);

    public static string ToString(int month)
    {
        return month switch
        {
            1 => "Январь",
            2 => "Февраль",
            3 => "Март",
            4 => "Апрель",
            5 => "Май",
            6 => "Июнь",
            7 => "Июль",
            8 => "Август",
            9 => "Сентябрь",
            10 => "Октябрь",
            11 => "Ноябрь",
            12 => "Декабрь",
            _ => throw new Exception($"Wrong month: {month}")
        };
    }

    public static int FromString(string month)
    {
        return month switch
        {
            "Январь" => 1,
            "Февраль" => 2,
            "Март" => 3,
            "Апрель" => 4,
            "Май" => 5,
            "Июнь" => 6,
            "Июль" => 7,
            "Август" => 8,
            "Сентябрь" => 9,
            "Октябрь" => 10,
            "Ноябрь" => 11,
            "Декабрь" => 12,
            _ => -1
        };
    }
}