namespace Spent.Commons.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<Gender>))]
public enum Gender
{
    Male,

    Female,

    Other
}
