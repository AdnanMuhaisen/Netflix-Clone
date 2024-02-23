## Get User Watch List Endpoint
### Summary 
	- Get the user watch list that contains a list of content that the user wish to watch it
### Parameters
	- There`s No parameters
### Returns 
	- return a user watch list dto that contains all of the user watch list information 

## Add To User Watch List Endpoint
### Summary
	- This api will add the target content with the determined id to the user watch list, in case the user have not 
	a user watch list this api will create the user watch list and then add the content to it
## Parameters
	- Taks the target content id to add to the list
## Returns
	- Boolean value indecates if the content is added or not

## Delete From User Watch list Endpoint
### Summary 
	- Delete the content with specified content id from the user watch list
### Parameters
	- Takes the target content id to delete 
### Returns
	- Returns a deletion result dto that represents if the content is deleted from the user watch list or not