using Microsoft.AspNetCore.SignalR;
using MoviePoint.Models;

using MoviePoint.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MoviePoint.Hubs
{
	public class MovieHub:Hub
    {
		
		IMovieRepository movieRepository;
		ICommentsRepository commentsRepository;
		public MovieHub( IMovieRepository _movRepo, ICommentsRepository _commentsRepo)
		{
			
			movieRepository = _movRepo;
			commentsRepository = _commentsRepo;
		}


		public void WriteComment(string com, string MovieId , string UserID, string date)
		{
			Comment comment = new Comment();
			comment.comment = com;
			comment.movieID=int.Parse(MovieId);
			comment.userID = UserID;
			comment.CommentDate=DateTime.Parse(date);
			commentsRepository.Insert(comment);

			Clients.All.SendAsync("NewComment", comment);
		}
	}
}
