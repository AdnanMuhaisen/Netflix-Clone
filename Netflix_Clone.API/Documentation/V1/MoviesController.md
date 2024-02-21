## GetAllMovies
### Summary
	- Get All Available movies from the database with out encoding the movie location. requires authenticated user 
	to get tha data
### Parameters
	- no parameters
### Returns 
	- returns a list of MovieDto that contains the movie info

## Add New Movie
### Summery
	- creates a new record in the database and creates a nwe file for the created movie in the disk. this api also creates
	a compressed file for each movie to easily transfer the file

### Parameters 
	- taks a Movie to insert dto that contains the essentiaol data to create a movie in the system
### Returns
	- returns the created movie as movie dto

## Delete Movie
### Summery
	- delete the movie from the database also delete the files of the target movie from the disk (both original 
	and compressed files)
### Parameters
	- takes the content id of the movie (movie id)
### Returns
	- boolean value indicates if the movie is deleted or not

## Update Movie Info
### Summery 
	- This api will update the movie info in the database , and if the location of the movie is changed the 
	api will remove the old files of the target movie and then add the new one (for both original and compressed 
	files)
### Parameters
	- takes movie dto that contains the target movie id and the updated movie info
### Returns
	- returns movie dto that represents the actual updated movie in the system

## GetMovie (By Id)
### Summery
	- this api get the movie record by id , when you call this api the system will add the requested movie
	to the user watch history. 
	and this api will return the full path for the requested movie
### Parameters
	- takes the Id of the target movie (content id)
### Returns 
	- returns the requested movie info as movie dto

## Download Movie
### Summery
	- Download the requested movie to the Downloads Directory by default , and if the user need to download this
	movi in specific location the api will respond for the user request
### Parameters
	- takes a DownloadMovieRequestDto that contains a movie id and the location to download the movie in it
### Returns
	- returns Download Movie Response Dto that represents if the movie downloaded or not

## Get Recommended Movies
### Summery
	- Get the top 10 (by default) recommended movies according to the user history. This api will gather the genres
	of the contents in the user history and get the movies based on it.
### Parameters 
	- takes only one parameter that specifies the number of requested movies
### Returns
	- return a list of recommended movies

## Get Movies By
### Summery 
	- this api will filter the movies in the system based multiple properties for the movie and then get 
	the required movies
### Parameters 
	- takes the Id of Genre,ReleaseYear,MinimumAge,LanguageId,DirectorId to filter the content
### Returns 
	- return a list of movies based on the paramters 