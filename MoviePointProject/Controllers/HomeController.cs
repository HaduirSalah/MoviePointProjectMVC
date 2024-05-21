using Microsoft.AspNetCore.Mvc;
using MoviePoint.Models;
using MoviePoint.Repository;
using MoviePoint.ViewModel;
using System.Diagnostics;

namespace MoviePoint.Controllers
{
    public class HomeController : Controller
    {
		IMovieRepository movieRepository;
		IActorRepository actorRepository;
        IActorMovieRepository actorMovieRepository;
        MoviePointContext context=new MoviePointContext();
		public HomeController
			(IMovieRepository _MovieRepo, IActorRepository _ActorRepo,IActorMovieRepository _ActorMovieRepo)//inject
		{
			movieRepository = _MovieRepo; 
			actorRepository = _ActorRepo;
            actorMovieRepository = _ActorMovieRepo;
		}
		public IActionResult Home()
        {
			List<MovieWithActorViewModel> movieWithActorViewModels = new List<MovieWithActorViewModel>();
			List<Movie> movies = movieRepository.GetAll();
            foreach (var itemMovie in movies)
            {
                MovieWithActorViewModel viewModel = new MovieWithActorViewModel();
				List<Actor> allActorsInMovie = new List<Actor>();
				viewModel.MovieName = itemMovie.Name;
                viewModel.MovieDescription = itemMovie.Description;
                viewModel.MoviePicture = itemMovie.ImageUrl;
                viewModel.MovieStartDate = itemMovie.StartDate;
                viewModel.MovieEndDate = itemMovie.EndtDate;
                List<int> actorsID = actorMovieRepository.ActorById(itemMovie.Id);

				if (actorsID!=null)
                {
					foreach (int actor in actorsID)
					{
                        
                        Actor temp = actorRepository.GetById(actor);
						allActorsInMovie.Add(temp);
						
					}
					viewModel.Actors = allActorsInMovie;
					movieWithActorViewModels.Add(viewModel);
				}
                
            }
            return View(movieWithActorViewModels);
        }         
    }
}