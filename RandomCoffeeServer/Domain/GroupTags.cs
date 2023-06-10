namespace RandomCoffeeServer.Domain;

public static class GroupTags
{
    // ReSharper disable MemberCanBePrivate.Global
    public const string Art = "Творчество";
    public const string Technologies = "Техника";
    public const string Books = "Книги";
    public const string Movies = "Кино";
    public const string Medicine = "Медицина и здоровье";
    public const string Fashion = "Мода и стиль";
    public const string Music = "Музыка";
    public const string Culture = "Культура";
    public const string Sports = "Спорт";
    public const string Dancing = "Танцы";
    public const string InformationTechnology = "IT";
    public const string Traveling = "Путешествия";
    public const string Cooking = "Кулинария";
    public const string Cars = "Авто";
    public const string SelfDevelopment = "Саморазвитие";
    public const string Family = "Семья";
    public const string Meetings = "Знакомства";

    public static readonly IReadOnlyList<string> Tags = new List<string>
    {
        Art,
        Technologies,
        Books,
        Movies,
        Medicine,
        Fashion,
        Music,
        Culture,
        Sports,
        Dancing,
        InformationTechnology,
        Traveling,
        Cooking,
        Cars,
        SelfDevelopment,
        Family,
        Meetings
    };
}