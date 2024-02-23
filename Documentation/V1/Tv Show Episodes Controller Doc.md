## Get TV Show Season Episodes Endpoint
### Summary
    - Get all episodes for the target tv show season based on the tv show id and the season id , and returns the decoded locations for those episodes
### Parameters
    - Takes TV Show Season Episodes Request Dto that contains the TV Show Id and season id
### Returns
    - A list of TV Show Episode Dto that contains the episode information

## Add New Season Episode Endpoint
### Summary
    - This endpoin create a record in the database that represents the target season episode and creates the episode file in the disk inside the target tv 
    show season directory to save the target season episodes
### Parameters
    - Takes TV Show Episode To Insert Dto that contains the required information to add the tv show season
### Returns 
    - TV Show Episode Dto that represents the created tv show episode

## Delete Season Episode Endpoint
### Summary 
    - This end point delete the tv show episode from the database based on the episode id that taks as a parameter and deletes the episode file from 
    the target season directory
### Parameters
    - Takes A TV Show Season Episode To Delete Dto that contains the required Id`s to access and delete the episode
### Returns 
    - Returns Deletion Result Dto that represents if the episode deleted or not

## Get TV Show Episode Endpoint
### Summary 
    - Get the tv show episode based on the tv show id , season id and the episode id
### Parameters 
    - Takse A TV Show Episode Request Dto that contains the required information to retrive the episode
### Returns 
    - Returns TV Show Episode Dto that contains the information of the target episode

## Download TV Show Episode Endpoint 
### Summary 
    - This endpoint copy the content file from the source and then copy it to the required location that specified by the user. by default 
    the file is downloaded in the downloads directory ,and then add a record in the User Downloads table in the database.
### Parameters 
    - Takes Download Episode Request Dto that contains the required Id`s and the target location to download inside it.
### Returns
    - Returns string value that contains the Downloaded File Name
