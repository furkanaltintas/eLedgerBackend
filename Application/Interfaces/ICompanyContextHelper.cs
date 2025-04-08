namespace Application.Interfaces;

public interface ICompanyContextHelper
{
    T GetCompanyFromContext<T>(string name);
    void SetCompanyInContext<T>(string name, T value);
    void RemoveCompanyFromContext(string name);
}