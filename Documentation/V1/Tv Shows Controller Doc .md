## Get All TVShows Endpoint
### Summary 
    - Retrive all the tv shows in the database
### Parameters
    - There`s no parameters
### Returns 
    - Returns a list of tv shows as a TV Show Dto that contains the required tv show information

## Add New TV Show Endpoint
### Summary 
    - This endpoint insert a new record in the database and creates a directory for the target tv show to add the tv show seasons and episodes 
### Parameters 
    - Takes TV Show To Insert Dto that contains the required properties to insert the tv show
### Returns
    - Returns the created tv show as a tv show dto

## Delete TV Show Endpoint
### Summary 
    - Delete all the tv show information including tv show seasons and season episodes also deletes the directory for the target tv show.
### Parameters
    - Takes only the tv show id
### Returns 
    - Returns Deletion Result Dto that represents if the tv show deleted or not

## Get TV Show By Id Endpoint
### Summary 
    - Get the tv show based on the tv show id 
### Parameters
    - Takes only the Tv Show Id
### Returns 
    - Returns the target tv show as Tv Show Dto

## Get Recommended TV Shows Endpoint
### Summary 
    - Get the recommended tv shows based on the previous user watch history and the preferd content genres 
### Parameters
    - Takes Total Number Of Items Retrieved that represents the number of records to retrive .(Default value is 10, for pagination purposes)
### Returns 
    - Returns a list of recommended tv shows as a list of tv show dto

## (Get TV Shows By) Endpoint
### Summary 
    - Get the tv shows after filtering them.
### Parameters
    - Takes the filter criteria's to filter
### Returns 
    - Returns a list of tv show dto 