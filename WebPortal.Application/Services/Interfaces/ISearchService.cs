using WebPortal.Application.Models;

namespace WebPortal.Application.Services.Interfaces;

public interface ISearchService
{
    public Task<SearchModel> Search(string searchText);
}