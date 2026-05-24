namespace AlertSystem.API.Features;

public class Routes
{
    public const string Base = "api/alerts";
    public const string GetAll = Base;
    public const string GetById = Base + "/{id:guid}";
    public const string Create = Base;
    public const string Update = Base + "/{id:guid}";
    public const string Delete = Base + "/{id:guid}";
}
