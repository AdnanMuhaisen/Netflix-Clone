## Get User Watch List
### Summery 
	- get the user watch list that contains a list of content that the user wish to watch it
### Parameters
	- no parameters
### Returns 
	- return a user watch list dto that contains all of the user watch list information 

## Add To User Watch List
### Summery
	- this api will add the target content with the determined id to the user watch list, in case the user have not 
	a user watch list this api will create the user watch list and then add the content to it
## Parameters
	- taks the target content id to add to the list
## Returns
	- boolean value indecates if the content is added or not

## Delete From User Watch list
### Summery 
	- delete the content with specified content id from the user watch list
### Parameters
	- takes the target content id to delete 
### Returns
	- returns a deletion result dto that represents if the content is deleted from the user watch list or not