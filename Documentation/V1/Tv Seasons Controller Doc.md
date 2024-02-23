## Get TV Show Seasons Endpoint
### Summary 
    - Get all the tv show seasons from the database for the specified tv show based on the tv show id.
### Parameters
    - Takes TV Show Content Id to retrieve the seasons based on it.
### Returns
    - Returns a list of tv show season dto that represents the tv show seasons.

## Add New Season For TV Show Endpoint
### Summary 
    - This endpoint add a new season record and creates a directory inside the target tv show to hold the tv show seaseo episodes.
### Parameters
    - Takes TV Show Season To Insert Dto that contains the required information to add the season.
### Returns 
    - Returns TV Show Season Dto represents the created season.

## Delete TV Show Season Endpoint
### Summary 
    - Delete the target tv show season and delete its directory and all the season episodes.
### Parameters
    - Takes Delete TV Show Season Request Dto that contains the required information to get and delete the season.
### Returns 
    - Returns Deletion Result Dto that indicates if the season is deleted or not.