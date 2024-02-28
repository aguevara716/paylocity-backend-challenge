using Api.Models;

namespace Api.Extensions;

public static class DependentExtensions
{
    public static bool IsSignificantOther(this Dependent dependent)
    {
        return dependent.Relationship == Relationship.Spouse || dependent.Relationship == Relationship.DomesticPartner;
    }
}
