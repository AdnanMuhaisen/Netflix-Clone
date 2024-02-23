## GetAllMovies Endpoint
### Summary
	- Get All Available movies from the database with out encoding the movie location. requires authenticated user 
	to get tha data
### Parameters
	- There`s no parameters
### Returns 
	- Returns a list of MovieDto that contains the movie info

## Add New Movie Endpoint
### Summary
	- Creates a new record in the database and creates a nwe file for the created movie in the disk. this api also creates
	a compressed file for each movie to easily transfer the file

### Parameters 
	- Takes a Movie to insert dto that contains the essentiaol data to create a movie in the system
### Returns
	- Returns the created movie as movie dto

## Delete Movie Endpoint
### Summary
	- Delete the movie from the database also delete the files of the target movie from the disk (both original 
	and compressed files)
### Parameters
	- Takes the content id of the movie (movie id)
### Returns
	- Boolean value indicates if the movie is deleted or not

## Update Movie Info Endpoint
### Summary 
	- This api will update the movie info in the database , and if the location of the movie is changed the 
	api will remove the old files of the target movie and then add the new one (for both original and compressed 
	files)
### Parameters
	- Takes movie dto that contains the target movie id and the updated movie info
### Returns
	- Returns movie dto that represents the actual updated movie in the system

## GetMovie (By Id) Endpoint
### Summary
	- This api get the movie record by id , when you call this api the system will add the requested movie
	to the user watch history. 
	and this api will return the full path for the requested movie
### Parameters
	- Takes the Id of the target movie (content id)
### Returns 
	- Returns the requested movie info as movie dto

## Download Movie Endpoint
### Summary
	- Download the requested movie to the Downloads Directory by default , and if the user need to download this
	movi in specific location the api will respond for the user request
### Parameters
	- Takes a DownloadMovieRequestDto that contains a movie id and the location to download the movie in it
### Returns
	- Returns Download Movie Response Dto that represents if the movie downloaded or not

## Get Recommended Movies Endpoint
### Summary
	- Get the top 10 (by default) recommended movies according to the user history. This api will gather the genres
	of the contents in the user history and get the movies based on it.
### Parameters 
	- Takes only one parameter that specifies the number of requested movies
### Returns
	- Return a list of recommended movies

## (Get Movies By) Endpoint
### Summary 
	- This api will filter the movies in the system based multiple properties for the movie and then get 
	the required movies
### Parameters 
	- Takes the Id of Genre,ReleaseYear,MinimumAge,LanguageId,DirectorId to filter the content
### Returns 
	- Return a list of movies based on the paramters 