using MvcProject2.Models;
using MvcProject2.ViewModels;

namespace MvcProject2.Repository
{
    public interface IMovieRepository
    {
        //void AddMovie(Movie movie);
        void DeleteMovie(int id);
        List<Movie> GetAllAsync();
        Task<Movie> GetMovieById(int id);
        NewMovieDropdownListVM GetNewMovieDropdownsValues();
       // void UpdateMovie(Movie movie);
        Task AddNewMovieAsync(NewMovieVM data);
        Task UpdateMovieAsync(NewMovieVM data);
    }
}