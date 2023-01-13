using WebPortal.Application.Models;

namespace WebPortal.Services.Interfaces;

public interface ISearchService
{
    public Task<SearchModel> Search(string searchText);
}